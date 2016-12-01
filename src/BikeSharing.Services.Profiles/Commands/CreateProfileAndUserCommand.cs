using BikeSharing.Models.Profiles;
using BikeSharing.Services.Core.Commands;
using MyBikes.Services.Profiles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Commands
{
    public class CreateProfileAndUserCommand : CommandBase
    {
        public UserProfile Profile { get; }
        public User User { get; }

        private readonly UserAndProfileModel _model;


        public CreateProfileAndUserCommand(UserAndProfileModel model)
        {
            _model = model;
            Profile = CreateProfileFromModel(model);
            User = CreateUserFromModel(model);

            Profile.User = User;
            User.Profile = Profile;
        }

        private User CreateUserFromModel(UserAndProfileModel model)
        {
            var user = new User()
            {
                Password = model.Password,
                UserName = model.UserName,
                TenantId = model.TenantId
            };

            return user;
        }

        private UserProfile CreateProfileFromModel(UserAndProfileModel model)
        {
            var profile = new UserProfile()
            {
                BirthDate = model.BirthDate,
                Email = model.Email,
                FirstName = model.FirstName,
                Gender = model.Gender,
                LastName = model.LastName,
                Skype = model.Skype
            };

            return profile;
        }

        protected override IEnumerable<string> OnValidation()
        {
            if (_model == null)
            {
                yield return "invalid or no payload received";
            }
        }
    }
}
