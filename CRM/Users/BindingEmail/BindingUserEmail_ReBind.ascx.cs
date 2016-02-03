using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users.BindingEmail
{
    public partial class BindingUserEmail_ReBind : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Security.User_EmailIsActive)
            {
                this.lblUSER_EMail.Text = Security.EMAIL1;
            }
           
        }
    }
}