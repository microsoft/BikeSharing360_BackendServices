using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Profiles.Commands;
using BikeSharing.Services.Profiles.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBikes.Services.Profiles.Commands
{
    public class AddSubscriptionCommandHandler : ICommandHandler<AddSubscriptionCommand>
    {
        private readonly ProfilesDbContext _db;
        public AddSubscriptionCommandHandler(ProfilesDbContext db)
        {
            _db = db;
        }

        public CommandHandlerResult Handle(AddSubscriptionCommand command)
        {
            var existing = _db.Users.SingleOrDefault(u => u.Id == command.UserId);
            if (existing == null)
            {
                return CommandHandlerResult.Error($"User {command.UserId} not found");
            }

            existing.Subscriptions.Add(command.Subscription);
            return CommandHandlerResult.Ok;
        }
    }
}
