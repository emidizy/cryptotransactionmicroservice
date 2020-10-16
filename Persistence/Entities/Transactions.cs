using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence.Entities
{
    [Table("Transactions")]
    public class Transactions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "int")]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string ClientId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(2000)")]
        public string WalletAddress { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string CurrencyType { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string TransactionRef { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string TransactionStatus { get; set; }
    }
}
