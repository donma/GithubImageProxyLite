﻿using No2DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite
{
    public partial class upload1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["token"] == null)
            {
                Response.Write("error:token null");
                return;
            }

            if (Request["owner"] == null)
            {
                Response.Write("error:owner null");
                return;
            }

            if (Request["token"] != "zxcasdqwe")
            {
                Response.Write("error:token error");
                return;
            }


            try
            {

                string newFilename = No2DB.Util.GuidShort();

                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    newFilename = Request["id"];
                    newFilename = newFilename.ToLower();
                }


                string ext = "jpg";

                if (Request["ext"] == null)
                {
                    if (!string.IsNullOrEmpty(Request["ext"]))
                    {
                        ext = Request.QueryString["ext"];
                    }
                }

                var tempPath = AppDomain.CurrentDomain.BaseDirectory + "temp" + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "temp" + Path.DirectorySeparatorChar);

                DirectoryInfo FileDir = new DirectoryInfo(tempPath);

                if (FileDir.Exists == false)
                {
                    FileDir.Create();
                }


                if (Request.Files.Count > 0)
                {
                    HttpPostedFile file = null;
                    file = Request.Files[0];
                    ext = Path.GetExtension(file.FileName);
                    file.SaveAs(tempPath + newFilename + ".gif");

                }

                var d = Global.GetAllGitTokens().ToList();

                if (d.Count > 0)
                {
                    try
                    {
                        Global.UploadFile(d[0].Token, d[0].UserName, d[0].RepoName, d[0].RepoId, tempPath + newFilename + ".gif", Request["owner"]);
                        FileInfo info = new FileInfo(tempPath + newFilename + ".gif");

                        Global.UpdateGitToken(d[0], Convert.ToDecimal(info.Length) / (1024m*1024m));


                        Response.Write("success");
                    }
                    catch (Exception exx)
                    {
                        Response.Write("error:" + exx.Message);
                    }
                    finally
                    {
                        //File.Delete(tempPath + newFilename + ".gif");

                        //        Task task = Task.Delay(10000)
                        //.ContinueWith(t => {

                        //    File.Delete(tempPath + newFilename + ".gif");
                        //});

                        //        task.Wait();
                        var t = Task.Run(() =>
                        {
                            Thread.Sleep(10000);
                            File.Delete(tempPath + newFilename + ".gif");
                        });

                     //   t.Start();
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                Response.Write("error:" + ex.Message);
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void SaveFile(Stream stream, FileStream fs)
        {
            try
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                //  File.WriteAllText(PathHubs.LogPath + "[UPLOAD]" + DateTime.Now.ToString("yyyyMMdd hhmmss"), ex.Message + "\r\n" + ex.StackTrace);
            }
        }

    }
}