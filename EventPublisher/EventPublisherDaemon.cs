using EventPublisher.Clients.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;

namespace EventPublisher
{
    public class EventPublisherDaemon : MessageClient, IHostedService, IDisposable
    {
        private ILogger _logger;
        private IBroadcaster _eventPublisher;
        private readonly BrokerConfig _brokerConfig;

        public EventPublisherDaemon(ILogger<EventPublisherDaemon> loggerFactory, 
            IOptions<BrokerConfig> brokerConfig, 
            IServiceProvider IserviceProvider,
            IBroadcaster eventPublisher,
            Logger logger)
            : 
            base(loggerFactory, IserviceProvider, brokerConfig, eventPublisher, logger)
        {
            _logger = loggerFactory;
            _brokerConfig = brokerConfig.Value;
            _eventPublisher = eventPublisher;
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting reciever daemon: " + _brokerConfig.ServiceName);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping daemon.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _eventPublisher.DisposeConnection();
        }
    }
}
