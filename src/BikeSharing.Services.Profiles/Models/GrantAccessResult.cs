using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Models
{
    public class GrantAccessResult
    {
        public int UserId { get; set; }
        public int ProfileId { get; set; }

        public string AccessToken { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
    }
}
