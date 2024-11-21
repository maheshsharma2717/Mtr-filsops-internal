using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SendBy { get; set; }
        public int SendTo { get; set; }
        public string Message { get; set; }
        public DateTime SendTime { get; set; }
        public bool IsReceived { get; set; }
        public int? DomainId { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey(nameof(SendBy))]
        public Fieldo_UserDetails UserDetailsSendBy { get; set; }
        [ForeignKey(nameof(SendTo))]
        public Fieldo_UserDetails UserDetailsSendTo { get; set; }

    }
}
