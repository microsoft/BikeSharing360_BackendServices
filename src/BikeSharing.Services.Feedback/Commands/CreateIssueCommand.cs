using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Feedback.Api.Models;
using BikeSharing.Services.Feedback.Api.StdHost.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.StdHost.Commands
{
    public class CreateIssueCommand : CommandBase
    {
        private readonly CreateIssueRequest _request;

        public CreateIssueRequest Request => _request;

        public CreateIssueCommand(CreateIssueRequest request)
        {
            _request = request;
        }


        protected override IEnumerable<string> OnValidation()
        {
            if (_request.UserId == 0)
            {
                yield return "Must enter userId";
            }

            if (_request.Type == ReportedIssueType.Unknown)
            {
                yield return "Must enter an issueType";
            }

            if (_request.BikeId == 0)
            {
                yield return "Must enter bikeId";
            }

            if (_request.StopId.HasValue)
            {
                if (_request.Latitude.HasValue || _request.Longitude.HasValue)
                {
                    yield return "If stopid is used, latitude and longitude can't be set.";
                }
            }
            else
            {
                if (!_request.Latitude.HasValue || !_request.Longitude.HasValue)
                {
                    yield return "If stopid is not set, MUST set latitude and longitude";
                }
            }
        }
    }
}
