using BikeSharing.Services.Feedback.Api.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.ApiModels
{
    public class IssueApiModel
    {


        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public StationApiModel Station { get; set; }
        public string Time { get; set; }
        public bool Solved { get; set; }

        

        public IssueApiModel()
        {
            Station = new StationApiModel();
        }


        public static TModel FromReportedIssue<TModel>(ReportedIssue issue) where
            TModel : IssueApiModel, new()
        {
            var model = new TModel();

            model.Id = issue.Id;
            model.Type = issue.IssueType.ToString();
            model.Title = model.Title ?? model.Type.ToString();
            model.Solved = issue.Solved;
            model.Subtitle = issue.Description;
            model.Station.Latitude = issue.Latitude;
            model.Station.Longitude = issue.Longitude;
            model.Station.Id = issue.StopId.HasValue ? issue.StopId.Value : 0;
            model.Time = issue.UtcTime.ToString("r", CultureInfo.InvariantCulture);

            return model;
        }

    }
}
