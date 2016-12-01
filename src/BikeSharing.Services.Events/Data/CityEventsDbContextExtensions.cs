using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BikeSharing.Services.Events.Data
{
    public static class CityEventsDbContextExtensions
    { 
        public static void Seed(this CityEventsDbContext db)
        {
            //db.Database.Migrate();
            //db.Dispose();
        }
    }
}
