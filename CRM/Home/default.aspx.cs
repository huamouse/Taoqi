using System;
using System.Data;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;


namespace Taoqi.Home
{
    /// <summary>
    /// Summary description for Default.
    /// </summary>
    public class Default : SplendidPage
    {
        private void Page_Load(object sender, System.EventArgs e)
        {
            //DbProviderFactory dbf = DbProviderFactories.GetFactory();
            //using (IDbConnection con = dbf.CreateConnection())
            //{
            //    con.Open();
            //    using (IDbCommand cmd = con.CreateCommand())
            //    {
            //        cmd.CommandText = "declare @n1 uniqueidentifier" + ControlChars.CrLf
            //        + "set @n1=(select C_ClientID from vwTQUsers) " + ControlChars.CrLf;
            //        cmd.CommandText = "if exists (select ID from vwTQClient_List where ID=n1) select '1' else select '0'";
            //        int n=Int32.Parse(cmd.CommandText);
            //        if(n == 1){
            //            Response.Redirect("../Order/", true);
            //        }
            //        else
            //        {
            //            Response.Redirect("../users/EditMyAccount.aspx", true);
            //        }
            //    }
            //}
            string RedirectURL = "../Users/PersonalInfo.aspx";

            if(Security.isAdmin)
                RedirectURL = "../Client/";
            else if (Security.isCompany == 0)
                RedirectURL = "../Users/PersonalInfo.aspx";
            else if(Security.isBuyer == 1)
                RedirectURL = "../Order/";
            else if (Security.isSeller == 1)
                RedirectURL = "../OrderSell/";

            Response.Redirect(RedirectURL, true);
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

        private void InitializeComponent()
        {

            SetMenu("Home");
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}


