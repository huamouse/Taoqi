<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.AuditEvents.ListView" %>
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
<div id="divListView">
	<%@ Register TagPrefix="Taoqi" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Users" Title="Administration.LBL_AUDIT_EVENTS_TITLE" EnablePrint="true" HelpName="index" EnableHelp="true" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="SearchView" Src="~/_controls/SearchView.ascx" %>
	<Taoqi:SearchView ID="ctlSearchView" Module="AuditEvents" ShowSearchTabs="false" Visible="<%# !PrintView %>" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="ExportHeader" Src="~/_controls/ExportHeader.ascx" %>
	<Taoqi:ExportHeader ID="ctlExportHeader" Module="Users" Title="Administration.LBL_AUDIT_EVENTS_TITLE" Runat="Server" />
	
	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>
	
	<Taoqi:SplendidGrid id="grdMain" SkinID="grdListView" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
		<Columns>
		</Columns>
	</Taoqi:SplendidGrid>

	<%@ Register TagPrefix="Taoqi" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>

