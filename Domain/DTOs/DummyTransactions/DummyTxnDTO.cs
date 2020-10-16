using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs.DummyTransactions
{
    public class DummyTxnDTO
    {
        public string ClientId { get; set; }
        public string WalletAddress { get; set; }
        public decimal Amount { get; set; }
        public string TransactionRef { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
