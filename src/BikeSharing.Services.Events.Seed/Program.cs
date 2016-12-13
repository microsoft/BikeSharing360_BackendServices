using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BikeSharing.Events.Seed
{
    public class Program
    {

        public static IConfiguration Configuration { get; private set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args);
            Configuration = builder.Build();
            
            Console.WriteLine("+++ Begin import");
            var importTask = ImportEvents();
            importTask.Wait();

            Console.WriteLine("+++ Done!");
        }
        
        private static async Task ImportEvents()
        {
            var maxpages = int.Parse(Configuration["maxpages"]);
            using (var db = ConnectDb())
            {
                var loader = new EventsImporter(db);
                await loader.LoadDataAsync();
                await loader.InsertIntoDbAsync(maxpages);
            }
        }

        private static CityEventsDbContext ConnectDb()
        {
            var constr = Program.Configuration["ConnectionStrings:DefaultConnection"];
            Console.WriteLine($"+++ connecting to database {constr}");
            return new CityEventsDbContext();
        }
    }
}
