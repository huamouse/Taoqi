using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Taoqi.Controllers;

namespace Taoqi.Users
{
    public partial class ModifyPassword_phone : SplendidPage
    {
        private string userName;
        private string phone;
        private string new_Password;
        private string new_ReapeterPassword;
        private Guid gID = Guid.Empty;

        //protected bool refreshRandCode

        public int Tick { get; set; }   // 倒计时时间（s）

        protected void Page_Load(object sender, EventArgs e)
        {
            phone = lblUSER_PHONE.Text = Security.UserMobile;

            //如果用户的电话信息不存在，则跳转
            if (phone == string.Empty)
            {
                Response.Redirect("~/users/Login.aspx");
            }

            if (IsPostBack)
            {
                userName = Security.USER_NAME;

                new_Password = NewPassword.Text;
                new_ReapeterPassword = RepeatNewPassword.Text;
            }
            else
            {
                verificationImg.Src = "../account/verifycode.aspx";
            }
        }

        protected void Radio_modifyType_CheckedChanged(object sender, EventArgs e)
        {
            Response.Redirect("~/users/ModifyPassword_password.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string code = smsCode.Text.Trim();


            if (code.Length != 6)
            {
                lblError.Text = "提示：请输入6位短信验证码。";
                return;
            }
            else if (Session["smsCode"] == null)
            {
                lblError.Text = "提示：请获取短信验证码。";
                return;
            }
            else if (code != Session["smsCode"].ToString())
            {
                lblError.Text = "提示：请输入正确的短信验证码。";
                return;
            }
            else if (new_Password.Length == 0)
            {
                this.lblError.Text = "提示：请输入密码。";
                return;
            }
            else if (new_Password.Length < 6)
            {
                this.lblError.Text = "提示：密码不应该小于6位。";
                return;
            }
            else if (new_Password != new_ReapeterPassword)
            {
                this.lblError.Text = "提示：两次输入密码不一致。";
                return;
            }
            else //进入下一步
            {
                int sqlBreak = 0;

                //加密密码
                string hashPassword = Security.HashPassword(new_Password);

                SqlProcs.spTQUsers_resetPassword
                    (ref gID
                    , userName
                    , hashPassword
                    , ref sqlBreak
                    );

                if (sqlBreak == 0)
                {
                    lblError.Text = "用户不存在。";
                    return;
                }
                else if (sqlBreak == 1)
                {
                    //执行下一步
                    Response.Write("<script>alert('修改成功');window.location='./PersonalInfo.aspx';</script>");
                }
            }
        }

        protected void btnGetCode_Click(object sender, EventArgs e)
        {
            string code = verificationCode.Text.Trim().ToUpper();

            if (code == HttpContext.Current.Session["verificationCode"].ToString())
            {
                SMS sms = new SMS();
                sms.SendCode(phone);
                Tick = 120;

                if (sms.ErrorMessage == "成功")
                    lbSMS.Text = "短信验证已发送（2分钟内有效）。";
                else
                    lbSMS.Text = sms.ErrorMessage;
            }
            else
                lbSMS.Text = "图片验证码不正确";
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