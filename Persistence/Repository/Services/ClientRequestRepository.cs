using Persistence.Entities;
using Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Services
{
    public class ClientRequestRepository : Repository<ClientRequest>, IClientRequestRepository
    {
        public ClientRequestRepository(DatabaseContext dbContext) : base(dbContext)
        {

        }

        public ClientRequest CheckClientRequestExist(string requestId)
        {
            var request = FirstOrDefault(req=> req.RequestReference == requestId);
            return request;
        }

        public async Task SaveClientRequest(ClientRequest request)
        {
           await AddRecord(request);
            
        }
    }
  
}
