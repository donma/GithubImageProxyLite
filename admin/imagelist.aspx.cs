using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite.admin
{
    public partial class imagelist : System.Web.UI.Page
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
                datas = datas.Where(x => x.Id.Contains( Request["id"])).ToList();
            }

            if (!string.IsNullOrEmpty(Request["owner"]))
            {
                datas = datas.Where(x => x.Owner.Contains(Request["owner"])).ToList();
            }
     


            Response.Write(JsonConvert.SerializeObject(datas, Formatting.Indented));
        }
    }
}