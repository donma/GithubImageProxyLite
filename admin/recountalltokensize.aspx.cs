using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite.admin
{
    public partial class recountalltokensize : System.Web.UI.Page
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

            var tokens = Global._Role.GetQ<Models.GitToken>("TOKENS").AllDatasList();

            foreach (var t in tokens)
            {
                try
                {

                    Global.ReCountTokenId(t.Id);
                }
                catch
                {

                }
            }

            Response.Write("success");
            return;
        }
    }
}