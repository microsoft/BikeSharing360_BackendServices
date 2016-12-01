using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeSharing.Services.Profiles.Queries;
using BikeSharing.Models.Profiles;
using BikeSharing.Services.Profiles.Commands;
using BikeSharing.Services.Core.Controllers;
using BikeSharing.Services.Core.Commands;
using MyBikes.Services.Profiles.Models;
using MyBikes.Models.Profiles;
using Microsoft.Extensions.Configuration;

namespace BikeSharing.Services.Profiles.Controllers
{
    [Route("api/[controller]")]
    public class CustomerServiceController : BaseApiController
    {
        public CustomerServiceController(ICommandBus bus) : base(bus)
        {
        }
    }
}
