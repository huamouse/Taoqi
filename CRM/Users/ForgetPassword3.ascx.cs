using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users
{
    public partial class ForgetPassword3 : System.Web.UI.UserControl
    {
        public delegate void nextStep(string whoVisible);
        public event nextStep NSEvent;

        protected Guid gID = Guid.Empty;
        private string phone;
        private string userName;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.Session["FP_userName"] != null)
            {
                userName = Session["FP_userName"].ToString();
            }
        }

        protected void btnFP_Next(object sender, EventArgs e)
        {
            int sqlBreak = 0;
            string password = RegisterPassword.Text.Trim();
            string password2 = RegisterPassword2.Text.Trim();

            if (password.Length == 0)
            {
                this.lblError.Text = "提示：请输入密码。";
                return;
            }

            //if (password.Length < 6)
            //{
            //    this.lblError.Text = "提示：密码不应该小于6位。";
            //    return;
            //}

            if (password != password2)
            {
                this.lblError.Text = "提示：两次输入密码不一致。";
                return;
            }

            //加密密码
            string hashPassword = Security.HashPassword(password);

            SqlProcs.spTQUsers_resetPassword
                (ref gID
                , userName
                , hashPassword
                , ref sqlBreak
                );

            if (sqlBreak == 0)
            {
                lblError.Text = "用户不存在。";
            }
            else if (sqlBreak == 1)
            {
                //执行下一步
                NSEvent("FP4");
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
        #endregion

    }
}