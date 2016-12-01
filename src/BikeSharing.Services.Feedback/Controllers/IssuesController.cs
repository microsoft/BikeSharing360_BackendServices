using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeSharing.Services.Feedback.Api.Queries;
using BikeSharing.Services.Feedback.Api.ApiModels;
using BikeSharing.Services.Core.Controllers;
using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Feedback.Api.StdHost.Commands;
using BikeSharing.Services.Feedback.Api.StdHost.Requests;

namespace BikeSharing.Services.Feedback.Api.Controllers
{
    [Route("api/[controller]")]
    public class IssuesController : BaseApiController
    {

        private readonly IIssuesQueries _issuesQueries;
        private readonly IRidesQueries _ridesQueries;
        public IssuesController(ICommandBus bus, IIssuesQueries queries, IRidesQueries ridesQueries ) : base(bus)
        {
            _issuesQueries = queries;
            _ridesQueries = ridesQueries;
        }

        [HttpPost()]
        public IActionResult CreateIssue([FromBody]CreateIssueRequest data)
        {
            if (data == null)
            {
                return BadRequest();
            }

            return ProcessCommand(new CreateIssueCommand(data));
        }

        [HttpGet("to/{userid:alpha}")]
        public async Task<IActionResult> GetIssuesAssignedToUser(string userid)
        {   
            var issues = await _issuesQueries.GetAssignedToUserAsync(userid);
            var apiIssues = issues.ToList().Select(i => IssueApiModel.FromReportedIssue<IssueApiModel>(i));
            return Ok(apiIssues);
        }

        [HttpGet("from/{userid:int}")]
        public async Task<IActionResult> GetIssuesFromUser(int userid)
        {
            var issues = await _issuesQueries.GetIssuesByUserId(userid);
            var apiIssues = issues.ToList().Select(i => IssueApiModel.FromReportedIssue<IssueApiModel>(i));
            return Ok(apiIssues);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetIssueDetails(int id)
        {
            var issue = await _issuesQueries.GetById(id);
            if (issue == null)
            {
                return NotFound($"Issue {id} not found.");
            }

            var apiIssue = IssueApiModel.FromReportedIssue<IssueDetailsApiModel>(issue);
            var extraIssues = _issuesQueries.GetTopByBikeId(3, issue.BikeId);
            extraIssues = extraIssues.Where(i => i.Id != id);
            if (extraIssues.Any())
            {
                apiIssue.addExtraIncidences(extraIssues);
            }

            if (issue.StopId.HasValue)
            {
                var stationResponse = await _ridesQueries.GetStationAsync(issue.StopId.Value);
                if (stationResponse != null)
                {
                    apiIssue.Station = StationApiModel.FromStationResponse(stationResponse);
                }
            }


            var bikeInfo = await _ridesQueries.GetBikeAsync(issue.BikeId);
            if (bikeInfo != null)
            {
                apiIssue.BikeInfo = bikeInfo;
            }

            return Ok(apiIssue);
        }

        [HttpPut("solved/{id:int}")]
        public async Task<IActionResult> PutToSolveIssue(int id)
        {
            var issue = await _issuesQueries.GetById(id);
            if (issue == null)
            {
                return NotFound($"Issue {id} not found.");
            }

            issue.Solved = true;

            return ProcessCommand(new UpdateIssueCommand(issue));
        }

        [HttpDelete("solved/{id:int}")]
        public async Task<IActionResult> DeleteToSolveIssue(int id)
        {
            var issue = await _issuesQueries.GetById(id);
            if (issue == null)
            {
                return NotFound($"Issue {id} not found.");
            }

            issue.Solved = false;

            return ProcessCommand(new UpdateIssueCommand(issue));
        }

        [HttpGet("status/{id:int}")]
        public async Task<IActionResult> CheckStatus(int id)
        {
            var issue = await _issuesQueries.GetById(id);
            if (issue.Solved)
            {
                return Ok(new { Id = issue.Id, Solved = issue.Solved, Title = issue.Title });
            }
            else
            {
                return Ok(new { Id = issue.Id, Solved = issue.Solved,
                    Eta = DateTime.UtcNow.Add(TimeSpan.FromDays(1)).Date,
                    Title = issue.Title });
            }
        }

    }
}
