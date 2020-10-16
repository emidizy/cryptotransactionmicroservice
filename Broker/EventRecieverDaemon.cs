using EventReciever.Clients.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using Utilities.Binders;

namespace EventReciever
{
    public class EventRecieverDaemon : MessageClient, IHostedService, IDisposable
    {
        private ILogger _logger;
        private IReceiver _eventReciever;
        private readonly BrokerConfig _brokerConfig;

        public EventRecieverDaemon(ILogger<EventRecieverDaemon> loggerFactory, 
            IOptions<BrokerConfig> brokerConfig, 
            IServiceProvider IserviceProvider,
            IReceiver eventReciever,
            Logger logger)
            : 
            base(loggerFactory, IserviceProvider, brokerConfig, eventReciever, logger)
        {
            _logger = loggerFactory;
            _brokerConfig = brokerConfig.Value;
            _eventReciever = eventReciever;
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting reciever daemon: " + _brokerConfig.ServiceName);
            return _eventReciever.StartListeningForPublishedPayload();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping daemon.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _eventReciever.DisposeConnection();
        }
    }
}
