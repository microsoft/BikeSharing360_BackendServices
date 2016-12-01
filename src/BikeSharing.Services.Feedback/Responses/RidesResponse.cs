using System;

namespace BikeSharing.Services.Feedback.Api.StdHost.Responses
{
    public class RidesResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RideType { get; set; }
        public int Duration { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public RideBikeResponse Bike { get; set; }
    }
}
