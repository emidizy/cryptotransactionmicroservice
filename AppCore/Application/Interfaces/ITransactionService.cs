using Domain.DTOs.GetTransactionReq;
using Domain.DTOs.UpdateTransactionReq;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<ResponseParam> CheckForTransactionUpdate(UpdateTransactionReq updateReq);
        ResponseParam RetrieveAllTransactions();
        ResponseParam GetClientTransactions(GetTransactionReq clientId);
    }
}
