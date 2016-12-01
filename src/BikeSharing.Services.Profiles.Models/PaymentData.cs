using System;

namespace BikeSharing.Models.Profiles
{
    public class PaymentData
    {
        public int Id { get; set; }
        public string CreditCard { get; set; }
        public CreditCardType CreditCardType { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}