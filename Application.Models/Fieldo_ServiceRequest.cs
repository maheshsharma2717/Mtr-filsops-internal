using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;

namespace Application.Models
{
    public class Fieldo_ServiceRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Documents { get; set; }
        public Decimal? OfferPrice { get; set; }
        public RequestStatus? Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? AssignedTo { get; set; }
        public int? AssignedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public Fieldo_UserDetails ServiceRequestCreatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public Fieldo_UserDetails ServiceRequestUpdatedBy { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Fieldo_RequestCategory RequestCategoryCategoryId { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public Fieldo_UserDetails? UserDetailsAssignedTo { get; set; }

        [ForeignKey(nameof(AssignedBy))]
        public Fieldo_UserDetails? UserDetailsAssignedBy { get; set; }
        public int? DomainId { get; set; }
    }
}
