using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.StdHost.Responses
{
    public class BikeResponse
    {
        public string SerialNumber { get; set; }
        public int TotalDistanceTraveled { get; set; }
        public int TotalIncidences { get; set; }
        public DateTime InCirculationSince { get; set; }
    }
}
