using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Models.Profiles
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public Tenant()
        {
            Users = new List<User>();
        }
    }
}
