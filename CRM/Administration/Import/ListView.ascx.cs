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
using System.Diagnostics;
using System.Xml;

namespace Taoqi.Administration.Import
{
	/// <summary>
	///		Summary description for ListView.
	/// </summary>
	public class ListView : SplendidControl
	{
		protected Label         lblError       ;
		protected HtmlInputFile fileIMPORT     ;
		protected CheckBox      chkTruncate    ;
		protected Literal       lblImportErrors;
		protected RequiredFieldValidator reqFILENAME;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "Next" )
			{
				if ( Page.IsValid )
				{
					try
					{
						HttpPostedFile pstIMPORT = fileIMPORT.PostedFile;
						if ( pstIMPORT != null )
						{
							if ( pstIMPORT.FileName.Length > 0 )
							{
								string sFILENAME       = Path.GetFileName (pstIMPORT.FileName);
								string sFILE_EXT       = Path.GetExtension(sFILENAME);
								string sFILE_MIME_TYPE = pstIMPORT.ContentType;
								if ( sFILE_MIME_TYPE == "text/xml" )
								{
									using ( MemoryStream mstm = new MemoryStream() )
									{
										using ( BinaryWriter mwtr = new BinaryWriter(mstm) )
										{
											using ( BinaryReader reader = new BinaryReader(pstIMPORT.InputStream) )
											{
												byte[] binBYTES = reader.ReadBytes(8*1024);
												while ( binBYTES.Length > 0 )
												{
													for(int i=0; i < binBYTES.Length; i++ )
													{
														// MySQL dump seems to dump binary 0 & 1 for byte values. 
														if ( binBYTES[i] == 0 )
															mstm.WriteByte(Convert.ToByte('0'));
														else if ( binBYTES[i] == 1 )
															mstm.WriteByte(Convert.ToByte('1'));
														else
															mstm.WriteByte(binBYTES[i]);
													}
													binBYTES = reader.ReadBytes(8*1024);
												}
											}
											mwtr.Flush();
											mstm.Seek(0, SeekOrigin.Begin);
											XmlDocument xml = new XmlDocument();
											xml.Load(mstm);
											try
											{
												// 09/30/2006   Clear any previous error. 
												lblImportErrors.Text = "";
												SplendidImport.Import(xml, null, chkTruncate.Checked);
											}
											catch(Exception ex)
											{
												lblImportErrors.Text = ex.Message;
											}
										}
									}
								}
								else
								{
									throw(new Exception(L10n.Term("Administration.LBL_IMPORT_DATABASE_ERROR")));
								}
							}
						}
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
						lblError.Text = ex.Message;
						return;
					}
				}
			}
			else if ( e.CommandName == "Back" )
			{
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term("Administration.LBL_MODULE_NAME"));
			// 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			// 03/10/2010   Apply full ACL security rules. 
			this.Visible = (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
			{
				// 03/17/2010   We need to rebind the parent in order to get the error message to display. 
				Parent.DataBind();
				return;
			}

			// 07/02/2006   The required fields need to be bound manually. 
			reqFILENAME.DataBind();
			// 12/17/2005   Don't buffer so that the connection can be kept alive. 
			Response.BufferOutput = false;
			if ( !IsPostBack )
			{
				// 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();
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
			m_sMODULE = "Import";
			// 05/06/2010   The menu will show the admin Module Name in the Six theme. 
			SetMenu(m_sMODULE);
		}
		#endregion
	}
}


