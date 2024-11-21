using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_WalletTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? WalletId { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public int? TaskId { get; set; }
        public string TransactionType { get; set; } // e.g., Credit, Debit
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey(nameof(TaskId))]
        public Fieldo_Task Task { get; set; }
        [ForeignKey("WalletId")]
        public Fieldo_Wallet Wallet { get; set; }
        public int? DomainId { get; set; }
    }
}
