using BikeSharing.Models.Events;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BikeSharing.Events.Seed
{
    public class EventsImporter
    {
        private JToken _pages;
        private JToken _events;
        private JToken _links;
        private readonly CityEventsDbContext _db;

        public EventsImporter(CityEventsDbContext db)
        {
            _db = db;
        }

        public async Task LoadDataAsync()
        {
            Console.WriteLine($"+++ Fetching from API");
            await FetchEventsPage(0);
            Console.WriteLine("+++ Data fetched!");
        }

        private async Task FetchEventsPage(int page)
        {
            var url = GetUrl(page);
            Console.WriteLine($"+++ Fetching page {page} using url: '{url}'");
            var client = new HttpClient();
            var response =
                await client.GetAsync(url);

            var responseString = await response.Content.ReadAsStringAsync();

            var data = JObject.Parse(responseString);
            _events = data["_embedded"]["events"];
            _pages = data["page"];
            _links = data["_links"];

            Console.WriteLine($"+++ Page {page} fetched");
        }

        private string GetUrl(int page)
        {
            var city = Program.Configuration["city"];
            var items = int.Parse(Program.Configuration["items"]);
            var startDate = Program.Configuration["startDate"];         // Must be in ISO 8601 (YYYY-MM-DDTHH:mm:ssZ)
            var endDate = Program.Configuration["endDate"];             // Must be in ISO 8601 (YYYY-MM-DDTHH:mm:ssZ)

            if (startDate == null)
            {
                startDate = DateTime.Now.Date.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }

            if (endDate == null)
            {
                endDate = DateTime.Now.Date.AddDays(10).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }

            var apiKey = Program.Configuration["apikey"];
            var url = $"https://app.ticketmaster.com/discovery/v2/events.json?city={city}&size={items}&apikey={apiKey}";
            if (!string.IsNullOrEmpty(startDate)) {
                url += $"&startDateTime={startDate}";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                url += $"&endDateTime={endDate}";
            }
            if (page > 0)
            {
                url += $"&page={page}";
            }
            return url;
        }

        public async Task InsertIntoDbAsync(int maxpages)
        {
            dynamic pages = _pages;
            var numpages = maxpages <= 0 ? (int)pages.totalPages : maxpages;

            for (var currentPage = 0; currentPage < numpages; currentPage++)
            {
                Console.WriteLine($"+++ Processing page {currentPage + 1} of {numpages}");
                if (currentPage != 0)
                {
                    await FetchEventsPage(currentPage);
                }
                await InsertAllEventsAsync();
            }
        }

        private async Task InsertAllEventsAsync()
        {
            foreach (dynamic @event in _events)
            {
                CityEvent info = GetEventData(@event);
                if (info.Genre == null || info.SubGenre == null || info.Segment == null)
                {
                    Console.WriteLine($"+++ Skipped {@event.name} (no segment or genre or subgenre)");
                }
                else
                {
                    EventVenue venue = GetVenue(@event);
                    if (venue != null)
                    {
                        if (IsNewCityEvent(info))
                        {
                            info.Venue = venue;
                            _db.Events.Add(info);
                            Console.WriteLine($"+++ Added {@event.name} in db AND docdb");
                        }
                        else
                        {
                            Console.WriteLine($"+++ Skipped {@event.name} (already exists)");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"+++ Skipped {@event.name} because has not venue");
                    }
                }

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"+++ Error saving {@event.name}: {ex.Message}");
                }
            }
        }
        
        private bool IsNewCityEvent(CityEvent info)
        {
            return !_db.Events.Any(e => e.ExternalId == info.ExternalId);
        }

        private CityEvent GetEventData(dynamic @event)
        {
            var data = new CityEvent();
            data.Name = (string)@event.name;
            data.ExternalId = (string)@event.id;
            data.StartTime = GetStartTime(@event);
            data.EndTime = GetEndTime(@event);
            IEnumerable<Classification> classifs = GetClassifications(@event);
            data.Segment = classifs.FirstOrDefault(c => c.Type == ClassificationType.Segment);
            data.Genre = classifs.FirstOrDefault(c => c.Type == ClassificationType.Genre);
            data.SubGenre = classifs.FirstOrDefault(c => c.Type == ClassificationType.Subgenre);

            if (@event.images != null)
            {
                foreach (var image in @event.images)
                {
                    if (image.width > 1900)
                    {
                        data.ImagePath = image.url;
                    }
                }
            }

            return string.IsNullOrEmpty(data.ImagePath) ? null : data;
        }

        private IEnumerable<Classification> GetClassifications(dynamic @event)
        {
            var classifs = @event.classifications;
            var data = new List<Classification>();
            if (classifs != null)
            {
                foreach (dynamic classif in classifs)
                {
                    if ((bool)classif.primary)
                    {

                        dynamic segment = classif.segment;
                        if (segment != null)
                        {
                            data.Add(GetOrCreateClassification(
                                new Classification()
                                {
                                    Name = segment.name,
                                    ExternalId = segment.id,
                                    Type = ClassificationType.Segment
                                }
                             ));
                        }

                        dynamic genre = classif.genre;
                        if (genre != null)
                        {
                            data.Add(GetOrCreateClassification(
                                 new Classification()
                                 {
                                     Name = genre.name,
                                     ExternalId = genre.id,
                                     Type = ClassificationType.Genre
                                 }
                              ));
                        }
                        dynamic subgenre = classif.subGenre;
                        if (subgenre != null)
                        {
                            data.Add(GetOrCreateClassification(
                                 new Classification()
                                 {
                                     Name = subgenre.name,
                                     ExternalId = subgenre.id,
                                     Type = ClassificationType.Subgenre
                                 }
                              ));
                        }
                    }
                }
            }

            return data;
        }

        private Classification GetOrCreateClassification(Classification classification)
        {
            var current = _db.Classifications.FirstOrDefault(c => c.ExternalId == classification.ExternalId);
            if (current == null)
            {
                _db.Classifications.Add(classification);
                Console.WriteLine($"+++ Added classification {classification.Name} ({classification.Type.ToString()})");
                return classification;
            }

            return current;
        }

        private DateTime? GetStartTime(dynamic @event)
        {
            try
            {
                dynamic dates = @event.dates;
                if (dates != null)
                {
                    dynamic start = dates.start;
                    if (start != null)
                    {
                        return (DateTime)start.dateTime;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DateTime? GetEndTime(dynamic @event)
        {
            try
            {
                dynamic dates = @event.dates;
                if (dates != null)
                {
                    dynamic end = dates.end;
                    if (end != null)
                    {
                        return (DateTime)end.dateTime;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private EventVenue GetVenue(dynamic @event)
        {
            dynamic embedded = @event._embedded;
            if (embedded != null)
            {
                var venues = embedded.venues;
                if (venues != null)
                {
                    dynamic venue = venues.First;

                    if (venue != null)
                    {
                        dynamic latlong = venue.location;
                        if (latlong != null)
                        {
                            return GetOrCreateVenue(
                                new EventVenue()
                                {
                                    ExternalId = (string)venue.id,
                                    Name = (string)venue.name,
                                    Latitude = (decimal)latlong.latitude,
                                    Longitude = (decimal)latlong.longitude
                                });
                        }
                    }
                }
            }

            return null;
        }

        private EventVenue GetOrCreateVenue(EventVenue eventVenue)
        {
            var existing = _db.Venues.FirstOrDefault(e => e.ExternalId == eventVenue.ExternalId);
            if (existing != null)
            {
                return existing;
            }

            _db.Add(eventVenue);
            return eventVenue;
        }
    }
}

