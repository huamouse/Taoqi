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
using System.Threading;

namespace Taoqi._controls
{
	/// <summary>
	///		Summary description for DatePicker.
	/// </summary>
	public class DatePicker : SplendidControl
	{
		protected TextBox  txtDATE      ;
		protected Label    lblDateFormat;
		protected System.Web.UI.WebControls.Image    imgCalendar  ;
		protected RequiredFieldValidator     reqDATE;
		// 08/31/2006   We cannot use a regular expression validator because there are just too many date formats.
		protected DateValidator              valDATE;

		public DateTime Value
		{
			get
			{
				// 07/09/2006   Dates are no longer converted inside this control. 
				return Sql.ToDateTime(txtDATE.Text);
			}
			set
			{
				txtDATE.Text = Sql.ToDateString(value);
			}
		}

		public string DateText
		{
			get
			{
				return txtDATE.Text;
			}
			set
			{
				txtDATE.Text = value;
			}
		}

		public string DateClientID
		{
			get
			{
				return txtDATE.ClientID;
			}
		}

		public short TabIndex
		{
			get
			{
				return txtDATE.TabIndex;
			}
			set
			{
				txtDATE.TabIndex = value;
			}
		}

		public bool EnableDateFormat
		{
			get
			{
				return lblDateFormat.Visible;
			}
			set
			{
				lblDateFormat.Visible = value;
			}
		}

		public bool Enabled
		{
			set
			{
				txtDATE.Enabled = value;
				imgCalendar.Visible = value;
			}
		}

		// 04/05/2006   Need a way to clear the date. 
		public void Clear()
		{
			txtDATE.Text = String.Empty;
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
			// 11/07/2005   Not sure why rglDATE is not available. 
			//rglDATE.Enabled = true;
			// 04/15/2006   The error message is not binding properly.  Just assign here as a quick solution. 
			// 06/09/2006   Now that we have solved the data binding issues, we can let the binding fill the message. 
			//rglDATE.ErrorMessage = L10n.Term(".ERR_INVALID_DATE");
			// 08/31/2006   Enable and perform date validation. 
			reqDATE.Validate();
			valDATE.Validate();
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 06/09/2006   Always set the message as this control does not remember its state. 
			reqDATE.ErrorMessage = L10n.Term(".ERR_REQUIRED_FIELD");
			// 08/31/2006   Need to bind the text. 
			valDATE.ErrorMessage = L10n.Term(".ERR_INVALID_DATE");

			// 11/26/2008   In order for javascript to render in an UpdatePanel, it must be registered with the ScriptManager. 
			string sChangeJS = "<script type=\"text/javascript\">\nfunction ChangeDate" + txtDATE.ClientID.Replace(":", "_") + "(sDATE)\n{\n\tdocument.getElementById('" + txtDATE.ClientID + "').value = sDATE;\n}\n</script>\n";
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
				// 12/12/2009   The calendar popup will not work on a Blackberry. 
				bool bSupportsPopups = true;
				if ( this.IsMobile )
				{
					// 11/24/2010   .NET 4 has broken the compatibility of the browser file system. 
					// We are going to minimize our reliance on browser files in order to reduce deployment issues. 
					bSupportsPopups = Utils.SupportsPopups;
				}
				imgCalendar.Visible = bSupportsPopups;
				// 06/29/2006   The image needs to be manually bound in Administration/ProductTemplates/EditView.ascx
				imgCalendar.DataBind();
				// 07/05/2006   Need to bind the label manually. 
				// 07/06/2005   lblDateFormat is not defined in ChartDatePicker, so we must test if lblDateFormat exists. 
				if ( lblDateFormat != null )
					lblDateFormat.DataBind();
				//this.DataBind();
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


