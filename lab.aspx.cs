using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite
{
    public partial class lab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private string UploadImage(string localpath, string id, string owner, string token, string url)
        {
            var src = System.IO.File.ReadAllBytes(localpath);
            Stream stream = new MemoryStream(src);

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpContent fileStreamContent = new StreamContent(stream);
            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = "xxx.jpg" };
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(token), "token");
                formData.Add(new StringContent(owner), "owner");

                if (!string.IsNullOrEmpty(id))
                {
                    formData.Add(new StringContent(id), "id"); //new add in 2020
                }
                else
                {
                    formData.Add(new StringContent(Path.GetFileNameWithoutExtension(localpath)), "id");
                }

                formData.Add(fileStreamContent, "file");
                var response = client.PostAsync(url, formData).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var res = UploadImage(AppDomain.CurrentDomain.BaseDirectory + "ddd.jpg", "sample", "local", Global.ServerToken, Global.UploadPath);
            Response.Write(res);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var res = UploadImage(AppDomain.CurrentDomain.BaseDirectory + "yyy.jpg", "sample", "local", Global.ServerToken, Global.UploadPath);
            Response.Write(res);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            var files = FileUpload1.PostedFiles;
            var t = AppDomain.CurrentDomain.BaseDirectory + "";

            var r = AppDomain.CurrentDomain.BaseDirectory + "ttt" + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(r);
            if (files != null)
            {

                foreach (var f in files)
                {
                    try
                    {
                        f.SaveAs(r + f.FileName);
                        var res = UploadImage(r + f.FileName, "", "local", Global.ServerToken, Global.UploadPath);
                        Response.Write(r + f.FileName + ":" + res + "<br>");
                        File.Delete(r + f.FileName);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(r + f.FileName + ":" + ex.Message + "<br>");
                    }
                }

            }
        }
    }
}