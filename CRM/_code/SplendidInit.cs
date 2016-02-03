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
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Xml;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace Taoqi
{
	/// <summary>
	/// Summary description for SplendidInit.
	/// </summary>
	public class SplendidInit
	{
		// 10/24/2009   As a performance optimziation, we need a way to avoid calling spSYSTEM_TRANSACTIONS_Create for every transaction. 
		public static bool bUseSQLServerToken = false;
		public static bool bEnableACLFieldSecurity = false;

		private static void InitAppURLs(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			if ( Sql.IsEmptyString(Application["imageURL"]) )
			{
				Assembly asm = Assembly.GetExecutingAssembly();

				HttpRequest Request = Context.Request;
				// 12/22/2007   We can no longer rely upon the Request object being valid as we might be inside the timer event. 
				string sServerName      = Request.ServerVariables["SERVER_NAME"];
				// 01/14/2008   Capture the IP Address as it is harder to get inside a scheduled task. 
				string sServerIPAddress = Request.ServerVariables["LOCAL_ADDR" ];
				string sApplicationPath = Request.ApplicationPath;
				// 12/22/2007   The DbFactory code will need the original ApplicationPath. 
				Application["SplendidVersion"] = asm.GetName().Version.ToString();
				Application["ServerName"     ] = sServerName     ;
				Application["ServerIPAddress"] = sServerIPAddress;
				Application["ApplicationPath"] = sApplicationPath;
				if ( !sApplicationPath.EndsWith("/") )
					sApplicationPath += "/";
				Application["rootURL"  ] = sApplicationPath;
				// 07/28/2006   Mono requires case-significant paths. 
				Application["imageURL" ] = sApplicationPath + "Include/images/";
				Application["scriptURL"] = sApplicationPath + "Include/javascript/";
				Application["chartURL" ] = sApplicationPath + "Include/charts/";
			}
		}

		public static void InitTerminology(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			try
			{
				// 12/03/2008   This function can be called from a scheduled task, so we must pass the application to the GetFactory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					// 05/20/2008   Only load terminology from Active languages. 
					sSQL = "select NAME                " + ControlChars.CrLf
					     + "     , LANG                " + ControlChars.CrLf
					     + "     , MODULE_NAME         " + ControlChars.CrLf
					     + "     , DISPLAY_NAME        " + ControlChars.CrLf
					     + "  from vwTERMINOLOGY_Active" + ControlChars.CrLf
					     + " where LIST_NAME is null   " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
// 01/20/2006   Enable all languages when debugging. 
//#if DEBUG
//						sSQL += "   and LANG = 'en-us'" + ControlChars.CrLf;
//#endif
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								//Application[Sql.ToString(rdr["LANG"]) + "." + Sql.ToString(rdr["MODULE_NAME"]) + "." + Sql.ToString(rdr["NAME"])] = Sql.ToString(rdr["DISPLAY_NAME"]);
								string sLANG         = Sql.ToString(rdr["LANG"        ]);
								string sMODULE_NAME  = Sql.ToString(rdr["MODULE_NAME" ]);
								string sNAME         = Sql.ToString(rdr["NAME"        ]);
								string sDISPLAY_NAME = Sql.ToString(rdr["DISPLAY_NAME"]);
								// 01/20/2009   We need to pass the Application to the Term function. 
								L10N.SetTerm(Application, sLANG, sMODULE_NAME, sNAME, sDISPLAY_NAME);
							}
						}
					}
					// 05/20/2008   Only load terminology from Active languages. 
					sSQL = "select NAME                 " + ControlChars.CrLf
					     + "     , LANG                 " + ControlChars.CrLf
					     + "     , MODULE_NAME          " + ControlChars.CrLf
					     + "     , LIST_NAME            " + ControlChars.CrLf
					     + "     , DISPLAY_NAME         " + ControlChars.CrLf
					     + "  from vwTERMINOLOGY_Active " + ControlChars.CrLf
					     + " where LIST_NAME is not null" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
// 01/20/2006   Enable all languages when debugging. 
//#if DEBUG
//						sSQL += "   and LANG = 'en-us'" + ControlChars.CrLf;
//#endif
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								// 01/13/2006   Don't include MODULE_NAME when used with a list. 
								// DropDownLists are populated without the module name in the list name. 
								// 01/13/2006   We can remove the module, but not the dot.  
								// Otherwise it breaks all other code that references a list term. 
								//Application[Sql.ToString(rdr["LANG"]) + "." + sMODULE_NAME + "." + Sql.ToString(rdr["LIST_NAME"]) + "." + Sql.ToString(rdr["NAME"])] = Sql.ToString(rdr["DISPLAY_NAME"]);
								string sLANG         = Sql.ToString(rdr["LANG"        ]);
								string sMODULE_NAME  = Sql.ToString(rdr["MODULE_NAME" ]);
								string sNAME         = Sql.ToString(rdr["NAME"        ]);
								string sLIST_NAME    = Sql.ToString(rdr["LIST_NAME"   ]);
								string sDISPLAY_NAME = Sql.ToString(rdr["DISPLAY_NAME"]);
								// 01/20/2009   We need to pass the Application to the Term function. 
								L10N.SetTerm(Application, sLANG, sMODULE_NAME, sLIST_NAME, sNAME, sDISPLAY_NAME);
							}
						}
					}

					sSQL = "select ALIAS_NAME           " + ControlChars.CrLf
					     + "     , ALIAS_MODULE_NAME    " + ControlChars.CrLf
					     + "     , ALIAS_LIST_NAME      " + ControlChars.CrLf
					     + "     , NAME                 " + ControlChars.CrLf
					     + "     , MODULE_NAME          " + ControlChars.CrLf
					     + "     , LIST_NAME            " + ControlChars.CrLf
					     + "  from vwTERMINOLOGY_ALIASES" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								string sALIAS_NAME         = Sql.ToString(rdr["ALIAS_NAME"        ]);
								string sALIAS_MODULE_NAME  = Sql.ToString(rdr["ALIAS_MODULE_NAME" ]);
								string sALIAS_LIST_NAME    = Sql.ToString(rdr["ALIAS_LIST_NAME"   ]);
								string sNAME               = Sql.ToString(rdr["NAME"              ]);
								string sMODULE_NAME        = Sql.ToString(rdr["MODULE_NAME"       ]);
								string sLIST_NAME          = Sql.ToString(rdr["LIST_NAME"         ]);
								// 01/20/2009   We need to pass the Application to the Term function. 
								L10N.SetAlias(Application, sALIAS_MODULE_NAME, sALIAS_LIST_NAME, sALIAS_NAME, sMODULE_NAME, sLIST_NAME, sNAME);
							}
						}
					}
					// 11/20/2009   Move module init to a separate function. 
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static void InitModules(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			try
			{
				// 12/03/2008   This function can be called from a scheduled task, so we must pass the application to the GetFactory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					// 07/13/2006   The reporting module needs a quick way to translate a module name to a table name. 
					// 12/29/2007   We need to know if the module is audited. 
					// 01/20/2010   Order by module to ease debugging. 
					sSQL = "select *                " + ControlChars.CrLf
					     + "  from vwMODULES_AppVars" + ControlChars.CrLf
					     + " order by MODULE_NAME   " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							Regex r = new Regex(@"[^A-Za-z0-9_]");
							while ( rdr.Read() )
							{
								string sMODULE_NAME            = Sql.ToString (rdr["MODULE_NAME"        ]);
								string sTABLE_NAME             = Sql.ToString (rdr["TABLE_NAME"         ]);
								string sRELATIVE_PATH          = Sql.ToString (rdr["RELATIVE_PATH"      ]);
								// 05/06/2010   Add DISPLAY_NAME for the Six theme. 
								string sDISPLAY_NAME           = Sql.ToString (rdr["DISPLAY_NAME"       ]);
								bool   bIS_AUDITED             = Sql.ToBoolean(rdr["IS_AUDITED"         ]);
								bool   bIS_TEAMED              = Sql.ToBoolean(rdr["IS_TEAMED"          ]);
								bool   bIS_ASSIGNED            = Sql.ToBoolean(rdr["IS_ASSIGNED"        ]);
								bool   bCUSTOM_PAGING          = false;
								bool   bMASS_UPDATE_ENABLED    = true ;
								bool   bDEFAULT_SEARCH_ENABLED = true ;
								bool   bEXCHANGE_SYNC          = false;
								bool   bEXCHANGE_FOLDERS       = false;
								bool   bEXCHANGE_CREATE_PARENT = false;
								bool   bIS_ADMIN               = false;
								bool   bREST_ENABLED           = false;
								bool   bDUPLICATE_CHECHING_ENABLED = false;
								try
								{
									// 01/07/2010   Ignore the error if the field does not exist. 
									// This will allow us to debug old databases. 
									bCUSTOM_PAGING       = Sql.ToBoolean(rdr["CUSTOM_PAGING"      ]);
									// 12/03/2009   Ignore the error if the field does not exist. 
									// This will allow us to debug old databases. 
									// If the field is NULL, then assume true. 
									if ( rdr["MASS_UPDATE_ENABLED"] != DBNull.Value )
										bMASS_UPDATE_ENABLED = Sql.ToBoolean(rdr["MASS_UPDATE_ENABLED"]);
									// 01/13/2010   Ignore the error if the field does not exist. 
									// If the field is NULL, then assume true. 
									if ( rdr["DEFAULT_SEARCH_ENABLED"] != DBNull.Value )
										bDEFAULT_SEARCH_ENABLED = Sql.ToBoolean(rdr["DEFAULT_SEARCH_ENABLED"]);
									
									// 04/04/2010   Add EXCHANGE_SYNC so that we can enable/disable the sync buttons on the MassUpdate panels. 
									bEXCHANGE_SYNC          = Sql.ToBoolean(rdr["EXCHANGE_SYNC"         ]);
									// 04/04/2010   Add EXCHANGE_FOLDERS so that we can enable/disable the sync buttons on the MassUpdate panels. 
									bEXCHANGE_FOLDERS       = Sql.ToBoolean(rdr["EXCHANGE_FOLDERS"      ]);
									// 04/05/2010   Need to be able to disable Account creation. This is because the email may come from a personal email address.
									bEXCHANGE_CREATE_PARENT = Sql.ToBoolean(rdr["EXCHANGE_CREATE_PARENT"]);
									// 07/28/2010   We will use the Admin flag in the ModuleHeader. 
									bIS_ADMIN               = Sql.ToBoolean(rdr["IS_ADMIN"              ]);
								}
								catch
								{
								}
								try
								{
									// 08/22/2011   Add admin control to REST API. 
									bREST_ENABLED           = Sql.ToBoolean(rdr["REST_ENABLED"          ]);
								}
								catch
								{
								}
								try
								{
									// 03/14/2014   DUPLICATE_CHECHING_ENABLED enables duplicate checking. 
									bDUPLICATE_CHECHING_ENABLED = Sql.ToBoolean(rdr["DUPLICATE_CHECHING_ENABLED"]);
								}
								catch
								{
								}
								// 11/03/2009   As extra precaution, make sure that the table name is valid. 
								sTABLE_NAME = r.Replace(sTABLE_NAME, "");  // Regex.Replace(sTABLE_NAME, @"[^A-Za-z0-9_]", "");
								// 10/10/2006   After importing, we need an easy way to get back to the root of the module. 
								// 12/30/2007   We need a dynamic way to determine if the module record can be assigned or placed in a team. 
								// Teamed and Assigned flags are automatically determined based on the existence of TEAM_ID and ASSIGNED_USER_ID fields. 
								Application["Modules." + sMODULE_NAME + ".TableName"    ] = sTABLE_NAME         ;
								Application["Modules." + sMODULE_NAME + ".RelativePath" ] = sRELATIVE_PATH      ;
								// 05/06/2010   Add DISPLAY_NAME for the Six theme. 
								Application["Modules." + sMODULE_NAME + ".DisplayName"  ] = sDISPLAY_NAME       ;
								Application["Modules." + sMODULE_NAME + ".Audited"      ] = bIS_AUDITED         ;
								Application["Modules." + sMODULE_NAME + ".Teamed"       ] = bIS_TEAMED          ;
								Application["Modules." + sMODULE_NAME + ".Assigned"     ] = bIS_ASSIGNED        ;
								Application["Modules." + sMODULE_NAME + ".CustomPaging" ] = bCUSTOM_PAGING      ;
								// 07/28/2010   We will use the Admin flag in the ModuleHeader. 
								Application["Modules." + sMODULE_NAME + ".IsAdmin"      ] = bIS_ADMIN           ;
								// 12/02/2009   Add the ability to disable Mass Updates. 
								Application["Modules." + sMODULE_NAME + ".MassUpdate"   ] = bMASS_UPDATE_ENABLED;
								// 01/13/2010   Allow default search to be disabled. 
								Application["Modules." + sMODULE_NAME + ".DefaultSearch"] = bDEFAULT_SEARCH_ENABLED;
								// 04/04/2010   Add EXCHANGE_SYNC so that we can enable/disable the sync buttons on the MassUpdate panels. 
								Application["Modules." + sMODULE_NAME + ".ExchangeSync" ] = bEXCHANGE_SYNC;
								// 04/04/2010   Add EXCHANGE_SYNC so that we can enable/disable the sync buttons on the MassUpdate panels. 
								Application["Modules." + sMODULE_NAME + ".ExchangeFolders"] = bEXCHANGE_FOLDERS;
								// 04/05/2010   Need to be able to disable Account creation. This is because the email may come from a personal email address.
								Application["Modules." + sMODULE_NAME + ".ExchangeCreateParent"] = bEXCHANGE_CREATE_PARENT;
								// 11/06/2009   We need a fast way for the offline client to detect if the files exist for a specific module. 
								// 10/27/2010   ASP.NET files will not exist on an iPad. 
								Application["Modules." + sMODULE_NAME + ".Exists"      ] = (Sql.IsEffiProz(con) || Sql.IsEmptyString(sRELATIVE_PATH) || File.Exists(Context.Server.MapPath(sRELATIVE_PATH + "default.aspx")));
								// 01/18/2010   We need a quick test for a valid module. 
								Application["Modules." + sMODULE_NAME + ".Valid"       ] = true;
								// 01/19/2010   In the reporting area, we need a quick way to get a Module from a Table Name. 
								if ( !Sql.IsEmptyString(sTABLE_NAME) )
									Application["Modules." + sTABLE_NAME + ".ModuleName"] = sMODULE_NAME;
								// 08/22/2011   Add admin control to REST API. 
								Application["Modules." + sMODULE_NAME + ".RestEnabled"  ] = bREST_ENABLED;
								// 03/14/2014   DUPLICATE_CHECHING_ENABLED enables duplicate checking. 
								Application["Modules." + sMODULE_NAME + ".DuplicateCheckingEnabled"] = bDUPLICATE_CHECHING_ENABLED;
							}
						}
					}
					// 11/20/2009   We need to make sure that the ModulePopupScripts.aspx file is cached by the browser, but updated when appropriate. 
					sSQL = "select max(DATE_MODIFIED_UTC)" + ControlChars.CrLf
					     + "  from vwMODULES             " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						DateTime dtLastModified = Sql.ToDateTime(cmd.ExecuteScalar());
						Application["Modules.LastModified"] = dtLastModified.ToString();
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static void InitModuleACL(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			try
			{
				// 12/03/2008   This function can be called from a scheduled task, so we must pass the application to the GetFactory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					// 03/09/2010   Admin roles are managed separately. 
					sSQL = "select MODULE_NAME          " + ControlChars.CrLf
					     + "     , ACLACCESS_ADMIN      " + ControlChars.CrLf
					     + "     , ACLACCESS_ACCESS     " + ControlChars.CrLf
					     + "     , ACLACCESS_VIEW       " + ControlChars.CrLf
					     + "     , ACLACCESS_LIST       " + ControlChars.CrLf
					     + "     , ACLACCESS_EDIT       " + ControlChars.CrLf
					     + "     , ACLACCESS_DELETE     " + ControlChars.CrLf
					     + "     , ACLACCESS_IMPORT     " + ControlChars.CrLf
					     + "     , ACLACCESS_EXPORT     " + ControlChars.CrLf
					     + "     , IS_ADMIN             " + ControlChars.CrLf
					     + "  from vwACL_ACCESS_ByModule" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								string sMODULE_NAME = Sql.ToString(rdr["MODULE_NAME"]);
								// 02/03/2009   This function might be called from a background process. 
								Security.SetModuleAccess(Application, sMODULE_NAME, "admin" , Sql.ToInteger(rdr["ACLACCESS_ADMIN" ]));
								Security.SetModuleAccess(Application, sMODULE_NAME, "access", Sql.ToInteger(rdr["ACLACCESS_ACCESS"]));
								Security.SetModuleAccess(Application, sMODULE_NAME, "view"  , Sql.ToInteger(rdr["ACLACCESS_VIEW"  ]));
								Security.SetModuleAccess(Application, sMODULE_NAME, "list"  , Sql.ToInteger(rdr["ACLACCESS_LIST"  ]));
								Security.SetModuleAccess(Application, sMODULE_NAME, "edit"  , Sql.ToInteger(rdr["ACLACCESS_EDIT"  ]));
								Security.SetModuleAccess(Application, sMODULE_NAME, "delete", Sql.ToInteger(rdr["ACLACCESS_DELETE"]));
								Security.SetModuleAccess(Application, sMODULE_NAME, "import", Sql.ToInteger(rdr["ACLACCESS_IMPORT"]));
								Security.SetModuleAccess(Application, sMODULE_NAME, "export", Sql.ToInteger(rdr["ACLACCESS_EXPORT"]));
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static void InitConfig(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			try
			{
				// 12/03/2008   This function can be called from a scheduled task, so we must pass the application to the GetFactory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					sSQL = "select NAME    " + ControlChars.CrLf
					     + "     , VALUE   " + ControlChars.CrLf
					     + "  from vwCONFIG" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								Application["CONFIG." + Sql.ToString(rdr["NAME"])] = Sql.ToString(rdr["VALUE"]);
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		// 02/26/2011   Add Field Validators for use by browser extensions. 
		public static void InitFieldValidators(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			try
			{
				// 12/03/2008   This function can be called from a scheduled task, so we must pass the application to the GetFactory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					sSQL = "select NAME              " + ControlChars.CrLf
					     + "     , REGULAR_EXPRESSION" + ControlChars.CrLf
					     + "  from vwFIELD_VALIDATORS" + ControlChars.CrLf
					     + " where VALIDATION_TYPE = 'RegularExpressionValidator'" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								Application["FIELD_VALIDATORS." + Sql.ToString(rdr["NAME"])] = Sql.ToString(rdr["REGULAR_EXPRESSION"]);
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static void InitTimeZones(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			try
			{
				// 12/03/2008   This function can be called from a scheduled task, so we must pass the application to the GetFactory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					sSQL = "select *          " + ControlChars.CrLf
					     + "  from vwTIMEZONES" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								// 01/02/2012   Add iCal TZID. 
								// 03/26/2013   iCloud uses linked_timezone values from http://tzinfo.rubyforge.org/doc/. 
								string sTZID = String.Empty;
								string sLINKED_TIMEZONE = String.Empty;
								try
								{
									sTZID = Sql.ToString(rdr["TZID"]);
									sLINKED_TIMEZONE = Sql.ToString(rdr["LINKED_TIMEZONE"]);
								}
								catch
								{
								}
								TimeZone oTimeZone = new TimeZone
									( Sql.ToGuid   (rdr["ID"                   ])
									, Sql.ToString (rdr["NAME"                 ])
									, Sql.ToString (rdr["STANDARD_NAME"        ])
									, Sql.ToString (rdr["STANDARD_ABBREVIATION"])
									, Sql.ToString (rdr["DAYLIGHT_NAME"        ])
									, Sql.ToString (rdr["DAYLIGHT_ABBREVIATION"])
									, Sql.ToInteger(rdr["BIAS"                 ])
									, Sql.ToInteger(rdr["STANDARD_BIAS"        ])
									, Sql.ToInteger(rdr["DAYLIGHT_BIAS"        ])
									, Sql.ToInteger(rdr["STANDARD_YEAR"        ])
									, Sql.ToInteger(rdr["STANDARD_MONTH"       ])
									, Sql.ToInteger(rdr["STANDARD_WEEK"        ])
									, Sql.ToInteger(rdr["STANDARD_DAYOFWEEK"   ])
									, Sql.ToInteger(rdr["STANDARD_HOUR"        ])
									, Sql.ToInteger(rdr["STANDARD_MINUTE"      ])
									, Sql.ToInteger(rdr["DAYLIGHT_YEAR"        ])
									, Sql.ToInteger(rdr["DAYLIGHT_MONTH"       ])
									, Sql.ToInteger(rdr["DAYLIGHT_WEEK"        ])
									, Sql.ToInteger(rdr["DAYLIGHT_DAYOFWEEK"   ])
									, Sql.ToInteger(rdr["DAYLIGHT_HOUR"        ])
									, Sql.ToInteger(rdr["DAYLIGHT_MINUTE"      ])
									, Sql.ToBoolean(Application["CONFIG.GMT_Storage"])
									, sTZID
									);
								Application["TIMEZONE." + oTimeZone.ID.ToString()] = oTimeZone;
								// 01/02/2012   We need quick way to convert a TZID to a GUID. 
								if ( !Sql.IsEmptyString(sTZID) )
									Application["TIMEZONE.TZID." + oTimeZone.TZID] = oTimeZone;
								// 03/26/2013   iCloud uses linked_timezone values from http://tzinfo.rubyforge.org/doc/. 
								if ( !Sql.IsEmptyString(sLINKED_TIMEZONE) )
									Application["TIMEZONE.TZID." + sLINKED_TIMEZONE] = oTimeZone;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static void InitCurrencies(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			try
			{
				// 12/03/2008   This function can be called from a scheduled task, so we must pass the application to the GetFactory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					sSQL = "select *           " + ControlChars.CrLf
					     + "  from vwCURRENCIES" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								// 11/10/2008   PayPal uses the ISO value. 
								Currency C10n = new Currency
									( Sql.ToGuid  (rdr["ID"             ])
									, Sql.ToString(rdr["NAME"           ])
									, Sql.ToString(rdr["SYMBOL"         ])
									, Sql.ToString(rdr["ISO4217"        ])
									, Sql.ToFloat (rdr["CONVERSION_RATE"])
									);
								Application["CURRENCY." + C10n.ID.ToString()] = C10n;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		
		public static void InitApp(HttpContext Context)
		{
			HttpApplicationState Application = Context.Application;
			try
			{
				
				DataTable dtSystemErrors = Application["SystemErrors"] as DataTable;

				
				Hashtable hashSystemSync = new Hashtable();
			
				foreach ( string key in Application.Keys )
				{
					if ( key.StartsWith("SystemSync.") && !hashSystemSync.Contains(key) )
						hashSystemSync.Add(key, Application[key]);
				}
				Application.Clear();
				foreach ( object oKey in hashSystemSync )
				{
					Application[oKey.ToString()] = hashSystemSync[oKey];
				}
				hashSystemSync.Clear();
				// 11/28/2005   Save and restore the system errors table. 
				Application["SystemErrors"] = dtSystemErrors;

				InitAppURLs(Context);
				// 03/06/2008   We cannot log the application start until the the ServerName has been stored in the Application cache. 
				// 04/22/2008   Include the version in the system log.
				if ( Application.Count == 0 )
					SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "Application start. Version " + Sql.ToString(Application["SplendidVersion"]));
				else
					SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "Application restart. Version " + Sql.ToString(Application["SplendidVersion"]));

				// 11/28/2005   Clear all cache variables as well. 
				// 02/18/2008   HttpRuntime.Cache is a better and faster way to get to the cache. 
				foreach(DictionaryEntry oKey in HttpRuntime.Cache)
				{
					string sKey = oKey.Key.ToString();
					HttpRuntime.Cache.Remove(sKey);
				}
				// 06/03/2006   Clear the cached data that is stored in the Session object. 
				if ( Context.Session != null )
				{
					Hashtable hashSessionKeys = new Hashtable();
					foreach(string sKey in Context.Session.Keys)
					{
						hashSessionKeys.Add(sKey, null);
					}
					// 06/03/2006   We can't remove a key when it is used in the enumerator. 
					foreach(string sKey in hashSessionKeys.Keys )
					{
						if ( sKey.StartsWith("vwSHORTCUTS_Menu_ByUser") || sKey.StartsWith("vwMODULES_TabMenu_ByUser") )
							Context.Session.Remove(sKey);
					}
				}

				// 07/01/2008   Allow config values to be initialized from the Web.config. 
				// This is so that the default_theme could be specified early. 
				foreach (string sConfig in System.Configuration.ConfigurationManager.AppSettings )
				{
					Application["CONFIG." + sConfig] = Sql.ToString(System.Configuration.ConfigurationManager.AppSettings[sConfig]);
				}

				// 12/03/2008   This function can be called from a scheduled task, so we must pass the application to the GetFactory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					// 07/28/2006   Test the database connection and allow an early exit if failed. 
					con.Open();
					// 10/24/2009   As a performance optimziation, we need a way to avoid calling spSYSTEM_TRANSACTIONS_Create for every transaction. 
					bUseSQLServerToken = false;
					// 01/17/2010   ACL Field Security is only enabled when the vwACL_FIELD_ACCESS_ByUserAlias view exists. 
					bEnableACLFieldSecurity = false;
					if ( Sql.IsSQLServer(con) )
					{
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = "select @@VERSION";
							string sSqlVersion = Sql.ToString(cmd.ExecuteScalar());
							// 10/13/2009   Azure Product database has a different version than the CTP environment. 
							bool bSQLAzure = false;
							if ( sSqlVersion.StartsWith("Microsoft SQL Azure") || (sSqlVersion.IndexOf("SQL Server") > 0 && sSqlVersion.IndexOf("CloudDB") > 0) )
							{
								bSQLAzure = true;
								SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "Connected to Microsoft SQL Azure.");
							}
							bUseSQLServerToken = !bSQLAzure;
						}
					}
					// 10/27/2010   Set the Server Token flag so that we will not waste cycles creating a transaction token. 
					// We will not use auditing on a portable device. 
					else if ( Sql.IsEffiProz(con) )
					{
						bUseSQLServerToken = true;
					}
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						// 01/17/2010   Oracle is case-significant, and will return a view in uppercase. 
						cmd.CommandText = "select count(*) from vwSqlViews where VIEW_NAME = upper('vwACL_FIELD_ACCESS_ByUserAlias')";
						bEnableACLFieldSecurity = Sql.ToBoolean(cmd.ExecuteScalar());
					}
				}

				// 01/12/2006   Separate out the terminology so that it can be called when importing a language pack. 
				InitTerminology(Context);
				// 11/20/2009   Move module init to a separate function. 
				InitModules    (Context);
				InitConfig     (Context);
				// 02/26/2011   Add Field Validators for use by browser extensions. 
				InitFieldValidators(Context);
				InitTimeZones  (Context);
				InitCurrencies (Context);
				// 11/18/2009   We should have been initializing the default Module rights a long time ago. 
				InitModuleACL  (Context);

				// 08/15/2008   If Silverlight is enabled and we are running on Linux, then disable Silverlight and enable Flash. 
				// Mono is having a problem with the inline WPF.
				// 09/22/2008   Move Silverlight disable after InitConfig. 
				int nPlatform = (int) Environment.OSVersion.Platform;
				if ( nPlatform == 4 || nPlatform == 128 )
				{
					SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "Silverlight is disabled on Mono.");
					Application["CONFIG.enable_silverlight"] = false;
					Application["CONFIG.enable_flash"      ] = true;
				}

				
				bool bIsNet45OrNewer = false;
				try
				{
					bIsNet45OrNewer = (Type.GetType("System.Reflection.ReflectionContext", false) != null);
				}
				catch
				{
				}
				Application["System.NET45"] = bIsNet45OrNewer;

				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					string sSQL;
					// 08/02/2008   Track the last date that we loaded the app vars. 
					sSQL = "select max(DATE_ENTERED)" + ControlChars.CrLf
					     + "  from vwSYSTEM_EVENTS  " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						DateTime dtLastUpdate = Sql.ToDateTime(cmd.ExecuteScalar());
						if ( dtLastUpdate == DateTime.MinValue )
							dtLastUpdate = DateTime.Now;
						Application["SYSTEM_EVENTS.MaxDate"] = dtLastUpdate;
						SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "System Events Last Update on " + dtLastUpdate.ToString());
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		// 10/28/2008   Log application stop so that we can track when IIS7 recycles the app. 
		public static void StopApp(HttpContext Context)
		{
			try
			{
				Guid   gUSER_ID          = Guid.Empty;
				string sUSER_NAME        = String.Empty;
				string sMACHINE          = String.Empty;
				string sASPNET_SESSIONID = String.Empty;
				string sREMOTE_HOST      = String.Empty;
				string sSERVER_HOST      = String.Empty;
				string sTARGET           = String.Empty;
				string sRELATIVE_PATH    = String.Empty;
				string sPARAMETERS       = String.Empty;
				string sFILE_NAME        = String.Empty;
				string sMETHOD           = String.Empty;
				string sERROR_TYPE       = "Warning";
				string sMESSAGE          = "Application stop.";
				Int32  nLINE_NUMBER      = 0;

				try
				{
					// 09/17/2009   Azure does not support MachineName.  Just ignore the error. 
					sMACHINE = System.Environment.MachineName;
				}
				catch
				{
				}
				StackFrame stack = new StackTrace(true).GetFrame(0);
				if ( stack != null )
				{
					sFILE_NAME   = stack.GetFileName();
					sMETHOD      = stack.GetMethod().ToString();
					nLINE_NUMBER = stack.GetFileLineNumber();
				}
				try
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory(Context.Application);
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						// 10/07/2009   We need to create our own global transaction ID to support auditing and workflow on SQL Azure, PostgreSQL, Oracle, DB2 and MySQL. 
						using ( IDbTransaction trn = Sql.BeginTransaction(con) )
						{
							try
							{
								SqlProcs.spSYSTEM_LOG_InsertOnly(gUSER_ID, sUSER_NAME, sMACHINE, sASPNET_SESSIONID, sREMOTE_HOST, sSERVER_HOST, sTARGET, sRELATIVE_PATH, sPARAMETERS, sERROR_TYPE, sFILE_NAME, sMETHOD, nLINE_NUMBER, sMESSAGE, trn);
								trn.Commit();
							}
							catch //(Exception ex)
							{
								trn.Rollback();
								// 10/26/2008   Can't throw an exception here as it could create an endless loop. 
								//SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), Utils.ExpandException(ex));
							}
						}
						if ( Sql.IsEffiProz(con) )
						{
							// 12/31/2010 Irantha.  Shutdown command CheckPoints the database (remove the .log file and rewrite the .script file). 
							// If this is not done then the CheckPointing automatically happens when database is reopened next time. 
							// This would increase the next start-up time.  Taoqi has auto shutdown set to false in connection string. 
							// So doing a Shutdown on APPLICATION_END event would reduce the next application start-up time. 
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = "SHUTDOWN";
								cmd.ExecuteNonQuery();
							}
						}
					}
				}
				catch
				{
				}
			}
			catch//(Exception ex)
			{
				//SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				//HttpContext.Current.Response.Write(ex.Message);
			}
		}

		public static XmlDocument InitUserPreferences(string sUSER_PREFERENCES)
		{
			XmlDocument xml = null;
			try
			{
				xml = new XmlDocument();
				// 01/28/2009   Check for empty string before attempting to load preferences. 
				// 03/25/2009   The empty string check needed a NOT condition. 
				if ( !Sql.IsEmptyString(sUSER_PREFERENCES) )
				{
					// 10/17/2009   The XML may not start with processing instructions. 
					if ( !sUSER_PREFERENCES.StartsWith("<?xml ") && !sUSER_PREFERENCES.StartsWith("<xml>") )
					{
						sUSER_PREFERENCES = XmlUtil.ConvertFromPHP(sUSER_PREFERENCES);
					}
					xml.LoadXml(sUSER_PREFERENCES);
				}
				else
				{
					xml.AppendChild(xml.CreateProcessingInstruction("xml" , "version=\"1.0\" encoding=\"UTF-8\""));
					xml.AppendChild(xml.CreateElement("USER_PREFERENCE"));
				}
				
				HttpApplicationState Application = HttpContext.Current.Application;
				string sCulture    = L10N.NormalizeCulture(XmlUtil.SelectSingleNode(xml, "culture"));
				//2014.12.1
                //string sTheme      = XmlUtil.SelectSingleNode(xml, "theme"      );
                string sTheme = "Atlantic";
               
				string sDateFormat = XmlUtil.SelectSingleNode(xml, "dateformat" );
				string sTimeFormat = XmlUtil.SelectSingleNode(xml, "timeformat" );
				string sTimeZone   = XmlUtil.SelectSingleNode(xml, "timezone"   );
				string sCurrencyID = XmlUtil.SelectSingleNode(xml, "currency_id");
				if ( Sql.IsEmptyString(sCulture) )
				{
					XmlUtil.SetSingleNode(xml, "culture", SplendidDefaults.Culture());
				}
				if ( Sql.IsEmptyString(sTheme) )
				{
					XmlUtil.SetSingleNode(xml, "theme", SplendidDefaults.Theme());
				}
				if ( Sql.IsEmptyString(sDateFormat) )
				{
					XmlUtil.SetSingleNode(xml, "dateformat", SplendidDefaults.DateFormat());
				}
				// 11/12/2005   "m" is not valid for .NET month formatting.  Must use MM. 
				// 11/12/2005   Require 4 digit year.  Otherwise default date in Pipeline of 12/31/2100 would get converted to 12/31/00. 
				if ( SplendidDefaults.IsValidDateFormat(sDateFormat) )
				{
					XmlUtil.SetSingleNode(xml, "dateformat", SplendidDefaults.DateFormat(sDateFormat));
				}
				if ( Sql.IsEmptyString(sTimeFormat) )
				{
					XmlUtil.SetSingleNode(xml, "timeformat", SplendidDefaults.TimeFormat());
				}
				if ( Sql.IsEmptyString(sCurrencyID) )
				{
					XmlUtil.SetSingleNode(xml, "currency_id", SplendidDefaults.CurrencyID());
				}
				// 09/01/2006   Only use timez if provided.  Otherwise we will default to GMT. 
				if ( Sql.IsEmptyString(sTimeZone) && !Sql.IsEmptyString(XmlUtil.SelectSingleNode(xml, "timez")) )
				{
					int nTimez = Sql.ToInteger(XmlUtil.SelectSingleNode(xml, "timez"));
					sTimeZone = SplendidDefaults.TimeZone(nTimez);
					XmlUtil.SetSingleNode(xml, "timezone", sTimeZone);
				}
				// 09/01/2006   Default TimeZone was not getting set properly. 
				if ( Sql.IsEmptyString(sTimeZone) )
				{
					sTimeZone = SplendidDefaults.TimeZone();
					XmlUtil.SetSingleNode(xml, "timezone", sTimeZone);
				}
				// 08/12/2009   A customer wants the ability to turn off the saved searches, both globally and on a per user basis. 
				string sSaveQuery  = XmlUtil.SelectSingleNode(xml, "save_query");
				if ( Sql.IsEmptyString(sSaveQuery) )
					XmlUtil.SetSingleNode(xml, "save_query", Sql.ToBoolean(Application["CONFIG.save_query"]).ToString());
				// 02/26/2010   Allow users to configure use of tabs. 
				string sGroupTabs  = XmlUtil.SelectSingleNode(xml, "group_tabs");
				if ( Sql.IsEmptyString(sGroupTabs) )
					XmlUtil.SetSingleNode(xml, "group_tabs", Sql.ToBoolean(Application["CONFIG.default_group_tabs"]).ToString());
				string sSubPanelTabs  = XmlUtil.SelectSingleNode(xml, "subpanel_tabs");
				if ( Sql.IsEmptyString(sSubPanelTabs) )
					XmlUtil.SetSingleNode(xml, "subpanel_tabs", Sql.ToBoolean(Application["CONFIG.default_subpanel_tabs"]).ToString());
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
			}
			return xml;
		}

		// 11/06/2009   Allow preferences to be called with a context. 
		public static void LoadUserPreferences(Guid gID, string sTheme, string sCulture)
		{
			LoadUserPreferences(HttpContext.Current, gID, sTheme, sCulture);
		}

		public static void LoadUserPreferences(HttpContext Context, Guid gID, string sTheme, string sCulture)
		{
            sTheme = "Atlantic";

			HttpApplicationState Application = Context.Application;
			HttpSessionState     Session     = Context.Session    ;
			string sApplicationPath = Sql.ToString(Context.Application["rootURL"]);

			// 10/24/2009   As a performance optimziation, don't lookup the user if we are just trying to initialize preferences prior to login. 
			if ( !Sql.IsEmptyGuid(gID) )
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					string sSQL ;
					// 03/31/2010   We don't need to use vwUSERS_Edit as it adds ADDRESS_HTML and we don't need that field. 
					sSQL = "select *       " + ControlChars.CrLf
					     + "  from vwUSERS " + ControlChars.CrLf
					     + " where ID = @ID" + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Sql.AddParameter(cmd, "@ID", gID);
						con.Open();
						using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
						{
							if ( rdr.Read() )
							{
								// 11/05/2010   Each user can have their own email account, but they all will share the same server. 
								// Remove all references to USER_SETTINGS/MAIL_FROMADDRESS and USER_SETTINGS/MAIL_FROMNAME. 
								Security.EMAIL1    = Sql.ToString(rdr["EMAIL1"   ]);
                                Security.USER_NAME = Sql.ToString(rdr["USER_NAME"]);
								Security.FULL_NAME = Sql.ToString(rdr["FULL_NAME"]);
                                Security.UserMobile = Sql.ToString(rdr["PHONE_MOBILE"]);
                                Security.User_EmailIsActive = Sql.ToBoolean(rdr["C_EmailIsActive"]);
                                                                
								try
								{
									// 11/19/2005   Not sure why the login screen has the language, but it would seem to allow overriding the default. 
									//Session["USER_SETTINGS/CULTURE"          ] = Sql.IsEmptyString(sCulture) ? Sql.ToString(rdr["LANG" ]) : sCulture;
                                    Session["USER_SETTINGS/CULTURE"] = "zh-CN";
									// 11/22/2005   The theme can be overridden as well. 
									//Session["USER_SETTINGS/THEME"            ] = Sql.IsEmptyString(sTheme  ) ? Sql.ToString(rdr["THEME"]) : sTheme  ;
                                    Session["USER_SETTINGS/THEME"] = Sql.IsEmptyString(sTheme) ? Sql.ToString(rdr["THEME"]) : sTheme;
                                    
                                    // 11/30/2012   Save the default them for the user, as specified in the preferences. This is to allow the user to go from the Mobile theme to the full site. 
									Session["USER_SETTINGS/DEFAULT_THEME"    ] = sTheme;

									Session["themeURL"                       ] = sApplicationPath + "App_Themes/" + sTheme + "/";
									//Session["USER_SETTINGS/DATEFORMAT"       ] = Sql.ToString(rdr["DATE_FORMAT"   ]);
                                    Session["USER_SETTINGS/DATEFORMAT"] = "yyyy-MM-dd";
									Session["USER_SETTINGS/TIMEFORMAT"       ] = Sql.ToString(rdr["TIME_FORMAT"   ]);
									// 08/12/2009   A customer wants the ability to turn off the saved searches, both globally and on a per user basis. 
									Session["USER_SETTINGS/SAVE_QUERY"       ] = Sql.ToBoolean(rdr["SAVE_QUERY"   ]);
									// 02/26/2010   Allow users to configure use of tabs. 
									Session["USER_SETTINGS/GROUP_TABS"       ] = Sql.ToBoolean(rdr["GROUP_TABS"   ]);
									Session["USER_SETTINGS/SUBPANEL_TABS"    ] = Sql.ToBoolean(rdr["SUBPANEL_TABS"]);
									Session["USER_SETTINGS/TIMEZONE"         ] = Sql.ToGuid   (rdr["TIMEZONE_ID"  ]);
									// 10/06/2007   Save the original timezone value so that we can display the timezone selector if necessary. 
									Session["USER_SETTINGS/TIMEZONE/ORIGINAL"] = Sql.ToString (rdr["TIMEZONE_ID"  ]);
									Session["USER_SETTINGS/CURRENCY"         ] = Sql.ToString (rdr["CURRENCY_ID"  ]);
								}
								catch(Exception ex)
								{
									SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
								}
								// 08/24/2013   Add EXTENSION_C in preparation for Asterisk click-to-call. 
								// 09/20/2013   Move EXTENSION to the main table. 
								// 09/27/2013   SMS messages need to be opt-in. 
								try
								{
									Session["PHONE_WORK"  ] = Sql.ToString(rdr["PHONE_WORK"  ]);
									Session["EXTENSION"   ] = Sql.ToString(rdr["EXTENSION"   ]);
									Session["PHONE_MOBILE"] = Sql.ToString(rdr["PHONE_MOBILE"]);
									Session["SMS_OPT_IN"  ] = Sql.ToString(rdr["SMS_OPT_IN"  ]);
								}
								catch(Exception ex)
								{
									SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
								}
							}
						}
					}
				}
			}
			// 11/21/2005   New users may not have any settings, so we need to initialize the defaults.
			// It is best to do it here rather than wrap the variables in a function that would return the default if null.
			sCulture    = Sql.ToString(Session["USER_SETTINGS/CULTURE"   ]);
			sTheme      = Sql.ToString(Session["USER_SETTINGS/THEME"     ]);
			string sDateFormat = Sql.ToString(Session["USER_SETTINGS/DATEFORMAT"]);
			string sTimeFormat = Sql.ToString(Session["USER_SETTINGS/TIMEFORMAT"]);
			string sTimeZone   = Sql.ToString(Session["USER_SETTINGS/TIMEZONE"  ]);
			string sCurrencyID = Sql.ToString(Session["USER_SETTINGS/CURRENCY"  ]);
			if ( Sql.IsEmptyString(sCulture) )
			{
				Session["USER_SETTINGS/CULTURE"   ] = SplendidDefaults.Culture();
			}
			// 11/17/2007   If running on a mobile device, then use the mobile theme. 
			if ( Utils.IsMobileDevice )
			{
				if ( Directory.Exists(Context.Server.MapPath("~/App_MasterPages/" + SplendidDefaults.MobileTheme())) )
				{
					sTheme = SplendidDefaults.MobileTheme();
					Session["USER_SETTINGS/THEME"] = sTheme;
				}
			}
			else if ( Sql.IsEmptyString(sTheme) )
			{
				sTheme = SplendidDefaults.Theme();
				Session["USER_SETTINGS/THEME"] = sTheme;
			}
			// 03/07/2007   Version 1.4 moved its themes folder to the .NET 2.0 default App_Themes. 
			Session["themeURL"] = sApplicationPath + "App_Themes/" + sTheme + "/";
			if ( Sql.IsEmptyString(sDateFormat) )
			{
				Session["USER_SETTINGS/DATEFORMAT"] = SplendidDefaults.DateFormat();
			}
			// 11/12/2005   "m" is not valid for .NET month formatting.  Must use MM. 
			// 11/12/2005   Require 4 digit year.  Otherwise default date in Pipeline of 12/31/2100 would get converted to 12/31/00. 
			if ( SplendidDefaults.IsValidDateFormat(sDateFormat) )
			{
				Session["USER_SETTINGS/DATEFORMAT"] = SplendidDefaults.DateFormat(sDateFormat);
			}
			if ( Sql.IsEmptyString(sTimeFormat) )
			{
				Session["USER_SETTINGS/TIMEFORMAT"] = SplendidDefaults.TimeFormat();
			}
			if ( Sql.IsEmptyString(sCurrencyID) )
			{
				Session["USER_SETTINGS/CURRENCY"  ] = SplendidDefaults.CurrencyID();
			}
			if ( Sql.IsEmptyString(sTimeZone) )
			{
				Session["USER_SETTINGS/TIMEZONE"  ] = SplendidDefaults.TimeZone();
			}

			// 08/12/2009   A customer wants the ability to turn off the saved searches, both globally and on a per user basis. 
			string sSaveQuery  = Sql.ToString(Session["USER_SETTINGS/SAVE_QUERY"]);
			if ( Sql.IsEmptyString(sSaveQuery) )
				Session["USER_SETTINGS/SAVE_QUERY"] = Sql.ToBoolean(Application["CONFIG.save_query"]).ToString();
			// 02/27/2010   This area will initialize fields for users created via NTLM. 
			string sGroupTabs  = Sql.ToString(Session["USER_SETTINGS/GROUP_TABS"]);
			if ( Sql.IsEmptyString(sGroupTabs) )
				Session["USER_SETTINGS/GROUP_TABS"] = Sql.ToBoolean(Application["CONFIG.default_group_tabs"]).ToString();
			string sSubPanelTabs  = Sql.ToString(Session["USER_SETTINGS/SUBPANEL_TABS"]);
			if ( Sql.IsEmptyString(sSubPanelTabs) )
				Session["USER_SETTINGS/SUBPANEL_TABS"] = Sql.ToBoolean(Application["CONFIG.default_subpanel_tabs"]).ToString();
		}

		// 01/18/2010   Provide a way to clear ACL rules so that the admin can see immediate effects of the rules (when debugging). 
		public static void ClearUserACL()
		{
			HttpContext          Context     = HttpContext.Current;
			HttpSessionState     Session     = Context.Session    ;
			
			Hashtable hashSessionKeys = new Hashtable();
			foreach(string sKey in Context.Session.Keys)
			{
				hashSessionKeys.Add(sKey, null);
			}
			// 06/03/2006   We can't remove a key when it is used in the enumerator. 
			foreach(string sKey in hashSessionKeys.Keys )
			{
				if ( sKey.StartsWith("ACLACCESS_") || sKey.StartsWith("ACLFIELD_") )
					Context.Session.Remove(sKey);
			}
		}

		// 06/09/2009   We need to access LoadUserACL from the SOAP calls. 
		public static void LoadUserACL(Guid gUSER_ID)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory(HttpContext.Current.Application);
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				// 03/09/2010   Admin roles are managed separately. 
				sSQL = "select MODULE_NAME          " + ControlChars.CrLf
				     + "     , ACLACCESS_ADMIN      " + ControlChars.CrLf
				     + "     , ACLACCESS_ACCESS     " + ControlChars.CrLf
				     + "     , ACLACCESS_VIEW       " + ControlChars.CrLf
				     + "     , ACLACCESS_LIST       " + ControlChars.CrLf
				     + "     , ACLACCESS_EDIT       " + ControlChars.CrLf
				     + "     , ACLACCESS_DELETE     " + ControlChars.CrLf
				     + "     , ACLACCESS_IMPORT     " + ControlChars.CrLf
				     + "     , ACLACCESS_EXPORT     " + ControlChars.CrLf
				     + "     , IS_ADMIN             " + ControlChars.CrLf
				     + "  from vwACL_ACCESS_ByUser  " + ControlChars.CrLf
				     + " where USER_ID = @USER_ID   " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@USER_ID", gUSER_ID);
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							string sMODULE_NAME = Sql.ToString(rdr["MODULE_NAME"]);
							Security.SetUserAccess(sMODULE_NAME, "admin" , Sql.ToInteger(rdr["ACLACCESS_ADMIN" ]));
							Security.SetUserAccess(sMODULE_NAME, "access", Sql.ToInteger(rdr["ACLACCESS_ACCESS"]));
							Security.SetUserAccess(sMODULE_NAME, "view"  , Sql.ToInteger(rdr["ACLACCESS_VIEW"  ]));
							Security.SetUserAccess(sMODULE_NAME, "list"  , Sql.ToInteger(rdr["ACLACCESS_LIST"  ]));
							Security.SetUserAccess(sMODULE_NAME, "edit"  , Sql.ToInteger(rdr["ACLACCESS_EDIT"  ]));
							Security.SetUserAccess(sMODULE_NAME, "delete", Sql.ToInteger(rdr["ACLACCESS_DELETE"]));
							Security.SetUserAccess(sMODULE_NAME, "import", Sql.ToInteger(rdr["ACLACCESS_IMPORT"]));
							Security.SetUserAccess(sMODULE_NAME, "export", Sql.ToInteger(rdr["ACLACCESS_EXPORT"]));
						}
					}
				}
				if ( bEnableACLFieldSecurity )
				{
					sSQL = "select MODULE_NAME                   " + ControlChars.CrLf
					     + "     , FIELD_NAME                    " + ControlChars.CrLf
					     + "     , ACLACCESS                     " + ControlChars.CrLf
					     + "  from vwACL_FIELD_ACCESS_ByUserAlias" + ControlChars.CrLf
					     + " where USER_ID = @USER_ID            " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						Sql.AddParameter(cmd, "@USER_ID", gUSER_ID);
						using ( IDataReader rdr = cmd.ExecuteReader() )
						{
							while ( rdr.Read() )
							{
								string sMODULE_NAME = Sql.ToString (rdr["MODULE_NAME"]);
								string sFIELD_NAME  = Sql.ToString (rdr["FIELD_NAME" ]);
								int    nACLACCESS   = Sql.ToInteger(rdr["ACLACCESS"  ]);
								Security.SetUserFieldSecurity(sMODULE_NAME, sFIELD_NAME, nACLACCESS);
							}
						}
					}
				}
			}
		}

		// 11/11/2010   Provide quick access to ACL Roles and Teams. 
		public static void LoadACLRoles(Guid gUSER_ID)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory(HttpContext.Current.Application);
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				// 03/09/2010   Admin roles are managed separately. 
				sSQL = "select ROLE_NAME            " + ControlChars.CrLf
				     + "  from vwACL_ROLES_USERS    " + ControlChars.CrLf
				     + " where USER_ID = @USER_ID   " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@USER_ID", gUSER_ID);
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							string sROLE_NAME = Sql.ToString(rdr["ROLE_NAME"]);
							Security.SetACLRoleAccess(sROLE_NAME);
						}
					}
				}
			}
		}

		public static void LoadTeams(Guid gUSER_ID)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory(HttpContext.Current.Application);
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				// 03/09/2010   Admin roles are managed separately. 
				sSQL = "select distinct TEAM_NAME     " + ControlChars.CrLf
				     + "  from vwTEAM_MEMBERSHIPS_List" + ControlChars.CrLf
				     + " where USER_ID = @USER_ID     " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@USER_ID", gUSER_ID);
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						while ( rdr.Read() )
						{
							string sTEAM_NAME = Sql.ToString(rdr["TEAM_NAME"]);
							Security.SetTeamAccess(sTEAM_NAME);
						}
					}
				}
			}
		}

		// 02/20/2011   Log the failure so that we can lockout the user. 
		// The current implementation uses the Application cache instead of a database table. 
		public static void LoginTracking(HttpApplicationState Application, string sUSER_NAME, bool bValidUser)
		{
			if ( bValidUser )
			{
				Application.Remove("Users.LoginFailures." + sUSER_NAME);
			}
			else
			{
				int nLoginFailures = Sql.ToInteger(Application["Users.LoginFailures." + sUSER_NAME]);
				Application["Users.LoginFailures." + sUSER_NAME] = nLoginFailures + 1;
			}
		}

		public static int LoginFailures(HttpApplicationState Application, string sUSER_NAME)
		{
			return Sql.ToInteger(Application["Users.LoginFailures." + sUSER_NAME]);
		}

		// 04/16/2013   Allow system to be restricted by IP Address. 
		public static bool InvalidIPAddress(HttpApplicationState Application, string sUserHostAddress)
		{
			string sIPAddresses = Sql.ToString(Application["CONFIG.Authentication.IPAddresses"]);
			// 04/16/2013   Allow the separator to be either a comma or a space character. 
			sIPAddresses = sIPAddresses.Replace(",", " ");
			sIPAddresses = sIPAddresses.Trim();
			// 04/16/2013   If no IP Addresses are specified, then assume that all are valid. 
			if ( !Sql.IsEmptyString(sIPAddresses) )
			{
				string[] arrIPAddresses = sIPAddresses.Split(' ');
				foreach ( string sValidIP in arrIPAddresses )
				{
					if ( sUserHostAddress == sValidIP )
						return false;
				}
				return true;
			}
			return false;
		}

        public static bool LoginUser(string sUSER_NAME, string sPASSWORD, string sTHEME, string sLANGUAGE, string sUSER_DOMAIN, bool bIS_ADMIN, bool isEncryptPwd = false)
		{
			HttpContext          Context     = HttpContext.Current;
			HttpApplicationState Application = Context.Application;
			HttpSessionState     Session     = Context.Session    ;
			HttpRequest          Request     = Context.Request    ;

			bool bValidUser = false;
			DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				// 03/22/2006   The user name should be case-insignificant.  The password is case-significant. 
				// 03/22/2006   DB2 does not like lower(USER_NAME) = lower(@USER_NAME).  It returns the following error. 
				// ERROR [42610] [IBM][DB2/NT] SQL0418N A statement contains a use of a parameter marker that is not valid. SQLSTATE=42610 
				// 05/23/2006   Use vwUSERS_Login so that USER_HASH can be removed from vwUSERS to prevent its use in reports. 
				// 11/25/2006   Include TEAM_ID and TEAM_NAME as they will be used everywhere. 
				// 03/16/2010   Add IS_ADMIN_DELEGATE. 
				// 03/16/2010   Retrieve all fields to allow field errors to be caught and reported. 
				sSQL = "select *            " + ControlChars.CrLf
				     + "  from vwUSERS_Login" + ControlChars.CrLf;
				// 03/16/2010   Stop using lower() on SQL Server to increase performance. 
				if ( Sql.IsOracle(con) || Sql.IsDB2(con) || Sql.IsPostgreSQL(con) )
					sSQL += " where lower(USER_NAME) = @USER_NAME" + ControlChars.CrLf;
				else
					sSQL += " where USER_NAME = @USER_NAME" + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					// 01/15/2009   On slow systems running Express, the first login event can take longer than expected, so just wait forever.
					cmd.CommandTimeout = 0;
/*
#if DEBUG
					if ( sUSER_NAME == "paulrony" && Sql.ToString(Application["SplendidProvider"]) == "MySql.Data" )
						sUSER_NAME = "admin";
#endif
*/
					// 03/22/2006   Convert the name to lowercase here. 
					Sql.AddParameter(cmd, "@USER_NAME", sUSER_NAME.ToLower());
					string sLOGIN_TYPE = "Windows";
					// 11/19/2005   sUSER_DOMAIN is used to determine if NTLM is enabled. 
					if ( Sql.IsEmptyString(sUSER_DOMAIN) )
					{
						sLOGIN_TYPE = "Anonymous";
						if ( !Sql.IsEmptyString(sPASSWORD) )
						{
							//string sUSER_HASH = Security.HashPassword(sPASSWORD);

                            //2015.4.29 
                            string sUSER_HASH = string.Empty;

                            if (isEncryptPwd) //����Ǽ��ܵ����룬�򲻴���
                            {
                                sUSER_HASH = sPASSWORD;
                            }
                            else //������������룬�����
                            {
                                sUSER_HASH = Security.HashPassword(sPASSWORD);
                            }

							cmd.CommandText += "   and USER_HASH = @USER_HASH" + ControlChars.CrLf;
							Sql.AddParameter(cmd, "@USER_HASH", sUSER_HASH);
						}
						else
						{
							// 11/19/2005   Handle the special case of the password stored as NULL or empty string. 
							cmd.CommandText += "   and (USER_HASH = '' or USER_HASH is null)" + ControlChars.CrLf;
						}
					}
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						//string sApplicationPath = Sql.ToString(Application["rootURL"]);
						Guid gUSER_LOGIN_ID = Guid.Empty;
						if ( rdr.Read() )
						{
							// 11/19/2005   Clear all session values. 
							// 02/28/2007   Centralize session reset to prepare for WebParts. 
							Security.Clear();
							Security.USER_ID     = Sql.ToGuid   (rdr["ID"         ]);
							Security.USER_NAME   = Sql.ToString (rdr["USER_NAME"  ]);
							Security.FULL_NAME   = Sql.ToString (rdr["FULL_NAME"  ]);
							Security.isAdmin    = Sql.ToBoolean(rdr["IS_ADMIN"   ]);
                            Security.PORTAL_ONLY = Sql.ToBoolean(rdr["PORTAL_ONLY"]);
                            Security.RealName    = Sql.ToString (rdr["LAST_NAME"]);
                            Permission.Init();

							try
							{
								// 11/25/2006   Keep the private team information in the Session for quick access. 
								// The private team may be replaced by the desired default in User Preferences. 
								Security.TEAM_ID        = Sql.ToGuid  (rdr["TEAM_ID"       ]);
								Security.TEAM_NAME      = Sql.ToString(rdr["TEAM_NAME"     ]);
							}
							catch(Exception ex)
							{
								// 11/25/2006   Ignore any team related issue as this error could prevent 
								// anyone from logging in.  The CRM would then be completely dead. 
								SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), "Failed to read TEAM_ID. " + ex.Message);
								HttpContext.Current.Response.Write("Failed to read TEAM_ID. " + ex.Message);
							}
							try
							{
								// 04/04/2010   Add Exchange Alias so that we can enable/disable Exchange appropriately. 
								Security.EXCHANGE_ALIAS = Sql.ToString(rdr["EXCHANGE_ALIAS"]);
								// 04/07/2010   Add Exchange Email as it will be need for Push Subscriptions. 
								Security.EXCHANGE_EMAIL = Sql.ToString(rdr["EXCHANGE_EMAIL"]);
								// 11/05/2010   Each user can have their own email account, but they all will share the same server. 
								// Remove all references to USER_SETTINGS/MAIL_FROMADDRESS and USER_SETTINGS/MAIL_FROMNAME. 
								Security.EMAIL1 = Sql.ToString(rdr["EMAIL1"]);
							}
							catch(Exception ex)
							{
								// 11/25/2006   Ignore any team related issue as this error could prevent 
								// anyone from logging in.  The CRM would then be completely dead. 
								SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), "Failed to read EXCHANGE_ALIAS. " + ex.Message);
								HttpContext.Current.Response.Write("Failed to read EXCHANGE_ALIAS. " + ex.Message);
							}
							try
							{
								// 07/09/2010   Move the SMTP values from USER_PREFERENCES to the main table to make it easier to access. 
								Security.MAIL_SMTPUSER = Sql.ToString (rdr["MAIL_SMTPUSER"    ]);
								Security.MAIL_SMTPPASS = Sql.ToString (rdr["MAIL_SMTPPASS"    ]);
							}
							catch(Exception ex)
							{
								SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), "Failed to read MAIL_SMTPUSER. " + ex.Message);
								HttpContext.Current.Response.Write("Failed to read MAIL_SMTPUSER. " + ex.Message);
							}
							try
							{
								// 03/16/2010   Add IS_ADMIN_DELEGATE. 
								Security.IS_ADMIN_DELEGATE = Sql.ToBoolean(rdr["IS_ADMIN_DELEGATE"]);
							}
							catch(Exception ex)
							{
								SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), "Failed to read IS_ADMIN_DELEGATE. " + ex.Message);
								HttpContext.Current.Response.Write("Failed to read IS_ADMIN_DELEGATE. " + ex.Message);
							}
							try
							{
								// 02/22/2011   Add PWD_LAST_CHANGED and SYSTEM_GENERATED_PASSWORD. 
								// 02/22/2011   Password expiration only applies to Anonymous Authentication. 
								if ( sLOGIN_TYPE == "Anonymous" )
								{
									bool     bSYSTEM_GENERATED_PASSWORD = Sql.ToBoolean (rdr["SYSTEM_GENERATED_PASSWORD"]);
									DateTime dtPWD_LAST_CHANGED         = Sql.ToDateTime(rdr["PWD_LAST_CHANGED"         ]);
									int nExpirationDays = Crm.Password.ExpirationDays(Application);
									if ( nExpirationDays > 0 )
									{
										if ( dtPWD_LAST_CHANGED == DateTime.MinValue || dtPWD_LAST_CHANGED.AddDays(nExpirationDays) < DateTime.Now )
										{
											// 02/22/2011   Use the same System Generated flag to force the password change. 
											bSYSTEM_GENERATED_PASSWORD = true;
										}
									}
									Session["SYSTEM_GENERATED_PASSWORD"] = bSYSTEM_GENERATED_PASSWORD;
								}
							}
							catch(Exception ex)
							{
								SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), "Failed to read SYSTEM_GENERATED_PASSWORD. " + ex.Message);
								HttpContext.Current.Response.Write("Failed to read SYSTEM_GENERATED_PASSWORD. " + ex.Message);
							}
							
							Guid gUSER_ID = Sql.ToGuid(rdr["ID"]);
							// 03/02/2008   Log the logins. 
							// 10/27/2010   No need to log the logins on a portable device. 
							if ( !Sql.IsEffiProz(con) )
								SqlProcs.spUSERS_LOGINS_InsertOnly(ref gUSER_LOGIN_ID, gUSER_ID, sUSER_NAME, sLOGIN_TYPE, "Succeeded", Session.SessionID, Request.UserHostName, Request.Url.Host, Request.Path, Request.AppRelativeCurrentExecutionFilePath, Request.UserAgent);
							Security.USER_LOGIN_ID = gUSER_LOGIN_ID;
							// 02/20/2011   Log the success so that we can lockout the user. 
							SplendidInit.LoginTracking(Application, sUSER_NAME, true);

							// 08/08/2006   Don't supply the Language as it prevents the user value from being used. 
							// This bug is a hold-over from the time we removed the Lauguage combo from the login screen. 
							LoadUserPreferences(gUSER_ID, sTHEME, String.Empty);
							LoadUserACL(gUSER_ID);
							// 11/11/2010   Provide quick access to ACL Roles and Teams. 
							LoadACLRoles(gUSER_ID);
							LoadTeams(gUSER_ID);
							bValidUser = true;
							SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "User login.");
						}
						else if ( Security.IsWindowsAuthentication() )
						{
							rdr.Close();
							// 11/04/2005.  If user does not exist, then create it, but only if NTLM is used. 
							Guid gUSER_ID = Guid.Empty;
							SqlProcs.spUSERS_InsertNTLM(ref gUSER_ID, sUSER_DOMAIN, sUSER_NAME, bIS_ADMIN);

							// 11/19/2005   Clear all session values. 
							// 02/28/2007   Centralize session reset to prepare for WebParts. 
							Security.Clear();
							Security.USER_ID     = gUSER_ID  ;
							Security.USER_NAME   = sUSER_NAME;
							Security.isAdmin    = bIS_ADMIN ;
							Security.PORTAL_ONLY = false     ;

							// 03/02/2008   Log the logins. 
							// 10/27/2010   No need to log the logins on a portable device. 
							if ( !Sql.IsEffiProz(con) )
								SqlProcs.spUSERS_LOGINS_InsertOnly(ref gUSER_LOGIN_ID, gUSER_ID, sUSER_NAME, sLOGIN_TYPE, "Succeeded", Session.SessionID, Request.UserHostName, Request.Url.Host, Request.Path, Request.AppRelativeCurrentExecutionFilePath, Request.UserAgent);
							Security.USER_LOGIN_ID = gUSER_LOGIN_ID;
							// 02/20/2011   Log the success so that we can lockout the user. 
							SplendidInit.LoginTracking(Application, sUSER_NAME, true);

							// 11/25/2006   Retrieve TEAM_ID and TEAM_NAME as they will be used everywhere. 
							sSQL = "select TEAM_ID      " + ControlChars.CrLf
							     + "     , TEAM_NAME    " + ControlChars.CrLf
							     + "  from vwUSERS_Login" + ControlChars.CrLf
							     + " where ID = @ID     " + ControlChars.CrLf;
							cmd.Parameters.Clear();
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@ID", Security.USER_ID);
							using ( IDataReader rdrTeam = cmd.ExecuteReader() )
							{
								if ( rdrTeam.Read() )
								{
									try
									{
										// 11/25/2006   Keep the private team information in the Session for quick access. 
										// The private team may be replaced by the desired default in User Preferences. 
										Security.TEAM_ID   = Sql.ToGuid   (rdrTeam["TEAM_ID"  ]);
										Security.TEAM_NAME = Sql.ToString (rdrTeam["TEAM_NAME"]);
									}
									catch(Exception ex)
									{
										SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), "Failed to read TEAM_ID. " + ex.Message);
										HttpContext.Current.Response.Write("Failed to read TEAM_ID. " + ex.Message);
									}
								}
							}

							// 11/21/2005   Load the preferences to initialize cuture, date, time and currency preferences.
							LoadUserPreferences(gUSER_ID, String.Empty, String.Empty);
							LoadUserACL(gUSER_ID);
							// 11/11/2010   Provide quick access to ACL Roles and Teams. 
							LoadACLRoles(gUSER_ID);
							LoadTeams(gUSER_ID);
							bValidUser = true;
							SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "User login.");
						}
						else
						{
							// 03/02/2008   Log the logins. 
							// 10/27/2010   No need to log the logins on a portable device. 
							if ( !Sql.IsEffiProz(con) )
								SqlProcs.spUSERS_LOGINS_InsertOnly(ref gUSER_LOGIN_ID, Guid.Empty, sUSER_NAME, sLOGIN_TYPE, "Failed", Session.SessionID, Request.UserHostName, Request.Url.Host, Request.Path, Request.AppRelativeCurrentExecutionFilePath, Request.UserAgent);
							// 02/20/2011   Log the failure so that we can lockout the user. 
							SplendidInit.LoginTracking(Application, sUSER_NAME, false);
							// 11/22/2005   Initialize preferences even if login fails so that the theme gets set to the default value. 
							LoadUserPreferences(Guid.Empty, String.Empty, String.Empty);
						}
					}
				}
			}
			return bValidUser; // throw(new Exception("Users.ERR_INVALID_PASSWORD"));
		}

		// 03/19/2011   Facebook login uses the FACEBOOK_ID field. 
		public static bool FacebookLoginUser(string sFACEBOOK_ID)
		{
			bool bValidUser = false;
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				sSQL = "select ID                        " + ControlChars.CrLf
				     + "  from vwUSERS_Login             " + ControlChars.CrLf
				     + " where FACEBOOK_ID = @FACEBOOK_ID" + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					cmd.CommandTimeout = 0;
					Sql.AddParameter(cmd, "@FACEBOOK_ID", sFACEBOOK_ID.ToLower());
					Guid gUSER_ID = Sql.ToGuid(cmd.ExecuteScalar());
					if ( !Sql.IsEmptyGuid(gUSER_ID) )
					{
						LoginUser(gUSER_ID, "facebook");
						bValidUser = true;
					}
				}
			}
			return bValidUser;
		}

		// 04/11/2011   We need to allow a login by ID in order to support impersonation by admin. 
		public static void LoginUser(Guid gUSER_ID, string sLOGIN_TYPE)
		{
			HttpContext          Context     = HttpContext.Current;
			HttpApplicationState Application = Context.Application;
			HttpSessionState     Session     = Context.Session    ;
			HttpRequest          Request     = Context.Request    ;

			DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				sSQL = "select *            " + ControlChars.CrLf
				     + "  from vwUSERS_Login" + ControlChars.CrLf
				     + " where ID = @ID     " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					cmd.CommandTimeout = 0;
					Sql.AddParameter(cmd, "@ID", gUSER_ID);
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						Guid gUSER_LOGIN_ID = Guid.Empty;
						if ( rdr.Read() )
						{
							Security.Clear();
							Security.USER_ID                    = Sql.ToGuid    (rdr["ID"                       ]);
							Security.USER_NAME                  = Sql.ToString  (rdr["USER_NAME"                ]);
							Security.FULL_NAME                  = Sql.ToString  (rdr["FULL_NAME"                ]);
							Security.isAdmin                   = Sql.ToBoolean (rdr["IS_ADMIN"                 ]);
							Security.PORTAL_ONLY                = Sql.ToBoolean (rdr["PORTAL_ONLY"              ]);
							Security.TEAM_ID                    = Sql.ToGuid    (rdr["TEAM_ID"                  ]);
							Security.TEAM_NAME                  = Sql.ToString  (rdr["TEAM_NAME"                ]);
							Security.EXCHANGE_ALIAS             = Sql.ToString  (rdr["EXCHANGE_ALIAS"           ]);
							Security.EXCHANGE_EMAIL             = Sql.ToString  (rdr["EXCHANGE_EMAIL"           ]);
							Security.EMAIL1                     = Sql.ToString  (rdr["EMAIL1"                   ]);
							Security.MAIL_SMTPUSER              = Sql.ToString  (rdr["MAIL_SMTPUSER"            ]);
							Security.MAIL_SMTPPASS              = Sql.ToString  (rdr["MAIL_SMTPPASS"            ]);
							Security.IS_ADMIN_DELEGATE          = Sql.ToBoolean (rdr["IS_ADMIN_DELEGATE"        ]);
							string sUSER_NAME = Security.USER_NAME;
							
							
							if ( !Sql.IsEffiProz(con) )
								SqlProcs.spUSERS_LOGINS_InsertOnly(ref gUSER_LOGIN_ID, gUSER_ID, sUSER_NAME, sLOGIN_TYPE, "Succeeded", Session.SessionID, Request.UserHostName, Request.Url.Host, Request.Path, Request.AppRelativeCurrentExecutionFilePath, Request.UserAgent);
							Security.USER_LOGIN_ID = gUSER_LOGIN_ID;
							SplendidInit.LoginTracking(Application, sUSER_NAME, true);

							LoadUserPreferences(gUSER_ID, String.Empty, String.Empty);
							LoadUserACL(gUSER_ID);
							LoadACLRoles(gUSER_ID);
							LoadTeams(gUSER_ID);
							SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "User login.");
						}
						else
						{
							throw(new Exception("Users.ERR_INVALID_USER"));
						}
					}
				}
			}
		}

		// 04/13/2009   We need a separate login function for portal users. 
		public static bool LoginPortalUser(string sUSER_NAME, string sPASSWORD, string sTHEME, string sLANGUAGE)
		{
			return LoginPortalUser(sUSER_NAME, sPASSWORD, sTHEME, sLANGUAGE, false);
		}
		
		// 03/19/2011   If the facebook user has been authenticated, then all we will have is the user name. 
		public static bool LoginPortalUser(string sUSER_NAME, string sPASSWORD, string sTHEME, string sLANGUAGE, bool bFacebookLogin)
		{
			HttpContext          Context     = HttpContext.Current;
			HttpApplicationState Application = Context.Application;
			HttpSessionState     Session     = Context.Session    ;
			HttpRequest          Request     = Context.Request    ;

			bool bValidUser = false;
			DbProviderFactory dbf = DbProviderFactories.GetFactory(Application);
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				sSQL = "select ID                                   " + ControlChars.CrLf
				     + "     , PORTAL_NAME                          " + ControlChars.CrLf
				     + "     , FULL_NAME                            " + ControlChars.CrLf
				     + "     , TEAM_ID                              " + ControlChars.CrLf
				     + "     , TEAM_NAME                            " + ControlChars.CrLf
				     + "  from vwCONTACTS_PortalLogin               " + ControlChars.CrLf
				     + " where lower(PORTAL_NAME) = @PORTAL_NAME    " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					// 01/15/2009   On slow systems running Express, the first login event can take longer than expected, so just wait forever.
					cmd.CommandTimeout = 0;
					// 03/22/2006   Convert the name to lowercase here. 
					Sql.AddParameter(cmd, "@PORTAL_NAME", sUSER_NAME.ToLower());
					
					string sLOGIN_TYPE = "Anonymous";
					// 03/19/2011   If the facebook user has been authenticated, then all we will have is the user name. 
					if ( !bFacebookLogin )
					{
						cmd.CommandText += "   and PORTAL_PASSWORD    = @PORTAL_PASSWORD" + ControlChars.CrLf;
						string sUSER_HASH = Security.HashPassword(sPASSWORD);
						Sql.AddParameter(cmd, "@PORTAL_PASSWORD", sUSER_HASH);
					}
					using ( IDataReader rdr = cmd.ExecuteReader() )
					{
						Guid gUSER_LOGIN_ID = Guid.Empty;
						if ( rdr.Read() )
						{
							// 11/19/2005   Clear all session values. 
							// 02/28/2007   Centralize session reset to prepare for WebParts. 
							Security.Clear();
							Security.USER_ID     = Sql.ToGuid   (rdr["ID"         ]);
							Security.USER_NAME   = Sql.ToString (rdr["PORTAL_NAME"]);
							Security.FULL_NAME   = Sql.ToString (rdr["FULL_NAME"  ]);
							Security.isAdmin    = false;
							Security.PORTAL_ONLY = true;
							Security.TEAM_ID     = Sql.ToGuid   (rdr["TEAM_ID"    ]);
							Security.TEAM_NAME   = Sql.ToString (rdr["TEAM_NAME"  ]);
							
							Guid gUSER_ID = Sql.ToGuid(rdr["ID"]);
							// 03/02/2008   Log the logins. 
							// 10/27/2010   No need to log the logins on a portable device. 
							if ( !Sql.IsEffiProz(con) )
								SqlProcs.spUSERS_LOGINS_InsertOnly(ref gUSER_LOGIN_ID, gUSER_ID, sUSER_NAME, sLOGIN_TYPE, "Succeeded", Session.SessionID, Request.UserHostName, Request.Url.Host, Request.Path, Request.AppRelativeCurrentExecutionFilePath, Request.UserAgent);
							Security.USER_LOGIN_ID = gUSER_LOGIN_ID;
							// 02/20/2011   Log the success so that we can lockout the user. 
							SplendidInit.LoginTracking(Application, sUSER_NAME, true);

							// 08/08/2006   Don't supply the Language as it prevents the user value from being used. 
							// This bug is a hold-over from the time we removed the Lauguage combo from the login screen. 
							LoadUserPreferences(gUSER_ID, sTHEME, String.Empty);
							LoadUserACL(gUSER_ID);
							// 11/11/2010   Provide quick access to ACL Roles and Teams. 
							LoadACLRoles(gUSER_ID);
							LoadTeams(gUSER_ID);
							bValidUser = true;
							SplendidError.SystemMessage(Context, "Warning", new StackTrace(true).GetFrame(0), "User login.");
						}
						else
						{
							// 03/02/2008   Log the logins. 
							// 10/27/2010   No need to log the logins on a portable device. 
							if ( !Sql.IsEffiProz(con) )
								SqlProcs.spUSERS_LOGINS_InsertOnly(ref gUSER_LOGIN_ID, Guid.Empty, sUSER_NAME, sLOGIN_TYPE, "Failed", Session.SessionID, Request.UserHostName, Request.Url.Host, Request.Path, Request.AppRelativeCurrentExecutionFilePath, Request.UserAgent);
							// 02/20/2011   Log the failure so that we can lockout the user. 
							SplendidInit.LoginTracking(Application, sUSER_NAME, false);
							// 11/22/2005   Initialize preferences even if login fails so that the theme gets set to the default value. 
							LoadUserPreferences(Guid.Empty, String.Empty, String.Empty);
						}
					}
				}
			}
			return bValidUser; // throw(new Exception("Users.ERR_INVALID_PASSWORD"));
		}

		public static void ChangeTheme(string sTHEME, string sLANGUAGE)
		{
			// 05/04/2010   Theme may not be available in the master page. 
			if ( !Sql.IsEmptyString(sTHEME) )
			{
				string sApplicationPath = Sql.ToString(HttpContext.Current.Application["rootURL"]);
				// 04/26/2006   The theme variable also needs to be updated.
				HttpContext.Current.Session["USER_SETTINGS/THEME"  ] = sTHEME;
				// 03/07/2007   Version 1.4 moved its themes folder to the .NET 2.0 default App_Themes. 
				HttpContext.Current.Session["themeURL"             ] = sApplicationPath + "App_Themes/" + sTHEME + "/";
			}
			// 05/04/2010   Language may not be available in the master page. 
			if ( !Sql.IsEmptyString(sLANGUAGE) )
				HttpContext.Current.Session["USER_SETTINGS/CULTURE"] = sLANGUAGE;
		}

		public static bool LoginUser(string sUSER_NAME, string sPASSWORD, string sTHEME, string sLANGUAGE)
		{
			return LoginUser(sUSER_NAME, sPASSWORD, sTHEME, sLANGUAGE, String.Empty, false);
		}

		public static void InitSession(HttpContext Context)
		{
			InitAppURLs(Context);
			try
			{
				// 11/22/2005   Always initialize the theme and language. 
				HttpSessionState Session = Context.Session ;
				string sTheme = SplendidDefaults.Theme();
				// 11/24/2010   .NET 4 has broken the compatibility of the browser file system. 
				// We are going to minimize our reliance on browser files in order to reduce deployment issues. 
				Session["Browser"            ] = Context.Request.Browser.Browser;
				Session["IsMobileDevice"     ] = Context.Request.Browser.IsMobileDevice;
				Session["SupportsPopups"     ] = true;
				Session["AllowAutoComplete"  ] = true;
				Session["SupportsSpeech"     ] = false;
				Session["SupportsHandwriting"] = false;
				Session["SupportsTouch"      ] = false;
				// 05/17/2013   We need to be able to detect draggable. 
				Session["SupportsDraggable"  ] = true;
				if ( Context.Request.Browser.Browser == "IE" )
				{
					float fVersion = 0;
					if ( float.TryParse(Context.Request.Browser.Version, out fVersion) )
					{
						if ( fVersion < 9.0 )
							Session["SupportsDraggable"  ] = false;
					}
				}
				// 07/11/2011   UserAgent might be null, and it was causing bad XML data in the language packs. 
				string sUserAgent = Sql.ToString(Context.Request.UserAgent);
				if ( sUserAgent.Contains("Android") )
				{
					// 07/28/2012   Android Tablet should not be treated as a mobile device. 
					// http://stackoverflow.com/questions/5341637/how-do-detect-android-tablets-in-general-useragent
					if ( sUserAgent.Contains("Mobile") )
					{
						Session["Browser"          ] = "Android Mobile";
						Session["IsMobileDevice"   ] = true;
					}
					else
					{
						Session["Browser"          ] = "Android Tablet";
						Session["IsMobileDevice"   ] = false;
					}
					Session["SupportsPopups"     ] = true;  // 11/24/2010   Confirmed support for popups. 
					Session["AllowAutoComplete"  ] = true;  // 11/24/2010   Confirmed support for auto-complete. 
					Session["SupportsSpeech"     ] = true;  // 08/22/2012   Android devices should support speech. 
					Session["SupportsHandwriting"] = true;  // 08/22/2012   Android devices should support handwriting. 
					Session["SupportsTouch"      ] = true;  // 11/14/2012   Assume all android devices support touch. 
				}
				else if ( sUserAgent.Contains("BlackBerry") )
				{
					Session["Browser"          ] = "BlackBerry";
					Session["IsMobileDevice"   ] = true;
					Session["SupportsPopups"   ] = false;
					Session["AllowAutoComplete"] = false;
					if ( !Sql.IsEmptyString(Context.Request.Browser["supportsPopups"]) )
						Session["SupportsPopups"] = Sql.ToBoolean(Context.Request.Browser["supportsPopups"]);
					if ( !Sql.IsEmptyString(Context.Request.Browser["AjaxAutoComplete"]) )
						Session["AllowAutoComplete"] = Sql.ToBoolean(Context.Request.Browser["AjaxAutoComplete"]);
				}
				else if ( sUserAgent.Contains("IEMobile") )
				{
					Session["Browser"          ] = "IEMobile";
					Session["IsMobileDevice"   ] = true;
					Session["SupportsPopups"   ] = false;
					Session["AllowAutoComplete"] = false;
				}
				// 07/28/2012   iPad should not be treated as a mobile device. 
				// http://www.labnol.org/tech/ipad-user-agent-string/13230/
				else if ( sUserAgent.Contains("iPad") )
				{
					Session["Browser"            ] = "iPad";
					Session["IsMobileDevice"     ] = false;
					Session["SupportsPopups"     ] = true;
					Session["AllowAutoComplete"  ] = true;
					Session["SupportsSpeech"     ] = true;  // 08/22/2012   Apple devices should support speech. 
					Session["SupportsHandwriting"] = true;  // 08/22/2012   Apple devices should support handwriting. 
					Session["SupportsTouch"      ] = true;  // 11/14/2012   All iPads support touch. 
				}
				else if ( sUserAgent.Contains("iPhone") )
				{
					Session["Browser"            ] = "iPhone";
					Session["IsMobileDevice"     ] = true;
					Session["SupportsPopups"     ] = true;  // 11/24/2010   Confirmed support for popups. 
					Session["AllowAutoComplete"  ] = true;  // 11/24/2010   Confirmed support for auto-complete. 
					Session["SupportsSpeech"     ] = true;  // 08/22/2012   Apple devices should support speech. 
					Session["SupportsHandwriting"] = true;  // 08/22/2012   Apple devices should support handwriting. 
					Session["SupportsTouch"      ] = true;  // 11/14/2012   All iPhones support touch. 
				}
				else if ( sUserAgent.Contains("iPod") )
				{
					Session["Browser"            ] = "iPod";
					Session["IsMobileDevice"     ] = true;
					Session["SupportsPopups"     ] = true;
					Session["AllowAutoComplete"  ] = true;
					Session["SupportsSpeech"     ] = true;  // 08/22/2012   Apple devices should support speech. 
					Session["SupportsHandwriting"] = true;  // 08/22/2012   Apple devices should support handwriting. 
					Session["SupportsTouch"      ] = true;  // 11/14/2012   All iPods support touch. 
				}
				else if ( sUserAgent.Contains("Opera Mini") )
				{
					Session["Browser"          ] = "Opera Mini";
					Session["IsMobileDevice"   ] = true;
					Session["SupportsPopups"   ] = false;  // 11/24/2010   Cannot confirm support. 
					Session["AllowAutoComplete"] = false;  // 11/24/2010   Cannot confirm support. 
					if ( !Sql.IsEmptyString(Context.Request.Browser["supportsPopups"]) )
						Session["SupportsPopups"] = Sql.ToBoolean(Context.Request.Browser["supportsPopups"]);
					if ( !Sql.IsEmptyString(Context.Request.Browser["AjaxAutoComplete"]) )
						Session["AllowAutoComplete"] = Sql.ToBoolean(Context.Request.Browser["AjaxAutoComplete"]);
				}
				else if ( sUserAgent.Contains("Palm") )
				{
					Session["Browser"          ] = "Palm";
					Session["IsMobileDevice"   ] = true;
					Session["SupportsPopups"   ] = false;  // 11/24/2010   Cannot confirm support. 
					Session["AllowAutoComplete"] = false;  // 11/24/2010   Cannot confirm support. 
					if ( !Sql.IsEmptyString(Context.Request.Browser["supportsPopups"]) )
						Session["SupportsPopups"] = Sql.ToBoolean(Context.Request.Browser["supportsPopups"]);
					if ( !Sql.IsEmptyString(Context.Request.Browser["AjaxAutoComplete"]) )
						Session["AllowAutoComplete"] = Sql.ToBoolean(Context.Request.Browser["AjaxAutoComplete"]);
				}
				else if ( sUserAgent.Contains("Chrome") )
				{
					// 08/31/2012   Lets just assume that all Chrome browsers now support speech and handwriting. 
					Session["SupportsSpeech"     ] = true;
					Session["SupportsHandwriting"] = true;
				}
				else if ( sUserAgent.Contains("Touch") )
				{
					Session["SupportsTouch"      ] = true;  // 11/14/2012   Microsoft Surface has Touch in the agent string. 
				}
				// 11/17/2007   If running on a mobile device, then use the mobile theme. 
				if ( Utils.IsMobileDevice )
				{
					if ( Directory.Exists(Context.Server.MapPath("~/App_MasterPages/" + SplendidDefaults.MobileTheme())) )
					{
						sTheme = SplendidDefaults.MobileTheme();
					}
				}
				Session["USER_SETTINGS/THEME"  ] = sTheme;
				Session["USER_SETTINGS/CULTURE"] = SplendidDefaults.Culture();
				// 03/07/2007   Version 1.4 moved its themes folder to the .NET 2.0 default App_Themes. 
				Session["themeURL"             ] = Sql.ToString(Context.Application["rootURL"]) + "App_Themes/" + sTheme + "/";
				// 11/19/2005   AUTH_USER is the clear indication that NTLM is enabled. 
				if ( Security.IsWindowsAuthentication() )
				{
					string[] arrUserName = Context.User.Identity.Name.Split('\\');
					// 11/09/2007   The domain will not be provided when debugging anonymous. 
					string sUSER_DOMAIN = String.Empty;
					string sUSER_NAME   = String.Empty;
					string sMACHINE     = String.Empty;
					try
					{
						// 09/17/2009   Azure does not support MachineName.  Just ignore the error. 
						sMACHINE = System.Environment.MachineName;
					}
					catch
					{
					}
					if ( arrUserName.Length > 1 )
					{
						sUSER_DOMAIN = arrUserName[0];
						sUSER_NAME   = arrUserName[1];
					}
					else
					{
						// 12/15/2007   Use environment variable as it is always available, where as the server object is not. 
						sUSER_DOMAIN = sMACHINE;
						sUSER_NAME   = arrUserName[0];
					}
					// 09/17/2009   The machine or domain name may be empty, so protect against their use. 
					bool bIS_ADMIN = Context.User.IsInRole("BUILTIN\\Administrators") 
					              || (!Sql.IsEmptyString(sUSER_DOMAIN) && Context.User.IsInRole(sUSER_DOMAIN + "\\Taoqi Administrators"))
					              || (!Sql.IsEmptyString(sMACHINE    ) && Context.User.IsInRole(sMACHINE     + "\\Taoqi Administrators"))
					              || (!Sql.IsEmptyString(sUSER_DOMAIN) && Context.User.IsInRole(sUSER_DOMAIN + "\\Domain Admins"));
				
					LoginUser(sUSER_NAME, String.Empty, String.Empty, String.Empty, sUSER_DOMAIN, bIS_ADMIN);
					// 09/22/2010   Check for redirect to allow for GenerateDemo.aspx
					string sRedirect = Sql.ToString(Context.Request["Redirect"]);
					// 09/22/2010   Only allow virtual relative paths. 
					if ( sRedirect.StartsWith("~/") )
						Context.Response.Redirect(sRedirect);
					// 07/07/2010   Redirect to the AdminWizard. 
					// 07/08/2010   Don't run the AdminWizard on the Offline Client. 
					// 02/14/2011   Don't run the wizard when being called from a web service. 
					else if ( bIS_ADMIN && Sql.IsEmptyString(Context.Application["CONFIG.Configurator.LastRun"]) && !Utils.IsOfflineClient && !Context.Request.Path.EndsWith(".asmx") )
						Context.Response.Redirect("~/Administration/Configurator/");
					// 10/06/2007   Prompt the user for the timezone. 
					// 07/08/2010   Redirect to the new User Wizard. 
					// 07/09/2010   The user cannot be modified on the Offline Client. 
					// 02/14/2011   Don't run the wizard when being called from a web service. 
					//else if ( Sql.IsEmptyString(Session["USER_SETTINGS/TIMEZONE/ORIGINAL"]) && !Utils.IsOfflineClient && !Context.Request.Path.EndsWith(".asmx") )
					//	Context.Response.Redirect("~/Users/Wizard.aspx");  //Context.Response.Redirect("~/Users/SetTimezone.aspx");
				}
				else
				{
					// 11/22/2005   Assume portal user for the unauthenticated screen as that is the least restrictive. 
					Security.PORTAL_ONLY = true;
					LoadUserPreferences(Guid.Empty, String.Empty, String.Empty);
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), ex);
				// 07/11/2011   Do not write the error as it is causing problems with the language pack exports. 
				//Context.Response.Write(ex.Message);
			}
		}

		public static void Application_OnError(HttpContext Context)
		{
			try
			{
				HttpApplicationState Application = Context.Application;
				HttpServerUtility Server = Context.Server;
				Exception ex = Server.GetLastError();
				if ( ex != null )
				{
					while ( ex.InnerException != null )
						ex = ex.InnerException;
					string sException = ex.GetType().Name;
					StringBuilder sbMessage = new StringBuilder();
					sbMessage.Append(ex.Message);
					// 03/10/2006   .NET 2.0 returns lowercase type names. Use typeof instead. 
					if ( ex.GetType() == typeof(FileNotFoundException) )
					{
						// We can get this error for forbidden files such as web.config and global.asa. 
						//return ; // Return would work if 404 entry was made in web.config. 
						//Response.Redirect("~/Home/FileNotFound.aspx?aspxerrorpath=" + Server.UrlEncode(Request.Path));
						sbMessage = new StringBuilder("File Not Found");
					}
					// 03/10/2006   .NET 2.0 returns lowercase type names. Use typeof instead. 
					else if ( ex.GetType() == typeof(HttpException) )
					{
						HttpException exHttp = (HttpException) ex;
						int nHttpCode = exHttp.GetHttpCode();
						if ( nHttpCode == 403 )
						{
							//return ; // Return would work if 403 entry was made in web.config. 
							//Response.Redirect("~/Home/AccessDenied.aspx?aspxerrorpath=" + Server.UrlEncode(Request.Path));
							sbMessage = new StringBuilder("Access Denied");
						}
						else if ( nHttpCode == 404 )
						{
							//return ; // Return would work if 404 entry was made in web.config. 
							//Response.Redirect("~/Home/FileNotFound.aspx?aspxerrorpath=" + Server.UrlEncode(Request.Path));
							sbMessage = new StringBuilder("File Not Found");
						}
					}
					// 03/10/2006   .NET 2.0 returns lowercase type names. Use typeof instead. 
					else if ( ex.GetType() == typeof(HttpCompileException) )
					{
						HttpCompileException exCompile = (HttpCompileException) ex;
						CompilerErrorCollection col = exCompile.Results.Errors;
						foreach(CompilerError err in col)
						{
							sbMessage.Append("  ");
							sbMessage.Append(err.ErrorText);
						}
					}
					SplendidError.SystemMessage(Context, "Error", new StackTrace(true).GetFrame(0), sbMessage.ToString());
					Server.ClearError();
					string sQueryString = String.Format("aspxerrorpath={0}&Exception={1}&Message={2}", Server.UrlEncode(Context.Request.Path), sException, Server.UrlEncode(sbMessage.ToString()));
					Context.Response.Redirect("~/Home/ServerError.aspx?" + sQueryString);
				}
			}
			catch
			{
			}
		}
	}
}


