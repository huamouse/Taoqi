<%@ Control CodeBehind="Logins.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Users.Logins" %>
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
<div id="divUsersLogins">
	<br />
	<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<Taoqi:ListHeader Title="Users.LBL_LOGINS" Runat="Server" />
	
	<asp:Panel CssClass="button-panel" Visible='<%# Taoqi.Security.AdminUserAccess(m_sMODULE, "view") >= 0 %>' runat="server">
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>

	<Taoqi:SplendidGrid id="grdMain" SkinID="grdSubPanelView" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
		<Columns>
			<asp:TemplateColumn  HeaderText="Users.LBL_LIST_ADMIN" ItemStyle-Width="2%">
				<ItemTemplate>
					<div style="DISPLAY: <%# Sql.ToInteger(DataBinder.Eval(Container.DataItem, "IS_ADMIN")) == 1 ? "inline" : "none" %>">
						<asp:Image SkinID="check_inline" Runat="server" />
					</div>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</Taoqi:SplendidGrid>
</div>


