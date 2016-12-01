using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeSharing.Services.Events.Queries;
using BikeSharing.Models.Events;

namespace BikeSharing.Services.Events.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventsQueries _queries;

        public EventsController(IEventsQueries queries)
        {
            _queries = queries;
        }

        [HttpGet]
        public IActionResult GetMySuggestedEvents()
        {
            var data = _queries.GetSuggestedEvents(1);
            return Ok(data);
        }

    }
}
