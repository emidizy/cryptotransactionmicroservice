using Persistence.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        ITransactionRepository Transactions { get; }
        IClientRequestRepository ClientRequests { get; }
        int SaveChanges();
        void Dispose();
    }
}
