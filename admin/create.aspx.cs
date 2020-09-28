using Newtonsoft.Json;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite.admin
{
    public partial class create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["token"]))
            {
                Response.Write("error:toke null");
                return;
            }
            if (string.IsNullOrEmpty(Request["name"]))
            {
                Response.Write("error:name null");
                return;
            }
            if (string.IsNullOrEmpty(Request["repoid"]))
            {
                Response.Write("error:repoid null");
                return;
            }

            if (string.IsNullOrEmpty(Request["reponame"]))
            {
                Response.Write("error:reponame null");
                return;
            }
            if (string.IsNullOrEmpty(Request["pass"]))
            {
                Response.Write("error:pass null");
                return;
            }
            if (Request["pass"] != Global.ServerToken)
            {
                Response.Write("error:pass error");
                return;
            }


            var d = Global._Role.GetQ<Models.GitToken>("TOKENS").DataByKey(Request["reponame"] + Request["repoid"]);
            if (d == null)
            {
                d = new Models.GitToken();
            }
            d.CreateDate = DateTime.Now;
            d.RepoId = Request["repoid"];

            d.UserName = Request["name"];
            d.RepoName = Request["reponame"];
            d.Id = d.UserName +  d.RepoId;
            d.Token = Request["token"];

            Global._Role.GetOp("TOKENS").Update(d.Id, d);
            Response.Write(JsonConvert.SerializeObject(d, Formatting.Indented));

        }
    }
}