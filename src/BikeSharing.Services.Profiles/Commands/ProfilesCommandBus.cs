using Autofac;
using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Profiles.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Commands
{
    public class ProfilesCommandBus : CommandBus<ProfilesDbContext>
    {
        public ProfilesCommandBus(ILifetimeScope scope) : base(scope)
        {
        }
    }
}
