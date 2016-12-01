using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeSharing.Services.Feedback.Api.Queries;
using BikeSharing.Services.Core.Controllers;
using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Feedback.Api.StdHost.Commands;
using BikeSharing.Services.Feedback.Api.StdHost.Requests;
using System.Linq;
using BikeSharing.Services.Feedback.Api.StdHost.Models;
using BikeSharing.Services.Feedback.Api.Models;
using System;
using BikeSharing.Services.Feedback.Api.StdHost.Responses;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace BikeSharing.Services.Feedback.Api.Controllers
{
    [Route("api/[controller]")]
    public class LastRidesIssuesController : BaseApiController
    {
        private readonly IIssuesQueries _issuesQueries;
        private readonly IRidesQueries _ridesQueries;
        private readonly IUserQueries _userQueries;
        public LastRidesIssuesController(ICommandBus bus, IIssuesQueries issuesQueries, IRidesQueries ridesQueries, 
            IUserQueries userQueries) : base(bus)
        {
            _ridesQueries = ridesQueries;
            _issuesQueries = issuesQueries;
            _userQueries = userQueries;
        }

        [HttpGet()]
        public async Task<IActionResult> Get(int from = 0, int size = 20)
        {
            var issues = (await _issuesQueries.GetTopLastIssues(from, size)).ToList();

            var userIds = issues.Select(i => i.UserId).Distinct();
            var users = await _userQueries.GetUsersById(userIds);

            var ridesRequest = issues.Select(i => new RidesByUserBike { UserId = i.UserId, BikeId = i.BikeId }).Distinct();
            var rides = await _ridesQueries.GetRidesByUserAndBike(ridesRequest);

            var userById = users.ToDictionary(i => i.UserId, u => u);
            var ridesByUserAndBike = 
                rides.GroupBy(r => new { BikeId = r.Bike.BikeId, UserId = r.UserId })
                .ToDictionary(x => $"{x.Key.BikeId}-{x.Key.UserId}", x => x.First());
            
            var issuesByKey = 
                issues
                .GroupBy(i => new { BikeId = i.BikeId, UserId = i.UserId })
                .ToDictionary(x => $"{x.Key.BikeId}-{x.Key.UserId}", x=> x.First());

            SetCountPaginationHeader(issuesByKey.Count());
            var response = issuesByKey
                .Select((keypair) =>
                {
                    var issue = keypair.Value;
                    var key = keypair.Key;
                    var user = userById.ContainsKey(issue.UserId) ? userById[issue.UserId] : null;
                    var ride = ridesByUserAndBike.ContainsKey(key) ? ridesByUserAndBike[key] : null;
                    return MapIssue(issue, user, ride);
                });

            return Ok(response);
        }

        private void SetCountPaginationHeader(int count)
        {
            var countHeaderValue = new StringValues(count.ToString());
            var header = new KeyValuePair<string, StringValues> ("total", countHeaderValue);

            Response.Headers.Add(header);
        }

        private DetailedIssue MapIssue(ReportedIssue issue, User user, RidesResponse ride)
        {
            return new DetailedIssue
            {
                IssueId = issue.Id,
                IssueDate = issue.UtcTime,
                IssueTitle = issue.Title,
                IssueDescription = issue.Description,
                IssueSolved = issue.Solved,
                IssueType = issue.IssueType.ToString(),
                UserId = issue.UserId,
                UserName = user?.GetFullName(),
                UserEmail = user?.Email,
                BikeId = issue.BikeId,
                BikeSerialNumber = ride?.Bike?.SerialNumber,
                RideDuration = ride?.Duration ?? 0,
                RideFrom = ride?.From,
                RideTo = ride?.To,
                RideStart = ride?.Start ?? DateTime.UtcNow,
                RideStop = ride?.Stop ?? DateTime.UtcNow,
                RideType = ride?.RideType
            };
        }
    }
}
