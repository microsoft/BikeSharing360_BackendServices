using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Models.Events
{
    public class Classification
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public ClassificationType Type { get; set; }

        public Classification()
        {
            Type = ClassificationType.Segment;
        }
    }
}
