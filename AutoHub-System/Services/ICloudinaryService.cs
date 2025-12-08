using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AutoHub_System.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, string folder);
    }
}
