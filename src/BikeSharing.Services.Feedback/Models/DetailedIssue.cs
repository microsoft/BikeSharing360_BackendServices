using System;

namespace BikeSharing.Services.Feedback.Api.StdHost.Models
{
    public class DetailedIssue
    {
        public int IssueId { get; set; }
        public string IssueType { get; set; }
        public string IssueTitle { get; set; }
        public string IssueDescription { get; set; }
        public bool IssueSolved { get; set; }
        public DateTime IssueDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public int BikeId { get; set; }
        public string BikeSerialNumber { get; set; }
        public string RideType { get; set; }
        public int RideDuration { get; set; }
        public DateTime RideStart { get; set; }
        public DateTime RideStop { get; set; }
        public string RideFrom { get; set; }
        public string RideTo { get; set; }
    }
}
