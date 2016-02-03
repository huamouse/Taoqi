<%@ Control CodeBehind="SystemView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.SystemView" %>
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
<div id="divSystemView" visible='<%# 
(  Taoqi.Security.AdminUserAccess("Config"        , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Currencies"    , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("SystemLog"     , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Administration", "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Import"        , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Schedulers"    , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("PaymentGateway", "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Undelete"      , "access") >= 0 
) %>' runat="server">
	<p>
	<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<Taoqi:ListHeader Title="Administration.LBL_ADMINISTRATION_HOME_TITLE" Runat="Server" />
	<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />

	<asp:Table Width="100%" CssClass="tabDetailView2" runat="server">
		<asp:TableRow>
			<asp:TableCell Width="20%" CssClass="tabDetailViewDL2" Wrap="false" Visible='<%# Taoqi.Security.AdminUserAccess("Config", "access") >= 0 %>'>
				<asp:Image SkinID="Administration" AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURE_SETTINGS_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_CONFIGURE_SETTINGS_TITLE") %>' NavigateUrl="~/Administration/Config/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell Width="30%" CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Config", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_CONFIGURE_SETTINGS") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell Width="20%" CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Administration", "access") >= 0 %>'>
				<asp:Image SkinID="SystemCheck" AlternateText='<%# L10n.Term("Administration.LBL_UPGRADE_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_SYSTEM_CHECK_TITLE") %>' NavigateUrl="~/SystemCheck.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
				<br />
				<div align="center">
				(
				<asp:LinkButton Text="Reload"     CommandName="System.Reload" OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				&nbsp;
				<asp:HyperLink  Text='Precompile' NavigateUrl="~/_devtools/Precompile.aspx" CssClass="tabDetailViewDL2Link" Target="PrecompileTaoqi" Runat="server" />
				)
				</div>
			</asp:TableCell>
			<asp:TableCell Width="30%" CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Administration", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_SYSTEM_CHECK") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Currencies", "access") >= 0 %>'>
				<asp:Image SkinID="Currencies" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_CURRENCIES") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_CURRENCIES") %>' NavigateUrl="~/Administration/Currencies/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Currencies", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_CURRENCY") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("SystemLog", "access") >= 0 %>'>
				<asp:Image SkinID="Upgrade" AlternateText='<%# L10n.Term("Administration.LBL_SYSTEM_LOG_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_SYSTEM_LOG_TITLE") %>' NavigateUrl="~/Administration/SystemLog/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("SystemLog", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_SYSTEM_LOG") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Import", "access") >= 0 %>'>
				<asp:Image SkinID="Import" AlternateText='<%# L10n.Term("Administration.LBL_IMPORT_DATABASE_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_IMPORT_DATABASE_TITLE") %>' NavigateUrl="~/Administration/Import/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Import", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_IMPORT_DATABASE") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Import", "export") >= 0 %>'>
				<asp:Image SkinID="Export" AlternateText='<%# L10n.Term("Administration.LBL_EXPORT_DATABASE_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_EXPORT_DATABASE_TITLE") %>' NavigateUrl="~/Administration/Export/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Import", "export") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_EXPORT_DATABASE") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Schedulers", "access") >= 0 %>'>
				<asp:Image SkinID="Schedulers" AlternateText='<%# L10n.Term("Administration.LBL_SUGAR_SCHEDULER_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_SUGAR_SCHEDULER_TITLE") %>' NavigateUrl="~/Administration/Schedulers/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Schedulers", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_SUGAR_SCHEDULER") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.isAdmin %>'>
				<asp:Image SkinID="Backups" AlternateText='<%# L10n.Term("Administration.LBL_BACKUPS_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_BACKUPS_TITLE") %>' NavigateUrl="~/Administration/Backups/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
				<div align="center">
				(
				<asp:LinkButton Text='<%# L10n.Term("Administration.LBL_PURGE_DEMO") %>' CommandName="System.PurgeDemo" OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				)
				</div>
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.isAdmin %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_BACKUPS") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Config", "access") >= 0 %>'>
				<asp:Image SkinID="Administration" AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURATOR_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_CONFIGURATOR_TITLE") %>' NavigateUrl="~/Administration/Configurator/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Config", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_CONFIGURATOR") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Undelete", "access") >= 0 %>'>
				<asp:Image SkinID="Delete" AlternateText='<%# L10n.Term("Administration.LBL_UNDELETE_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_UNDELETE_TITLE") %>' NavigateUrl="~/Administration/Undelete/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Undelete", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_UNDELETE") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
</div>

