using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Profiles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Commands
{
    public class GrantAccessCommand : CommandBase
    {

        public LoginModel Data { get; }
        public GrantAccessCommand(LoginModel data)
        {
            Data = data;
        }

        protected override IEnumerable<string> OnValidation()
        {
            if (Data.GrantType != LoginModel.PasswordGrantType)
            {
                yield return $"Invalid grantType: {Data.GrantType}";
            }
        }
    }
}
