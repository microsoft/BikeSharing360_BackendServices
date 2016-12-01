using BikeSharing.Services.Core.Commands;
using MyBikes.Services.Profiles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Commands
{
    public class UpdateProfileImageCommand : CommandBase
    {

        public UpdateImageModel Model { get; }

        public byte[] ImageData { get; private set; }
        public int UserId { get; set; }

        public UpdateProfileImageCommand(int userid, UpdateImageModel model)
        {
            Model = model;
            UserId = userid;
            
        }

        protected override IEnumerable<string> OnValidation()
        {
            if (Model == null || string.IsNullOrEmpty(Model.Data))
            {
                yield return "Payload not found or payload data is empty";
            }
            else
            {
                ImageData = GetImageData(Model.Data);
                if (ImageData == null)
                {
                    yield return "Payload.data is not base64";
                }
            }
        }

        private static byte[] GetImageData(string base64)
        {
            try
            {
                return Convert.FromBase64String(base64);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
