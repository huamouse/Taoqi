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
using System.Collections;

namespace Taoqi.Users
{
	public class ListView : SplendidControl
	{
		protected _controls.ExportHeader ctlExportHeader;
		protected _controls.SearchView   ctlSearchView  ;

		protected UniqueStringCollection arrSelectFields;
		protected DataView      vwMain         ;
		protected SplendidGrid  grdMain        ;
		protected Label         lblError       ;
		protected Label         lblMaxUsers    ;
        protected HtmlInputControl username;
        
		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Search" )
				{
					grdMain.CurrentPageIndex = 0;
					grdMain.DataBind();
				}
				else if ( e.CommandName == "SortGrid" )
				{
					grdMain.SetSortFields(e.CommandArgument as string[]);
					arrSelectFields.AddFields(grdMain.SortColumn);
				}
				else if ( e.CommandName == "Export" )
				{
					int nACLACCESS = Taoqi.Security.GetUserAccess(m_sMODULE, "export");
					if ( nACLACCESS  >= 0 )
					{
						if ( vwMain == null )
							grdMain.DataBind();
						string[] arrID = Request.Form.GetValues("chkMain");
						SplendidExport.Export(vwMain, m_sMODULE, ctlExportHeader.ExportFormat, ctlExportHeader.ExportRange, grdMain.CurrentPageIndex, grdMain.PageSize, arrID, grdMain.AllowCustomPaging);
					}
				}
				else if ( e.CommandName == "Impersonate" )
				{
					Guid gUSER_ID = Sql.ToGuid(e.CommandArgument);
					SplendidInit.LoginUser(gUSER_ID, "Impersonate");
					Session["USER_IMPERSONATION"] = true;
					Response.Redirect("~/Home/");
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				lblError.Text = ex.Message;
			}
		}

		protected void grdMain_OnSelectMethod(int nCurrentPageIndex, int nPageSize)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					string sTABLE_NAME = Crm.Modules.TableName(m_sMODULE);
					cmd.CommandText = "  from vw" + sTABLE_NAME + "_List" + ControlChars.CrLf
					                + " where 1 = 1"                      + ControlChars.CrLf;
					ctlSearchView.SqlSearchClause(cmd);
					cmd.CommandText = "select " + Sql.FormatSelectFields(arrSelectFields)
					                + cmd.CommandText;
					if ( nPageSize > 0 )
					{
						Sql.PageResults(cmd, sTABLE_NAME, grdMain.OrderByClause(), nCurrentPageIndex, nPageSize);
					}
					else
					{
						cmd.CommandText += grdMain.OrderByClause();
					}
					
					if ( bDebug )
						RegisterClientScriptBlock("SQLPaged", Sql.ClientScriptBlock(cmd));
					
					if ( PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE) )
					{
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							using ( DataTable dt = new DataTable() )
							{
								da.Fill(dt);
								this.ApplyGridViewRules(m_sMODULE + "." + LayoutListView, dt);
								
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
			SetPageTitle(L10n.Term(m_sMODULE + ".LBL_LIST_FORM_TITLE"));
			this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0) && (Security.isCompanyAdmin == 1);
			if ( !this.Visible ) return;

			try
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						grdMain.OrderByClause("C_Status", "asc");
                        cmd.CommandText = "  from vwTQAccount_List" + ControlChars.CrLf
                                        + " where C_ClientID = '" + Security.UserClientID + "' " + ControlChars.CrLf;

						ctlSearchView.SqlSearchClause(cmd);

                        if(!Sql.IsEmptyString(username.Value))
                        {
                            cmd.CommandText += "and USER_NAME like '%" + username.Value + "%' or LAST_NAME like '%" + username.Value + "%'";
                        }

						if ( grdMain.AllowCustomPaging )
						{
						    cmd.CommandText = "select count(*)" + ControlChars.CrLf
							                + cmd.CommandText;
							
						    if ( PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE) )
						    {
							    grdMain.VirtualItemCount = Sql.ToInteger(cmd.ExecuteScalar());
						    }
						}
						else
						{ 
							cmd.CommandText = "select " + Sql.FormatSelectFields(arrSelectFields)
							                + cmd.CommandText
							                + grdMain.OrderByClause();
						}

                        if (PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE))
						{
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								using ( DataTable dt = new DataTable() )
								{
									da.Fill(dt);
									this.ApplyGridViewRules(m_sMODULE + "." + LayoutListView, dt);

                                    vwMain = dt.DefaultView;
                                    grdMain.DataSource = vwMain;
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

                grdMain.DataBind();
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
            m_sMODULE = "TQAccount";
			SetMenu(m_sMODULE);
			arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("C_UserID");
            arrSelectFields.Add("C_Status");
			arrSelectFields.Add("isCompanyAdmin");
			this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);

            grdMain.ItemCommand += grdMain_ItemCommand;
		}
		#endregion

        void grdMain_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Guid gID;
            Guid.TryParse(e.CommandArgument.ToString(), out gID);

            if (e.CommandName == "Delete")
            {
                SqlProcs.spTQAccount_Delete(gID);
                Msg.BossAudit(gID);
                Response.Redirect(Request.RawUrl);
            }
            else if (e.CommandName == "Agree")
            {
                SqlProcs.spTQAccountStatus(gID, 1);
                Msg.BossAudit(gID, true);
                Response.Redirect(Request.RawUrl);
            }
            else if (e.CommandName == "Edit")
            {
                Response.Redirect(string.Format("./edit.aspx?id={0}", gID.ToString()));
            }
        }
	}
}


