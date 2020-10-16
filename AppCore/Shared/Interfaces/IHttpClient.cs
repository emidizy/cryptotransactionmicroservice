using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Shared.Interfaces
{
    public interface IHttpClient
    {
        Task<string> Post(string parameter, string url, string requestId, string header = null, string token = null);
        
    }
}
