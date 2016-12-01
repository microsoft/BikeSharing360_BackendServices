using BikeSharing.Services.Feedback.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.Queries
{
    public interface IIssuesQueries
    {
        Task<IEnumerable<ReportedIssue>> GetAssignedToUserAsync(string userid);
        Task<ReportedIssue> GetById(int id);
        IEnumerable<ReportedIssue> GetTopByBikeId(int top, int bikeid);
        Task<IEnumerable<ReportedIssue>> GetIssuesByUserId(int userid);
        Task<IEnumerable<ReportedIssue>> GetTopLastIssues(int skip, int take);
    }
}
