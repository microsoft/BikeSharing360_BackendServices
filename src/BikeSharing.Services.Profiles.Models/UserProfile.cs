using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Models.Profiles
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        public User User { get; set; }
        public Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public PaymentData Payment { get; set; }

        public int? PaymentId { get; set; }

        public Guid? FaceProfileId { get; set; }

        public Guid? VoiceProfileId { get; set; }

        public string VoiceSecretPhrase { get; set; }

        public string Skype { get; set; }
        public string Mobile { get; set; }

        public string PhotoUrl { get; set; }

        public UserProfile()
        {
            Gender = Gender.NotSpecified;
        }
    }
}
