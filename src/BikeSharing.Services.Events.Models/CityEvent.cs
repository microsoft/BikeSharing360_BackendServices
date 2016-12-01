using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Models.Events
{
    public class CityEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }    

        public string ImagePath { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int VenueId { get; set; }
        public EventVenue Venue { get; set; }
        public string ExternalId { get; set; }

        public int SegmentId { get; set; }
        public Classification Segment { get; set; }
        public int GenreId { get; set; }
        public Classification Genre { get; set; }
        public int SubGenreId { get; set; }
        public Classification SubGenre { get; set; }
    }
}
