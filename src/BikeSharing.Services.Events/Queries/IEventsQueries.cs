using BikeSharing.Models.Events;
using System;
using System.Collections.Generic;

namespace BikeSharing.Services.Events.Queries
{
    public interface IEventsQueries
    {
        IEnumerable<CityEvent> GetSuggestedEvents(int userid);
    }
}
