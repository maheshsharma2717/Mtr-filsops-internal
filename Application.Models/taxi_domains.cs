using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class taxi_domains
    {
        [Key]
        public int id { get; set; }
        public string DomainName { get; set; }
    }
}
