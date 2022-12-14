
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace WebApi.interfaces
{
    public interface IphotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        
    }
}