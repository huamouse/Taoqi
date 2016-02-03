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
using System.IO;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Diagnostics;

namespace Taoqi.Administration.EditCustomFields
{
	/// <summary>
	///		Summary description for EditView.
	/// </summary>
	public class EditView : SplendidControl
	{
		protected _controls.DynamicButtons ctlDynamicButtons;
		// 01/13/2010   Add footer buttons. 
		protected _controls.DynamicButtons ctlFooterButtons ;

		protected Guid                       gID                ;
		protected TableRow                   trDROPDOWN_LIST    ;
		protected DataView                   vwDROPDOWN_LIST    ;
		protected DataView                   vwPICK_LIST_VALUES ;
		protected DataGrid                   grdPICK_LIST_VALUES;
		protected TextBox                    txtNAME            ;
		protected TextBox                    txtLABEL           ;
		protected DropDownList               lstDATA_TYPE       ;
		protected TableRow                   trMAX_SIZE         ;
		protected TextBox                    txtMAX_SIZE        ;
		protected CheckBox                   chkREQUIRED        ;
		protected TextBox                    txtDEFAULT_VALUE   ;
		protected CheckBox                   chkAUDITED         ;
		protected DropDownList               lstDROPDOWN_LIST   ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			string sMODULE_NAME = Sql.ToString(ViewState["MODULE_NAME"]);
			if ( e.CommandName == "Save" )
			{
				if ( Page.IsValid )
				{
					try
					{
						// 04/24/2006   Upgrade to SugarCRM 4.2 Schema. 
						// 04/24/2006   We dont support MassUpdate at this time. 

						// 07/18/2006   Manually create the command so that we can increase the timeout. 
						// 07/18/2006   Keep the original procedure call so that we will get a compiler error if something changes. 
						bool bIncreaseTimeout = true;
						if ( !bIncreaseTimeout )
						{
							SqlProcs.spFIELDS_META_DATA_Update(gID, Sql.ToInteger(txtMAX_SIZE.Text), chkREQUIRED.Checked, chkAUDITED.Checked, txtDEFAULT_VALUE.Text, lstDROPDOWN_LIST.SelectedValue, false);
						}
						else
						{
							Int32  nMAX_SIZE      = Sql.ToInteger(txtMAX_SIZE.Text);
							bool   bREQUIRED      = chkREQUIRED.Checked            ;
							bool   bAUDITED       = chkAUDITED.Checked             ;
							string sDEFAULT_VALUE = txtDEFAULT_VALUE.Text          ;
							string sDROPDOWN_LIST = lstDROPDOWN_LIST.SelectedValue ;
							bool   bMASS_UPDATE   = false                          ;
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
											cmd.CommandText = "spFIELDS_META_DATA_Update";
											// 01/06/2006   Tripple the default timeout.  The operation was timing-out on QA machines and on the demo server. 
											// 02/03/2007   Increase timeout to 5 minutes.  It should not take that long, but some users are reporting a timeout. 
											// 07/01/2008   Let the function run forever. 
											cmd.CommandTimeout = 0;
											IDbDataParameter parID               = Sql.AddParameter(cmd, "@ID"              , gID                );
											IDbDataParameter parMODIFIED_USER_ID = Sql.AddParameter(cmd, "@MODIFIED_USER_ID",  Security.USER_ID  );
											IDbDataParameter parMAX_SIZE         = Sql.AddParameter(cmd, "@MAX_SIZE"        , nMAX_SIZE          );
											IDbDataParameter parREQUIRED         = Sql.AddParameter(cmd, "@REQUIRED"        , bREQUIRED          );
											IDbDataParameter parAUDITED          = Sql.AddParameter(cmd, "@AUDITED"         , bAUDITED           );
											IDbDataParameter parDEFAULT_VALUE    = Sql.AddParameter(cmd, "@DEFAULT_VALUE"   , sDEFAULT_VALUE     , 255);
											// 01/10/2007   DROPDOWN_LIST was added as it can be modified. 
											IDbDataParameter parDROPDOWN_LIST    = Sql.AddParameter(cmd, "@DROPDOWN_LIST"   , sDROPDOWN_LIST     ,  50);
											IDbDataParameter parMASS_UPDATE      = Sql.AddParameter(cmd, "@MASS_UPDATE"     , bMASS_UPDATE       );
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
						}
						// 01/10/2006   Clear the cache. 
						//SplendidCache.ClearFieldsMetaData(sMODULE_NAME);
						// 06/03/2008   We need to reload the entire application cache as there are many possible cached items. 
						// 10/26/2008   IIS7 Integrated Pipeline does not allow HttpContext access inside Application_Start. 
						SplendidInit.InitApp(HttpContext.Current);
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
						ctlDynamicButtons.ErrorText = ex.Message;
						return;
					}
					Response.Redirect("default.aspx?MODULE_NAME=" + sMODULE_NAME);
				}
			}
			else if ( e.CommandName == "Cancel" )
			{
				Response.Redirect("default.aspx?MODULE_NAME=" + sMODULE_NAME);
			}
		}

