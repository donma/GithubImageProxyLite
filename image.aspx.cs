using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GithubImageLite
{
    public partial class image : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"]))
            {
                Response.Redirect(Global.ImageNotFound);
                return;
            }

            var data = Global._Role.GetQ<Models.ImageInfo>("IMAGES").DataByKey(Request["id"].ToLower());

            if (data == null)
            {
                Response.Redirect(Global.ImageNotFound);
                return;
            }

            Response.Redirect(data.GitPath);

        }
    }
}