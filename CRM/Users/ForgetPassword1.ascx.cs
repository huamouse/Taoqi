using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using Taoqi.Controllers;

namespace Taoqi.Users
{
    public partial class ForgetPassword1 : System.Web.UI.UserControl
    {
        public int Tick { get; set; }   // 倒计时时间（s）

        private Guid gId = Guid.Empty;

        public delegate void nextStep(string whoVisible);
        public event nextStep NSEvent;

        protected void btnFG_Click(object sender, EventArgs e)
        {
            string code = this.verificationCode.Text.Trim().ToUpper();

            //对于图片验证码的判断
            if (code == null)
            {
                this.lblError.Text = "请输入图片验证码";
                return;
            }
            else if (code != HttpContext.Current.Session["verificationCode"].ToString())
            {
                this.lblError.Text = "图片验证码不正确";
                return;
            }

            string userName = this.txtUSERNAME.Text.Trim();

            //存储用户名(手机号)
            Session["FP_userName"] = userName;

            NSEvent("FP2");
        }

        public void ProcessHandler()
        {
            verificationImg.Src = "../account/verifycode.aspx";

            if (Context.Session["FP_userName"] != null)
            {
                txtUSERNAME.Text = Session["FP_userName"].ToString();
                verificationCode.Text = "";
            }
        }
    }
}