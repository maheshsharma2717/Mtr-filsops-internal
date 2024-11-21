using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? TaskId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string? NotificationImage { get; set; }
        public int? DomainId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(TaskId))]
        public Fieldo_Task Task { get; set; }
        [ForeignKey(nameof(UserId))]
        public Fieldo_UserDetails User { get; set; }
    }
}
