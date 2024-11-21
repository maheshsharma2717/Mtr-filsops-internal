using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.Common;


namespace Application.Models
{
    public class Fieldo_Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RequestStatus? ViewStatus { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
        public string Address { get; set; }
        public string? Documents { get; set; }
        public int CreatedBy { get; set; }
        public int? CreatedByAdminUserId { get; set; }
        public bool? IsTaskCreatedByAdmin { get; set; }
        public string? CreatedByAdminUserName { get; set; }
        public AdminUserType? CreatedByAdminUserType { get; set; }

        public int? CancelledBy { get; set; }
        public int? CancelledByAdminUserId { get; set; }
        public bool? IsTaskCancelledByAdmin { get; set; }
        public string? CancelledByAdminUserName { get; set; }
        public AdminUserType? CancelledByAdminUserType { get; set; }

        public int? ViewedBy { get; set; }
        public int? ViewedByUserId { get; set; }
        public string? ViewedByUserName { get; set; }
        public AdminUserType? ViewedByAdminUserType { get; set; }
        public int? AssignedBy { get; set; }
        public int? AssignedByUserId { get; set; }
        public string? AssignedByUserName { get; set; }
        public AdminUserType? AssignedByAdminUserType { get; set; }
        public int? UpdatedBy { get; set; }
        public int? AssignedTo { get; set; }
        public bool? IsViewed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ViewedTime { get; set; }
        public DateTime? AssignedTime { get; set; }
        public DateTime? CancellationDateTime { get; set; }
        public DateTime? WorkStartTime { get; set; }
        public DateTime? WorkCompleteTime { get; set; }
        public long? Amount { get; set; }
        public string? currency { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? PaymentDateTime { get; set; }

        //status updated time.
        public DateTime UpdatedAt { get; set; }
        public DateTime? AssignedAt { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public Fieldo_UserDetails? UserDetailsUpdatedBy { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public Fieldo_UserDetails? UserDetailsAssignedTo { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public Fieldo_UserDetails? UserDetailsCreatedBy { get; set; }
        [ForeignKey(nameof(ViewedBy))]
        public Fieldo_UserDetails? UserDetailsViewedBy { get; set; }

        [ForeignKey(nameof(AssignedBy))]
        public Fieldo_UserDetails? UserDetailsAssingedBy { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Fieldo_RequestCategory TaskCategory{ get; set; }
        [ForeignKey(nameof(CancelledBy))]
        public Fieldo_RequestCategory UserDetailsCancelledBy { get; set; }
        public int? DomainId { get; set; }
    }
}
