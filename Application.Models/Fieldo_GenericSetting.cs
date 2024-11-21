using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_GenericSetting
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string SubSettingName { get; set; }

        [StringLength(20)]
        public string DefaultTextValue20_1 { get; set; }
        [StringLength(20)]
        public string DefaultTextValue20_2 { get; set; }

        [StringLength(50)]
        public string DefaultTextValue50_1 { get; set; }
        [StringLength(50)]
        public string DefaultTextValue50_2 { get; set; }

        [StringLength(100)]
        public string DefaultTextValue100_1 { get; set; }
        [StringLength(100)]
        public string DefaultTextValue100_2 { get; set; }

        [StringLength(250)]
        public string DefaultTextValue250_1 { get; set; }
        [StringLength(250)]
        public string DefaultTextValue250_2 { get; set; }
        public string DefaultTextMax { get; set; }
        public string DefaultTextMax1 { get; set; }
        public int DefalutInteger1 { get; set; }
        public int DefalutInteger2 { get; set; }
        public decimal DefaultDecimal1 { get; set; }
        public decimal DefaultDecimal2 { get; set; }
        public DateTime? DefaultDateTime1 { get; set; }
        public DateTime? DefaultDateTime2 { get; set; }
        public bool DefaultBool1 { get; set; }
        public bool DefaultBool2 { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public Fieldo_UserDetails ServiceRequestCreatedBy { get; set; }
        [ForeignKey(nameof(UpdatedBy))]
        public Fieldo_UserDetails ServiceRequestUpdatedBy { get; set; }
        public int? DomainId { get; set; }
    }
}
