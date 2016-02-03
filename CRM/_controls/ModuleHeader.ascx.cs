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
	///		Summary description for ModuleHeader.
	/// </summary>
	public class ModuleHeader : SplendidControl
	{
		protected Taoqi.Themes.Sugar.ModuleHeader ctlModuleHeader;
		protected Panel     pnlHeader;
		protected string    sModule   = String.Empty;
		protected string    sTitle    = String.Empty;
		protected string    sHelpName = String.Empty;
		protected string    sTitleText= String.Empty;
		protected bool      bEnableModuleLabel = true;
		protected bool      bEnablePrint       = false;
		protected bool      bEnableHelp        = false;
		// 03/31/2012   Add support for favorites. 
		protected bool      bEnableFavorites   = false;
        //protected string sModuleDisplayName = String.Empty;

		public string Module
		{
			get
			{
				if ( ctlModuleHeader != null )
					return ctlModuleHeader.Module;
				else
					return sModule;
			}
			set
			{
				sModule = value;
				if ( ctlModuleHeader != null )
					ctlModuleHeader.Module = value;
			}
		}

		public string Title
		{
			get
			{
				if ( ctlModuleHeader != null )
					return ctlModuleHeader.Title;
				else
					return sTitle;
			}
			set
			{
				sTitle = value;
				if ( ctlModuleHeader != null )
					ctlModuleHeader.Title = value;
			}
		}

		public string HelpName
		{
			get
			{
				if ( ctlModuleHeader != null )
					return ctlModuleHeader.HelpName;
				else
					return sHelpName;
			}
			set
			{
				sHelpName = value;
				if ( ctlModuleHeader != null )
					ctlModuleHeader.HelpName = value;
			}
		}

		public string TitleText
		{
			get
			{
				if ( ctlModuleHeader != null )
					return ctlModuleHeader.TitleText;
				else
					return sTitleText;
			}
			set
			{
				sTitleText = value;
				if ( ctlModuleHeader != null )
					ctlModuleHeader.TitleText = value;
			}
		}

		public bool EnableModuleLabel
		{
			get
			{
				if ( ctlModuleHeader != null )
					return ctlModuleHeader.EnableModuleLabel;
				else
					return bEnableModuleLabel;
			}
			set
			{
				bEnableModuleLabel = value;
				if ( ctlModuleHeader != null )
					ctlModuleHeader.EnableModuleLabel = value;
			}
		}

		public bool EnablePrint
		{
			get
			{
				if ( ctlModuleHeader != null )
					return ctlModuleHeader.EnablePrint;
				else
					return bEnablePrint;
			}
			set
			{
				bEnablePrint = value;
				if ( ctlModuleHeader != null )
					ctlModuleHeader.EnablePrint = value;
			}
		}

		public bool EnableHelp
		{
			get
			{
				if ( ctlModuleHeader != null )
					return ctlModuleHeader.EnableHelp;
				else
					return bEnableHelp;
			}
			set
			{
				bEnableHelp = value;
				if ( ctlModuleHeader != null )
					ctlModuleHeader.EnableHelp = value;
			}


		}

        /*
        public string ModuleDisplayName
        {
            get
            {
                if (ctlModuleHeader != null)
                    return ctlModuleHeader.ModuleDisplayName;
                else
                    return sModuleDisplayName;
            }
            set
            {
                sModuleDisplayName = value;
                if (ctlModuleHeader != null)
                    ctlModuleHeader.ModuleDisplayName = value;
            }
        }
        */

		// 03/31/2012   Add support for favorites. 
		public bool EnableFavorites
		{
			get
			{
				if ( ctlModuleHeader != null )
					return ctlModuleHeader.EnableFavorites;
				else
					return bEnableFavorites;
			}
			set
			{
				bEnableFavorites = value;
				if ( ctlModuleHeader != null )
					ctlModuleHeader.EnableFavorites = value;
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
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
			string sTheme = Page.Theme;
			if ( String.IsNullOrEmpty(sTheme) )
				sTheme = "Sugar";
			string sModuleHeaderPath = "~/App_MasterPages/" + Page.Theme + "/ModuleHeader.ascx";
			// 08/25/2013   File IO is slow, so cache existance test. 
			if ( Utils.CachedFileExists(Context, sModuleHeaderPath) )
			{
				ctlModuleHeader = LoadControl(sModuleHeaderPath) as Taoqi.Themes.Sugar.ModuleHeader;
				if ( ctlModuleHeader != null )
				{
					ctlModuleHeader.Module            = sModule           ;
					ctlModuleHeader.Title             = sTitle            ;
					ctlModuleHeader.HelpName          = sHelpName         ;
					ctlModuleHeader.TitleText         = sTitleText        ;
					ctlModuleHeader.EnableModuleLabel = bEnableModuleLabel;
					ctlModuleHeader.EnablePrint       = bEnablePrint      ;
					ctlModuleHeader.EnableHelp        = bEnableHelp       ;
					ctlModuleHeader.EnableFavorites   = bEnableFavorites  ;
                    //ctlModuleHeader.ModuleDisplayName = sModuleDisplayName;
					pnlHeader.Controls.Add(ctlModuleHeader);
				}
			}
		}
		#endregion
	}
}


