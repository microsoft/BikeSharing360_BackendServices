using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Profiles.Queries;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BikeSharing.Services.Profiles.Commands
{
    public class UpdateProfileImageCommandHandler : ICommandHandler<UpdateProfileImageCommand>
    {

        private readonly string _blobConnectionString;

        public UpdateProfileImageCommandHandler(IConfiguration cfg)
        {
            
            _blobConnectionString = cfg["ConnectionStrings:avatarBlob"];
        }

        public CommandHandlerResult Handle(UpdateProfileImageCommand command)
        {
            var imgData = command.ImageData;
            var fileName = $"user_{command.UserId}.jpg";
            return SaveDataToBlobs(imgData, fileName).Result;
        }

        private async Task<CommandHandlerResult> SaveDataToBlobs(byte[] imgData, string fileName)
        {
            try
            {
                await SaveDataToBlob(imgData, fileName, "avatar-output");
                await SaveDataToBlob(imgData, fileName, "avatar-input");
                return CommandHandlerResult.Ok;
            }
            catch (Exception ex)
            {
                return CommandHandlerResult.Error($"Error {ex.GetType().Name} - {ex.Message}");
            }
        }

        private async Task SaveDataToBlob(byte[] imgData, string fileName, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(_blobConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(fileName);
            using (var stream = new MemoryStream(imgData))
            {
                await blockBlob.UploadFromStreamAsync(stream);
            }
        }

    }


}
