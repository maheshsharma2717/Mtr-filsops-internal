using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? SquareId { get; set; }
        public double? Amount { get; set; }
        public string? SecretKey { get; set; }
        public string? IntentId { get; set; }
        public string Currency { get; set; }
        public int? UserId { get; set; }
        public string? RequestId { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public bool IsStripe { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? DomainId { get; set; }
    }
}
