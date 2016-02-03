<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.Undelete.ListView" %>
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
	<%--
        <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Undelete" Title=".moduleList.Home" EnablePrint="true" HelpName="index" EnableHelp="true" Runat="Server" />
       --%>
       
    <div class="filter">
        <div class="title">
            <a href="default.aspx">
                <div class="left active">
                    ªÿ ’’æ
                </div>
            </a>
         
            

        </div>

    </div>

	<%@ Register TagPrefix="Taoqi" Tagname="SearchBasic" Src="SearchBasic.ascx" %>
	<Taoqi:SearchBasic ID="ctlSearchBasic" Visible="<%# !PrintView %>" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<Taoqi:ListHeader ID="ctlListHeader" Module="Undelete" Title="…æ≥˝º«¬º" Runat="Server" />
	
	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>
	
	<Taoqi:SplendidGrid id="grdMain" SkinID="grdListView" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
		<Columns>
			<asp:TemplateColumn HeaderText="" ItemStyle-Width="1%">
				<ItemTemplate><%# grdMain.InputCheckbox(!PrintView, ctlCheckAll.FieldName, Sql.ToGuid(Eval("AUDIT_ID")), ctlCheckAll.SelectedItems) %></ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn    HeaderText="Undelete.LBL_LIST_NAME"        DataField="NAME"        SortExpression="NAME"        ItemStyle-Width="70%" ItemStyle-VerticalAlign="Top" />
			<asp:TemplateColumn HeaderText="Undelete.LBL_LIST_AUDIT_TOKEN"                         SortExpression="AUDIT_TOKEN" ItemStyle-Width="10%" ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="false">
				<ItemTemplate><%# HttpUtility.HtmlEncode(Sql.ToString(Eval("AUDIT_TOKEN"))) %></ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn    HeaderText="Undelete.LBL_LIST_MODIFIED_BY" DataField="MODIFIED_BY" SortExpression="MODIFIED_BY" ItemStyle-Width="10%" ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="false" />
			<asp:BoundColumn    HeaderText="Undelete.LBL_LIST_AUDIT_DATE"  DataField="AUDIT_DATE"  SortExpression="AUDIT_DATE"  ItemStyle-Width="10%" ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="false" />
		</Columns>
	</Taoqi:SplendidGrid>
	<%@ Register TagPrefix="Taoqi" Tagname="CheckAll" Src="~/_controls/CheckAll.ascx" %>
	<Taoqi:CheckAll ID="ctlCheckAll" Visible="<%# !PrintView %>" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>


