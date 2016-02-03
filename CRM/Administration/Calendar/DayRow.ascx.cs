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
using System.Globalization;

namespace Taoqi.Calendar
{
	/// <summary>
	///		Summary description for DayRow.
	/// </summary>
	public class DayRow : SplendidControl
	{
		protected DateTime      dtDATE_START = DateTime.MinValue;
		//protected DateTime      dtDATE_END   = DateTime.MaxValue;
		protected DataView      vwMain             ;
		protected DataList      lstMain            ;
		protected Label         lblError           ;

		protected RadioButton   radScheduleCall    ;
		protected RadioButton   radScheduleMeeting ;
		protected TextBox       txtNAME            ;
		// 06/14/2006   The Italian problem was that it was using the culture separator, but DataView only supports the en-US format. 
		protected Button        btnSave            ;
		protected CultureInfo   ciEnglish          ;

		public CommandEventHandler Command;

		public DateTime DATE_START
		{
			get
			{
				return dtDATE_START;
			}
			set
			{
				dtDATE_START = value;
				// 06/15/2006   Instead of binding the command argument at render time, we must bind 
				// here to ensure that ciEnglish has been created.  This is normally not a problem, but it is with DayRow
				// because we are manually loading the control during the rendering of DayGrid. 
				if ( ciEnglish == null )
					ciEnglish = CultureInfo.CreateSpecificCulture("en-US");
				btnSave.CommandArgument = dtDATE_START.ToString(CalendarControl.SqlDateTimeFormat);
			}
		}
		/*
		public DateTime DATE_END
		{
			get
			{
				return dtDATE_END;
			}
			set
			{
				dtDATE_END = value;
			}
		}
		*/
		public DataView DataSource
		{
			set
			{
				vwMain = value;
			}
		}

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "Save" )
			{
				if ( !Sql.IsEmptyString(txtNAME.Text) && Information.IsDate(e.CommandArgument) )
				{
					// 06/09/2006   Add code to create call or meeting. This code did not make the 1.0 release. 
					dtDATE_START = Sql.ToDateTime(e.CommandArgument);
					if ( radScheduleCall.Checked )
					{
						Guid gID = Guid.Empty;
						// 01/16/2012   Assigned User ID and Team ID are now parameters. 
						//SqlProcs.spCALLS_New(ref gID, txtNAME.Text, T10n.ToServerTime(dtDATE_START), Security.USER_ID, Security.TEAM_ID, String.Empty);
						// 01/27/2011   In order to honor team management, we need to use the base procedure. 
						// 12/26/2012   Add EMAIL_REMINDER_TIME. 
						// 03/07/2013   Add ALL_DAY_EVENT. 
						// 03/20/2013   Add REPEAT fiels. 
						// 12/23/2013   Add SMS_REMINDER_TIME. 
						SqlProcs.spCALLS_Update
							( ref gID
							, Security.USER_ID
							, txtNAME.Text
							, 1
							, 0
							, T10n.ToServerTime(dtDATE_START)
							, String.Empty
							, Guid.Empty
							, "Planned"
							, "Outbound"
							, -1
							, String.Empty
							, String.Empty
							, Security.TEAM_ID
							, String.Empty
							, -1                 // EMAIL_REMINDER_TIME
							, false              // ALL_DAY_EVENT
							, String.Empty       // REPEAT_TYPE
							, 0                  // REPEAT_INTERVAL
							, String.Empty       // REPEAT_DOW
							, DateTime.MinValue  // REPEAT_UNTIL
							, 0                  // REPEAT_COUNT
							, -1                 // SMS_REMINDER_TIME
							);
					}
					else if ( radScheduleMeeting.Checked )
					{
						Guid gID = Guid.Empty;
						// 01/16/2012   Assigned User ID and Team ID are now parameters. 
						//SqlProcs.spMEETINGS_New(ref gID, txtNAME.Text, T10n.ToServerTime(dtDATE_START), Security.USER_ID, Security.TEAM_ID, String.Empty);
						// 01/27/2011   In order to honor team management, we need to use the base procedure. 
						// 12/26/2012   Add EMAIL_REMINDER_TIME. 
						// 03/07/2013   Add ALL_DAY_EVENT. 
						// 03/20/2013   Add REPEAT fields. 
						// 12/23/2013   Add SMS_REMINDER_TIME. 
						SqlProcs.spMEETINGS_Update
							( ref gID
							, Security.USER_ID
							, txtNAME.Text
							, String.Empty
							, 1
							, 0
							, T10n.ToServerTime(dtDATE_START)
							, "Planned"
							, String.Empty
							, Guid.Empty
							, -1
							, String.Empty
							, String.Empty
							, Security.TEAM_ID
							, String.Empty
							, -1                 // EMAIL_REMINDER_TIME
							, false              // ALL_DAY_EVENT
							, String.Empty       // REPEAT_TYPE
							, 0                  // REPEAT_INTERVAL
							, String.Empty       // REPEAT_DOW
							, DateTime.MinValue  // REPEAT_UNTIL
							, 0                  // REPEAT_COUNT
							, -1                 // SMS_REMINDER_TIME
							);
					}
				}
			}
			if ( Command != null )
				Command(this, e);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			/*
			// 09/27/2005   Instead of performing a query for each cell, just do one query for the entire range and filter before each cell. 
			try
			{
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					string sSQL;
					sSQL = "select *                                                       " + ControlChars.CrLf
					     + "  from vwACTIVITIES_List                                       " + ControlChars.CrLf;
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sSQL;
						// 11/27/2006   Make sure to filter relationship data based on team access rights. 
						Security.Filter(cmd, "Calls", "list");
						// 01/16/2007   Use AppendParameter so that duplicate ASSIGNED_USER_ID can be avoided. 
						// 01/19/2007   Fix AppendParamenter.  @ should not be used in field name. 
						Sql.AppendParameter(cmd, Security.USER_ID, "ASSIGNED_USER_ID");
						cmd.CommandText += "   and (   DATE_START >= @DATE_START and DATE_START < @DATE_END" + ControlChars.CrLf;
						cmd.CommandText += "        or DATE_END   >= @DATE_START and DATE_END   < @DATE_END" + ControlChars.CrLf;
						cmd.CommandText += "        or DATE_START <  @DATE_START and DATE_END   > @DATE_END" + ControlChars.CrLf;
						cmd.CommandText += "       )                                                       " + ControlChars.CrLf;
						cmd.CommandText += " order by DATE_START asc, NAME asc                             " + ControlChars.CrLf;
						// 03/19/2007   Need to query activities based on server time. 
						Sql.AddParameter(cmd, "@DATE_START", T10n.ToServerTime(dtDATE_START));
						Sql.AddParameter(cmd, "@DATE_END"  , T10n.ToServerTime(dtDATE_END  ));

						if ( bDebug )
							RegisterClientScriptBlock("vwACTIVITIES_List" + dtDATE_START.ToOADate().ToString(), Sql.ClientScriptBlock(cmd));

						try
						{
							using ( DbDataAdapter da = dbf.CreateDataAdapter() )
							{
								((IDbDataAdapter)da).SelectCommand = cmd;
								using ( DataTable dt = new DataTable() )
								{
									da.Fill(dt);
									vwMain = dt.DefaultView;
									lstMain.DataSource = vwMain ;
									lstMain.DataBind();
								}
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
							lblError.Text = ex.Message;
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				lblError.Text = ex.Message;
			}
			*/
			//lstMain.DataSource = vwMain ;
			//lstMain.DataBind();
			// 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
			//Page.DataBind();
		}

		private void Page_DataBind(object sender, System.EventArgs e)
		{
			// 03/19/2007   We were having a problem with the calendar data appearing during print view.  We needed to rebind the data. 
			lstMain.DataSource = vwMain ;
			lstMain.DataBind();
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
			this.DataBinding += new System.EventHandler(this.Page_DataBind);
		}
		#endregion
	}
}


