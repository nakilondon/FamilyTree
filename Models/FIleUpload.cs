using Microsoft.AspNetCore.Http;

namespace ReactNet.Models
{
    public class FIleUpload
    {
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
