using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventReciever.Clients.Interfaces
{
    public interface IReceiver
    {
        void InitRabbitMQEventReciever(IConnection factory);
        Task StartListeningForPublishedPayload();
        void DisposeConnection();
    }
}
