using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users
{
    public partial class BindingUserEmail : SplendidPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = string.Empty;
            if (Request["validSucess"]=="1")
            {
                //加载成功 的页面
                url = "BindingUserEmail_Sucess.ascx";
                
            }
            else if (Request["NewReBind"] == "1")
            {
                url = "BindingUserEmail_New.ascx";
            }
            else if(Security.User_EmailIsActive)
            {
                url = "BindingUserEmail_ReBind.ascx";
            }
            else
            {
                url = "BindingUserEmail_New.ascx";
            }
            //动态加载个人组建
            UserControl uc = LoadControl(url) as UserControl;
            this.PLBindingEmail.Controls.Clear();
            this.PLBindingEmail.Controls.Add(uc);
        }
    }
}