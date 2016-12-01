using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Credentials { get; set; }

        public string GrantType { get; set; }
        public const string PasswordGrantType = "password";
    }
}
