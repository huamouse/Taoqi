<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.UserLogins.ListView" %>
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
    <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>

    <%--
        <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Users" Title="Administration.LBL_USERS_LOGINS_TITLE" EnablePrint="false" HelpName="index" EnableHelp="false" Runat="Server" />
    --%>

    <%@ Register TagPrefix="Taoqi" TagName="SearchView" Src="~/_controls/SearchView.ascx" %>
    <div id="nav">
        <div class="nav_left">
            <span class="title">登录日志
            </span>
            <span class="site_Path">当前位置：后台管理 > <span class="current1">登录日志</span>

            </span>
        </div>
        <div class="nav_right">


            <a href="../../Admin/default.aspx">
                <img align="absmiddle" border="0" src="../../Include/images/back.png" />
                返回</a>

        </div>
    </div>

    <div class="tabGroup">
        <a href="default.aspx"></a>
        <div class="tab">登录日志</div>

       
    </div>

    <div class="container_body">

        <Taoqi:SearchView ID="ctlSearchView" Module="UserLogins" ShowSearchTabs="false" Visible="<%# !PrintView %>" runat="Server" />

        <%@ Register TagPrefix="Taoqi" TagName="ExportHeader" Src="~/_controls/ExportHeader.ascx" %>
        <Taoqi:ExportHeader ID="ctlExportHeader" Module="Users" Title="日志" Visible="false" runat="Server" />

        <asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
            <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
        </asp:Panel>

        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="Users.LBL_LIST_ADMIN" ItemStyle-Width="2%">
                    <ItemTemplate>
                        <div style="DISPLAY: <%# Sql.ToInteger(DataBinder.Eval(Container.DataItem, "IS_ADMIN")) == 1 ? "inline" : "none" %>">
                            <asp:Image SkinID="check_inline" runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>

    </div>
</div>


