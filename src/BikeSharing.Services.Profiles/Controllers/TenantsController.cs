using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Core.Controllers;
using BikeSharing.Services.Profiles.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Controllers
{
    [Route("api/[controller]")]
    public class TenantsController : BaseApiController
    {
        private readonly ITenantQueries _queries;
        public TenantsController(ITenantQueries queries, ICommandBus bus)
            : base(bus)
        {
            _queries = queries;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var tenant = _queries.GetById(id);
            return tenant == null ? (IActionResult)NotFound() : Ok(new
            {
                Id = tenant.Id,
                Name = tenant.Name
            });
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _queries.GetAllAsync();
            return Ok(tenants);
        }
    }
}
