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

namespace Taoqi.Administration.Config
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

		protected Guid          gID        ;
		protected TextBox       txtNAME    ;
		protected TextBox       txtCATEGORY;
		protected TextBox       txtVALUE   ;
		protected TableRow      trImage    ;
		protected HtmlInputFile fileIMAGE  ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "Save" )
			{
				if ( Page.IsValid )
				{
					try
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
									// 02/16/2006   Trim the Name and Category, but not the Value.
									string sNAME     = txtNAME    .Text.Trim();
									string sCATEGORY = txtCATEGORY.Text.Trim();
									string sVALUE    = txtVALUE   .Text;
									// 11/11/2008   Display an error message if max users is being edited. 
									if ( String.Compare(sNAME, "max_users", true) == 0 )
										throw(new Exception(L10n.Term("Config.ERR_CANNOT_EDIT_MAX_USERS")));
									
									// 01/07/2013   Config values ending in .Encrypted are encrypted. 
									if ( sNAME.EndsWith(".Encrypted") )
									{
										Guid gINBOUND_EMAIL_KEY = Sql.ToGuid(Application["CONFIG.InboundEmailKey"]);
										Guid gINBOUND_EMAIL_IV  = Sql.ToGuid(Application["CONFIG.InboundEmailIV" ]);
										string sENCRYPTED_VALUE = Security.EncryptPassword(sVALUE, gINBOUND_EMAIL_KEY, gINBOUND_EMAIL_IV);
										if ( Security.DecryptPassword(sENCRYPTED_VALUE, gINBOUND_EMAIL_KEY, gINBOUND_EMAIL_IV) != sVALUE )
											throw(new Exception("Decryption failed"));
										sVALUE = sENCRYPTED_VALUE;
									}
									
									// 08/09/2009   Allow an image to be uploaded and used in the config area. 
									if ( trImage.Visible )
									{
										Guid   gImageID  = Guid.Empty;
										string sFILENAME = String.Empty;
										Taoqi.FileBrowser.FileWorkerUtils.LoadImage(ref gImageID, ref sFILENAME, fileIMAGE.UniqueID, trn);
										if ( !Sql.IsEmptyGuid(gImageID) )
										{
											sVALUE = "~/Images/EmailImage.aspx?ID=" + gImageID.ToString();
										}
									}

									SqlProcs.spCONFIG_Update(sCATEGORY, sNAME, sVALUE);
									Application["CONFIG." + sNAME] = sVALUE;
									trn.Commit();
								}
								catch(Exception ex)
								{
									trn.Rollback();
									SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
									ctlDynamicButtons.ErrorText = ex.Message;
									return;
								}
							}
						}

					}
					catch(Exception ex)
					{
						ctlDynamicButtons.ErrorText = ex.Message;
						return;
					}
					Response.Redirect("default.aspx");
				}
			}
			else if ( e.CommandName == "Cancel" )
			{
				Response.Redirect("default.aspx");
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term(".moduleList.Administration"));
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
				gID = Sql.ToGuid(Request["ID"]);
				if ( !IsPostBack )
				{
					// 03/20/2008   Dynamic buttons need to be recreated in order for events to fire. 
					ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
					ctlFooterButtons .AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);

					Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
					if ( !Sql.IsEmptyGuid(gID) || !Sql.IsEmptyGuid(gDuplicateID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *            " + ControlChars.CrLf
							     + "  from vwCONFIG_Edit" + ControlChars.CrLf
							     + " where ID = @ID     " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								if ( !Sql.IsEmptyGuid(gDuplicateID) )
								{
									Sql.AddParameter(cmd, "@ID", gDuplicateID);
									gID = Guid.Empty;
								}
								else
								{
									Sql.AddParameter(cmd, "@ID", gID);
								}
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
											ctlModuleHeader.Title = Sql.ToString(rdr["NAME"]);
											SetPageTitle(L10n.Term(".moduleList.Administration") + " - " + ctlModuleHeader.Title);
											ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;

											txtNAME.ReadOnly = true;
											txtNAME    .Text = Sql.ToString(rdr["NAME"    ]);
											txtCATEGORY.Text = Sql.ToString(rdr["CATEGORY"]);
											// 01/07/2013   Config values ending in .Encrypted are encrypted. 
											if ( !txtNAME.Text.EndsWith(".Encrypted") )
												txtVALUE   .Text = Sql.ToString(rdr["VALUE"   ]);

											trImage.Visible = txtNAME.Text.EndsWith("_image");
										}
									}
								}
							}
						}
					}
				}
				else
				{
					// 12/02/2005   When validation fails, the header title does not retain its value.  Update manually. 
					ctlModuleHeader.Title = Sql.ToString(ViewState["ctlModuleHeader.Title"]);
					SetPageTitle(L10n.Term(".moduleList.Config") + " - " + ctlModuleHeader.Title);
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
			m_sMODULE = "Config";
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


