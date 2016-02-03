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
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using Taoqi._controls;

namespace Taoqi.Calendar
{
	/// <summary>
	///		Summary description for NewRecord.
	/// </summary>
	public class NewRecord : SplendidControl
	{
		protected Label                      lblError          ;
		protected RadioButton                radScheduleCall   ;
		protected RadioButton                radScheduleMeeting;
		protected TextBox                    txtNAME           ;
		protected Label                      lblDATEFORMAT     ;
		protected Label                      lblTIMEFORMAT     ;
		protected DatePicker                 ctlDATE_START     ;
		protected TextBox                    txtTIME_START     ;
		protected RequiredFieldValidator     reqNAME           ;
		protected RequiredFieldValidator     reqTIME_START     ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "NewRecord" )
			{
				reqNAME      .Enabled = true;
				reqTIME_START.Enabled = true;
				reqNAME      .Validate();
				reqTIME_START.Validate();
				if ( Page.IsValid )
				{
					Guid gID = Guid.Empty;
					try
					{
						// 02/28/2006   The easiest way to parse the two separate date/time fields is to combine the text. 
						DateTime dtDATE_START = T10n.ToServerTime(Sql.ToDateTime(ctlDATE_START.DateText + " " + txtTIME_START.Text));
						// 01/16/2012   Assigned User ID and Team ID are now parameters. 
						if ( radScheduleCall.Checked )
							SqlProcs.spCALLS_New(ref gID, txtNAME.Text, dtDATE_START, Security.USER_ID, Security.TEAM_ID, String.Empty);
						else
							SqlProcs.spMEETINGS_New(ref gID, txtNAME.Text, dtDATE_START, Security.USER_ID, Security.TEAM_ID, String.Empty);
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
						lblError.Text = ex.Message;
					}
					if ( !Sql.IsEmptyGuid(gID) )
					{
						if ( radScheduleCall.Checked )
							Response.Redirect("~/Calls/view.aspx?ID=" + gID.ToString());
						else
							Response.Redirect("~/Meetings/view.aspx?ID=" + gID.ToString());
					}
				}
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			radScheduleCall   .Visible = (Taoqi.Security.GetUserAccess("Calls"   , "edit") >= 0);
			radScheduleMeeting.Visible = (Taoqi.Security.GetUserAccess("Meetings", "edit") >= 0);

			// 06/04/2006   NewRecord should not be displayed if the user does not have edit rights. 
			this.Visible = radScheduleCall.Visible || radScheduleMeeting.Visible;
			if ( !this.Visible )
				return;

			// 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
			//this.DataBind();  // Need to bind so that Text of the Button gets updated. 
			reqNAME      .ErrorMessage = L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Calls.LBL_LIST_SUBJECT") + "<br>";
			reqTIME_START.ErrorMessage = L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Calls.LBL_LIST_TIME"   ) + "<br>";
			// 05/06/2010   Use a special Page flag to override the default IsPostBack behavior. 
			bool bIsPostBack = this.IsPostBack && !NotPostBack;
			if ( !bIsPostBack )
			{
				// 05/06/2010   When the control is created out-of-band, we need to manually bind the controls. 
				if ( NotPostBack )
					this.DataBind();
				DateTime dt1100PM = DateTime.Today.AddHours(23);
				lblDATEFORMAT.Text = "(" + Session["USER_SETTINGS/DATEFORMAT"] + ")";
				lblTIMEFORMAT.Text = "(" + dt1100PM.ToShortTimeString() + ")";

				DateTime dtNow = T10n.FromServerTime(DateTime.Now);
				ctlDATE_START.Value = dtNow;
				txtTIME_START.Text  = Sql.ToTimeString(dtNow);
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
		}
		#endregion
	}
}


