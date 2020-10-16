using Persistence.Repository.Interfaces;
using Persistence.Repository.Services;
using Persistence.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.UnitOfWork.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext _dbContext;
        public ITransactionRepository Transactions { get; private set; }
        public IClientRequestRepository ClientRequests { get; private set; }

        public UnitOfWork(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            Transactions = new TransactionRepository(dbContext);
            ClientRequests = new ClientRequestRepository(dbContext);
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
