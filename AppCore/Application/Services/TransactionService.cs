using AppCore.Application.Interfaces;
using AppCore.Shared.Interfaces;
using Domain.DTOs.DummyTransactions;
using Domain.DTOs.GetTransactionReq;
using Domain.DTOs.UpdateTransactionReq;
using Domain.Model;
using EventPublisher.Clients.Interfaces;
using EventPublisher.Events;
using Newtonsoft.Json;
using Persistence.Entities;
using Persistence.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace AppCore.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly Logger _logger;
        private readonly IHttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBroadcaster _eventBus;
        private readonly IResponseHandler _responeHandler;

        public TransactionService(Logger logger,
            IResponseHandler responseHandler,
            IHttpClient httpClient,
            IUnitOfWork unitOfWork,
            IBroadcaster eventBus)
        {
            _logger = logger;
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _responeHandler = responseHandler;
        }

        public async Task<ResponseParam> CheckForTransactionUpdate(UpdateTransactionReq updateReq)
        {
            var requestId = updateReq.RequestId;
            try
            {
                var response = _responeHandler.InitializeResponse(requestId);
                //Save unique requests to update transactions
                //todo: a. check if reference exist;  b. create record for request if no exist
                var rowCount = 0;
                var requestExist = _unitOfWork.ClientRequests.CheckClientRequestExist(requestId);
                if(requestExist == null)
                {
                    var clientReq = new ClientRequest()
                    {
                        RequestReference = requestId,
                        ClientId = updateReq?.ClientId ?? "n/a",
                        WalletAddress = updateReq?.WalletAddress ?? "n/a",
                        CurrencyType = updateReq?.CurrencyType ?? "n/a",
                        DateRecieved = DateTime.Now
                    };
                    await _unitOfWork.ClientRequests.SaveClientRequest(clientReq);
                    rowCount = _unitOfWork.SaveChanges();
                    
                }
                
                //Proceed to Query imaginary 3rd party crypto API
                var url = $"https://mycryptowave/api/transactions";
                var param = new
                {
                    clientId = updateReq.ClientId,
                    walletAddress = updateReq.WalletAddress,
                    currencyType = updateReq.CurrencyType
                };
                var reqBody = JsonConvert.SerializeObject(param);

                var resp = await _httpClient.Post(reqBody, url, requestId);

                var apiTxnDTO = JsonConvert.DeserializeObject<DummyTxnDTO>(resp);

                //Verify if response is a duplicate transaction
                //todo: a. Verify reference and amount against record on the db
                // b. Log trasaction to db if transaction is new

                var transactionExist = _unitOfWork.Transactions.GetTransaction(apiTxnDTO?.TransactionRef);

                if(transactionExist != null)
                {
                    //Duplicate transaction
                    response = _responeHandler.CommitResponse(requestId, ResponseCodes.DUPLICATE_FUNC, "Sorry, this transaction is duplicate!", transactionExist);
                }
                else
                {
                    //Save Transaction
                    var newTxn = new Transactions()
                    {
                        ClientId = apiTxnDTO.ClientId,
                        WalletAddress = apiTxnDTO.WalletAddress,
                        Amount = apiTxnDTO.Amount,
                        CurrencyType = updateReq.CurrencyType,
                        TransactionRef = apiTxnDTO.TransactionRef,
                        TransactionDate = apiTxnDTO.Date,
                        TransactionStatus = apiTxnDTO.Status
                    };

                    await _unitOfWork.Transactions.AddTransaction(newTxn);
                   rowCount = _unitOfWork.SaveChanges();
                   

                    if(rowCount > 0)
                    {
                        //Publish Transaction Recieved to EventBus
                        var payload = JsonConvert.SerializeObject(newTxn);
                        await _eventBus.PublishPayload(payload, BrokerEvents.TransactionRecieved);

                        //Update response
                        response = _responeHandler.CommitResponse(requestId, ResponseCodes.SUCCESS, "Success! new transaction retrieved", newTxn);
                    }
                    else
                    {
                        response = _responeHandler.CommitResponse(requestId, ResponseCodes.DB_ERROR, "Error writing new transaction to database");
                    }
                }
                _logger.LogInfo($"[TransactionService][CheckForTransactionUpdate]=> {JsonConvert.SerializeObject(response)} | [requestId]=>{requestId}");

                if (rowCount > 0) _unitOfWork.Dispose();

                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"[TransactionService][CheckForTransactionUpdate]=> {ex.Message} | {JsonConvert.SerializeObject(ex.InnerException)} | [requestId]=>{requestId}");
                return _responeHandler.HandleException(requestId);
            }
        }

        public ResponseParam GetClientTransactions(GetTransactionReq req)
        {
            var requestId = Helper.GenerateUniqueId(7);
            try
            {
                var response = _responeHandler.InitializeResponse(requestId);
                var transactions = _unitOfWork.Transactions.GetTransactionsByClientId(req?.ClientId);
                _unitOfWork.Dispose();

                if (transactions.Count > 0)
                {
                    response = _responeHandler.CommitResponse(requestId, ResponseCodes.SUCCESS, "Success! transactions retrieved.", transactions);
                }
                else
                {
                    response = _responeHandler.CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Sorry, we could not find any transaction for the supplied client", transactions);
                }

                _logger.LogInfo($"[TransactionService][GetClientTransactions]=> {JsonConvert.SerializeObject(response)} | [requestId]=> {requestId}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[TransactionService][GetClientTransactions]=> {ex.Message} | {ex.InnerException} | [requestId]=>{requestId}");
                return _responeHandler.HandleException(requestId);
            }
        }

        public ResponseParam RetrieveAllTransactions()
        {
            var requestId = Helper.GenerateUniqueId(7);
            try
            {
                var response = _responeHandler.InitializeResponse(requestId);
                var transactions = _unitOfWork.Transactions.GetAllTransactions();
                _unitOfWork.Dispose();

                if(transactions.Count > 0)
                {
                    response = _responeHandler.CommitResponse(requestId, ResponseCodes.SUCCESS, "Success! transactions retrieved.", transactions);
                }
                else
                {
                    response = _responeHandler.CommitResponse(requestId, ResponseCodes.UNSUCCESSFUL, "Sorry, we could not find any transaction", transactions);
                }

                _logger.LogInfo($"[TransactionService][RetrieveTransactions]=> {JsonConvert.SerializeObject(response)} | [requestId]=> {requestId}");
                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError($"[TransactionService][RetrieveTransactions]=> {ex.Message} | {ex.InnerException} | [requestId]=>{requestId}");
                return _responeHandler.HandleException(requestId);
            }
        }
    }
}
