using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite.admin
{
    public partial class images : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        

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


            var datas = Global._Role.GetQ<Models.ImageInfo>("IMAGES").AllDatasList();

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                datas = datas.Where(x => x.Id.Contains(Request["id"])).ToList();
            }

            if (!string.IsNullOrEmpty(Request["owner"]))
            {
                datas = datas.Where(x => x.Owner.Contains(Request["owner"])).ToList();
            }
            datas = datas.OrderByDescending(x => x.LastUpdate).ToList();
            var images = "<tr><td>Image</td><td>Id</td><td>Repo</td><td>Sizte</td><td>Date</td></tr>";
            ltlC.Text = "";

            foreach (var c in datas) {

                ltlC.Text += "<tr><td>"+"<img src='"+c.GitPath+"' style='width:100px' />"+ "</td><td><span style='color:red'>" + c.Id+
                    "</span></td><td><span style='color:borwn'>" +c.TokenId+"</span>"+
                    "</td><td><span style='color:gray'>" + c.Length.ToString("#.##")+ "</span></td><td><span style='color:dimgray'>" + c.LastUpdate.ToString("yyyy-MM-dd HH:mm")+"</span></td></tr>";
            }



        }
    }
}