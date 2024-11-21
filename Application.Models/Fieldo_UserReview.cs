using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_UserReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddedBy { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey(nameof(UserId))]
        public Fieldo_UserDetails UserDetails { get; set; }
        [ForeignKey(nameof(AddedBy))]
        public Fieldo_UserDetails UserDetailsAddedBy { get; set; }
        public int? DomainId { get; set; }

    }
}
