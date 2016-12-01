using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeSharing.Services.Profiles.Commands;
using BikeSharing.Services.Profiles.Models;
using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Core.Controllers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeSharing.Services.Profiles.Controllers
{
    [Route("api/[controller]")]

    public class LoginController : BaseApiController
    {



        public LoginController(ICommandBus commandBus) : base(commandBus) { }

        [HttpPost]
        public IActionResult PostLogin([FromBody] LoginModel data)
        {
            var command = new GrantAccessCommand(data);
            return ProcessCommand(command);

        }
    }
}
