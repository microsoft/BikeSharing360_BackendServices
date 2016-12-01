using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Feedback.Api.Models;
using BikeSharing.Services.Feedback.Api.StdHost.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.StdHost.Commands
{
    public class UpdateIssueCommand : CommandBase
    {
        private readonly ReportedIssue _issue;

        public ReportedIssue Issue => _issue;

        public UpdateIssueCommand(ReportedIssue issue)
        {
            _issue = issue;
        }


        protected override IEnumerable<string> OnValidation()
        {
            if (_issue.Id == 0)
            {
                yield return "Must have userId";
            }

            if (_issue.IssueType == ReportedIssueType.Unknown)
            {
                yield return "Must have an issueType";
            }
        }
    }
}
