using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Utilities.Binders;

namespace EventReciever
{
    class ClientChannel
    {
        public static IModel InitClientBroadcastChannel(IConnection _connection, BrokerConfig config)
        {
            // create channel
           IModel  _channel = _connection.CreateModel();

            //declare exchange to be used for message delivery
            _channel.ExchangeDeclare(config.Server.Exchange, ExchangeType.Fanout);

            //declare queue where message will be sent
            _channel.QueueDeclare(config.ClientQueue.QueueId, false, false, false, null);

            //Bind queue to use defined exchange
            _channel.QueueBind(config.ClientQueue.QueueId, config.Server.Exchange, config.ClientQueue.RoutingKey, null);
            _channel.BasicQos(0, config.ClientQueue.MaxQueueCount, false);

            return _channel;
        }

    }
}
