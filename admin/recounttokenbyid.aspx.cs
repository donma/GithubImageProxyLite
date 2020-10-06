using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite.admin
{
    public partial class recounttokenbyid : System.Web.UI.Page
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

            var info = Global._Role.GetQ<Models.GitToken>("TOKENS").DataByKey(Request["id"]);
            if (info == null)
            {
                Response.Write("error:token not found");
                return;
            }


            Global.ReCountTokenId(info.Id);

            Response.Write("success");
            return;
        }
    }
}