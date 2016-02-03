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

namespace Taoqi.Administration.EditCustomFields
{
	/// <summary>
	///		Summary description for ListView.
	/// </summary>
	public class ListView : SplendidControl
	{
		protected DataView      vwMain         ;
		protected SplendidGrid  grdMain        ;
		protected Label         lblError       ;
		protected string        sMODULE_NAME   ;
		protected SearchBasic          ctlSearch   ;
		protected NewRecord            ctlNewRecord;

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Search" )
				{
					if ( ctlNewRecord != null )
					{
						ctlNewRecord.Clear();
					}
				}
				else if ( e.CommandName == "NewRecord" )
				{
					FIELDS_META_DATA_Bind();
				}
				else if ( e.CommandName == "EditCustomFields.Delete" )
				{
					Guid gID = Sql.ToGuid(e.CommandArgument);

					// 07/18/2006   Manually create the command so that we can increase the timeout. 
					// 07/18/2006   Keep the original procedure call so that we will get a compiler error if something changes. 
					bool bIncreaseTimeout = true;
					if ( !bIncreaseTimeout )
					{
						SqlProcs.spFIELDS_META_DATA_Delete(gID);
					}
					else
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							con.Open();
							// 10/07/2009   We need to create our own global transaction ID to support auditing and workflow on SQL Azure, PostgreSQL, Oracle, DB2 and MySQL. 
							using ( IDbTransaction trn = Sql.BeginTransaction(con) )
							{
								try
								{
									using ( IDbCommand cmd = con.CreateCommand() )
									{
										cmd.Transaction = trn;
										cmd.CommandType = CommandType.StoredProcedure;
										cmd.CommandText = "spFIELDS_META_DATA_Delete";
										// 07/18/2006   Tripple the default timeout.  The operation was timing-out on QA machines and on the demo server. 
										// 02/03/2007   Increase timeout to 5 minutes.  It should not take that long, but some users are reporting a timeout. 
										// 07/01/2008   Let the function run forever. 
										cmd.CommandTimeout = 0;
										IDbDataParameter parID               = Sql.AddParameter(cmd, "@ID"              , gID                );
										IDbDataParameter parMODIFIED_USER_ID = Sql.AddParameter(cmd, "@MODIFIED_USER_ID",  Security.USER_ID  );
										cmd.ExecuteNonQuery();
									}
									trn.Commit();
								}
								catch(Exception ex)
								{
									trn.Rollback();
									throw(new Exception(ex.Message, ex.InnerException));
								}
							}
						}
						// 12/12/2009   Only update the semantic model if this is the Enterprise or Professional edition. 
						string sServiceLevel = Sql.ToString(Application["CONFIG.service_level"]).ToLower();
						if ( sServiceLevel == "enterprise" || sServiceLevel == "professional" )
						{
							// 12/12/2009   Update the Semantic Model in a background thread. 
							System.Threading.Thread t = new System.Threading.Thread(Utils.UpdateSemanticModel);
							t.Start(this.Context);
						}
					}
					// 01/10/2006   Clear the cache. 
					//SplendidCache.ClearFieldsMetaData(sMODULE_NAME);
					// 06/03/2008   We need to reload the entire application cache as there are many possible cached items. 
					// 10/26/2008   IIS7 Integrated Pipeline does not allow HttpContext access inside Application_Start. 
					SplendidInit.InitApp(HttpContext.Current);
					FIELDS_META_DATA_Bind();
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				lblError.Text = ex.Message;
			}
		}

		private void FIELDS_META_DATA_Bind()
		{
			try
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					string sSQL;
					sSQL = "select *                             " + ControlChars.CrLf
					     + "  from vwFIELDS_META_DATA_List       " + ControlChars.CrLf
					     + " where 1 = 1                         " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						if ( IsPostBack )
							ctlSearch.SqlSearchClause(cmd);
						else
							Sql.AppendParameter(cmd, sMODULE_NAME, "CUSTOM_MODULE");
						cmd.CommandText += " order by NAME" + ControlChars.CrLf;

						if ( bDebug )
							RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							using ( DataTable dt = new DataTable() )
							{
								da.Fill(dt);
								// 10/06/2005   Convert the term here so that sorting will apply. 
								foreach(DataRow row in dt.Rows)
								{
									row["REQUIRED_OPTION"] = L10n.Term(Sql.ToString(row["REQUIRED_OPTION"]));
								}
								vwMain = dt.DefaultView;
								grdMain.DataSource = vwMain ;
								// 01/06/2006   Always bind the table, otherwise the table events will not fire. 
								grdMain.DataBind();
								// 01/05/2006   Need to rebind this control so that the grid headers get translated. 
								// 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
								//this.DataBind();
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				lblError.Text = ex.Message;
			}
			if ( ctlNewRecord != null )
			{
				ctlNewRecord.MODULE_NAME = sMODULE_NAME;
				ctlNewRecord.Command += new CommandEventHandler(Page_Command);
			}
			// 01/04/2006  EnableNewRecord is not working, but I don't know why.
			// As an alternative, hide a div tag in NewRecord.ascx. 
			if ( ctlNewRecord != null )
			{
				ctlNewRecord.Visible = grdMain.Visible;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term("EditCustomFields.LBL_LIST_FORM_TITLE"));
			// 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			// 03/10/2010   Apply full ACL security rules. 
			this.Visible = (Taoqi.Security.AdminUserAccess(m_sMODULE, "list") >= 0);
			if ( !this.Visible )
			{
				// 03/17/2010   We need to rebind the parent in order to get the error message to display. 
				Parent.DataBind();
				return;
			}

			// 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
			//Page.DataBind();
			// 02/08/2007   The NewRecord control is now in the MasterPage. 
			ContentPlaceHolder plcSidebar = Page.Master.FindControl("cntSidebar") as ContentPlaceHolder;
			if ( plcSidebar != null )
			{
				ctlNewRecord = plcSidebar.FindControl("ctlNewRecord") as NewRecord;
			}
			// 05/17/2010   Move the NewRecord control to the bottom of the ListView so that it will be visible with the Six theme. 
			if ( ctlNewRecord == null )
			{
				ctlNewRecord = Parent.FindControl("ctlNewRecord") as NewRecord;
			}
			if ( IsPostBack )
			{
				sMODULE_NAME = ctlSearch.MODULE_NAME;
			}
			else
			{
				sMODULE_NAME = Sql.ToString(Request["MODULE_NAME"]);
				// 01/05/2006   Fix Form Action so that Query String parameters will not continue to get passed around. 
				if ( !Sql.IsEmptyString(sMODULE_NAME) )
					RegisterClientScriptBlock("frmRedirect", "<script type=\"text/javascript\">document.forms[0].action='default.aspx';</script>");
			}

			grdMain.Visible = IsPostBack || !Sql.IsEmptyString(sMODULE_NAME);
			FIELDS_META_DATA_Bind();
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
			// We have to load the control in here, otherwise the control will not initialized before the Page_Load above. 
			ctlSearch.Command += new CommandEventHandler(Page_Command);
			m_sMODULE = "EditCustomFields";
			// 05/06/2010   The menu will show the admin Module Name in the Six theme. 
			SetMenu(m_sMODULE);
		}
		#endregion
	}
}


