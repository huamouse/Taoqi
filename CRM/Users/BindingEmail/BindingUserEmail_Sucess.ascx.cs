using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users.BindingEmail
{
    public partial class BindingUserEmail_Sucess : System.Web.UI.UserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Literal1.Text = Security.EMAIL1;
        }
    }
}