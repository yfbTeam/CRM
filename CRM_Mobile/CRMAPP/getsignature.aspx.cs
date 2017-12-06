using DDHelper.Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRMAPP
{
    public partial class getsignature : System.Web.UI.Page
    {
        public EnterPrise_MB signPackage = new EnterPrise_MB();
        protected void Page_Load(object sender, EventArgs e)
        {
            signPackage.GetConfig(Request);
            Response.Write(JsonConvert.SerializeObject(signPackage));
        }
    }
}