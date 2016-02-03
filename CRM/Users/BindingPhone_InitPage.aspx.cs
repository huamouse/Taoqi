using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users
{
    public partial class BindingPhone_InitPage : SplendidPage
    {
        protected string phone;
        protected void Page_Load(object sender, EventArgs e)
        {
            phone = lblUSER_PHONE.Text = Security.UserMobile;
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