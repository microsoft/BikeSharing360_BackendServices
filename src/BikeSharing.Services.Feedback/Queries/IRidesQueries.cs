using BikeSharing.Services.Feedback.Api.StdHost.Requests;
using BikeSharing.Services.Feedback.Api.StdHost.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.Queries
{
    public interface IRidesQueries
    {
        Task<StationResponse> GetStationAsync(int id);
        Task<BikeResponse> GetBikeAsync(int id);
        Task<IEnumerable<RidesResponse>> GetRidesByUserAndBike(IEnumerable<RidesByUserBike> ridesByUserBike);
    }
}
