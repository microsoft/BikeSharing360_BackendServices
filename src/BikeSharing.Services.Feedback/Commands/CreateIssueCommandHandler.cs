using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Feedback.Api.Data;
using BikeSharing.Services.Feedback.Api.Models;
using BikeSharing.Services.Feedback.Api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.StdHost.Commands
{
    public class CreateIssueCommandHandler : ICommandHandler<CreateIssueCommand>
    {
        private readonly FeedbackDbContext _db;
        private readonly IRidesQueries _ridesQueries;
        public CreateIssueCommandHandler(FeedbackDbContext db, IRidesQueries ridesQueries)
        {
            _db = db;
            _ridesQueries = ridesQueries;
        }

        public CommandHandlerResult Handle(CreateIssueCommand command)
        {

            var request = command.Request;
            var issueToAdd = new ReportedIssue()
            {
                BikeId = request.BikeId,
                Description = request.Description,
                Solved = false,
                UtcTime = DateTime.UtcNow,
                IssueType = request.Type,
                Title = request.Title
            };

            if (request.StopId.HasValue)
            {
                issueToAdd.StopId = request.StopId.Value;
                FillLatitudeAndLongitude(issueToAdd);
            }
            else
            {
                issueToAdd.Latitude = request.Latitude.Value;
                issueToAdd.Longitude = request.Longitude.Value;
            }

            issueToAdd.UserId = request.UserId;

            _db.Issues.Add(issueToAdd);


            return CommandHandlerResult.Ok;
        }

        private void FillLatitudeAndLongitude(ReportedIssue issueToAdd)
        {
            var stationResponse = _ridesQueries.GetStationAsync(issueToAdd.StopId.Value).Result;

            if (stationResponse != null)
            {
                issueToAdd.Latitude = stationResponse.Latitude;
                issueToAdd.Longitude = stationResponse.Longitude;
            }
            else
            {
                issueToAdd.Latitude = 0;
                issueToAdd.Longitude = 0;
            }
            

        }
    }
}
