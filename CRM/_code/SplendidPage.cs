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
using System.Threading;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Taoqi._controls;

namespace Taoqi
{
	/// <summary>
	/// Summary description for SplendidPage.
	/// </summary>
	public class SplendidPage : System.Web.UI.Page
	{
		protected bool     bDebug = false;
		// 08/29/2005   Only store the absolute minimum amount of data.  
		// This means remove the data that is accessable from the Security object. 
		// The security data is not accessed frequently enough to justify initialization in every user control. 
		// High frequency objects are L10N and TimeZone. 
		protected string   m_sCULTURE    ;
		protected string   m_sDATEFORMAT ;
		protected string   m_sTIMEFORMAT ;
		protected Guid     m_gTIMEZONE   ;
		protected bool     m_bPrintView  = false;
		protected bool     m_bIsAdminPage = false;

		// L10n is an abbreviation for Localization (between the L & n are 10 characters. 
		protected L10N     L10n          ;  // 08/28/2005   Keep old L10n name, and rename the object to simplify updated approach. 
		protected TimeZone T10n          ;
		protected Currency C10n          ;

		public SplendidPage()
		{
			this.PreInit += new EventHandler(Page_PreInit);
		}

		public bool PrintView
		{
			get
			{
				return m_bPrintView;
			}
			set
			{
				m_bPrintView = value;
				Context.Items["PrintView"] = m_bPrintView;
			}
		}

		public bool IsAdminPage
		{
			get
			{
				return m_bIsAdminPage;
			}
			set
			{
				m_bIsAdminPage = value;
			}
		}

		public void SetMenu(string sMODULE)
		{
			// 02/25/2010   Use Items instead of manually injecting the ActiveTab. 
			Page.Items["ActiveTabMenu"] = sMODULE;
			/*
			// 01/20/2007   Move the menu code to a fuction so that will only get called in EditView, DetailView and ListView controls. 
			// 01/19/2007   If a MasterPage is in use, then we need to set the ActiveTab. 
			if ( !String.IsNullOrEmpty(sMODULE) )
			{
				if ( Master != null )
				{
					Taoqi.Themes.Sugar.TabMenu ctlTabMenu = Master.FindControl("ctlTabMenu") as Taoqi.Themes.Sugar.TabMenu;
					if ( ctlTabMenu != null )
					{
						// 01/20/2007   Only set the first control as each 
						// SplendidControl on page will pass through this code. 
						if ( String.IsNullOrEmpty(ctlTabMenu.ActiveTab) )
							ctlTabMenu.ActiveTab = sMODULE;
					}
				}
			}
			*/
		}

		// 07/24/2010   We need an admin flag for the areas that don't have a record in the Modules table. 
		public void SetAdminMenu(string sMODULE)
		{
			Page.Items["ActiveTabMenu"] = sMODULE;
			Page.Items["ActiveTabMenu.IsAdmin"] = true;
		}

		public void SetPageTitle(string sTitle)
		{
			// 01/20/2007   Wrap the page title function to minimized differences between Web1.2.
			Page.Title = sTitle;
		}

