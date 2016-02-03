<%@ Control CodeBehind="PopupView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.BusinessRules.PopupView" %>
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
<div id="divPopupView">
	<%@ Register TagPrefix="Taoqi" Tagname="SearchView" Src="~/_controls/SearchView.ascx" %>
	<Taoqi:SearchView ID="ctlSearchView" Module="BusinessRules" IsPopupSearch="true" ShowSearchTabs="false" Visible="<%# !PrintView %>" Runat="Server" />

	<script type="text/javascript">
	function SelectBusinessRule(sPARENT_ID, sPARENT_NAME)
	{
		if ( window.opener != null && window.opener.ChangeBusinessRule != null )
		{
			window.opener.ChangeBusinessRule(sPARENT_ID, sPARENT_NAME);
			window.close();
		}
		else
		{
			alert('Original window has closed.  Business Rule cannot be assigned.' + '\n' + sPARENT_ID + '\n' + sPARENT_NAME);
		}
	}
	function SelectChecked()
	{
		if ( window.opener != null && window.opener.ChangeBusinessRule != null )
		{
			var sSelectedItems = document.getElementById('<%= ctlCheckAll.SelectedItems.ClientID %>').value;
			window.opener.ChangeBusinessRule(sSelectedItems, '');
			window.close();
		}
		else
		{
			alert('Original window has closed.  Business Rule cannot be assigned.');
		}
	}
	function Clear()
	{
		if ( window.opener != null && window.opener.ChangeBusinessRule != null )
		{
			window.opener.ChangeBusinessRule('', '');
			window.close();
		}
		else
		{
			alert('Original window has closed.  Product Type cannot be assigned.');
		}
	}
	function Cancel()
	{
		window.close();
	}
	</script>
	<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<Taoqi:ListHeader Title="BusinessRules.LBL_LIST_FORM_TITLE" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
	<Taoqi:DynamicButtons ID="ctlDynamicButtons" Runat="Server" />

	<asp:UpdatePanel UpdateMode="Conditional" Visible='<%# !Sql.ToBoolean(Application["CONFIG.disable_popupview_inline"]) %>' runat="server">
		<ContentTemplate>
			<asp:Button ID="btnCreateInline" CommandName="NewRecord.Show" OnCommand="Page_Command" Text='<%# L10n.Term(m_sMODULE + ".LBL_CREATE_BUTTON_LABEL") %>' CssClass="button" style="margin-bottom: 4px;" Visible="<%# !this.IsMobile %>" runat="server" />
			<asp:Panel ID="pnlNewRecordInline" Style="display:none" runat="server">
				<%@ Register TagPrefix="Taoqi" Tagname="NewRecord" Src="EditView.ascx" %>
				<Taoqi:NewRecord ID="ctlNewRecord" Width="100%" ShowTopButtons="false" ShowBottomButtons="true" ShowHeader="false" ShowCancel="false" Runat="Server" />
			</asp:Panel>
		</ContentTemplate>
	</asp:UpdatePanel>

	<Taoqi:SplendidGrid id="grdMain" SkinID="grdPopupView" EnableViewState="true" runat="server">
		<Columns>
			<asp:TemplateColumn HeaderText="" ItemStyle-Width="2%">
				<ItemTemplate><%# grdMain.InputCheckbox(!PrintView && bMultiSelect, ctlCheckAll.FieldName, Sql.ToGuid(Eval("ID")), ctlCheckAll.SelectedItems) %></ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</Taoqi:SplendidGrid>
	<%@ Register TagPrefix="Taoqi" Tagname="CheckAll" Src="~/_controls/CheckAll.ascx" %>
	<Taoqi:CheckAll ID="ctlCheckAll" Visible="<%# !PrintView && bMultiSelect %>" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>

