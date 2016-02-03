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
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Taoqi._controls
{
	/// <summary>
	///		Summary description for CheckAll.
	/// </summary>
	public class CheckAll : SplendidControl
	{
		protected string      sFieldName      = "chkMain";
		protected bool        bShowSelectAll  = true;
		protected Label       lblSelectedLabel;
		protected HiddenField hidSelectedItems;
		protected HyperLink   lnkSelectPage   ;
		protected LinkButton  btnSelectAll    ;
		protected HyperLink   lnkDeselectAll  ;

		public CommandEventHandler Command ;

		public string FieldName
		{
			get { return sFieldName; }
			set { sFieldName = value; }
		}

		public bool ShowSelectAll
		{
			get { return bShowSelectAll; }
			set { bShowSelectAll = value; }
		}

		public HiddenField SelectedItems
		{
			get { return hidSelectedItems; }
		}

		// 09/18/2012   We need a quick way to determine if SelectAll was checked. 
		public bool SelectAllChecked
		{
			get { return btnSelectAll.UniqueID == Sql.ToString(Request["__EVENTTARGET"]); }
		}

		public string[] SelectedItemsArray
		{
			get
			{
				// 09/21/2013   Selected items is used by PayTrace as a collection of integer IDs. 
				// 10/24/2013   When not counting Guid lengths, make sure the string is not empty. 
				if ( hidSelectedItems.Value.Length >= 36 || (hidSelectedItems.Value.Length > 0 && sFieldName != "chkMain") )
					return hidSelectedItems.Value.Split(',');
				return null;
			}
		}

		public void SelectAll(DataView vw, string sFieldID)
		{
			StringBuilder sb = new StringBuilder();
			foreach ( DataRowView row in vw )
			{
				if ( sb.Length > 0 )
					sb.Append(",");
				sb.Append(Sql.ToString(row[sFieldID]));
			}
			hidSelectedItems.Value = sb.ToString();
			lblSelectedLabel.Text = String.Format("选择数：{0}", (hidSelectedItems.Value.Length+1)/37);
		}

		// 08/10/2013   Provide a way to clear all selected items. 
		public void ClearAll()
		{
			ClearAll(this.sFieldName);
		}

		public void ClearAll(string sFieldID)
		{
			StringBuilder sb = new StringBuilder();
			hidSelectedItems.Value = sb.ToString();
            lblSelectedLabel.Text = String.Format("选择数：{0}", 0);
		}

		protected void Page_Command(object sender, CommandEventArgs e)
		{
			if ( Command != null )
				Command(this, e) ;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 11/15/2007   CheckAll is not displayed on a mobile browser. 
			if ( this.IsMobile )
				this.Visible = false;
            lblSelectedLabel.Text = String.Format("选择数：{0}", (hidSelectedItems.Value.Length + 1) / 37);
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


