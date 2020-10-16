using AppCore.Application.Interfaces;
using AppCore.Application.Services;
using AppCore.Shared.Interfaces;
using AppCore.Shared.Services;
using Broker.Clients.Services;
using EventPublisher;
using EventPublisher.Clients.Interfaces;
using EventPublisher.Clients.Services;
using EventReciever;
using EventReciever.AppCallBacks.Interfaces;
using EventReciever.AppCallBacks.Services;
using EventReciever.Clients.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository.Interfaces;
using Persistence.Repository.Services;
using Persistence.UnitOfWork.Interfaces;
using Persistence.UnitOfWork.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace TransactionMicroService.ServiceRegistry
{
    public static class AppServiceCollection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<Logger>();
            
            //Register Db services
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IClientRequestRepository, ClientRequestRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            //Register Core services
            services.AddTransient<IRestResponse, RestResponse>();
            services.AddTransient<IHttpClient, HttpClient>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IResponseHandler, ResponseHandler>();

            //Register Broker services
            services.AddScoped<IAppCallBacks, AppCallBacks>();
            services.AddSingleton<IBroadcaster, Broadcaster>();
            services.AddSingleton<IReceiver, Receiver>();
            services.AddHostedService<EventRecieverDaemon>();
            services.AddHostedService<EventPublisherDaemon>();
            

            return services;
        }
    }
}
