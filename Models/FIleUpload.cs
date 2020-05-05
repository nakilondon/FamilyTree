using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ReactNet.Models
{
    public class FIleUpload
    {
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
