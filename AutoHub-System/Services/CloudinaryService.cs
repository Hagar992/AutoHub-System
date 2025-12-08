using AutoHub_System.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AutoHub_System.Services
{
    
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloud;

        public CloudinaryService(IConfiguration config)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );

            _cloud = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder)
        {
            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder
            };

            var result = await _cloud.UploadAsync(uploadParams);

            return result.SecureUrl.ToString();
        }
    }
}
