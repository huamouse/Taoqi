<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.Administration.ACLRoles.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
 * If not, see <http://www.gnu.org/licenses />.
 *
 * You can contact Taoqi Software, Inc. at email address support@Taoqi.com.
 *
 * In accordance with Section 7(b) of the GNU Affero General Public License version 3,
 * the Appropriate Legal Notices must display the following words on all interactive user interfaces:
 * "Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved."
 *********************************************************************************************************************/
</script>
<div id="nav">
    <div class="nav_left">
        <span class="title">新建/编辑角色管理
        </span>
        <span class="site_Path">当前位置：角色管理 > <span class="current1">新建/编辑角色管理</span>
        </span>
    </div>
    <div class="nav_right">

        <a href="../../Administration/ACLRoles/default.aspx">
            <img align="absmiddle" border="0" src="../../include/images/back.png" />
            返回</a>
    </div>
</div>
<div id="divEditView" runat="server" style="margin-top: 20px;">

    <div class="chartTitle">
        <div class="txt" style="font: bold large;">新建/编辑</div>
    </div>
    <div class="container_body">
        <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
        <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>

        <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Roles" EnablePrint="false" HelpName="EditView" EnableHelp="true" runat="Server" />

        <p>
            <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="false" ShowRequired="true" runat="Server" />
            <asp:Table SkinID="tabForm" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <table id="tblMain" class="tabEditView" runat="server">
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </p>

        

        <b><%= L10n.Term("ACLRoles.LBL_EDIT_VIEW_DIRECTIONS") %></b>
        <%@ Register TagPrefix="Taoqi" TagName="AccessView" Src="AccessView.ascx" %>
        <Taoqi:AccessView ID="ctlAccessView" EnableACLEditing="true" runat="Server" />

        <div style="margin-top:20px;">
            <Taoqi:DynamicButtons ID="ctlFooterButtons" Visible="<%# !PrintView %>" ShowRequired="false" runat="Server" />
        </div>
    </div>
</div>