using BikeSharing.Models.Profiles;
using BikeSharing.Services.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Commands
{
    public class SetPaymentDataCommand : CommandBase
    {
        public int UserId { get;  }
        public PaymentData Data { get; }
        public SetPaymentDataCommand(int userid, PaymentData data)
        {
            UserId = userid;
            Data = data;
        }

        protected override IEnumerable<string> OnValidation()
        {
            if (string.IsNullOrEmpty(Data.CreditCard))
            {
                yield return "Missing CC number";
            }

            if (Data.CreditCardType == CreditCardType.None)
            {
                yield return "Missing Credit Card Type";
            }

            if (Data.ExpirationDate.ToUniversalTime() < DateTime.UtcNow)
            {
                yield return "Invalid Expiration Date";
            }
        }

    }
}
