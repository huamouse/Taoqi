using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Web.UI.HtmlControls;

namespace Taoqi.Users
{
    public partial class ClientInfo : SplendidPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Security.isCompany == 0)
            {
                if (Security.isBuyer == 1)
                    Response.Redirect("../Order/");
                else if (Security.isSeller == 1)
                    Response.Redirect("../OrderSell/");
                else if (Security.isAdmin)
                    Response.Redirect("../Client/");
                else
                    Response.Redirect("../Users/PersonalInfo.aspx");
            }
        }
    }
}