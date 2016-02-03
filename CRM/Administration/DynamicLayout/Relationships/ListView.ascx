<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.DynamicLayout.Relationships.ListView" %>
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
	<Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Administration" Title="DynamicLayout.LBL_RELATIONSHIPS_LAYOUT" EnablePrint="true" HelpName="index" EnableHelp="true" Runat="Server" />
	
	<asp:Table Width="100%" runat="server">
		<asp:TableRow>
			<asp:TableCell Width="200px" VerticalAlign="Top">
				<%@ Register TagPrefix="Taoqi" Tagname="SearchBasic" Src="../_controls/SearchBasic.ascx" %>
				<Taoqi:SearchBasic ID="ctlSearch" ViewTableName="vwDETAILVIEWS_Layout" ViewFieldName="DETAIL_NAME" Runat="Server" />
			</asp:TableCell>
			<asp:TableCell VerticalAlign="Top">
				<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
				<Taoqi:ListHeader ID="ctlListHeader" Runat="Server" />
				
				<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
					<asp:HiddenField ID="txtINDEX" Runat="server" />
					<asp:Button ID="btnINDEX_MOVE" ValidationGroup="move" style="display: none" runat="server" />
					<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
				</asp:Panel>
				
				<Taoqi:SplendidGrid id="grdMain" AllowPaging="false" AllowSorting="false" EnableViewState="true" runat="server">
					<Columns>
						<asp:TemplateColumn ItemStyle-CssClass="dragHandle">
							<ItemTemplate><asp:Image SkinID="blank" Width="14px" runat="server" /></ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn    HeaderText="DynamicLayout.LBL_LIST_MODULE_NAME" DataField="MODULE_NAME" ItemStyle-Width="24%" />
						<asp:TemplateColumn HeaderText="DynamicLayout.LBL_LIST_TITLE"                               ItemStyle-Width="55%" ItemStyle-Wrap="false">
							<ItemTemplate>
								<%# L10n.Term(Sql.ToString(Eval("TITLE"))) %>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn    HeaderText="Administration.LBL_TAB_ORDER" DataField="RELATIONSHIP_ORDER" ItemStyle-Width="5%" />
						<asp:TemplateColumn HeaderText="" ItemStyle-Width="10%" ItemStyle-Wrap="false" Visible="false">
							<ItemTemplate>
								<asp:ImageButton CommandName="Relationships.MoveUp"   Visible='<%#  Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) && (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term("Dropdown.LNK_UP"  ) %>' SkinID="uparrow_inline" Runat="server" />
								<asp:LinkButton  CommandName="Relationships.MoveUp"   Visible='<%#  Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) && (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term("Dropdown.LNK_UP") %>' Runat="server" />
								&nbsp;
								<asp:ImageButton CommandName="Relationships.MoveDown" Visible='<%#  Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) && (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term("Dropdown.LNK_DOWN") %>' SkinID="downarrow_inline" Runat="server" />
								<asp:LinkButton  CommandName="Relationships.MoveDown" Visible='<%#  Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) && (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term("Dropdown.LNK_DOWN") %>' Runat="server" />
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Administration.LNK_ENABLED" ItemStyle-Width="5%" ItemStyle-Wrap="false">
							<ItemTemplate>
								<asp:Label Visible='<%#  Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) %>' Text='<%# L10n.Term(".LBL_YES") %>' Runat="server" />
								<asp:Label Visible='<%# !Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) %>' Text='<%# L10n.Term(".LBL_NO" ) %>' Runat="server" />
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="" ItemStyle-Width="10%" ItemStyle-Wrap="false">
							<ItemTemplate>
								<asp:ImageButton CommandName="Relationships.Disable"  Visible='<%#  Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) && (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term("Administration.LNK_DISABLE") %>' SkinID="minus_inline" Runat="server" />
								<asp:LinkButton  CommandName="Relationships.Disable"  Visible='<%#  Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) && (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term("Administration.LNK_DISABLE"         ) %>' Runat="server" />
								<asp:ImageButton CommandName="Relationships.Enable"   Visible='<%# !Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) && (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term("Administration.LNK_ENABLE" ) %>' SkinID="plus_inline" Runat="server" />
								<asp:LinkButton  CommandName="Relationships.Enable"   Visible='<%# !Sql.ToBoolean(Eval("RELATIONSHIP_ENABLED")) && (Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0) %>' CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term("Administration.LNK_ENABLE"          ) %>' Runat="server" />
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</Taoqi:SplendidGrid>
				
				<Taoqi:InlineScript runat="server">
					<script type="text/javascript">
					// http://www.isocra.com/2008/02/table-drag-and-drop-jquery-plugin/
					$(document).ready(function()
					{
						$("#<%= grdMain.ClientID %>").tableDnD
						({
							dragHandle: "dragHandle",
							onDragClass: "jQueryDragBorder",
							onDragStart: function(table, row)
							{
								var txtINDEX = document.getElementById('<%= txtINDEX.ClientID %>');
								txtINDEX.value = (row.parentNode.rowIndex-1);
							},
							onDrop: function(table, row)
							{
								var txtINDEX = document.getElementById('<%= txtINDEX.ClientID %>');
								txtINDEX.value += ',' + (row.rowIndex-1); 
								document.getElementById('<%= btnINDEX_MOVE.ClientID %>').click();
							}
						});
						$("#<%= grdMain.ClientID %> tr").hover
						(
							function()
							{
								if ( !$(this).hasClass("nodrag") )
									$(this.cells[0]).addClass('jQueryDragHandle');
							},
							function()
							{
								if ( !$(this).hasClass("nodrag") )
									$(this.cells[0]).removeClass('jQueryDragHandle');
							}
						);
					});
					</script>
				</Taoqi:InlineScript>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>

	<%@ Register TagPrefix="Taoqi" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>


