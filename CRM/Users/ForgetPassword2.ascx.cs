using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users
{
    public partial class ForgetPassword2 : System.Web.UI.UserControl
    {
        public delegate void nextStep(string whoVisible);

        public event nextStep NSEvent;

        public int Tick { get; set; }   // 倒计时时间（s）

        protected void btnFG_Pre(object sender, EventArgs e)
        {
            NSEvent("FP1");
        }

        protected void btnFG_Next(object sender, EventArgs e)
        {
            string code = this.txtCode.Text.Trim();

            if (Session["smsCode"] == null)
            {
                this.lblError.Text = "提示：请获取短信验证码。";
                return;
            }

            if (Session["smsCode"] != null && code != Convert.ToString(Session["smsCode"]))
            {
                this.lblError.Text = "提示：请输入正确的短信验证码。";

                return;
            }

            //执行下一步
            NSEvent("FP3");
        }

        protected void btnGetCode_Click(object sender, EventArgs e)
        {
            string to = Session["FP_userName"].ToString();

            SMS sms = new SMS();
            sms.SendCode(to);
            Tick = 120;

            if (sms.ErrorMessage == "成功")
                lbSMS.Text = "短信验证已发送（10分钟内有效）。";
            else
                lbSMS.Text = sms.ErrorMessage;
        }

        public void ProcessHandler()
        {
            if (Context.Session["FP_userName"] != null)
            {
                //处理电话号码的显示格式
                string phone = Session["FP_userName"].ToString();
                phone = (phone == null || phone == "") ? "XXXXXXXXXXX" : phone;
                phone = string.Format("{0}*****{1}", phone.Substring(0, 3), phone.Substring(7, 4));

                lblUSER_PHONE.Text = phone;
            }
        }
    }
}