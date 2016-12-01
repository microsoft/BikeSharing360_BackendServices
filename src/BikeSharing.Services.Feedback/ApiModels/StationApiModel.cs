using System;
using BikeSharing.Services.Feedback.Api.StdHost.Responses;

namespace BikeSharing.Services.Feedback.Api.ApiModels
{
    public class StationApiModel
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public static StationApiModel FromStationResponse(StationResponse stationResponse)
        {
            var model = new StationApiModel()
            {
                Id = stationResponse.Id,
                Latitude = stationResponse.Latitude,
                Longitude = stationResponse.Longitude,
                Address = stationResponse.Name,
                City = "New York",
                Country = "USA"
            };

            return model;
        }
    }
}