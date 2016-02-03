<%@ Control CodeBehind="SplendidLanguagePacks.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.Terminology.Import.SplendidLanguagePacks" %>
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
<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
<Taoqi:ListHeader SubPanel="divImportSplendidLanguagePacks" Title="Taoqi Language Packs" Runat="Server" />

<div id="divImportSplendidLanguagePacks" style='<%= "display:" + (CookieValue("divImportSplendidLanguagePacks") != "1" ? "inline" : "none") %>'>
	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>
	
	<Taoqi:SplendidGrid id="grdMain" SkinID="grdListView" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
		<Columns>
			<asp:TemplateColumn  HeaderText="Name"                                SortExpression="Name"        ItemStyle-Width="25%" ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:ImageButton CommandName="LanguagePack.Import" CommandArgument='<%# Eval("URL") %>' CausesValidation="false" OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term("Import.LBL_MODULE_NAME") %>' SkinID="Import" Runat="server" />
					<asp:LinkButton  CommandName="LanguagePack.Import" CommandArgument='<%# Eval("URL") %>' CausesValidation="false" OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# Eval("Name") %>' Runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn     HeaderText="Date"        DataField="Date"        SortExpression="Date"        ItemStyle-Width="15%" ItemStyle-Wrap="false" />
			<asp:BoundColumn     HeaderText="Description" DataField="Description" SortExpression="Description" ItemStyle-Width="60%" ItemStyle-Wrap="true" />
			<asp:TemplateColumn  HeaderText="" ItemStyle-Width="1%">
				<ItemTemplate>
					<asp:HyperLink  NavigateUrl='<%# Eval("URL") %>' SkinID="Backup" runat="server" />
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</Taoqi:SplendidGrid>
</div>


