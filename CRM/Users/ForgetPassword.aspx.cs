using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users
{
    public partial class ForgetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FP1.NSEvent += new ForgetPassword1.nextStep(FPVisible);
            FP2.NSEvent += new ForgetPassword2.nextStep(FPVisible);
            FP3.NSEvent += new ForgetPassword3.nextStep(FPVisible);

            if (!IsPostBack)
            {
                FP1.ProcessHandler();
            }
        }

        public void FPVisible(string whoVisible)
        {
            switch (whoVisible)
            {
                case "FP1": 
                    FP1.Visible = true;
                    FP2.Visible = false;
                    FP3.Visible = false;
                    FP4.Visible = false;

                    FP1.ProcessHandler();
                    break;
                case "FP2": 
                    FP1.Visible = false;
                    FP2.Visible = true;
                    FP3.Visible = false;
                    FP4.Visible = false;

                    FP2.ProcessHandler();
                    break;
                case "FP3": 
                    FP1.Visible = false;
                    FP2.Visible = false;
                    FP3.Visible = true;
                    FP4.Visible = false;
                    break;
                case "FP4": 
                    FP1.Visible = false;
                    FP2.Visible = false;
                    FP3.Visible = false;
                    FP4.Visible = true;

                    FP4.ProcessHandler();
                    break;
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