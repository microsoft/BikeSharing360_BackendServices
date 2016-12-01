using BikeSharing.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBikes.Services.Profiles.Models
{
    public class UserAndProfileModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public int TenantId { get; set; }

        public string Skype { get; set; }

    }
}
