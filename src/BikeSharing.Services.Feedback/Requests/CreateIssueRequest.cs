using BikeSharing.Services.Feedback.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.StdHost.Requests
{
    public class CreateIssueRequest
    {
        public int UserId { get; set; }
        public ReportedIssueType Type { get; set; }
        public int BikeId { get; set; }
        public int? StopId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}
