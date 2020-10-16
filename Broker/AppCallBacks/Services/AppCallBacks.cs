using EventReciever.AppCallBacks.Interfaces;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using AppCore.Application.Interfaces;
using Newtonsoft.Json;
using Domain.DTOs.UpdateTransactionReq;
using System.Threading.Tasks;
using EventReciever.Events;

namespace EventReciever.AppCallBacks.Services
{
    public class AppCallBacks : IAppCallBacks
    {
        private readonly ITransactionService _transactionSvc;

        public AppCallBacks(ITransactionService txSvc)
        {
            _transactionSvc = txSvc;
        }

        public void ExecuteAction(string recievedPayload, BasicDeliverEventArgs eventProperties)
        {
            var header = eventProperties.BasicProperties.Headers;
            dynamic eventIdByteValue;
            header.TryGetValue("eventId", out eventIdByteValue);
            if (eventIdByteValue == null) return;
            string eventId = Encoding.UTF8.GetString(eventIdByteValue);

            // Determine action from event ID
            switch (eventId)
            {
                case BrokerEvents.UpdateTransaction:
                    //Query Transactions
                    var updateReq = JsonConvert.DeserializeObject<UpdateTransactionReq>(recievedPayload);
                    _transactionSvc.CheckForTransactionUpdate(updateReq);
                    break;
            }
        }
    }
}
