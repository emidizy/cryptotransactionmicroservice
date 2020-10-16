using Persistence.Entities;
using Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository.Services
{
    public class TransactionRepository : Repository<Transactions>, ITransactionRepository
    {
        public TransactionRepository(DatabaseContext dbContext) : base(dbContext)
        {

        }

        public async Task AddTransaction(Transactions transaction)
        {
            await AddRecord(transaction);
        }

        public List<Transactions> GetTransactionsByClientId(string clientId)
        {
            var transactions = Find(tx=> tx.ClientId == clientId)?.ToList();
            return transactions;
        }

        public List<Transactions> GetAllTransactions()
        {
            var transactions = GetAllRecords().ToList();
            return transactions;
        }

        public Transactions GetTransaction(string transactionRef)
        {
            var transaction = FirstOrDefault(tx => tx.TransactionRef == transactionRef);

            return transaction;
        }
    }
}
