using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.StdHost.Responses
{
    public class StationResponse
    {
        public int Id { get;  set; }
        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
