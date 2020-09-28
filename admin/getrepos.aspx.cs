using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite.admin
{
    public partial class getrepos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["token"])) {
                Response.Write("error:toke null");
                return;
            }
            if (string.IsNullOrEmpty(Request["name"]))
            {
                Response.Write("error:name null");
                return;
            }

            try
            {

                //這 DONMATEST 可以任意都可以
                var client = new GitHubClient(new ProductHeaderValue("DONMATEST"));

                //從網站上取得的 personal access token https://github.com/settings/tokens 
                var tokenAuth = new Credentials(Request["token"]); // NOTE: not real token

                client.Credentials = tokenAuth;

                //下面那是你自己的 github name.
                var repository = client.Repository.GetAllForUser(Request["name"]).Result;

                foreach (var repo in repository)
                {
                    //印出 id , name.
                  Response.Write(repo.Id + ":" + repo.Name+"<br>");
                }

            }
            catch (Exception ex)
            {
               // Console.ForegroundColor = ConsoleColor.Red;
                Response.Write(ex.Message + "<br>" + ex.StackTrace);
            }

        }
    }
}