using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.StdHost.Responses
{
    public class RideBikeResponse
    {
        public int BikeId { get; set; }
        public string SerialNumber { get; set; }
        public int? StationId { get; set; }
    }
}
