using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Airlines25554.Helpers
{
    public interface IBlobHelper
    {
        Task<Guid> UploadBlobAsync(IFormFile file, string containerNamne);

        Task<Guid> UploadBlobAsync(byte[] file, string containerNamne);

        Task<Guid> UploadBlobAsync(string image, string containerNamne);
    }
}
