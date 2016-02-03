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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace Taoqi._controls
{
	/// <summary>
	///		Summary description for DateTimePicker.
	/// </summary>
	public class DateTimePicker : SplendidControl
	{
		protected int          nMinutesIncrement = 15;
		private   DateTime     dtValue  = DateTime.MinValue;
		protected TextBox      txtDATE      ;
		protected DropDownList lstHOUR      ;
		protected DropDownList lstMINUTE    ;
		protected DropDownList lstMERIDIEM  ;
		protected Label        lblDATEFORMAT;
		protected Label        lblTIMEFORMAT;
		protected System.Web.UI.WebControls.Image    imgCalendar  ;
		protected RequiredFieldValidator     reqDATE;
		// 08/31/2006   We cannot use a regular expression validator because there are just too many date formats.
		protected DateValidator              valDATE;

		public System.EventHandler Changed ;

		protected void Date_Changed(object sender, System.EventArgs e)
		{
			if ( Changed != null )
				Changed(this, e) ;
		}

		public int MinutesIncrement
		{
			get
			{
				return nMinutesIncrement;
			}
			set
			{
				nMinutesIncrement = value;
				if ( nMinutesIncrement <= 0 )
					nMinutesIncrement = 1;
			}
		}

		public bool AutoPostBack
		{
			get
			{
				return lstHOUR.AutoPostBack;
			}
			set
			{
				txtDATE    .AutoPostBack = value;
				lstHOUR    .AutoPostBack = value;
				lstMINUTE  .AutoPostBack = value;
				lstMERIDIEM.AutoPostBack = value;
			}
		}

		// 12/26/2008   The merge facility needs access to the client id. 
		public string DateClientID
		{
			get
			{
				return txtDATE.ClientID;
			}
		}

		public DateTime Value
		{
			get
			{
				dtValue = Sql.ToDateTime(txtDATE.Text);
				// 01/26/2008   SQL Server does not accept dates below 1753, but we will ignore anything below 1900. 
				if ( dtValue.Year >= 1900 )
				{
					bool b12Hour = lstMERIDIEM.Visible;
					if ( b12Hour )
					{
						if ( lstMERIDIEM.SelectedValue == "PM" )
						{
							if ( lstHOUR.SelectedValue == "12" )
								dtValue = dtValue.AddHours(12);
							else
								dtValue = dtValue.AddHours(12 + Sql.ToInteger(lstHOUR.SelectedValue));
							dtValue = dtValue.AddMinutes(Sql.ToInteger(lstMINUTE.SelectedValue));
						}
						else
						{
							if ( lstHOUR.SelectedValue != "12" )
								dtValue = dtValue.AddHours(Sql.ToInteger(lstHOUR.SelectedValue));
							dtValue = dtValue.AddMinutes(Sql.ToInteger(lstMINUTE.SelectedValue));
						}
					}
					else
					{
						dtValue = dtValue.AddHours  (Sql.ToInteger(lstHOUR  .SelectedValue));
						dtValue = dtValue.AddMinutes(Sql.ToInteger(lstMINUTE.SelectedValue));
					}
				}
				return dtValue;
			}
			set
			{
				dtValue = value;
				SetDate();
			}
		}

		public short TabIndex
		{
			get
			{
				return lstHOUR.TabIndex;
			}
			set
			{
				lstHOUR    .TabIndex = value;
				lstMINUTE  .TabIndex = value;
				lstMERIDIEM.TabIndex = value;
			}
		}

		public bool Enabled
		{
			set
			{
				txtDATE    .Enabled = value;
				imgCalendar.Visible = value;
				lstHOUR    .Enabled = value;
				lstMINUTE  .Enabled = value;
				lstMERIDIEM.Enabled = value;
			}
		}

		private void SetDate()
		{
			// 03/10/2006   Make sure to only populate the list once. 
			// We populate inside SetDate because we need the list to have values before the value can be set. 
			if ( lstMINUTE.Items.Count == 0 )
			{
				for ( int nMinute = 0 ; nMinute < 60 ; nMinute += nMinutesIncrement )
				{
					lstMINUTE.Items.Add(new ListItem(nMinute.ToString("00"), nMinute.ToString("00")));
				}
			}
			string sTimeFormat = Sql.ToString(Session["USER_SETTINGS/TIMEFORMAT"]);
			bool b12Hour = (sTimeFormat.IndexOf("tt") >= 0);
			// 03/10/2006   Make sure to only populate the list once. 
			// We populate inside SetDate because we need the list to have values before the value can be set. 
			if ( lstHOUR.Items.Count == 0 )
			{
				if ( b12Hour )
				{
					for ( int nHour = 1 ; nHour <= 12 ; nHour++ )
					{
						// 01/26/2008   Make sure that 12 is first. 
						if ( nHour == 12 )
							lstHOUR.Items.Insert(0, new ListItem(nHour.ToString("00"), nHour.ToString("00")));
						else
							lstHOUR.Items.Add(new ListItem(nHour.ToString("00"), nHour.ToString("00")));
					}
					lstMERIDIEM.Visible = true;
				}
				else
				{
					for ( int nHour = 0 ; nHour < 24 ; nHour++ )
					{
						lstHOUR.Items.Add(new ListItem(nHour.ToString("00"), nHour.ToString("00")));
					}
					lstMERIDIEM.Visible = false;
				}
			}
			if ( dtValue > DateTime.MinValue )
			{
				txtDATE.Text = Sql.ToDateString(dtValue);
				try
				{
					int nMinutes = dtValue.Minute;
					if ( nMinutesIncrement == 1 )
					{
						lstMINUTE.SelectedValue = nMinutes.ToString("00");
					}
					else
					{
						for ( int nMinute = 0 ; nMinute < 60 ; nMinute += nMinutesIncrement )
						{
							if ( nMinutes <= (nMinute + nMinutesIncrement / 2) )
							{
								lstMINUTE.SelectedValue = nMinute.ToString("00");
								break;
							}
						}
					}
					
					int nHours = dtValue.Hour;
					if ( b12Hour )
					{
						// 07/11/2006   The Meridiem dropdown needs to be populated before we set its value. 
						lstMERIDIEM_Bind();
						if ( nHours >= 12 )
						{
							nHours -= 12;
							lstMERIDIEM.SelectedValue = "PM";
						}
						else
						{
							lstMERIDIEM.SelectedValue = "AM";
						}
						if ( nHours == 0 )
							lstHOUR.SelectedValue = (12).ToString("00");
						else
							lstHOUR.SelectedValue = nHours.ToString("00");
					}
					else
					{
						lstHOUR.SelectedValue = nHours.ToString("00");
					}
				}
				catch(Exception ex)
				{
					SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex);
				}
			}
		}

		// 11/11/2010   Provide a way to disable validation in a rule. 
		public void Validate()
		{
			Validate(true);
		}

		public void Validate(bool bEnabled)
		{
			reqDATE.Enabled = bEnabled;
			valDATE.Enabled = bEnabled;
			// 04/15/2006   The error message is not binding properly.  Just assign here as a quick solution. 
			// 06/09/2006   Now that we have solved the data binding issues, we can let the binding fill the message. 
			//valDATE.ErrorMessage = L10n.Term(".ERR_INVALID_DATE");
			reqDATE.Validate();
			// 08/31/2006   Enable and perform date validation. 
			valDATE.Validate();
		}

		// 07/11/2006   The Meridiem dropdown may need to be populated before Page_Load. 
		private void lstMERIDIEM_Bind()
		{
			if ( lstMERIDIEM.Items.Count == 0 )
			{
				DateTimeFormatInfo oDateInfo = Thread.CurrentThread.CurrentCulture.DateTimeFormat;
				lstMERIDIEM.Items.Add(new ListItem(oDateInfo.AMDesignator, "AM"));
				lstMERIDIEM.Items.Add(new ListItem(oDateInfo.PMDesignator, "PM"));
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 06/09/2006   Always set the message as this control does not remember its state. 
			reqDATE.ErrorMessage = L10n.Term(".ERR_REQUIRED_FIELD");
			// 08/31/2006   Need to bind the text. 
			valDATE.ErrorMessage = L10n.Term(".ERR_INVALID_DATE");

			// 11/26/2008   In order for javascript to render in an UpdatePanel, it must be registered with the ScriptManager. 
			string sChangeJS = "<script type=\"text/javascript\">\nfunction ChangeDate" + txtDATE.ClientID.Replace(":", "_") + "(sDATE)\n{\n\tdocument.getElementById('" + txtDATE.ClientID + "').value = sDATE;\n" + (txtDATE.AutoPostBack ? "\tdocument.forms[0].submit();\n" : "") + "}\n</script>";
			ScriptManager mgrAjax = ScriptManager.GetCurrent(this.Page);
			if ( mgrAjax != null )
			{
				// 11/27/2008   The name of the script block must be unique for each instance of this control. 
				// 06/21/2009   Use RegisterStartupScript instead of RegisterClientScriptBlock so that the script will run after the control has been created. 
				ScriptManager.RegisterStartupScript(this, typeof(System.String), "AjaxChangeDate_" + txtDATE.ClientID.Replace(":", "_"), sChangeJS, false);
			}
			else
			{
				#pragma warning disable 618
				Page.ClientScript.RegisterStartupScript(typeof(System.String), "PageChangeDate_" + txtDATE.ClientID.Replace(":", "_"), sChangeJS);
				#pragma warning restore 618
			}
			// 05/06/2010   Use a special Page flag to override the default IsPostBack behavior. 
			bool bIsPostBack = this.IsPostBack && !NotPostBack;
			if ( !bIsPostBack )
			{
				DateTime dt1100PM = DateTime.Today.AddHours(23);
				lblDATEFORMAT.Text = "(" + Session["USER_SETTINGS/DATEFORMAT"] + ")";
				lblTIMEFORMAT.Text = "(" + dt1100PM.ToShortTimeString() + ")";
				
				lstMERIDIEM_Bind();
				SetDate();
				//this.DataBind();

				// 12/12/2009   The calendar popup will not work on a Blackberry. 
				bool bSupportsPopups = true;
				if ( this.IsMobile )
				{
					// 11/24/2010   .NET 4 has broken the compatibility of the browser file system. 
					// We are going to minimize our reliance on browser files in order to reduce deployment issues. 
					bSupportsPopups = Utils.SupportsPopups;
				}
				imgCalendar.Visible = bSupportsPopups;
				// 07/02/2006   The image needs to be manually bound in Contracts. 
				imgCalendar.DataBind();
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


