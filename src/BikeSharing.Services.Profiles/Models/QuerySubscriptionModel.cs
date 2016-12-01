namespace BikeSharing.Servicesubscription.Profilesubscription.Models
{
    public class QuerySubscriptionModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string SubscriptionType { get; set; }
        public string SubscriptionStatus { get; set; }
        public string CreditCard { get; set; }
        public string CreditCardType { get; set; }
    }
}
