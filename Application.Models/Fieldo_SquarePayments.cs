using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_SquarePayments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? SquareId { get; set; }
        public int TaskId { get; set; }
        public long? Amount { get; set; }
        public string? SecretKey { get; set; }
        public string Currency { get; set; }
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey(nameof(TaskId))]
        public Fieldo_Task Task { get; set; }
        public int? DomainId { get; set; }
    }
}
