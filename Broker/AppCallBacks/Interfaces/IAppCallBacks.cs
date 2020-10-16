using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventReciever.AppCallBacks.Interfaces
{
    public interface IAppCallBacks
    {
        void ExecuteAction(string recievedPayload, BasicDeliverEventArgs eventProperties);
    }
}
