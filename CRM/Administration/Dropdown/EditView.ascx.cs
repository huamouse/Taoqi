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
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.Administration.Dropdown
{
	/// <summary>
	///		Summary description for EditView.
	/// </summary>
	public class EditView : SplendidControl
	{
		protected _controls.ModuleHeader   ctlModuleHeader  ;
		protected _controls.DynamicButtons ctlDynamicButtons;
		// 01/13/2010   Add footer buttons. 
		protected _controls.DynamicButtons ctlFooterButtons ;

		protected TextBox         txtNAME                   ;
		// 09/18/2012   Allow selection of language. 
		protected DropDownList    lstLANGUAGE_OPTIONS       ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "Save" )
			{
				if ( Page.IsValid )
				{
					try
					{
						Guid gID = Guid.Empty;
						SqlProcs.spTERMINOLOGY_LIST_Insert(ref gID, String.Empty, lstLANGUAGE_OPTIONS.SelectedValue, String.Empty, txtNAME.Text, 1, String.Empty);
						// 01/20/2006   Clear the cache. 
						SplendidCache.ClearTerminologyPickLists();
						// 01/16/2006   If successful, go to dropdown editing. 
						Response.Redirect("default.aspx?DROPDOWN=" + txtNAME.Text + "&LANG=" + lstLANGUAGE_OPTIONS.SelectedValue);
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
						ctlDynamicButtons.ErrorText = ex.Message;
						return;
					}
				}
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term(".moduleList.Dropdown"));
			// 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			// 03/10/2010   Apply full ACL security rules. 
			this.Visible = (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
			{
				// 03/17/2010   We need to rebind the parent in order to get the error message to display. 
				Parent.DataBind();
				return;
			}
			if ( !IsPostBack )
			{
				// 09/18/2012   Allow selection of language. 
				lstLANGUAGE_OPTIONS.DataSource = SplendidCache.Languages();
				lstLANGUAGE_OPTIONS.DataBind();
				Utils.SetValue(lstLANGUAGE_OPTIONS, L10N.NormalizeCulture(L10n.NAME));
				// 03/20/2008   Dynamic buttons need to be recreated in order for events to fire. 
				ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
				ctlFooterButtons .AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This Task is required by the ASP.NET Web Form Designer.
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
			ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
			ctlFooterButtons .Command += new CommandEventHandler(Page_Command);
			m_sMODULE = "Dropdown";
			// 05/06/2010   The menu will show the admin Module Name in the Six theme. 
			SetMenu(m_sMODULE);
			if ( IsPostBack )
			{
				// 03/20/2008   Dynamic buttons need to be recreated in order for events to fire. 
				ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
				ctlFooterButtons .AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
			}
		}
		#endregion
	}
}


