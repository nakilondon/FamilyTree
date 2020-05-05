using System.Threading.Tasks;
using ReactNet.Models;

namespace ReactNet.Repositories
{
    public interface IImagesRepository
    {
        Task SaveImage(ImageData imageData);
        Task<ImageData> GetImage(string fileName, ImageType imageType);
    }
}
