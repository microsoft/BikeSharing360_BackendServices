using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeSharing.Models.Profiles;
using BikeSharing.Services.Profiles.Data;
using Microsoft.EntityFrameworkCore;

namespace BikeSharing.Services.Profiles.Queries
{
    public class ProfileQueries : IProfileQueries
    {
        private readonly ProfilesDbContext _ctx;
        public ProfileQueries(ProfilesDbContext ctx)
        {
            _ctx = ctx;
        }

        public UserProfile GetByUserId(int userid)
        {
            return _ctx.Profiles.Include(p=>p.Payment).
                SingleOrDefault(p => p.UserId == userid);
        }

        public UserProfile GetByFaceProfileId(Guid faceProfileId)
        {
            return _ctx.Profiles.Include(p => p.Payment)
                .SingleOrDefault(p => p.FaceProfileId.HasValue && p.FaceProfileId.Value == faceProfileId);
        }

        public UserProfile GetBySkype(string value)
        {
            return _ctx.Profiles.FirstOrDefault(p => p.Skype == value);
        }
        public UserProfile GetByEmail(string value)
        {
            return _ctx.Profiles.FirstOrDefault(p => p.Email == value);
        }
        public UserProfile GetByMobile(string value)
        {
            return _ctx.Profiles.FirstOrDefault(p => p.Mobile == value);
        }
    }
}