		// 03/07/2008   There is a better time to initialize the culture. 
		protected override void InitializeCulture()
		{
			// 08/30/2005   Move the L10N creation to this get function so that the first control 
			// that gets created will cause the creation of L10N.  The UserControls get the OnInit event before the Page onInit event. 
			// 03/07/2008   The page lifecycle has been designed to always call InitializeCulture before the page itself 
			// or any of its child controls have done any work with localized resources.
			m_sCULTURE     = Sql.ToString (Session["USER_SETTINGS/CULTURE"   ]);
			m_sDATEFORMAT  = Sql.ToString (Session["USER_SETTINGS/DATEFORMAT"]);
			m_sTIMEFORMAT  = Sql.ToString (Session["USER_SETTINGS/TIMEFORMAT"]);

			L10n = new L10N(m_sCULTURE);
			// 03/07/2008   We need to set the page culture so that the AJAX engine will initialize Sys.CultureInfo.CurrentCulture. 
			try
			{
				this.Culture = L10n.NAME;
			}
			catch
			{
				// 08/19/2013   An invalid default language can crash the app.  Always default to English. 
				// Don't log the error as it would be generated for every page request. 
				this.Culture = "en-US";
			}

			// 08/30/2005   Move the TimeZone creation to this get function so that the first control 
			// that gets created will cause the creation of TimeZone.  The UserControls get the OnInit event before the Page onInit event. 
			m_gTIMEZONE = Sql.ToGuid(Session["USER_SETTINGS/TIMEZONE"]);
			T10n = TimeZone.CreateTimeZone(m_gTIMEZONE);
			if ( T10n.ID != m_gTIMEZONE )
			{
				// 08/30/2005  If we are using a default, then update the session so that future controls will be quicker. 
				m_gTIMEZONE = T10n.ID ;
				Session["USER_SETTINGS/TIMEZONE"] = m_gTIMEZONE.ToString() ;
			}

			Guid gCURRENCY_ID = Sql.ToGuid(Session["USER_SETTINGS/CURRENCY"]);
			C10n = Currency.CreateCurrency(gCURRENCY_ID);
			if ( C10n.ID != gCURRENCY_ID )
			{
				// 05/09/2006  If we are using a default, then update the session so that future controls will be quicker. 
				gCURRENCY_ID = C10n.ID;
				Session["USER_SETTINGS/CURRENCY"] = gCURRENCY_ID.ToString();
				
			}
			// 08/05/2006   We cannot set the CurrencyDecimalSeparator directly on Mono as it is read-only.  
			// Hold off setting the CurrentCulture until we have updated all the settings. 
			CultureInfo culture = CultureInfo.CreateSpecificCulture(L10n.NAME);
			culture.DateTimeFormat.ShortDatePattern = m_sDATEFORMAT;
			culture.DateTimeFormat.ShortTimePattern = m_sTIMEFORMAT;
			// 05/29/2013   LongTimePattern is used in ListView. 
			culture.DateTimeFormat.LongTimePattern  = m_sTIMEFORMAT;

			// 03/30/2007   Always set the currency symbol.  It is not retained between page requests. 
			// 07/28/2006   We cannot set the CurrencySymbol directly on Mono as it is read-only.  
			// 03/07/2008   Move all localization to InitializeCulture(). 
			// Just clone the culture and modify the clone. 
			// CultureInfo culture = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
			culture.NumberFormat.CurrencySymbol   = C10n.SYMBOL;

			// 05/09/2006   Initialize the numeric separators. 
			// 03/07/2008   We are going to stop allowing the user to set the number separators. 
			// It does not work well when we allow the user to change the language using a drop-down. 
			// We are now using Microsoft AJAX localization, so there is no longer a need to manage the localization ourselves. 

			//string sGROUP_SEPARATOR   = Sql.ToString(Session["USER_SETTINGS/GROUP_SEPARATOR"  ]);
			//string sDECIMAL_SEPARATOR = Sql.ToString(Session["USER_SETTINGS/DECIMAL_SEPARATOR"]);
			// 06/03/2006   Setting the separators is causing some users a problem.  It may be because the strings were empty. 
			// 02/29/2008   The config value should only be used as an override.  We should default to the .NET culture value. 
			//if ( !Sql.IsEmptyString(sGROUP_SEPARATOR  ) ) culture.NumberFormat.CurrencyGroupSeparator   = sGROUP_SEPARATOR  ;
			//if ( !Sql.IsEmptyString(sDECIMAL_SEPARATOR) ) culture.NumberFormat.CurrencyDecimalSeparator = sDECIMAL_SEPARATOR;
			//if ( !Sql.IsEmptyString(sGROUP_SEPARATOR  ) ) culture.NumberFormat.NumberGroupSeparator     = sGROUP_SEPARATOR  ;
			//if ( !Sql.IsEmptyString(sDECIMAL_SEPARATOR) ) culture.NumberFormat.NumberDecimalSeparator   = sDECIMAL_SEPARATOR;

			// 08/30/2005  We don't need the long time pattern because we simply do not use it. 
			//Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern  = m_sTIMEFORMAT;
			//Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
			//08/05/2006   Apply the modified cultures. 
			Thread.CurrentThread.CurrentCulture   = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
		}

		public L10N GetL10n()
		{
			return L10n;
		}

		public TimeZone GetT10n()
		{
			return T10n;
		}

		public Currency GetC10n()
		{
			return C10n;
		}

		// 11/19/2005   Default to expiring everything. 
		virtual protected bool AuthenticationRequired()
		{
			return true;
		}

		virtual protected bool AdminPage()
		{
			return false;
		}

		public bool IsMobile
		{
			get
			{
				return (this.Theme == "Mobile");
			}
		}

		protected void AppendDetailViewRelationships(string sDETAIL_NAME, PlaceHolder plc)
		{
			AppendDetailViewRelationships(sDETAIL_NAME, plc, Guid.Empty);
		}