		protected void lstDROPDOWN_LIST_Changed(Object sender, EventArgs e)
		{
			try
			{
				if ( trDROPDOWN_LIST.Visible )
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select NAME                         " + ControlChars.CrLf
						     + "     , DISPLAY_NAME                 " + ControlChars.CrLf
						     + "  from vwTERMINOLOGY                " + ControlChars.CrLf
						     + " where lower(LIST_NAME) = @LIST_NAME" + ControlChars.CrLf
						     + "   and lower(LANG     ) = @LANG     " + ControlChars.CrLf
						     + " order by LIST_ORDER                " + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@LIST_NAME", lstDROPDOWN_LIST.SelectedValue.ToLower());
							Sql.AddParameter(cmd, "@LANG"     , L10n.NAME.ToLower());
							
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								DataTable dt = new DataTable();
								da.Fill(dt);
								vwPICK_LIST_VALUES = dt.DefaultView;
								grdPICK_LIST_VALUES.DataSource = vwPICK_LIST_VALUES ;
								grdPICK_LIST_VALUES.DataBind();
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				ctlDynamicButtons.ErrorText = ex.Message;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term(".moduleList.EditCustomFields"));
			// 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			// 03/10/2010   Apply full ACL security rules. 
			this.Visible = (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
			{
				// 03/17/2010   We need to rebind the parent in order to get the error message to display. 
				Parent.DataBind();
				return;
			}

			try
			{
				// 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();

				gID = Sql.ToGuid(Request["ID"]);
				if ( !IsPostBack )
				{
					// 03/20/2008   Dynamic buttons need to be recreated in order for events to fire. 
					ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
					ctlFooterButtons .AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);

					// 01/10/2007   Use cached data to populate the pick list. 
					lstDROPDOWN_LIST.DataSource = SplendidCache.TerminologyPickLists();
					lstDROPDOWN_LIST.DataBind();
					lstDROPDOWN_LIST_Changed(null, null);

					Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
					if ( !Sql.IsEmptyGuid(gID) || !Sql.IsEmptyGuid(gDuplicateID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *                      " + ControlChars.CrLf
							     + "  from vwFIELDS_META_DATA_Edit" + ControlChars.CrLf
							     + " where ID = @ID               " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@ID", gID);
								con.Open();

								if ( bDebug )
									RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

								// 11/22/2010   Convert data reader to data table for Rules Wizard. 
								using ( DbDataAdapter da = dbf.CreateDataAdapter() )
								{
									((IDbDataAdapter)da).SelectCommand = cmd;
									using ( DataTable dtCurrent = new DataTable() )
									{
										da.Fill(dtCurrent);
										if ( dtCurrent.Rows.Count > 0 )
										{
											DataRow rdr = dtCurrent.Rows[0];
											ViewState["MODULE_NAME"] = Sql.ToString(rdr["CUSTOM_MODULE"   ]);
											txtNAME         .Text    = Sql.ToString(rdr["NAME"            ]);
											SetPageTitle(L10n.Term("EditCustomFields.LBL_MODULE_NAME") + " - " + txtNAME.Text);
											txtLABEL        .Text    = Sql.ToString (rdr["LABEL"          ]);
											txtMAX_SIZE     .Text    = Sql.ToString (rdr["MAX_SIZE"       ]);
											txtDEFAULT_VALUE.Text    = Sql.ToString (rdr["DEFAULT_VALUE"  ]);
											chkAUDITED      .Checked = Sql.ToBoolean(rdr["AUDITED"        ]);
											chkREQUIRED     .Checked = (Sql.ToString(rdr["REQUIRED_OPTION"]) == "required") ? true : false;
											try
											{
												// 08/19/2010   Check the list before assigning the value. 
												Utils.SetSelectedValue(lstDATA_TYPE, Sql.ToString(rdr["DATA_TYPE"]));
											}
											catch(Exception ex)
											{
												SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex);
											}
											try
											{
												if ( lstDATA_TYPE.SelectedValue == "enum" )
													// 08/19/2010   Check the list before assigning the value. 
													Utils.SetSelectedValue(lstDROPDOWN_LIST, Sql.ToString(rdr["EXT1"]));
											}
											catch(Exception ex)
											{
												SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex);
											}
											trDROPDOWN_LIST.Visible = (lstDATA_TYPE.SelectedValue == "enum"   );
											trMAX_SIZE      .Visible = (lstDATA_TYPE.SelectedValue == "varchar");
											lstDROPDOWN_LIST_Changed(null, null);
										}
									}
								}
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				ctlDynamicButtons.ErrorText = ex.Message;
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
			ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
			ctlFooterButtons .Command += new CommandEventHandler(Page_Command);
			m_sMODULE = "EditCustomFields";
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


