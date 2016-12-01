using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.Models
{
    public class ReportedIssue
    {
        public int Id { get; set; }
        public ReportedIssueType IssueType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? StopId { get; set; }
        public int UserId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public DateTime UtcTime { get; set; }

        public bool Solved { get; set; }
        public int BikeId { get; set; }
    }
}
