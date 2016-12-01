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
    public class UpdateIssueCommandHandler : ICommandHandler<UpdateIssueCommand>
    {
        private readonly FeedbackDbContext _db;
        private readonly IRidesQueries _ridesQueries;
        public UpdateIssueCommandHandler(FeedbackDbContext db, IRidesQueries ridesQueries)
        {
            _db = db;
            _ridesQueries = ridesQueries;
        }

        public CommandHandlerResult Handle(UpdateIssueCommand command)
        {

            var issue = command.Issue;
            var dbIssue = _db.Issues.FirstOrDefault(x => x.Id == issue.Id);

            if(dbIssue == null)
            {
                return CommandHandlerResult.Error("Invalid Issue");
            }

            dbIssue.Solved = issue.Solved;

            return CommandHandlerResult.Ok;
        }
    }
}
