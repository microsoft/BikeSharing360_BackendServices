using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Profiles.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeSharing.Models.Profiles;

namespace BikeSharing.Services.Profiles.Commands
{
    public class SetPaymentDataCommandHandler : ICommandHandler<SetPaymentDataCommand>
    {

        private readonly ProfilesDbContext _db;
        public SetPaymentDataCommandHandler(ProfilesDbContext db)
        {
            _db = db;
        }

        public CommandHandlerResult Handle(SetPaymentDataCommand command)
        {
            var existing = _db.Profiles.Include(p => p.Payment).
                SingleOrDefault(p => p.UserId == command.UserId);

            if (existing == null)
            {
                return CommandHandlerResult.NotFound("Profile not found");
            }

            var payment = existing.Payment;
            if (payment == null)
            {
                return AddNewPayment(existing, command.Data);
            }

            return UpdatePayment(existing, command.Data);


        }

        private CommandHandlerResult UpdatePayment(UserProfile existing, PaymentData data)
        {
            var current = existing.Payment;
            current.CreditCard = data.CreditCard;
            current.CreditCardType = data.CreditCardType;
            current.ExpirationDate = data.ExpirationDate;

            return CommandHandlerResult.Ok;
        }

        private CommandHandlerResult AddNewPayment(UserProfile existing, PaymentData data)
        {
            existing.Payment = data;
            return CommandHandlerResult.Ok;
        }
    }
}
