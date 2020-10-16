using Domain.DTOs.DummyTransactions;
using Domain.DTOs.UpdateTransactionReq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities;

namespace AppCore.Dummy
{
    public class TransactionGenerator
    {

        public static string GenerateDummyTransaction(UpdateTransactionReq updateReq)
        {
            //generate a random number
            Random random = new Random();
            int txnCount = random.Next(1, 3);

            var dummyTxn = new DummyTxnDTO();

            if (txnCount % 2 == 0)
            {
                //Return a static/duplicate transaction
                dummyTxn = GenerateStaticTxn(updateReq);
            }
            else
            {
                //Return a new transaction
                var transactionRef = $"TID{Helper.GenerateUniqueId(7)}";

                dummyTxn = new DummyTxnDTO()
                {
                    ClientId = updateReq.ClientId,
                    WalletAddress = updateReq.WalletAddress,
                    Amount = random.Next(100, 100000),
                    TransactionRef = transactionRef,
                    Date = DateTime.Now,
                    Status = "success"
                };
            }
            return JsonConvert.SerializeObject(dummyTxn);
        }


        private static DummyTxnDTO GenerateStaticTxn(UpdateTransactionReq updateReq)
        {
            var clientId = updateReq.ClientId;
            var staticDigit = $"2{clientId.Length}";
            var walletAddress = updateReq.WalletAddress;

            var amount = Convert.ToDecimal(staticDigit);

            var transactionRef = $"TID{walletAddress?.Length}{walletAddress.Substring(0, 4)}";

            return new DummyTxnDTO()
            {
                ClientId = clientId,
                WalletAddress = walletAddress,
                Amount = amount,
                TransactionRef = transactionRef,
                Date = new DateTime(2020, 10, 16, 8, 0, 0),
                Status = "success"
            };
        }
    }
}
