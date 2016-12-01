using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeSharing.Services.Profiles.Queries;
using BikeSharing.Models.Profiles;
using BikeSharing.Services.Profiles.Commands;
using BikeSharing.Services.Core.Controllers;
using BikeSharing.Services.Core.Commands;
using MyBikes.Services.Profiles.Models;
using MyBikes.Models.Profiles;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BikeSharing.Services.Profiles.Controllers
{
    [Route("api/[controller]")]
    public class ProfilesController : BaseApiController
    {
        private readonly IProfileQueries _queries;
        private readonly string _avatarsUrl;

        public ProfilesController(IProfileQueries queries, IConfiguration cfg,  ICommandBus bus)
            : base (bus)
        {
            _queries = queries;
            _avatarsUrl = cfg["AvatarsUrl"];
        }

        [Route("{userid:int}")]
        [HttpGet()]
        public IActionResult GetProfileByUserId(int userid)
        {
            var profile = _queries.GetByUserId(userid);
            if (profile != null)
            {
                profile.PhotoUrl = $"{_avatarsUrl}user_{profile.UserId}.jpg";
                return Ok(profile);
            }

            return NotFound();
        }

        [Route("connectors")] 
        [HttpGet()]
        public IActionResult GetProfileByConnector(ConnectorType ctype, string value)
        {
            UserProfile profile = null;
            switch (ctype)
            {
                case ConnectorType.SMS:
                    profile = _queries.GetByMobile(value);
                    break;
                case ConnectorType.Email:
                    profile = _queries.GetByEmail(value);
                    break;
                case ConnectorType.Skype:
                    profile = _queries.GetBySkype(value);
                    break;
            }

            if (profile != null)

            {
                profile.PhotoUrl = $"{_avatarsUrl}user_{profile.UserId}.jpg";
                return Ok(profile);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateProfile([FromBody] UserAndProfileModel profile)
        {
            var command = new CreateProfileAndUserCommand(profile);

            return ProcessCommand(command);

        }

        [HttpPut]
        public IActionResult UpdateProfile([FromBody] UserProfile profile)
        {

            var command = new UpdateProfileCommand(profile, isPatch: false);
            return ProcessCommand(command);
        }

        [HttpPatch]
        public IActionResult PatchUpdateProfile([FromBody] UserProfile profile)
        {

            var command = new UpdateProfileCommand(profile, isPatch: true);
            return ProcessCommand(command);
        }

        [HttpPut("image/{userid:int}")]
        public IActionResult UpdateProfileImage(int userid, [FromBody] UpdateImageModel imageModel)
        {
            var command = new UpdateProfileImageCommand(userid,imageModel);
            return ProcessCommand(command);
        }

        [HttpPut]
        [Route("{userid:int}/payment")]
        public IActionResult SetPaymentData(int userid, [FromBody] PaymentData data)
        {
            var command = new SetPaymentDataCommand(userid, data);
            return ProcessCommand(command);
        }

        [Route("{userid:int}")]
        [HttpDelete]
        public IActionResult DeleteProfile(int userid)
        {
            var command = new DeleteProfileCommand(userid);
            return ProcessCommand(command);
        }

        [HttpPut]
        [Route("{userid:int}/faceprofile")]
        public IActionResult SetFaceProfileId(int userid, [FromBody] Guid id)
        {
            var profile = new UserProfile()
            {
                UserId = userid,
                FaceProfileId = id
            };
            var command = new UpdateProfileCommand(profile, isPatch: true);
            return ProcessCommand(command);
        }

        [HttpPut]
        [Route("{userid:int}/voiceprofile")]
        public IActionResult SetVoiceProfileId(int userid, [FromBody] Guid id)
        {
            var profile = new UserProfile()
            {
                UserId = userid,
                VoiceProfileId = id
            };
            var command = new UpdateProfileCommand(profile, isPatch: true);
            return ProcessCommand(command);
        }

        [HttpPut]
        [Route("{userid:int}/voicesecretphrase")]
        public IActionResult SetVoiceSecretPhrase(int userid, [FromBody] string phrase)
        {
            var profile = new UserProfile()
            {
                UserId = userid,
                VoiceSecretPhrase = phrase
            };
            var command = new UpdateProfileCommand(profile, isPatch: true);
            return ProcessCommand(command);
        }

        [HttpPost]
        [Route("byfaceid")]
        public IActionResult GetUserProfileByFaceProfileId([FromBody] Guid faceid)
        {
            var profile = _queries.GetByFaceProfileId(faceid);
            if (profile != null)
            {
                profile.PhotoUrl = $"{_avatarsUrl}user_{profile.UserId}.jpg";
                return Ok(profile);
            }

            return NotFound();
        }
    }
}
