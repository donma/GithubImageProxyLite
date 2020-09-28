using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubImageLite.Models
{
    public class ImageInfo
    {
        public string Id { get; set; }
        public DateTime LastUpdate { get; set; }

        public decimal Length { get; set; }

        public string Owner { get; set; }

        public string TokenId { get; set; }

        public string GitPath { get; set; }
    }
}