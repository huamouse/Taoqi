<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.UsersView" %>
<%@ Import Namespace="Taoqi.Crm" %>
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
<div id="divUsersView">
	<p>
	<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<Taoqi:ListHeader Title="Administration.LBL_USERS_TITLE" Runat="Server" />
	<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />

	<asp:Table Width="100%" CssClass="tabDetailView2" runat="server">
		<asp:TableRow>
			<asp:TableCell Width="20%" CssClass="tabDetailViewDL2">
				<asp:Image SkinID="Users" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_USERS_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_USERS_TITLE") %>' NavigateUrl="~/Users/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
				<br />
				<div align="center">
				(
				<asp:LinkButton ID="btnUserRequired" Visible='<%# !Config.require_user_assignment() %>' Text="Require"  CommandName="UserAssignement.Require"  OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				&nbsp;
				<asp:LinkButton ID="btnUserOptional" Visible='<%#  Config.require_user_assignment() %>' Text="Optional" CommandName="UserAssignement.Optional" OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				)
				</div>
			</asp:TableCell>
			<asp:TableCell Width="30%" CssClass="tabDetailViewDF2">
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MANAGE_USERS") %>' runat="server" /><br />
				User Assignment is <%# Config.require_user_assignment() ? "Required" : "Optional" %>
			</asp:TableCell>
			<asp:TableCell Width="20%" CssClass="tabDetailViewDL2">
				<asp:Image SkinID="Roles" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_ROLES_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_ROLES_TITLE") %>' NavigateUrl="~/Administration/ACLRoles/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
				<br />
				<div align="center">
				(
				<asp:LinkButton ID="btnAdminDelegationEnable"  Visible='<%# !Sql.ToBoolean(Application["CONFIG.allow_admin_roles"]) && (Taoqi.Security.AdminUserAccess("ACLRoles", "edit") >= 0) %>' Text='<%# L10n.Term("ACLRoles.LBL_ENABLE_ADMIN_DELEGATION" ) %>' CommandName="AdminDelegation.Enable"  OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				&nbsp;
				<asp:LinkButton ID="btnAdminDelegationDisable" Visible='<%#  Sql.ToBoolean(Application["CONFIG.allow_admin_roles"]) && (Taoqi.Security.AdminUserAccess("ACLRoles", "edit") >= 0) %>' Text='<%# L10n.Term("ACLRoles.LBL_DISABLE_ADMIN_DELEGATION") %>' CommandName="AdminDelegation.Disable" OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				)
				</div>
			</asp:TableCell>
			<asp:TableCell Width="30%" CssClass="tabDetailViewDF2">
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MANAGE_ROLES") %>' runat="server" /><br />
				<%# Sql.ToBoolean(Application["CONFIG.allow_admin_roles"]) ? L10n.Term("ACLRoles.LBL_ADMIN_DELEGATION_ENABLED") : L10n.Term("ACLRoles.LBL_ADMIN_DELEGATION_DISABLED") %>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2">
				<asp:Image SkinID="Users" AlternateText='<%# L10n.Term("Administration.LBL_USERS_LOGINS_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_USERS_LOGINS_TITLE") %>' NavigateUrl="~/Administration/UserLogins/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2"><asp:Label Text='<%# L10n.Term("Administration.LBL_USERS_LOGINS") %>' runat="server" /></asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Config", "access") >= 0 %>'>
				<asp:Image SkinID="Administration" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_PASSWORD_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_PASSWORD_TITLE") %>' NavigateUrl="~/Administration/PasswordManager/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Config", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MANAGE_PASSWORD") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
</div>

