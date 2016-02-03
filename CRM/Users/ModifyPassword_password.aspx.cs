using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using Taoqi.Controllers;

namespace Taoqi.Users
{
    public partial class ModifyPassword_password : SplendidPage
    {
        private string userName;
        private string old_Password;
        private string new_Password;
        private string new_ReapeterPassword;
        private Guid gID = Guid.Empty;

        public int Tick { get; set; }   // 倒计时时间（s）

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                userName = Security.USER_NAME;

                old_Password = OldPassword.Text;
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
            if (string.IsNullOrEmpty(Security.UserMobile))
            {         
                //Page.RegisterClientScriptBlock("js1", "<script>alert('提示：您没有手机号信息，无法使用手机验证修改密码。');window.location = window.location.href;</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "js1", "<script>alert('提示：您没有手机号信息，无法使用手机验证修改密码。');window.location = window.location.href;</script>");
            }
            else
            {
                Response.Redirect("~/users/ModifyPassword_phone.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string code = verificationCode.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(code))
            {
                this.lblError.Text = "请输入图片验证码";
                return;
            }
            else if (code != HttpContext.Current.Session["verificationCode"].ToString())
            {
                this.lblError.Text = "图片验证码不正确";
                return;
            }

            if (new_Password.Length == 0 || old_Password.Length == 0)
            {
                this.lblError.Text = "提示：请输入密码。";
                return;
            }

            if (new_Password.Length < 6)
            {
                this.lblError.Text = "提示：密码不应该小于6位。";
                return;
            }

            if (new_Password != new_ReapeterPassword)
            {
                this.lblError.Text = "提示：两次输入密码不一致。";
                return;
            }

            bool bValidUser = SplendidInit.LoginUser(userName, old_Password, String.Empty, String.Empty, String.Empty, false, false);

            if (bValidUser)
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
            else
            {
                this.lblError.Text = "提示：输入的原密码不正确。";
                return;
            }
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