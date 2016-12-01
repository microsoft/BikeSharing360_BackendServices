using BikeSharing.Services.Feedback.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeSharing.Services.Feedback.Api.Queries
{
    public interface IUserQueries
    {
        Task<IEnumerable<User>> GetUsersById(IEnumerable<int> ids);
    }
}
