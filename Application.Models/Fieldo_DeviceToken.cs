using Application.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class Fieldo_DeviceToken
    {
        public int Id { get; set; } // Primary Key

        public int UserID { get; set; } // Foreign Key to the User table
        public int DomainId { get; set; } // Foreign Key to the User table
        public string DeviceToken { get; set; } // The FCM Device Token
        public string DeviceType { get; set; } // Optional: iOS, Android, Web, etc.

        public DateTime LastUpdated { get; set; } // Timestamp for when the token was last updated

        // Optional: Navigation property to User entity (if you have a User model)
        [ForeignKey(nameof(UserID))]
        public Fieldo_UserDetails UserDetailsAddedBy { get; set; }
    }
}
