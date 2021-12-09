using Microsoft.AspNetCore.Http;

namespace Karma.Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
