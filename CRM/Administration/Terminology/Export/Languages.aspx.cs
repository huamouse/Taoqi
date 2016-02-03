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
using System.Text;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Taoqi.Administration.Terminology.Export
{
	/// <summary>
	/// Summary description for Default.
	/// </summary>
	public class Languages : SplendidPage
	{
		// 11/19/2005   Default to expiring everything. 
		override protected bool AuthenticationRequired()
		{
#if DEBUG
			return false;
#else
			return !Sql.ToBoolean(Application["CONFIG.enable_language_export"]);
#endif
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 03/10/2010   Apply full ACL security rules. 
			// 07/09/2010   This is a special page that may be publicly accessible. 
			// We need to allow end-users to download language packs from http://community.Taoqi.com
			this.Visible = !AuthenticationRequired() || (Taoqi.Security.AdminUserAccess("Languages", "export") >= 0);
			if ( !this.Visible )
				return;
			
			Response.ExpiresAbsolute = new DateTime(1970, 1, 1);
			try
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					string sLanguagePackURL = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Length - Request.Url.Segments[Request.Url.Segments.Length-1].Length - Request.Url.Segments[Request.Url.Segments.Length-2].Length) + "Export/Terminology.aspx?LANG=";
					sSQL = "select DISPLAY_NAME     as Name        " + ControlChars.CrLf
					     + "     , ''               as Date        " + ControlChars.CrLf
					     + "     , NATIVE_NAME      as Description " + ControlChars.CrLf
					     + "     , @PACK_URL + NAME as URL         " + ControlChars.CrLf
					     + "  from vwLANGUAGES                     " + ControlChars.CrLf
					     + " order by Name                         " + ControlChars.CrLf
					     + "  for xml raw('LanguagePack'), elements" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Sql.AddParameter(cmd, "@PACK_URL", sLanguagePackURL);
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							StringBuilder sbXML = new StringBuilder();
							sbXML.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
							sbXML.AppendLine("<xml>");
							while ( rdr.Read() )
								sbXML.Append(Sql.ToString(rdr[0]));
							sbXML.AppendLine("</xml>");
							
							// 07/11/2011   We are getting an unexplained "Object reference not set to an instance of an object", so make sure to clear the buffer. 
							Response.ContentType = "text/xml";
							Response.Clear();
							Response.Write(sbXML.ToString());
							Response.End();
						}
					}
				}
			}
			catch(Exception ex)
			{
				// 07/11/2011   Log the error instead of sending to the user. 
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				//Response.Write(ex.Message);
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


