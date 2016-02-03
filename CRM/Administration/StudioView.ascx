<%@ Control CodeBehind="StudioView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.StudioView" %>
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
<div id="divStudioView" visible='<%# 
(  Taoqi.Security.AdminUserAccess("DynamicLayout"    , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Dropdown"         , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("EditCustomFields" , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Modules"          , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("iFrames"          , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Terminology"      , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Shortcuts"        , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("Languages"        , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("DynamicButtons"   , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("FieldValidators"  , "access") >= 0 
|| Taoqi.Security.AdminUserAccess("ModuleBuilder"    , "access") >= 0 
) %>' runat="server">
	<p>
	<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<Taoqi:ListHeader Title="Administration.LBL_STUDIO_TITLE" Runat="Server" />
	<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />

	<asp:Table Width="100%" CssClass="tabDetailView2" runat="server">
		<asp:TableRow>
			<asp:TableCell Width="20%" CssClass="tabDetailViewDL2" Wrap="false" Visible='<%# Taoqi.Security.AdminUserAccess("DynamicLayout", "access") >= 0 %>'>
				<asp:Image SkinID="Layout" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_LAYOUT") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_LAYOUT") %>' NavigateUrl="~/Administration/DynamicLayout/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell Width="30%" CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("DynamicLayout", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_LAYOUT") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell Width="20%" CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Dropdown", "access") >= 0 %>'>
				<asp:Image SkinID="Dropdown" AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURE_SETTINGS_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_DROPDOWN_EDITOR") %>' NavigateUrl="~/Administration/Dropdown/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell Width="30%" CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Dropdown", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.DESC_DROPDOWN_EDITOR") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("EditCustomFields", "access") >= 0 %>'>
				<asp:Image SkinID="FieldLabels" AlternateText='<%# L10n.Term("Administration.LBL_EDIT_CUSTOM_FIELDS") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_EDIT_CUSTOM_FIELDS") %>' NavigateUrl="~/Administration/EditCustomFields/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
				<br />
				<div align="center">
				(
				<asp:LinkButton Text="Recompile"     CommandName="System.RecompileViews" OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				&nbsp;
				<asp:LinkButton Text="Rebuild Audit" CommandName="System.RebuildAudit"   OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				&nbsp;
				<asp:LinkButton ID="lnkUpdateModel" Text="UpdateModel" CommandName="System.UpdateModel" OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Visible="false" Runat="server" />
				)
				</div>
			</asp:TableCell>
			<asp:TableCell Width="30%" CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("EditCustomFields", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.DESC_EDIT_CUSTOM_FIELDS") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Modules", "access") >= 0 %>'>
				<asp:Image SkinID="ConfigureTabs" AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURE_TABS") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_CONFIGURE_TABS") %>' NavigateUrl="~/Administration/ConfigureTabs/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Modules", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_CHOOSE_WHICH") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Modules", "access") >= 0 %>'>
				<asp:Image SkinID="RenameTabs" AlternateText='<%# L10n.Term("Administration.LBL_CONFIGURE_TABS") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_RENAME_TABS") %>' NavigateUrl="~/Administration/RenameTabs/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Modules", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_CHANGE_NAME_TABS") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("iFrames", "access") >= 0 %>'>
				<asp:Image SkinID="iFrames" AlternateText='<%# L10n.Term("Administration.DESC_IFRAME") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_IFRAME") %>' NavigateUrl="~/iFrames/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("iFrames", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.DESC_IFRAME") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Terminology", "access") >= 0 %>'>
				<asp:Image SkinID="Terminology" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_TERMINOLOGY_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_TERMINOLOGY_TITLE") %>' NavigateUrl="~/Administration/Terminology/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Terminology", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MANAGE_TERMINOLOGY") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Modules", "access") >= 0 %>'>
				<asp:Image SkinID="Modules" AlternateText='<%# L10n.Term("Administration.LBL_MODULES_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MODULES_TITLE") %>' NavigateUrl="~/Administration/Modules/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
				<br />
				<div align="center">
				(
				<asp:LinkButton ID="btnPagingEnable"  Visible='<%# !Config.allow_custom_paging() && (Taoqi.Security.AdminUserAccess("Modules", "edit") >= 0) %>' Text='<%# L10n.Term("Modules.LBL_ENABLE" ) %>' CommandName="CustomPaging.Enable"  OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				&nbsp;
				<asp:LinkButton ID="btnPagingDisable" Visible='<%#  Config.allow_custom_paging() && (Taoqi.Security.AdminUserAccess("Modules", "edit") >= 0) %>' Text='<%# L10n.Term("Modules.LBL_DISABLE") %>' CommandName="CustomPaging.Disable" OnCommand="Page_Command" CssClass="tabDetailViewDL2Link" Runat="server" />
				)
				</div>
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Modules", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MODULES") %>' runat="server" /><br />
				<%# Config.allow_custom_paging() ? L10n.Term("Modules.LBL_CUSTOM_PAGING_ENABLED") : L10n.Term("Modules.LBL_CUSTOM_PAGING_DISABLED") %>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Shortcuts", "access") >= 0 %>'>
				<asp:Image SkinID="Shortcuts" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_SHORTCUTS_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_SHORTCUTS_TITLE") %>' NavigateUrl="~/Administration/Shortcuts/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Shortcuts", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MANAGE_SHORTCUTS") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Languages", "access") >= 0 %>'>
				<asp:Image SkinID="LanguagePacks" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_LANGUAGES") %>' runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_LANGUAGES") %>' NavigateUrl="~/Administration/Languages/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Languages", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MANAGE_LANGUAGES") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("DynamicButtons", "access") >= 0 %>'>
				<asp:Image SkinID="DynamicButtons" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_DYNAMIC_BUTTONS_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_DYNAMIC_BUTTONS_TITLE") %>' NavigateUrl="~/Administration/DynamicButtons/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("DynamicButtons", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MANAGE_DYNAMIC_BUTTONS") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("Terminology", "access") >= 0 %>'>
				<asp:Image SkinID="Terminology" AlternateText='<%# L10n.Term("Administration.LBL_IMPORT_TERMINOLOGY_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_IMPORT_TERMINOLOGY_TITLE") %>' NavigateUrl="~/Administration/Terminology/Import/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("Terminology", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_IMPORT_TERMINOLOGY") %>' runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("FieldValidators", "access") >= 0 %>'>
				<asp:Image SkinID="FieldValidators" AlternateText='<%# L10n.Term("Administration.LBL_MANAGE_FIELD_VALIDATORS_TITLE") %>' Runat="server" />
				&nbsp;
				<asp:HyperLink Text='<%# L10n.Term("Administration.LBL_MANAGE_FIELD_VALIDATORS_TITLE") %>' NavigateUrl="~/Administration/FieldValidators/default.aspx" CssClass="tabDetailViewDL2Link" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("FieldValidators", "access") >= 0 %>'>
				<asp:Label Text='<%# L10n.Term("Administration.LBL_MANAGE_FIELD_VALIDATORS") %>' runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDL2" Visible='<%# Taoqi.Security.AdminUserAccess("ModuleBuilder", "access") >= 0 %>'>
				<asp:Image ID="imgMODULE_BUILDER" SkinID="Modules" AlternateText='<%# L10n.Term("Administration.LBL_MODULE_BUILDER_TITLE") %>' Visible="false" Runat="server" />
				&nbsp;
				<asp:HyperLink ID="lnkMODULE_BUILDER" Text='<%# L10n.Term("Administration.LBL_MODULE_BUILDER_TITLE") %>' NavigateUrl="~/Administration/ModuleBuilder/default.aspx" CssClass="tabDetailViewDL2Link" Visible="false" Runat="server" />
			</asp:TableCell>
			<asp:TableCell CssClass="tabDetailViewDF2" Visible='<%# Taoqi.Security.AdminUserAccess("ModuleBuilder", "access") >= 0 %>'>
				<asp:Label ID="lblMODULE_BUILDER" Text='<%# L10n.Term("Administration.LBL_MODULE_BUILDER") %>' Visible="false" runat="server" />
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	</p>
</div>


