using Microsoft.EntityFrameworkCore;
using BikeSharing.Services.Profiles.Data;
using BikeSharing.Services.Profiles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BikeSharing.Services.Core.Commands;

namespace BikeSharing.Services.Profiles.Commands
{
    public class GrantAccessCommandHandler : ICommandHandler<GrantAccessCommand>
    {
        private readonly ProfilesDbContext _db;
        public GrantAccessCommandHandler(ProfilesDbContext db)
        {
            _db = db;
        }
        public CommandHandlerResult Handle(GrantAccessCommand command)
        {
            var existing = _db.Users.
                Include(u=>u.Tenant).
                Include(u => u.Profile).
                SingleOrDefault(p => p.UserName == command.Data.UserName);

            if (existing != null)
            {
                var result = new GrantAccessResult()
                {
                    UserId = existing.Id,
                    ProfileId = existing.Profile.Id,
                    TenantId = existing.TenantId,
                    TenantName = existing.Tenant.Name,
                    AccessToken = new BikeSharing.Security.TokenGenerator().CreateJwtToken(existing.Id, existing.Profile.Id, "edu@edu.edu")
                };
                return new CommandHandlerResult(HttpStatusCode.OK, result);
            }
            else
            {
                return new CommandHandlerResult(HttpStatusCode.Forbidden,
                    new
                    {
                        Error = new[] { "Invalid credentials" }
                    });
            }
        }
    }
}
