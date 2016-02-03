<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.Users.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type="text/javascript">
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

    window.onload=function(){
        $("#ctl00_cntBody_ctlDetailView_ctlDynamicButtons_btnEDIT").hide();
    }


</script>
<%--<div id="nav">
    <div class="nav_left">
        <span class="title">用户
        </span>
        <span class="site_Path">当前位置：用户 > <span class="current1">新建/编辑用户</span>

        </span>
    </div>
    <div class="nav_right">



        <a href="default.aspx">
            <img align="absmiddle" border="0" src="../include/images/back.png" />
            返回</a>

    </div>

</div>--%>
<div id="divMain" style="margin-top:20px;">
    <div class="chartTitle">
        <div class="txt" style="font-size:large; font-weight:bold;">新建/编辑用户</div>
    </div>
    <div class="container_body">
        <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
        <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Users" EnablePrint="false" HelpName="EditView" EnableHelp="true" runat="Server" />
        <div>
            <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/EditButtons.ascx" %>
            <div style="display: none">
                <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" ShowRequired="true" runat="Server" />
            </div>

            <asp:Table SkinID="tabForm" runat="server" Visible="false">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Table SkinID="tabEditView" runat="server">
                            <asp:TableRow>
                                <asp:TableCell Width="15%" CssClass="dataLabel"><%= L10n.Term("Users.LBL_LAST_NAME") %><asp:Label ID="Label1" CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' runat="server" /></asp:TableCell>
                                <asp:TableCell Width="35%" CssClass="dataField">
                                    <asp:TextBox ID="txtLAST_NAME" TabIndex="1" MaxLength="30" size="25" runat="server" />
                                    <asp:RequiredFieldValidator ID="reqLAST_NAME" ControlToValidate="txtLAST_NAME" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell Width="15%" CssClass="dataLabel"><%= L10n.Term("Users.LBL_USER_NAME") %>&nbsp;<asp:Label CssClass="required" Text='<%# L10n.Term(".LBL_REQUIRED_SYMBOL") %>' Runat="server" /></asp:TableCell>
                                <asp:TableCell Width="35%" CssClass="dataField">
                                    <asp:TextBox ID="txtUSER_NAME" TabIndex="2" MaxLength="30" size="20" runat="server" />
                                    <asp:RequiredFieldValidator ID="reqUSER_NAME" ControlToValidate="txtUSER_NAME" ErrorMessage='<%# L10n.Term(".ERR_REQUIRED_FIELD") %>' CssClass="required" EnableClientScript="false" EnableViewState="false" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="false">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_FIRST_NAME") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:TextBox ID="txtFIRST_NAME" TabIndex="1" MaxLength="30" size="25" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell ID="tdPASSWORD_Label" CssClass="dataLabel"><%= L10n.Term("Users.LBL_PASSWORD") %></asp:TableCell>
                                <asp:TableCell ID="tdPASSWORD_Field" CssClass="dataField">
                                    <asp:TextBox ID="txtPASSWORD" TextMode="Password" TabIndex="2" size="20" MaxLength="50" CssClass="dataField" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_STATUS") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:DropDownList ID="lstSTATUS" DataValueField="NAME" Width="160" DataTextField="DISPLAY_NAME" TabIndex="1" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell ID="tdDEFAULT_TEAM_Label" CssClass="dataLabel" VerticalAlign="top"><%= L10n.Term("Users.LBL_DEFAULT_TEAM") %></asp:TableCell>
                                <asp:TableCell ID="tdDEFAULT_TEAM_Field" CssClass="dataField">
                                    <asp:TextBox ID="DEFAULT_TEAM_NAME" ReadOnly="True" runat="server" />
                                    <input id="DEFAULT_TEAM" type="hidden" runat="server" />
                                    <input id="btnChangeTeam" type="button" cssclass="button" onclick="return ModulePopup('Teams', '<%= new Taoqi.DynamicControl(this, "DEFAULT_TEAM").ClientID %>    ', '<%= new Taoqi.DynamicControl(this, "DEFAULT_TEAM_NAME").ClientID %>    ', 'PRIVATE=0', false, null);" title="<%# L10n.Term(".LBL_SELECT_BUTTON_TITLE") %>" accesskey="<%# L10n.AccessKey(".LBL_SELECT_BUTTON_KEY") %>" value="<%# L10n.Term(".LBL_SELECT_BUTTON_LABEL") %>" />
                                    <input id="btnClearTeam" type="button" cssclass="button" onclick="return ClearModuleType('Teams', '<%= new Taoqi.DynamicControl(this, "DEFAULT_TEAM").ClientID %>    ', '<%= new Taoqi.DynamicControl(this, "DEFAULT_TEAM_NAME").ClientID %>    ', false);" title="<%# L10n.Term(".LBL_CLEAR_BUTTON_TITLE" ) %>" accesskey="<%# L10n.AccessKey(".LBL_CLEAR_BUTTON_KEY" ) %>" value="<%# L10n.Term(".LBL_CLEAR_BUTTON_LABEL" ) %>" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <div>
            <asp:HiddenField ID="LAYOUT_EDIT_VIEW" runat="server" />
            <asp:Table SkinID="tabForm" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <table id="tblMain" class="tabEditView" runat="server">
                            <tr>
                                <th colspan="4">
                                    <p style="font-size:larger; text-align:center;">
                                        <asp:Label Text='<%# L10n.Term("Users.LBL_USER_SETTINGS") %>' runat="server" />
                                    </p>
                                </th>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

        <div>
            <asp:Table SkinID="tabForm" runat="server"  style="margin-top:20px;">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Table SkinID="tabEditView" runat="server">
                            <asp:TableRow>
                                <asp:TableHeaderCell ColumnSpan="3"><p style="font-size:larger; text-align:center;"><asp:Label Text="管理员设置" runat="server" /></p></asp:TableHeaderCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="<%# Taoqi.Security.isAdmin %>">
                                <asp:TableCell Width="15%" CssClass="dataLabel"><%= L10n.Term("Users.LBL_ADMIN") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField" ColumnSpan="2">
                                    <asp:CheckBox ID="chkIS_ADMIN" TabIndex="3" runat="server" /><%= L10n.Term("Users.LBL_ADMIN_TEXT") %>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible='False'>
                                <asp:TableCell Width="20%" CssClass="dataLabel"><%= L10n.Term("Users.LBL_ADMIN_DELEGATE") %></asp:TableCell>
                                <asp:TableCell Width="15%" CssClass="dataField">
                                    <asp:CheckBox ID="chkIS_ADMIN_DELEGATE" TabIndex="3" CssClass="checkbox" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell Width="65%" CssClass="dataField"><%= L10n.Term("Users.LBL_ADMIN_DELEGATE_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="false">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_PORTAL_ONLY") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:CheckBox ID="chkPORTAL_ONLY" TabIndex="3" CssClass="checkbox" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_PORTAL_ONLY_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_RECEIVE_NOTIFICATIONS") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:CheckBox ID="chkRECEIVE_NOTIFICATIONS" TabIndex="3" CssClass="checkbox" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_RECEIVE_NOTIFICATIONS_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_THEME") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:DropDownList ID="lstTHEME" DataValueField="NAME" DataTextField="NAME" TabIndex="3" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_THEME_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_LANGUAGE") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:DropDownList ID="lstLANGUAGE" DataValueField="NAME" DataTextField="NATIVE_NAME" OnSelectedIndexChanged="lstLANGUAGE_Changed" AutoPostBack="true" TabIndex="3" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_LANGUAGE_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_DATE_FORMAT") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:DropDownList ID="lstDATE_FORMAT" TabIndex="3" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_DATE_FORMAT_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_TIME_FORMAT") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:DropDownList ID="lstTIME_FORMAT" TabIndex="3" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_TIME_FORMAT_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_TIMEZONE") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:DropDownList ID="lstTIMEZONE" DataValueField="ID" DataTextField="NAME" TabIndex="3" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_TIMEZONE_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_CURRENCY") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:DropDownList ID="lstCURRENCY" DataValueField="ID" DataTextField="NAME_SYMBOL" TabIndex="3" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_CURRENCY_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="false">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_REMINDER") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <!-- 08/05/2006   Remove stub of unsupported code. Reminder is not supported at this time. -->
                                    <asp:CheckBox ID="chkSHOULD_REMIND" TabIndex="3" CssClass="checkbox" runat="server" />
                                    <asp:DropDownList ID="lstREMINDER_TIME" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="3" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_REMINDER_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_SAVE_QUERY") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:CheckBox ID="chkSAVE_QUERY" TabIndex="3" CssClass="checkbox" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_SAVE_QUERY_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_GROUP_TABS") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:CheckBox ID="chkGROUP_TABS" TabIndex="3" CssClass="checkbox" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_GROUP_TABS_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_SUBPANEL_TABS") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:CheckBox ID="chkSUBPANEL_TABS" TabIndex="3" CssClass="checkbox" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_SUBPANEL_TABS_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Visible="False">
                                <asp:TableCell CssClass="dataLabel"><%= L10n.Term("Users.LBL_SYSTEM_GENERATED_PASSWORD") %></asp:TableCell>
                                <asp:TableCell CssClass="dataField">
                                    <asp:CheckBox ID="chkSYSTEM_GENERATED_PASSWORD" TabIndex="3" CssClass="checkbox" runat="server" />
                                </asp:TableCell>
                                <asp:TableCell CssClass="dataField"><%= L10n.Term("Users.LBL_SYSTEM_GENERATED_PASSWORD_TEXT") %></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        
        <p>
            <asp:Table SkinID="tabForm" runat="server" Visible="false">
                <asp:TableRow>
                    <asp:TableCell>
                        <table id="tblMailOptions" class="tabEditView" runat="server">
                            <tr>
                                <th colspan="4">
                                    <h4>
                                        <asp:Label Text='<%# L10n.Term("Users.LBL_MAIL_OPTIONS_TITLE") %>' runat="server" /></h4>
                                </th>
                            </tr>
                        </table>
                        <asp:Button ID="btnSmtpTest" CommandName="Smtp.Test" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term("Configurator.LBL_EMAIL_TEST_OUTBOUND_SETTINGS") + "  " %>' Visible="false" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </p>
        <p>
            <asp:Table ID="tblGoogleAppsPanel" SkinID="tabForm" Visible="false" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <table id="tblGoogleAppsOptions" class="tabEditView" runat="server">
                            <tr>
                                <th colspan="4">
                                    <h4>
                                        <asp:Label Text='<%# L10n.Term("Users.LBL_GOOGLEAPPS_OPTIONS_TITLE") %>' runat="server" /></h4>
                                </th>
                            </tr>
                        </table>
                        <asp:Button ID="btnGoogleAppsTest" CommandName="GoogleApps.Test" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term("Configurator.LBL_EMAIL_TEST_OUTBOUND_SETTINGS") + "  " %>' runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </p>
        <p>
            <asp:Table ID="tblICloudPanel" SkinID="tabForm" Visible="false" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <table id="tblICloudOptions" class="tabEditView" runat="server">
                            <tr>
                                <th colspan="4">
                                    <h4>
                                        <asp:Label Text='<%# L10n.Term("Users.LBL_ICLOUD_OPTIONS_TITLE") %>' runat="server" /></h4>
                                </th>
                            </tr>
                        </table>
                        <asp:Button ID="btnICloudTest" CommandName="iCloud.Test" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term("Configurator.LBL_EMAIL_TEST_OUTBOUND_SETTINGS") + "  " %>' runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </p>
        <Taoqi:DynamicButtons ID="ctlFooterButtons" Visible="<%# !PrintView %>" ShowRequired="false" runat="Server" />
         <%@ Register TagPrefix="Taoqi" TagName="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
        <Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" runat="Server" />
    </div>
</div>


