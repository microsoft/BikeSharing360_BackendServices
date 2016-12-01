using BikeSharing.Models.Events;
using BikeSharing.Services.Events.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BikeSharing.Services.Events.Queries
{
    public class EventsQueries : IEventsQueries
    {
        private readonly CityEventsDbContext _db;

        public EventsQueries(CityEventsDbContext db)
        {
            _db = db;
        }

        public IEnumerable<CityEvent> GetSuggestedEvents(int userid)
        {
            return _db.Events
                .Include(e => e.Venue)
                .Include(e => e.Segment)
                .Include(e => e.Genre)
                .Include(e => e.SubGenre)
                .Take(5);
        }

    }
}
