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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.Administration.SystemLog
{
	/// <summary>
	///		Summary description for ListView.
	/// </summary>
	public class ListView : SplendidControl
	{
		protected _controls.ExportHeader ctlExportHeader;
		protected _controls.SearchView   ctlSearchView  ;

		protected UniqueStringCollection arrSelectFields;
		protected DataView      vwMain         ;
		protected SplendidGrid  grdMain        ;
		protected Label         lblError       ;

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Search" )
				{
					// 10/13/2005   Make sure to clear the page index prior to applying search. 
					grdMain.CurrentPageIndex = 0;
					// 04/27/2008   Sorting has been moved to the database to increase performance. 
					grdMain.DataBind();
				}
				// 12/14/2007   We need to capture the sort event from the SearchView. 
				else if ( e.CommandName == "SortGrid" )
				{
					grdMain.SetSortFields(e.CommandArgument as string[]);
					// 04/27/2008   Sorting has been moved to the database to increase performance. 
					// 03/17/2011   We need to treat a comma-separated list of fields as an array. 
					arrSelectFields.AddFields(grdMain.SortColumn);
				}
				else if ( e.CommandName == "Export" )
				{
					// 11/03/2006   Apply ACL rules to Export. 
					int nACLACCESS = Taoqi.Security.AdminUserAccess(m_sMODULE, "export");
					if ( nACLACCESS  >= 0 )
					{
						// 10/05/2009   When exporting, we may need to manually bind.  Custom paging should be disabled when exporting all. 
						if ( vwMain == null )
							grdMain.DataBind();
						string[] arrID = null;  // 11/27/2010   Checkbox selection is not supported for this module. 
						SplendidExport.Export(vwMain, m_sMODULE, ctlExportHeader.ExportFormat, ctlExportHeader.ExportRange, grdMain.CurrentPageIndex, grdMain.PageSize, arrID, grdMain.AllowCustomPaging);
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				lblError.Text = ex.Message;
			}
		}

		protected void grdMain_OnItemCreated(object sender, DataGridItemEventArgs e)
		{
			if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
			{
				DataRowView row = e.Item.DataItem as DataRowView;
				if ( row != null )
				{
					string sERROR_TYPE = Sql.ToString(row["ERROR_TYPE"]);
					if ( sERROR_TYPE == "Error" )
					{
						e.Item.CssClass = "error";
					}
				}
			}
		}

		// 09/19/2009   Add support for custom paging. 
		protected void grdMain_OnSelectMethod(int nCurrentPageIndex, int nPageSize)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = "  from vwSYSTEM_LOG" + ControlChars.CrLf
					                + " where 1 = 1       " + ControlChars.CrLf;
					ctlSearchView.SqlSearchClause(cmd);
					cmd.CommandText = "select " + Sql.FormatSelectFields(arrSelectFields)
					                + cmd.CommandText;
					if ( nPageSize > 0 )
					{
						Sql.PageResults(cmd, "SYSTEM_LOG", grdMain.OrderByClause(), nCurrentPageIndex, nPageSize);
					}
					else
					{
						cmd.CommandText += grdMain.OrderByClause();
					}
					
					if ( bDebug )
						RegisterClientScriptBlock("SQLPaged", Sql.ClientScriptBlock(cmd));
					
					// 01/13/2010   Allow default search to be disabled. 
					if ( PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE) )
					{
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							using ( DataTable dt = new DataTable() )
							{
								da.Fill(dt);
								vwMain = dt.DefaultView;
								grdMain.DataSource = vwMain ;
							}
						}
						ctlExportHeader.Visible = true;
					}
					else
					{
						ctlExportHeader.Visible = false;
					}
				}
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term("Administration.LBL_SYSTEM_LOG_TITLE"));
			// 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			// 03/10/2010   Apply full ACL security rules. 
			this.Visible = (Taoqi.Security.AdminUserAccess(m_sMODULE, "list") >= 0);
			if ( !this.Visible )
			{
				// 03/17/2010   We need to rebind the parent in order to get the error message to display. 
				Parent.DataBind();
				return;
			}

			try
			{
				// 09/19/2009   Add support for custom paging in a DataGrid. Custom paging can be enabled / disabled per module. 
				if ( Crm.Config.allow_custom_paging() )
				{
					// 10/05/2009   We need to make sure to disable paging when exporting all. 
					grdMain.AllowCustomPaging = (Request.Form[ctlExportHeader.ExportUniqueID] == null || ctlExportHeader.ExportRange != "All");
					grdMain.SelectMethod     += new SelectMethodHandler(grdMain_OnSelectMethod);
				}

				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						grdMain.OrderByClause("DATE_ENTERED", "desc");
						
						cmd.CommandText = "  from vwSYSTEM_LOG" + ControlChars.CrLf
						                + " where 1 = 1       " + ControlChars.CrLf;
						ctlSearchView.SqlSearchClause(cmd);
						// 09/19/2009   Custom paging will always require two queries, the first is to get the total number of rows. 
						if ( grdMain.AllowCustomPaging )
						{
							cmd.CommandText = "select count(*)" + ControlChars.CrLf
							                + cmd.CommandText;
							
							if ( bDebug )
								RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));
							
							// 01/13/2010   Allow default search to be disabled. 
							if ( PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE) )
							{
								grdMain.VirtualItemCount = Sql.ToInteger(cmd.ExecuteScalar());
							}
						}
						else
						{
							// 04/27/2008   The fields in the search clause need to be prepended after any Saved Search sort has been determined.
							// 09/08/2009   The order by clause is separate as it can change due to SearchViews. 
							// 04/20/2011   If there are no fields in the GridView.Export, then return all fields (*). 
							cmd.CommandText = "select " + (Request.Form[ctlExportHeader.ExportUniqueID] != null ? SplendidDynamic.ExportGridColumns(m_sMODULE + ".Export") : Sql.FormatSelectFields(arrSelectFields))
							                + cmd.CommandText
							                + grdMain.OrderByClause();
							
							if ( bDebug )
								RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));
							
							// 01/13/2010   Allow default search to be disabled. 
							if ( PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE) )
							{
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dt = new DataTable() )
									{
										da.Fill(dt);
										vwMain = dt.DefaultView;
										grdMain.DataSource = vwMain ;
									}
								}
								ctlExportHeader.Visible = true;
							}
							else
							{
								ctlExportHeader.Visible = false;
							}
						}
					}
				}
				if ( !IsPostBack )
				{
					// 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
					//Page.DataBind();
					// 10/03/2009   Let the grid handle the differences between normal and custom paging. 
					// 10/03/2009   Bind outside of the existing connection so that a second connect would not get created. 
					grdMain.DataBind();
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				lblError.Text = ex.Message;
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
			ctlSearchView  .Command += new CommandEventHandler(Page_Command);
			ctlExportHeader.Command += new CommandEventHandler(Page_Command);
			// 11/26/2005   Add fields early so that sort events will get called. 
			// 03/18/2010   Change base module to SystemLog. 
			m_sMODULE = "SystemLog";
			SetMenu(m_sMODULE);
			// 02/08/2008   We need to build a list of the fields used by the search clause. 
			arrSelectFields = new UniqueStringCollection();
			arrSelectFields.Add("DATE_ENTERED"    );
			arrSelectFields.Add("USER_ID"         );
			arrSelectFields.Add("USER_NAME"       );
			arrSelectFields.Add("MACHINE"         );
			arrSelectFields.Add("ASPNET_SESSIONID");
			arrSelectFields.Add("REMOTE_HOST"     );
			arrSelectFields.Add("SERVER_HOST"     );
			arrSelectFields.Add("TARGET"          );
			arrSelectFields.Add("ERROR_TYPE"      );
			arrSelectFields.Add("MESSAGE"         );
			arrSelectFields.Add("FILE_NAME"       );
			arrSelectFields.Add("METHOD"          );
			arrSelectFields.Add("LINE_NUMBER"     );
			arrSelectFields.Add("RELATIVE_PATH"   );
			arrSelectFields.Add("PARAMETERS"      );
		}
		#endregion
	}
}


