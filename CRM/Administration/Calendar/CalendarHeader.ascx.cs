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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Calendar
{
	/// <summary>
	///		Summary description for CalendarHeader.
	/// </summary>
	public class CalendarHeader : SplendidControl
	{
		public    CommandEventHandler Command ;
		
		protected Button btnDay    ;
		protected Button btnWeek   ;
		protected Button btnMonth  ;
		protected Button btnYear   ;
		protected Button btnShared ;
		protected string sActiveTab;

		public string ActiveTab
		{
			get
			{
				return sActiveTab;
			}
			set
			{
				sActiveTab = value;
			}
		}

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			if ( Command != null )
				Command(this, e) ;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 01/16/2007   If calls are not visible on only visible to owners, then hide the Shared button. 
			btnShared.Visible = (Taoqi.Security.GetUserAccess("Calls", "list") >= ACL_ACCESS.OWNER);
			switch(sActiveTab)
			{
				case "Day"   :  btnDay   .CssClass = "buttonOn" ;  break;
				case "Week"  :  btnWeek  .CssClass = "buttonOn" ;  break;
				case "Month" :  btnMonth .CssClass = "buttonOn" ;  break;
				case "Year"  :  btnYear  .CssClass = "buttonOn" ;  break;
				case "Shared":  btnShared.CssClass = "buttonOn" ;  break;
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


