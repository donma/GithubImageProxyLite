using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace GithubImageLite
{
    public class Global : System.Web.HttpApplication
    {

        public static No2DB.Base.DRole _Role { get; set; }

        public static ConcurrentBag<Models.GitToken> _GitTokens { get; set; }

        public static string ServerToken { get; set; }
        public static string UploadPath { get; set; }

        public static decimal PerRepoLimit { get; set; }

        public static string ImageNotFound = "https://raw.githubusercontent.com/ctimggit/storage1/master/404.png";

        public static bool IsWriting { get; set; }
        protected void Application_Start(object sender, EventArgs e)
        {
            _Role = new No2DB.Base.DRole("GITHUBIMAGELITE");


            var settingStr = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "site.configsettings");

            var obj = JsonConvert.DeserializeObject<Models.ServerSetting>(settingStr);
            PerRepoLimit = obj.LimitMB;
            UploadPath = obj.UplpadPath;
            ServerToken = obj.ServerToken;

            GetAllGitTokens();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public static void UpdateGitToken(Models.GitToken token, decimal byteCount)
        {
            foreach (var c in _GitTokens)
            {
                if (c.Id == token.Id)
                {
                    c.UsedMB += byteCount;
                }
            }


            if (!IsWriting)
            {
                IsWriting = true;
                token.UsedMB += byteCount;
                Global._Role.GetOp("TOKENS").Update(token.Id, token);
                IsWriting = false;
            }
        }

        public static ConcurrentBag<Models.GitToken> GetAllGitTokens()
        {
            if (!IsWriting)
            {
                var tmp = Global._Role.GetQ<Models.GitToken>("TOKENS").AllDatasList();

                tmp = tmp.Where(x => x.UsedMB <= PerRepoLimit).OrderBy(x => x.UsedMB).ThenBy(x => x.RepoName).ThenBy(x => x.CreateDate).ToList();
                var t = new ConcurrentBag<Models.GitToken>();
                foreach (var c in tmp)
                {
                    t.Add(c);
                }
                _GitTokens = t;
            }

            return _GitTokens;
        }

        public static void ReCountTokenId(string tokeId)
        {
            var tokenInfo = Global._Role.GetQ<Models.GitToken>("TOKENS").DataByKey(tokeId);



            if (tokenInfo == null)
            {
                return;
            }

            decimal count = 0;

            //這 DONMATEST 可以任意都可以
            var client = new GitHubClient(new ProductHeaderValue("GISL"));

            //從網站上取得的 personal access token https://github.com/settings/tokens 
            var tokenAuth = new Credentials(tokenInfo.Token);

            client.Credentials = tokenAuth;

            try
            {
                var existingFiles = client.Repository.Content.GetAllContentsByRef(tokenInfo.UserName, tokenInfo.RepoName, "imgs", "master").Result;

                if (existingFiles.Count > 0)
                {
                    foreach (var f in existingFiles)
                    {
                        count += (f.Size / (1024m * 1024m));
                    }
                }

            }
            catch
            {

            }
            tokenInfo.UsedMB = count;
            Global._Role.GetOp("TOKENS").Update(tokenInfo.Id, tokenInfo);


        }


        //a16e3baf8fa8784dc45ec4f49a9856c239992818
        //
        public  static void UploadFile(string token, string userName, string reponame, string repoId, string localPath, string ownerId)
        {



            //這 DONMATEST 可以任意都可以
            var client = new GitHubClient(new ProductHeaderValue("GISL"));

            //從網站上取得的 personal access token https://github.com/settings/tokens 
            var tokenAuth = new Credentials(token);

            client.Credentials = tokenAuth;

            //try
            //{
            //    var existingFiles = client.Repository.Content.GetAllContentsByRef(userName, reponame, "imgs", "master").Result;
            //    //如果有找到已存在就刪除
            //    foreach (var f in existingFiles)
            //    {
            //        if (f.Name == Path.GetFileName(localPath))
            //        {
            //            await client.Repository.Content.DeleteFile(long.Parse(repoId), "imgs/" + Path.GetFileName(localPath), new DeleteFileRequest("delete file", f.Sha));
            //            break;
            //        }
            //    }
            //}
            //catch
            //{

            //}


            //最後一個參數是否要轉成 base64
            var updateRequest = new UpdateFileRequest("Hi , Upload by GISL Tool. " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Convert.ToBase64String(File.ReadAllBytes(localPath)), "SHA", false);

            var res = client.Repository.Content.UpdateFile(long.Parse(repoId), "imgs/" + Path.GetFileName(localPath), updateRequest).Result;

            var img = new Models.ImageInfo();

            img.Id = Path.GetFileNameWithoutExtension(localPath);
            img.LastUpdate = DateTime.Now;
            img.GitPath = res.Content.DownloadUrl;
            img.Length = Convert.ToDecimal(res.Content.Size) / ((1024m * 1024m));
            img.Owner = ownerId;
            img.TokenId = userName + repoId;
            Global._Role.GetOp("IMAGES").Update(img.Id, img);


        }
    }
}