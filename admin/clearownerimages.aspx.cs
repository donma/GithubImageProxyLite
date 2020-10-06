using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite.admin
{
    public partial class clearownerimages : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["owner"]))
            {
                Response.Write("error:owner null");
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


            var images = Global._Role.GetQ<Models.ImageInfo>("IMAGES").AllDatasList();

            images = images.Where(x => x.Owner == Request["owner"]).ToList();

            var tokens = Global.GetAllGitTokens();

            var delQ = Global._Role.GetOp("IMAGES");
            foreach (var tokenInfo in tokens)
            {

                //這 DONMATEST 可以任意都可以
                var client = new GitHubClient(new ProductHeaderValue("GISL"));

                //從網站上取得的 personal access token https://github.com/settings/tokens 
                var tokenAuth = new Credentials(tokenInfo.Token);

                client.Credentials = tokenAuth;

                try
                {

                    var existingFiles = client.Repository.Content.GetAllContentsByRef(tokenInfo.UserName, tokenInfo.RepoName, "imgs", "master").Result;
                    //如果有找到已存在就刪除
                    foreach (var f in existingFiles)
                    {
                        try
                        {
                            if (images.Any(x => x.Id + ".gif" == f.Name))
                                await client.Repository.Content.DeleteFile(long.Parse(tokenInfo.RepoId), "imgs/" + f.Name, new DeleteFileRequest("delete file", f.Sha));

                            delQ.Delete(f.Name.Replace(".gif", ""));
                            Thread.Sleep(400);
                        }
                        catch
                        {
                            continue;
                        }

                    }
                }
                catch
                {

                }


                Global.ReCountTokenId(tokenInfo.Id);


            }

            Response.Write("success");
        }
    }
}