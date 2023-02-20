using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureDemo.Settings
{
    public sealed class S3Settings
    {
        [Required]
        public string ServiceUrl { get; set; }

        [Required]
        public string AccessKey { get; set; }

        [Required]
        public string SecretKey { get; set; }
    }
}