		// 07/10/2009   We are now allowing relationships to be user-specific. 
		// 03/08/2014   Provide public access so that it can be called from the Seven master page. 
		public void AppendDetailViewRelationships(string sDETAIL_NAME, PlaceHolder plc, Guid gUSER_ID)
		{
			// 11/17/2007   Convert all view requests to a mobile request if appropriate.
			sDETAIL_NAME = sDETAIL_NAME + (this.IsMobile ? ".Mobile" : "");
			//int nPlatform = (int) Environment.OSVersion.Platform;
			DataTable dtFields = null;
			if ( Sql.IsEmptyGuid(gUSER_ID) )
				dtFields = SplendidCache.DetailViewRelationships(sDETAIL_NAME);
			else
				dtFields = SplendidCache.UserDashlets(sDETAIL_NAME, gUSER_ID);
			// 03/04/2010   The update panel makes debugging dashlets difficult.  Provide a way to disable the update panel. 
			bool bDebugDashlets = Sql.ToBoolean(Application["CONFIG.debug_dashlets"]);
			foreach(DataRow row in dtFields.Rows)
			{
				Guid   gDASHLET_ID   = Sql.ToGuid  (row["ID"          ]);
				string sMODULE_NAME  = Sql.ToString(row["MODULE_NAME" ]);
				string sCONTROL_NAME = Sql.ToString(row["CONTROL_NAME"]);
				// 04/27/2006   Only add the control if the user has access. 
				if ( Security.GetUserAccess(sMODULE_NAME, "list") >= 0 )
				{
					try
					{
						// 09/21/2008   Mono does not fully support AJAX at this time. 
						// 09/22/2008   The UpdatePanel is no longer crashing Mono, so resume using it. 
						//if ( nPlatform == 4 || nPlatform == 128 )
						//{
						//	Control ctl = LoadControl(sCONTROL_NAME + ".ascx");
						//	plc.Controls.Add(ctl);
						//}
						//else
						//{
							// 05/02/2008   Put an update panel around all sub panels. This will allow in-place pagination and sorting. 
							// 07/10/2009   If this is a Dashlet, then set the DetailView name. 
							Control ctl = LoadControl(sCONTROL_NAME + ".ascx");
							DashletControl ctlDashlet = ctl as DashletControl;
							if ( ctlDashlet != null )
							{
								ctlDashlet.DashletID  = gDASHLET_ID;
								ctlDashlet.DetailView = sDETAIL_NAME;
							}
							if ( bDebugDashlets )
							{
								plc.Controls.Add(ctl);
							}
							else
							{
								UpdatePanel pnl = new UpdatePanel();
								// 05/06/2010   Try using the UpdatePanel Conditional mode. 
								pnl.UpdateMode = UpdatePanelUpdateMode.Conditional;
								plc.Controls.Add(pnl);
								pnl.ContentTemplateContainer.Controls.Add(ctl);
							}
						//}
					}
					catch(Exception ex)
					{
						Label lblError = new Label();
						// 06/09/2006   Catch the error and display a message instead of crashing. 
						// 12/27/2008   Don't specify an ID as there can be multiple errors. 
						lblError.Text            = Utils.ExpandException(ex);
						lblError.ForeColor       = System.Drawing.Color.Red;
						lblError.EnableViewState = false;
						plc.Controls.Add(lblError);
					}
				}
			}
		}

		protected void AppendGridColumns(SplendidGrid grd, string sGRID_NAME)
		{
			AppendGridColumns(grd, sGRID_NAME, null);
		}

		// 02/08/2008   We need to build a list of the fields used by the search clause. 
		protected void AppendGridColumns(SplendidGrid grd, string sGRID_NAME, UniqueStringCollection arrSelectFields)
		{
			// 11/17/2007   Convert all view requests to a mobile request if appropriate.
			sGRID_NAME = sGRID_NAME + (this.IsMobile ? ".Mobile" : "");
			grd.AppendGridColumns(sGRID_NAME, arrSelectFields);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			if ( AdminPage() )
			{
				// 02/13/2007   The Shortcuts are in the master page. 
				Utils.AdminShortcuts(this, true);
			}
			if ( !IsPostBack )
			{
				Page.DataBind();
			}
		}

