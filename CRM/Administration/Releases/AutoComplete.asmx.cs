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
using System.Collections;
using System.Web.Services;
using System.ComponentModel;
using Taoqi;

namespace Taoqi.Releases
{
	public class Release
	{
		public Guid    ID  ;
		public string  NAME;

		public Release()
		{
			ID   = Guid.Empty  ;
			NAME = String.Empty;
		}
	}

	/// <summary>
	/// Summary description for AutoComplete
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.Web.Script.Services.ScriptService]
	[ToolboxItem(false)]
	public class AutoComplete : System.Web.Services.WebService
	{
		[WebMethod(EnableSession=true)]
		public Release RELEASES_RELEASE_NAME_Get(string sNAME)
		{
			Release item = new Release();
			//try
			{
				if ( !Security.IsAuthenticated() )
					throw(new Exception("Authentication required"));

				Taoqi.DbProviderFactory dbf = Taoqi.DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					sSQL = "select ID        " + ControlChars.CrLf
					     + "     , NAME      " + ControlChars.CrLf
					     + "  from vwRELEASES" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Security.Filter(cmd, "Releases", "list");
						// 07/12/2010   Allow fuzzy searching during AutoComplete. 
						Sql.AppendParameter(cmd, sNAME, (Sql.ToBoolean(Application["CONFIG.AutoComplete.Contains"]) ? Sql.SqlFilterMode.Contains : Sql.SqlFilterMode.StartsWith), "NAME");
						// 07/02/2007   Sort is important so that the first match is selected. 
						cmd.CommandText += " order by NAME" + ControlChars.CrLf;
						using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
						{
							if ( rdr.Read() )
							{
								item.ID   = Sql.ToGuid   (rdr["ID"  ]);
								item.NAME = Sql.ToString (rdr["NAME"]);
							}
						}
					}
				}
				if ( Sql.IsEmptyGuid(item.ID) )
				{
					string sCULTURE = Sql.ToString (Session["USER_SETTINGS/CULTURE"]);
					L10N L10n = new L10N(sCULTURE);
					throw(new Exception(L10n.Term("Releases.ERR_RELEASE_NOT_FOUND")));
				}
			}
			//catch
			{
				// 02/04/2007   Don't catch the exception.  
				// It is a web service, so the exception will be handled properly by the AJAX framework. 
			}
			return item;
		}

		// 03/30/2007   Enable sessions so that we can require authentication to access the data. 
		// 03/29/2007   In order for AutoComplete to work, the parameter names must be "prefixText" and "count". 
		[WebMethod(EnableSession=true)]
		public string[] RELEASES_RELEASE_NAME_List(string prefixText, int count)
		{
			string[] arrItems = new string[0];
			try
			{
				if ( !Security.IsAuthenticated() )
					throw(new Exception("Authentication required"));

				Taoqi.DbProviderFactory dbf = Taoqi.DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					string sSQL;
					// 10/08/2010   Since we are only returning the name, it is useful to return a distinct list. 
					sSQL = "select distinct  " + ControlChars.CrLf
					     + "       NAME      " + ControlChars.CrLf
					     + "  from vwRELEASES" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Security.Filter(cmd, "Releases", "list");
						// 07/12/2010   Allow fuzzy searching during AutoComplete. 
						Sql.AppendParameter(cmd, prefixText, (Sql.ToBoolean(Application["CONFIG.AutoComplete.Contains"]) ? Sql.SqlFilterMode.Contains : Sql.SqlFilterMode.StartsWith), "NAME");
						cmd.CommandText += " order by NAME" + ControlChars.CrLf;
						using ( DbDataAdapter da = dbf.CreateDataAdapter() )
						{
							((IDbDataAdapter)da).SelectCommand = cmd;
							using ( DataTable dt = new DataTable() )
							{
								da.Fill(0, count, dt);
								arrItems = new string[dt.Rows.Count];
								for ( int i=0; i < dt.Rows.Count; i++ )
									arrItems[i] = Sql.ToString(dt.Rows[i]["NAME"]);
							}
						}
					}
				}
			}
			catch
			{
			}
			return arrItems;
		}

		// 09/03/2009   The list can be retrived for the base module, or for a ModulePopup, 
		// so the field name can be NAME or RELEASE_NAME. 
		[WebMethod(EnableSession=true)]
		public string[] RELEASES_NAME_List(string prefixText, int count)
		{
			return RELEASES_RELEASE_NAME_List(prefixText, count);
		}

		// 08/05/2010   Add support for AutoComplete when editing Bugs. 
		[WebMethod(EnableSession=true)]
		public string[] RELEASES_FOUND_IN_RELEASE_List(string prefixText, int count)
		{
			return RELEASES_RELEASE_NAME_List(prefixText, count);
		}

		[WebMethod(EnableSession=true)]
		public string[] RELEASES_FIXED_IN_RELEASE_List(string prefixText, int count)
		{
			return RELEASES_RELEASE_NAME_List(prefixText, count);
		}
	}
}



