﻿using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite.admin
{
    public partial class delimagebyid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"]))
            {
                Response.Write("error:id null");
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


            var res=DeleteImageById(Request["id"]);


            Response.Write(res);
            return;
        }

        public static string DeleteImageById(string imageId)
        {


            var info = Global._Role.GetQ<Models.ImageInfo>("IMAGES").DataByKey(imageId);
            if (info == null)
            {
                return "success";

            }

            var tokenInfo = Global._Role.GetQ<Models.GitToken>("TOKENS").DataByKey(info.TokenId);

            if (tokenInfo == null)
            {
                return "error:token error";
            }

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
                    if (f.Name == imageId.ToLower() + ".gif")
                    {
                        client.Repository.Content.DeleteFile(long.Parse(tokenInfo.RepoId), "imgs/" + imageId.ToLower() + ".gif", new DeleteFileRequest("delete file", f.Sha)).RunSynchronously();

                        Global._Role.GetOp("IMAGES").Delete(imageId.ToLower());
                        break;
                    }
                }
            }
            catch
            {

            }
            return "success";

        }
    }
}