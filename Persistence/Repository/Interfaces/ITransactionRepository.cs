using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        List<Transactions> GetTransactionsByClientId(string clientId);
        Transactions GetTransaction(string transactionRef);
        Task AddTransaction(Transactions transaction);
        List<Transactions> GetAllTransactions();
    }
}
