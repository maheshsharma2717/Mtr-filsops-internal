using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Fieldo_WorkerTasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int AssignedBy { get; set; }
        public int? AssignedByUserId { get; set; }
        public string? AssignedByUserName { get; set; }
        public int AssignedTo { get; set; }
        public string TaskStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDated { get; set; }

        [ForeignKey(nameof(TaskId))]
        public Fieldo_Task Task { get; set; }

        [ForeignKey(nameof(AssignedBy))]
        public Fieldo_UserDetails UserDetailsAssignedBy { get; set; }

        [ForeignKey(nameof(AssignedTo))]
        public Fieldo_UserDetails UserDetailsAssignedTo { get; set; }
        public int? DomainId { get; set; }

    }
}
