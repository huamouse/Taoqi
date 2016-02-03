<%@ Control CodeBehind="Users.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.ACLRoles.Users" %>
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
function UserMultiSelect()
{
	return ModulePopup('Users', '<%= txtUSER_ID.ClientID %>', null, null, true, 'PopupMultiSelect.aspx');
}
</script>
<input ID="txtUSER_ID" type="hidden" Runat="server" />
<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
<Taoqi:ListHeader SubPanel="divRolesUsers" Title="Users.LBL_MODULE_NAME" Runat="Server" />

<div id="divRolesUsers" style='<%= "display:" + (CookieValue("divRolesUsers") != "1" ? "inline" : "none") %>'>
	<%@ Register TagPrefix="Taoqi" Tagname="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
	<Taoqi:DynamicButtons ID="ctlDynamicButtons" Runat="Server" />
 
	<Taoqi:SplendidGrid id="grdMain" SkinID="grdListView" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
		<Columns>
			<asp:HyperLinkColumn HeaderText="Users.LBL_LIST_NAME"       DataTextField="FULL_NAME" SortExpression="FULL_NAME"  ItemStyle-Width="25%" ItemStyle-CssClass="listViewTdLinkS1" DataNavigateUrlField="USER_ID" DataNavigateUrlFormatString="~/Users/view.aspx?id={0}" />
			<asp:BoundColumn     HeaderText="Users.LBL_LIST_USER_NAME"  DataField="USER_NAME"     SortExpression="USER_NAME"  ItemStyle-Width="25%" />
			<asp:BoundColumn     HeaderText="Users.LBL_LIST_EMAIL"      DataField="EMAIL1"        SortExpression="EMAIL1"     ItemStyle-Width="25%" />
			<asp:BoundColumn     HeaderText=".LBL_LIST_PHONE"           DataField="PHONE_WORK"    SortExpression="PHONE_WORK" ItemStyle-Width="21%" ItemStyle-Wrap="false" />
			<asp:TemplateColumn  HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:ImageButton Visible='<%# Taoqi.Security.AdminUserAccess("ACLRoles", "edit") >= 0 %>' CommandName="Users.Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "USER_ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LNK_REMOVE") %>' SkinID="delete_inline" Runat="server" />
					<asp:LinkButton  Visible='<%# Taoqi.Security.AdminUserAccess("ACLRoles", "edit") >= 0 %>' CommandName="Users.Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "USER_ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term(".LNK_REMOVE") %>' Runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</Taoqi:SplendidGrid>
</div>