		protected override void OnInit(EventArgs e)
		{
			this.Load += new System.EventHandler(this.Page_Load);
			if ( Request["PrecompileOnly"] == "1" )
				Response.End();
			if ( Sql.IsEmptyString(Application["imageURL"]) )
			{
				SplendidInit.InitSession(HttpContext.Current);
			}
			if ( AuthenticationRequired() )
			{
				// 11/17/2007   New function to determine if user is authenticated. 
				if ( !Security.IsAuthenticated() )
				{
					// 05/22/2008   Save the URL and redirect after login. 
					// 08/15/2008   Mono does not support ClientQueryString in its current 1.9.1 release, though it will be supported in the next build. 
					// For now, just use Request.Url.Query instead.  (Mono actually uses Request.UrlComponents.Query). 
					// Request.Url.Query is actually better because it includes the ?.
					// 11/06/2009   If this is an offline client installation, then redirect to the client login page. 
					if ( Utils.IsOfflineClient )
						Response.Redirect("~/Users/ClientLogin.aspx?Redirect=" + Server.UrlEncode(Page.AppRelativeVirtualPath + Request.Url.Query));
					else
						Response.Redirect("~/Users/Login.aspx?Redirect=" + Server.UrlEncode(Page.AppRelativeVirtualPath + Request.Url.Query));
				}
				// 02/22/2011   If this is a System Generated or Expired password, then force the user to change the password. 
				// 02/22/2011   The user cannot change the password on the Offline Client. 
				else if ( Sql.ToBoolean(Session["SYSTEM_GENERATED_PASSWORD"]) && !Utils.IsOfflineClient )
				{
					// 03/05/2011   We need to make sure not to redirect if already on the password expired page, otherwise we get into an endless loop. 
					if ( Page.AppRelativeVirtualPath != "~/Users/PasswordExpired.aspx" )
						Response.Redirect("~/Users/PasswordExpired.aspx?Redirect=" + Server.UrlEncode(Page.AppRelativeVirtualPath + Request.Url.Query));
				}
			}
			// 11/27/2006   We want to show the SQL on the Demo sites, so add a config variable to allow it. 
			bDebug = Sql.ToBoolean(Application["CONFIG.show_sql"]);
#if DEBUG
			bDebug = true;
#endif
			
			// 08/30/2005   Apply the new culture at the page level so that it is only applied once. 
			// 03/11/2008   GetL10n was getting called twice. No real harm, just not ideal. 
			// 04/30/2006   Use the Context to store pointers to the localization objects.
			// This is so that we don't need to require that the page inherits from SplendidPage. 
			// A port to DNN prompted this approach. 
			Context.Items["L10n"] = GetL10n();
			Context.Items["T10n"] = GetT10n();
			Context.Items["C10n"] = GetC10n();
			Context.Items["PrintView"] = m_bPrintView;
			base.OnInit(e);
		}
		
		protected void Page_PreInit(object sender, EventArgs e)
		{
            //2015.1.15
            if (Request.QueryString["isDlg"] != null)
            {
                this.MasterPageFile = "~/PopupView.Master";
            }

			//if ( Request["PrintView"] == "true" )
			//	this.MasterPageFile = "~/PrintView.master";
			string sTheme = Sql.ToString(Session["USER_SETTINGS/THEME"]);
			if ( String.IsNullOrEmpty(sTheme) )
			{
				// 07/01/2008   Check default theme.  The default will fall-back to Sugar. 
				sTheme = SplendidDefaults.Theme();
			}
			this.Theme = sTheme;
			if ( !String.IsNullOrEmpty(this.MasterPageFile) )
			{
				if ( !this.MasterPageFile.Contains("/App_MasterPages/") )
				{
					string sFileName = System.IO.Path.GetFileName(this.MasterPageFile);
					this.MasterPageFile = "~/App_MasterPages/" + sTheme + "/" + sFileName;
				}
			}
		}
	}

	// 02/08/2008   Create SplendidPopup so that m_sMODULE can be used in popups just as it is used in SplendidControls.
	/// <summary>
	/// Summary description for SplendidPopup.
	/// </summary>
	public class SplendidPopup : SplendidPage
	{
		protected string   m_sMODULE;  // 02/08/2008   Leave null so that we can get an error when not initialized. 
	}

	// 03/12/2008   Create SplendidAdminPage so that we can eliminate code behinds for admin default, view and edit pages.
	/// <summary>
	/// Summary description for SplendidAdminPage.
	/// </summary>
	public class SplendidAdminPage : SplendidPage
	{
		// 03/11/2008   Enable admin shortcuts. 
		override protected bool AdminPage()
		{
			return true;
		}
	}

	public class SplendidMaster : System.Web.UI.MasterPage
	{
		public virtual void Page_Command(object sender, CommandEventArgs e)
		{
		}
	}
}


