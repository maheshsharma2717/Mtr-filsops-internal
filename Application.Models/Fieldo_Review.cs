using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    [NotMapped]
    public class Fieldo_Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; } // User who is providing the rating
        public int ProviderId { get; set; } // ID of the service provider being rated
        public int TaskId { get; set; }
        public int? Rating { get; set; }
        public string ReviewText { get; set; }
        public int HelpfulCount { get; set; }
        public bool Flagged { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public Fieldo_UserDetails ServiceRequestCreatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public Fieldo_UserDetails ServiceRequestUpdatedBy { get; set; }
        [ForeignKey(nameof(UserId))]
        public Fieldo_UserDetails RatingUser { get; set; }
        [ForeignKey(nameof(ProviderId))]
        public Fieldo_UserDetails ProviderUser { get; set; }
        [ForeignKey(nameof(TaskId))]
        public Fieldo_Task ServiceRequest { get; set; }
        public int? DomainId { get; set; }
    }
}
