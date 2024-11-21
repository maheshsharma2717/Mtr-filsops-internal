using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_TaskAttachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? TaskStatusId { get; set; }
        public int TaskId { get; set; }
        public int AddedBy { get; set; }
        public string AttachmentUrl { get; set; } = null;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(TaskId))]
        public Fieldo_Task Task { get; set; }

        [ForeignKey(nameof(AddedBy))]
        public Fieldo_UserDetails UserDetails { get; set; }
        public int? DomainId { get; set; }
    }
}
