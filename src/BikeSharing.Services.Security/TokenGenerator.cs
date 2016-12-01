using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BikeSharing.Security
{
    public class TokenGenerator
    {
        public string CreateJwtToken(int userid, int profileid, string email)
        { 
            var payload = new Dictionary<string, object>()
                {
                    { JwtNames.Subject, userid},
                    { JwtNames.ProfileId, profileid },
                    { JwtNames.Email, email }
                    
                };
            string token = JWT.Encode(payload, null, JwsAlgorithm.none);

            return token;
        }


        public string DecodeToken(string token)
        {
            var decoded = JWT.Decode(token);
            return decoded;
        }
    }
}