using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_TaskStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UpdatedBy { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public Fieldo_UserDetails UserDetail { get; set; }

        [ForeignKey(nameof(TaskId))]
        public Fieldo_Task TaskDetail { get; set; }
        public int? DomainId { get; set; }
    }
}
