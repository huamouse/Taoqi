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
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Taoqi._controls
{
	/// <summary>
	///		Summary description for HeaderLeft.
	/// </summary>
	public class HeaderLeft : SplendidControl
	{
		protected Taoqi.Themes.Sugar.HeaderLeft ctlHeaderLeft;
		protected Panel  pnlHeader;
		protected string sTitle;
		protected Unit   uWidth;

		public string Title
		{
			get
			{
				if ( ctlHeaderLeft != null )
					return ctlHeaderLeft.Title;
				else
					return sTitle;
			}
			set
			{
				sTitle = value;
				if ( ctlHeaderLeft != null )
					ctlHeaderLeft.Title = value;
			}
		}

		public Unit Width
		{
			get
			{
				if ( ctlHeaderLeft != null )
					return ctlHeaderLeft.Width;
				else
					return uWidth;
			}
			set
			{
				uWidth = value;
				if ( ctlHeaderLeft != null )
					ctlHeaderLeft.Width = value;
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
			string sTheme = Page.Theme;
			if ( String.IsNullOrEmpty(sTheme) )
				sTheme = "Sugar";
			string sHeaderLeftPath = "~/App_MasterPages/" + Page.Theme + "/HeaderLeft.ascx";
			// 08/25/2013   File IO is slow, so cache existance test. 
			if ( Utils.CachedFileExists(Context, sHeaderLeftPath) )
			{
				ctlHeaderLeft = LoadControl(sHeaderLeftPath) as Taoqi.Themes.Sugar.HeaderLeft;
				if ( ctlHeaderLeft != null )
				{
					ctlHeaderLeft.Title = sTitle;
					if ( !uWidth.IsEmpty )
						ctlHeaderLeft.Width = uWidth;
					pnlHeader.Controls.Add(ctlHeaderLeft);
				}
			}
		}
		#endregion
	}
}


