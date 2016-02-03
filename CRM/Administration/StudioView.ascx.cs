/**********************************************************************************************************************
 * Taoqi is a Customer Relationship Management program created by Taoqi Software, Inc. 
 * Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved.
 * 
 * This program is free software: you can redistribute it and/or modify it under the terms of the 
 * GNU Affero General Public License as published by the Free Software Foundation, either version 3 
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 * See the GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>. 
 * 
 * You can contact Taoqi Software, Inc. at email address support@Taoqi.com. 
 * 
 * In accordance with Section 7(b) of the GNU Affero General Public License version 3, 
 * the Appropriate Legal Notices must display the following words on all interactive user interfaces: 
 * "Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved."
 *********************************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.Administration
{
	/// <summary>
	///		Summary description for StudioView.
	/// </summary>
	public class StudioView : SplendidControl
	{
		protected Label     lblError;
		protected Image     imgMODULE_BUILDER;
		protected HyperLink lnkMODULE_BUILDER;
		protected Label     lblMODULE_BUILDER;
		protected LinkButton lnkUpdateModel;

		// 09/11/2007   Provide quick access to team management flags. 
		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "System.RebuildAudit" )
				{
					// 12/31/2007   In case there is a problem, we need a way to rebuild the audit tables and triggers. 
					// 12/02/2009   Use a special version of spSqlBuildAllAuditTables that does not timeout. 
					Utils.BuildAllAuditTables();
				}
				else if ( e.CommandName == "System.RecompileViews" )
				{
					// 12/31/2007   Use a special version of spSqlRefreshAllViews that does not timeout. 
					Utils.RefreshAllViews();
				}
				else if ( e.CommandName == "System.UpdateModel" )
				{
					// 12/12/2009   Use a special version of spSEMANTIC_MODEL_Rebuild that does not timeout. 
					Utils.UpdateSemanticModel(this.Context);
				}
				else if ( e.CommandName == "System.Reload" )
				{
					// 01/18/2008   Speed the reload by doing directly instead of going to SystemCheck page. 
					// 10/26/2008   IIS7 Integrated Pipeline does not allow HttpContext access inside Application_Start. 
					SplendidInit.InitApp(HttpContext.Current);
					SplendidInit.LoadUserPreferences(Security.USER_ID, Sql.ToString(Session["USER_SETTINGS/THEME"]), Sql.ToString(Session["USER_SETTINGS/CULTURE"]));
				}
				// 09/11/2009   Make it easy to enable and disable custom paging. 
				else if ( e.CommandName == "CustomPaging.Enable"   )
				{
					SqlProcs.spCONFIG_Update("system", "allow_custom_paging", "true");
					Application["CONFIG.allow_custom_paging"] = true;
				}
				else if ( e.CommandName == "CustomPaging.Disable"  )
				{
					SqlProcs.spCONFIG_Update("system", "allow_custom_paging", "false");
					Application["CONFIG.allow_custom_paging"] = false;
				}
				Response.Redirect("default.aspx");
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				lblError.Text = ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( !IsPostBack )
			{
				// 09/12/2009   Only show the module builder if the files exist. 
				// 03/08/2010   The Module Builder can be disabled in the Web.config. 
				// 08/25/2013   File IO is slow, so cache existance test. 
				imgMODULE_BUILDER.Visible = Utils.CachedFileExists(Context, lnkMODULE_BUILDER.NavigateUrl) && !Sql.ToBoolean(Utils.AppSettings["DisableModuleBuilder"]);
				lnkMODULE_BUILDER.Visible = imgMODULE_BUILDER.Visible;
				lblMODULE_BUILDER.Visible = imgMODULE_BUILDER.Visible;

				// 12/12/2009   Only show update model link if this is the Enterprise or Professional edition. 
				// 08/07/2013   The Semantic Model is not being supported. 
				//string sServiceLevel = Sql.ToString(Application["CONFIG.service_level"]).ToLower();
				//lnkUpdateModel.Visible = (sServiceLevel == "enterprise" || sServiceLevel == "professional");
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}


