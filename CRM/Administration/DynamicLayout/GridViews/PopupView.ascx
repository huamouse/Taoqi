<%@ Control CodeBehind="PopupView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.DynamicLayout.GridViews.PopupView" %>
<script runat="server">
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
</script>
<script type="text/javascript">
function SelectLayoutField(sPARENT_ID, sPARENT_NAME)
{
	if ( window.opener != null && window.opener.ChangeLayoutField != null )
	{
		window.opener.ChangeLayoutField(sPARENT_ID, sPARENT_NAME);
		window.close();
	}
	else
	{
		alert('Original window has closed.  Layout Field cannot be selected.' + '\n' + sPARENT_ID + '\n' + sPARENT_NAME);
	}
}
function Cancel()
{
	window.close();
}
</script>
<div id="divPopupView">
	<%@ Register TagPrefix="Taoqi" Tagname="SearchView" Src="~/_controls/SearchView.ascx" %>
	<Taoqi:SearchView ID="ctlSearchView" Module="DynamicLayout" SearchMode="GridView.PopupView" IsPopupSearch="true" ShowSearchTabs="false" Visible="<%# !PrintView %>" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<Taoqi:ListHeader Title="DynamicLayout.LBL_GRID_VIEW_LAYOUT" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
	<Taoqi:DynamicButtons ID="ctlDynamicButtons" Runat="Server" />

	<Taoqi:SplendidGrid id="grdMain" SkinID="grdPopupView" EnableViewState="true" runat="server">
	</Taoqi:SplendidGrid>

	<%@ Register TagPrefix="Taoqi" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>


