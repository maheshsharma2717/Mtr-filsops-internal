using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class taxi_user
    {
        [Key]
        public int usr_id { get; set; }
        public string usr_name { get; set; }
        public string usr_password { get; set; }
        public string usr_email { get; set; }
        public string voip_ext { get; set; }



    }
}
