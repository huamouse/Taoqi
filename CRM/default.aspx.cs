using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi
{
	/// <summary>
	/// Summary description for Default.
	/// </summary>
	public class Default : SplendidPage
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			// 11/06/2009   If this is an offline client installation, then redirect to the client login page. 
			// 11/24/2009   We only need to link to the login if not authenticated. 
			if ( Utils.IsOfflineClient && !Security.IsAuthenticated() )
				Response.Redirect("~/Users/ClientLogin.aspx");
			else
				// 09/21/2008   Mono is case significant and all default pages are lower case. 
				Response.Redirect("~/Home/default.aspx");
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


