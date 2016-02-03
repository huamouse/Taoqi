using System;
using System.Web;

namespace Taoqi.Users
{
    public partial class BindingNewPhone : SplendidPage
    {
        public int Tick { get; set; }   // 倒计时时间（s）

        protected string phone;
        protected string NewPhone;

        protected void Page_Load(object sender, EventArgs e)
        {
            phone = lblUSER_PHONE1.Text = lblUSER_PHONE2.Text = Security.UserMobile;
            NewPhone = TXTNewPhone2.Text;
            //Tick = 0;

            if (!IsPostBack)
            {
                if(PlaceHolder1.Visible)
                    verificationImg1.Src = "../account/verifycode.aspx";
            }

            if (Session["TickValidityTime"] != null)
            {
                var tickValidityTime = Convert.ToDateTime(Session["TickValidityTime"]);

                if (tickValidityTime > DateTime.Now)
                {

                    this.Tick = (int)(tickValidityTime - DateTime.Now).TotalSeconds;

                }

            }
        }

        protected void btnLogin1_Click(object sender, EventArgs e)
        {
            string to = phone;
            string code = smsCode1.Text.Trim();

            if (to.Length != 11)
                lblError1.Text = "提示：请输入11位手机号码。";
            else if (code.Length != 6)
                lblError1.Text = "提示：请输入6位短信验证码。";
            else if (Session["smsCode"] == null)
                lblError1.Text = "提示：请获取短信验证码。";
            else if (code != Session["smsCode"].ToString())
                lblError1.Text = "提示：请输入正确的短信验证码。";
            else //进入下一步
            {
                PlaceHolder1.Visible = false;

                PlaceHolder2.Visible = true;
                verificationImg2.Src = "../account/verifycode.aspx";

                this.Tick = 0;
            }
        }

        protected void btnLogin2_Click(object sender, EventArgs e)
        {
            string to = NewPhone;
            string code = smsCode2.Text.Trim();

            if (to.Length != 11)
                lblError2.Text = "提示：请输入11位手机号码。";
            else if (code.Length != 6)
                lblError2.Text = "提示：请输入6位短信验证码。";
            else if (Session["smsCode"] == null)
                lblError2.Text = "提示：请获取短信验证码。";
            else if (code != Session["smsCode"].ToString())
                lblError2.Text = "提示：请输入正确的短信验证码。";
            else //进入下一步
            {
                Guid gID = Security.USER_ID;

                SqlProcs.spTQUsers_Update_BindingNewPhone(
                    ref gID,
                    NewPhone
                    );

                //更新Security中的数据，以便用户继续在CRM中操作不受影响，否则要做强制注销
                Security.UserMobile = NewPhone;

                PlaceHolder2.Visible = false;
                PlaceHolder3.Visible = true;

                TXTNewPhone3.Text = NewPhone;

                this.Tick = 0;
            }
        }

        protected void btnGetCode1_Click(object sender, EventArgs e)
        {
            string to = phone;
            //判断字符串是否为数字
            Int64 num = 0;
            if (!Int64.TryParse(to, out num))
            {
                lbSMS1.Text = "提示：请输入正确的手机号。";
                return;
            }

            string code = verificationCode1.Text.Trim().ToUpper();

            if (code == HttpContext.Current.Session["verificationCode"].ToString())
            {
                SMS sms = new SMS();
                sms.SendCode(to);
                Tick = 120;
                Session["TickValidityTime"] = DateTime.Now.AddMinutes(2);

                if (sms.ErrorMessage == "成功")
                    lbSMS1.Text = "短信验证已发送（10分钟内有效）。";
                else
                    lbSMS1.Text = sms.ErrorMessage;
            }
            else
                lbSMS1.Text = "图片验证码不正确";
        }

        protected void btnGetCode2_Click(object sender, EventArgs e)
        {
            string to = NewPhone;
            //判断字符串是否为数字
            Int64 num = 0;
            if (!Int64.TryParse(to, out num))
            {
                lbSMS2.Text = "提示：请输入正确的手机号。";
                return;
            }

            string code = verificationCode2.Text.Trim().ToUpper();

            if (code == HttpContext.Current.Session["verificationCode"].ToString())
            {
                SMS sms = new SMS();
                sms.SendCode(to);
                Tick = 120;
                Session["TickValidityTime"] = DateTime.Now.AddMinutes(2);

                if (sms.ErrorMessage == "成功")
                    lbSMS2.Text = "短信验证已发送（10分钟内有效）。";
                else
                    lbSMS2.Text = sms.ErrorMessage;
            }
            else
                lbSMS2.Text = "图片验证码不正确";
        }

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }

        #endregion Web Form Designer generated code
    }
}