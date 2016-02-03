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

namespace Taoqi
{
	public class KeySortDropDownList : System.Web.UI.WebControls.DropDownList
	{
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.Attributes.Add("onkeypress", "return KeySortDropDownList_onkeypress(this, false)");
			this.Attributes.Add("onkeydown" , "if (window.event.keyCode == 13||window.event.keyCode == 9||window.event.keyCode == 27){this.fireEvent('onChange');onchangefired=true;}");
			this.Attributes.Add("onclick"   , "if (this.selectedIndex!=" + this.SelectedIndex + " && onchangefired==false) {this.fireEvent('onChange');onchangefired=true;}");
			// 01/13/2010   KeySortDropDownList is causing OnChange will always fire when tabbed-away. 
			// This onblur could be the cause, but we are not ready to research the issue further.  
			// It was only an issue in the PARENT_TYPE dropdown, so we will simply not use the KeySort in the Parent Type area. 
			this.Attributes.Add("onblur"    , "if (this.selectedIndex!=" + this.SelectedIndex + " && onchangefired==false) {this.fireEvent('onChange')}");
		}
	}
}



