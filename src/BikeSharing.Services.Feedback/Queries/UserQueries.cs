using BikeSharing.Services.Feedback.Api.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly string _url;
        private const string _usersAction = "/api/users";
        public UserQueries(IConfiguration config)
        {
            _url = config["apis:profiles:url"];
        }

        public async Task<IEnumerable<User>> GetUsersById(IEnumerable<int> ids)
        {
            var action = new List<string>()
            {
                $"{_usersAction}?"
            };
            action.AddRange(ids.Select(id => $"id={id}&"));
            var request = string.Join(string.Empty, action);
            request = request.Remove(request.Length - 1);
            var users = await CallApiAsync<IEnumerable<User>>(request);
            return users;
        }

        private async Task<TR> CallApiAsync<TR>(string action) where TR : class
        {
            var client = new HttpClient();
            try
            {
                var response = await client.GetAsync($"{_url}{action}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<TR>(data,
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
                // TODO: Log!
                return null;
            }
        }
    }
}
