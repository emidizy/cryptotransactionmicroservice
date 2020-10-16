using EventReciever;
using EventReciever.AppCallBacks.Interfaces;
using EventReciever.Clients.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;

namespace Broker.Clients.Services
{
    public class Receiver : IReceiver
    {
        private IModel _clientChannel;
        private readonly Logger _logger;
        private IConnection _connection;
        private readonly IServiceProvider _brokerSvcProvider;
        private readonly BrokerConfig _brokerConfig;

        public Receiver(ILogger<EventRecieverDaemon> loggerFactory,
            IServiceProvider serviceProvider,
            IOptions<BrokerConfig> brokerConfig,
            Logger logger)
        {
            _logger = logger;
            _brokerSvcProvider = serviceProvider;
            _brokerConfig = brokerConfig.Value;
        }

        public void InitRabbitMQEventReciever(IConnection connection)
        {
            _connection = connection;
            // Initialize channel(s)
            _clientChannel = ClientChannel.InitClientBroadcastChannel(_connection, _brokerConfig);

            //Set custom event to be triggered on connection shutdown
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

        }

        public Task StartListeningForPublishedPayload()
        {
            //Listen for incoming payload on declared channel(s) here
            ListenForPayloadOnChannel(_clientChannel, _brokerConfig.ClientQueue.QueueId);
            return Task.CompletedTask;
        }

        private void ListenForPayloadOnChannel(IModel _channel, string queue)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                try
                {
                    // received message
                    var content = System.Text.Encoding.UTF8.GetString(ea.Body);

                    // handle the received message
                    HandleMessage(content, ea);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{_brokerConfig.ServiceName} - RabbitMQ: Exception while recieving payload with tag {ea.DeliveryTag} => {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, true, true);
                }
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(queue, false, consumer);
            //_logger.LogInformation("{serviceId} - RabbitMQ: Actively listening for incoming payload");
            _logger.LogInfo($"{_brokerConfig.ServiceName} - RabbitMQ: Actively listening for incoming payload on {queue}");
        }

        private void HandleMessage(string content, BasicDeliverEventArgs eventProperties)
        {
            //Log recieved payload
            _logger.LogInfo($"{_brokerConfig.ServiceName} - RabbitMQ: received {content}");

            using (var scope = _brokerSvcProvider.CreateScope())
            {

                //Manage scope inside a singleton background service
                var _dispatcher = scope.ServiceProvider.GetRequiredService<IAppCallBacks>();
                _dispatcher.ExecuteAction(content, eventProperties);
            }

        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInfo($"{_brokerConfig.ServiceName} - connection to RabbitMQ shut down {e.ReplyText}");
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            _logger.LogInfo($"{_brokerConfig.ServiceName} - RabbitMQ: consumer {e.ConsumerTag} connection has been cancelled");
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInfo($"{_brokerConfig.ServiceName} - RabbitMQ: consumer {e.ConsumerTag} unregistered");
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            _logger.LogInfo($"{_brokerConfig.ServiceName} - RabbitMQ: consumer {e.ConsumerTag} registered");
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInfo($"{_brokerConfig.ServiceName} - RabbitMQ: consumer {e.ReplyText} shutdown");
        }

        public void DisposeConnection()
        {
            _logger.LogInfo($"{_brokerConfig.ServiceName} - Closing RabbitMQ connection...");
            _clientChannel.Close();
            if (_connection.IsOpen)
            {
                _connection.Close();
            }
        }
    }
}
