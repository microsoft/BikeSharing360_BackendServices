using BikeSharing.Models.Profiles;
using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Profiles.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Commands
{
    public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand>
    {
        private readonly ProfilesDbContext _db;
        public UpdateProfileCommandHandler(ProfilesDbContext db)
        {
            _db = db;
        }
        public CommandHandlerResult Handle(UpdateProfileCommand command)
        {
            var profile = command.Profile;
            var patching = command.IsPatch;
            var existing = _db.Profiles.SingleOrDefault(p => p.UserId == profile.UserId);
            if (existing == null)
            {
                return CommandHandlerResult.Error("no profile found");
            }

            if (!patching)
            {
                existing.BirthDate = profile.BirthDate;
                existing.FirstName = profile.FirstName;
                existing.Gender = profile.Gender;
                existing.LastName = profile.LastName;
                existing.Email = profile.Email;
                if (profile.Skype != null)
                {
                    existing.Skype = profile.Skype;
                }
                if (profile.Mobile != null)
                {
                    existing.Mobile = profile.Mobile;
                }

                return CommandHandlerResult.Ok;
            }
            else return PatchProfile(existing, profile);
        }

        private CommandHandlerResult PatchProfile(UserProfile existing, UserProfile profile)
        {

            if (profile.BirthDate.HasValue)
            {
                existing.BirthDate = profile.BirthDate;
            }
            if (!string.IsNullOrEmpty(profile.FirstName)) {
                existing.FirstName = profile.FirstName;
            }
            if (!string.IsNullOrEmpty(profile.LastName))
            {
                existing.LastName = profile.LastName;
            }
            if (profile.Gender != Gender.NotSpecified)
            {
                existing.Gender = profile.Gender;
            }

            if (!string.IsNullOrEmpty(profile.Email))
            {
                existing.Email = profile.Email;
            }

            if (profile.Skype != null)
            {
                existing.Skype = profile.Skype;
            }

            if (profile.Mobile != null)
            {
                existing.Mobile = profile.Mobile;
            }

            if (profile.VoiceProfileId != null)
            {
                existing.VoiceProfileId = profile.VoiceProfileId;
            }

            if (profile.VoiceSecretPhrase != null)
            {
                existing.VoiceSecretPhrase = profile.VoiceSecretPhrase;
            }

            if (profile.FaceProfileId != null)
            {
                existing.FaceProfileId = profile.FaceProfileId;
            }

            return CommandHandlerResult.Ok;
        }
    }
}
