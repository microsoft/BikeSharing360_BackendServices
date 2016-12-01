using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeSharing.Services.Feedback.Api.Models;
using BikeSharing.Services.Feedback.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace BikeSharing.Services.Feedback.Api.Queries
{
    public class IssuesQueries : IIssuesQueries
    {
        private readonly FeedbackDbContext _ctx;
        private readonly IRidesQueries _ridesQueries;

        public IssuesQueries(FeedbackDbContext ctx, IRidesQueries ridesQueries)
        {
            _ctx = ctx;
            _ridesQueries = ridesQueries;
        }

        public async Task<IEnumerable<ReportedIssue>> GetAssignedToUserAsync(string userid)
        {
            var data = await _ctx.Issues.ToListAsync();
            return data;
        }


        public async Task<IEnumerable<ReportedIssue>> GetIssuesByUserId(int userid)
        {
            var data = await _ctx.Issues.Where(i => i.UserId == userid).ToListAsync();
            return data;
        }

        public async Task<ReportedIssue> GetById(int id)
        {
            var issue = await _ctx.Issues.SingleOrDefaultAsync(i => i.Id == id);
            return issue;
        }

        public IEnumerable<ReportedIssue> GetTopByBikeId(int top, int bikeid)
        {
            var issues = _ctx.Issues.Where(i => i.BikeId == bikeid).
                OrderByDescending(i => i.UtcTime).
                Take(top);

            return issues.AsEnumerable();
        }

        public Task<IEnumerable<ReportedIssue>> GetTopLastIssues(int skip, int take)
        {
            var issues = _ctx.Issues
                .OrderByDescending(i => i.UtcTime).
                Skip(skip).
                Take(take);

            return Task.FromResult(issues.AsEnumerable());
        }
    }
}
