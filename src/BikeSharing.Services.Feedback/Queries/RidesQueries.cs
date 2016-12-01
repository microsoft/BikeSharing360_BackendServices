using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeSharing.Services.Feedback.Api.StdHost.Responses;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using BikeSharing.Services.Feedback.Api.StdHost.Requests;
using System.Text;
namespace BikeSharing.Services.Feedback.Api.Queries
{
    public class RidesQueries : IRidesQueries
    {
        private readonly string _url;
        private const string JsonContentType = "application/json";

        public RidesQueries(IConfiguration config)
        {
            _url = config["apis:rides:url"];
        }

        public async Task<StationResponse> GetStationAsync(int id)
        {
            var data = await CallApiAsync<StationResponse>($"api/stations/{id}");
            return data;
        }

        public async Task<BikeResponse> GetBikeAsync(int id)
        {
            var data = await CallApiAsync<BikeResponse>($"api/bikes/{id}");
            return data;
        }
        public async Task<IEnumerable<RidesResponse>> GetRidesByUserAndBike(IEnumerable<RidesByUserBike> ridesByUserBike)
        {
            var data = await PostApiAsync<IEnumerable<RidesByUserBike>, IEnumerable<RidesResponse>>
                ("api/users/rides/last", ridesByUserBike);
            return data;
        }

        private async Task<TR> CallApiAsync<TR>(string actionUri) where TR : class
        {
            var client = new HttpClient();
            try
            {
                var response = await client.GetAsync($"{_url}/{actionUri}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var station = JsonConvert.DeserializeObject<TR>(data,
                        new JsonSerializerSettings()
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                    return station;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        private async Task<TResponse> PostApiAsync<TRequest, TResponse>(string actionUri, TRequest model) where TResponse : class
        {
            var client = new HttpClient();
            try
            {
                var serializedModel = JsonConvert.SerializeObject(model,
                    new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                var stringContent = new StringContent(serializedModel, Encoding.UTF8, JsonContentType);

                var response = await client.PostAsync($"{_url}/{actionUri}", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<TResponse>(data,
                        new JsonSerializerSettings()
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
                    return deserializedResponse;
                }
                else
                {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
        }
    }
}
