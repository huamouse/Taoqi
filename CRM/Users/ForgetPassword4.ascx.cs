using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users
{
    public partial class ForgetPassword4 : System.Web.UI.UserControl
    {

        protected string GetFromPath()
        {
            string path = Context.Request["from"];

            if (path == "/member/users/login.aspx")
            {
                return path;
            }
            else
            {
                return "/index.html";
            }
        }

        public void ProcessHandler()
        {
            //通过浏览器头做出指定时间（s）后跳转
            string strRedirectTime = "5";
            string strRedirectPage = GetFromPath();
            string strRedirect = string.Format("{0};url={1}", strRedirectTime, strRedirectPage);
            Response.AddHeader("refresh", strRedirect); 
        }

    }
}