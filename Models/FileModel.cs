using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Karma.Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
