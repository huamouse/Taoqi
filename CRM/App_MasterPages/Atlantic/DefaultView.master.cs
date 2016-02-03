using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.Themes.Atlantic
{
	public partial class DefaultView : System.Web.UI.MasterPage
	{
		protected L10N         L10n;
		protected HtmlTable          tblLoginHeader;
       
        protected HyperLink lnkMyAccount;
        //protected HyperLink lnkAdmin;

        protected HtmlContainerControl adminPanel;
        

		public bool PrintView
		{
			get
			{
				bool bPrintView = Sql.ToBoolean(Context.Items["PrintView"]);
				return bPrintView;
			}
		}
		public L10N GetL10n()
		{
            
			if ( L10n == null )
			{

				L10n = Context.Items["L10n"] as L10N;
				if ( L10n == null )
				{
					string sCULTURE  = Sql.ToString(Session["USER_SETTINGS/CULTURE" ]);
					L10n = new L10N(sCULTURE);
				}
			}
			return L10n;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
            string header_UserIcon = Security.FULL_NAME;
            this.lnkMyAccount.Text = string.IsNullOrEmpty(header_UserIcon) ? Security.USER_NAME : header_UserIcon;

            
            if (Security.isAdmin || Security.IS_ADMIN_DELEGATE)
            {
                //this.lnkAdmin.Visible = true;
                this.adminPanel.Visible = true;
            }
            else
            {
                //this.lnkAdmin.Visible = false;
                this.adminPanel.Visible = true;
            }
            
			if ( !IsPostBack )
			{
				try
				{
					
					HtmlContainerControl htmlRoot = FindControl("htmlRoot") as HtmlContainerControl;
					if ( htmlRoot != null )
					{
						if ( L10n.IsLanguageRTL() )
						{
							htmlRoot.Attributes.Add("dir", "rtl");
						}
					}

				}
				catch
				{
				}
			}
			ScriptManager mgrAjax = ScriptManager.GetCurrent(this.Page);

			Utils.RegisterJQuery(Page, mgrAjax);


          
		}

		#region Web Form Designer generated code
		protected override void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			GetL10n();
			this.Load += new System.EventHandler(this.Page_Load);
			base.OnInit(e);
		}
		#endregion
	}
}


