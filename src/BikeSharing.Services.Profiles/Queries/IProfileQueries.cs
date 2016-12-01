using BikeSharing.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Queries
{
    public interface IProfileQueries
    {
        UserProfile GetByUserId(int userid);

        UserProfile GetByFaceProfileId(Guid faceProfileId);
        UserProfile GetByMobile(string value);
        UserProfile GetByEmail(string value);
        UserProfile GetBySkype(string value);
    }
}
