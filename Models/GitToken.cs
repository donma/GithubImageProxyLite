using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubImageLite.Models
{
    public class GitToken
    {
        public string Id { get; set; }

        public string RepoName { get; set; }

        public string RepoId { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }
        public decimal UsedMB { get; set; }


        public DateTime CreateDate { get; set; }
    }
}