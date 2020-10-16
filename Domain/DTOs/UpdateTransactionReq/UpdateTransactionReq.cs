using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTOs.UpdateTransactionReq
{
    public class UpdateTransactionReq
    {
        [Required]
        public string RequestId { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string WalletAddress { get; set; }
        
        public string CurrencyType { get; set; }
    }
}
