using BikeSharing.Services.Feedback.Api.Models;
using BikeSharing.Services.Feedback.Api.StdHost.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.ApiModels
{
    public class IssueDetailsApiModel : IssueApiModel
    {
        private List<IssueApiModel> _incidences;


        public BikeResponse BikeInfo { get; set; }

        public IEnumerable<IssueApiModel> Incidences => _incidences;
        public IssueDetailsApiModel()
        {
            _incidences = new List<IssueApiModel>();
        }

        public void addExtraIncidences(IEnumerable<ReportedIssue> extraIssues)
        {
            foreach (var extraIssue in extraIssues)
            {
                _incidences.Add(new IssueApiModel()
                {
                    Id = extraIssue.Id,
                    Type = extraIssue.IssueType.ToString(),
                    Title = extraIssue.IssueType.ToString(),
                    Time = extraIssue.UtcTime.ToString("r", CultureInfo.InvariantCulture)
                });
            }
        }
    }
}
