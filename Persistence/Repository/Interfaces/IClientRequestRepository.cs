using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Interfaces
{
    public interface IClientRequestRepository
    {
        Task SaveClientRequest(ClientRequest request);
        ClientRequest CheckClientRequestExist(string requestId);
    }
}
