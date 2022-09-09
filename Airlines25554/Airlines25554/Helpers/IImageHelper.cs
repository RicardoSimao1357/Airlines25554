using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Airlines25554.Helpers
{
    public interface IImageHelper
    {
        Task<string> UpLoadImageAsync(IFormFile imageFile, string folder);
    }
}
