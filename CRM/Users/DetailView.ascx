<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="DetailView.ascx.cs" Inherits="Taoqi.Users.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<%--<a href="default.aspx">返回列表</a>--%>
<script type="text/javascript">
    function PasswordPopup() {
        return window.open('Password.aspx', 'PasswordPopup', 'width=500,height=300,resizable=1,scrollbars=1');
    }

    function ChangePassword(sOLD_PASSWORD, sNEW_PASSWORD, sCONFIRM_PASSWORD) {
        document.getElementById('<%= txtOLD_PASSWORD.ClientID     %>').value = sOLD_PASSWORD;
        document.getElementById('<%= txtNEW_PASSWORD.ClientID     %>').value = sNEW_PASSWORD;
        document.getElementById('<%= txtCONFIRM_PASSWORD.ClientID %>').value = sCONFIRM_PASSWORD;
        document.forms[0].submit();
    }
</script>

<%--<div id="nav">
    <%--<div class="nav_left">
        <span class="title">用户
        </span>
        <span class="site_Path">当前位置：用户 > <span class="current1">查看用户</span>
        </span>
    </div>--
    <div class="nav_right">

        <a href="default.aspx">
            <img align="absmiddle" border="0" src="../include/images/back.png" />返回</a>
    </div>
</div>--%>
<div class="chartTitle" style="margin-top: 20px;">
    <div class="detail_nav">
        <div class="txt" style="font-size:large; font-weight:bold;">查看用户</div>
    </div>
    <div class="detail_nav" style="margin-left: 13px;"></div>
    <div class="detail_nav_right">
        <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
        <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" runat="Server" />
    </div>
</div>

<div id="divMain">
    <input id="txtOLD_PASSWORD" type="hidden" runat="server" />
    <input id="txtNEW_PASSWORD" type="hidden" runat="server" />
    <input id="txtCONFIRM_PASSWORD" type="hidden" runat="server" />

    <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
    <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Users" EnablePrint="false" HelpName="DetailView" EnableHelp="false" runat="Server" />

    <%--<%@ Register TagPrefix="Taoqi" Tagname="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
	<Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" Runat="Server" />--%>

    <asp:Table SkinID="tabDetailView" runat="server" Visible="false">
        <asp:TableRow>
            <asp:TableCell Width="15%" VerticalAlign="top" CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_NAME") %></asp:TableCell>
            <asp:TableCell Width="35%" VerticalAlign="top" CssClass="tabDetailViewDF">
                <asp:Label ID="txtNAME" runat="server" />
            </asp:TableCell>
            <asp:TableCell Width="15%" VerticalAlign="top" CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_USER_NAME") %></asp:TableCell>
            <asp:TableCell Width="35%" VerticalAlign="top" CssClass="tabDetailViewDF">
                <asp:Label ID="txtUSER_NAME" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="top" CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_STATUS") %></asp:TableCell>
            <asp:TableCell VerticalAlign="top" CssClass="tabDetailViewDF" ColumnSpan="3">
                <asp:Label ID="txtSTATUS" runat="server" />
            </asp:TableCell>

            <asp:TableCell ID="tdDEFAULT_TEAM_Label" VerticalAlign="top" CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_DEFAULT_TEAM") %></asp:TableCell>
            <asp:TableCell ID="tdDEFAULT_TEAM_Field" VerticalAlign="top" CssClass="tabDetailViewDF">
                <asp:Label ID="DEFAULT_TEAM_NAME" runat="server" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</div>

<div id="divUserSettings">
    <div id="divDetailView" runat="server">
        <asp:HiddenField ID="LAYOUT_DETAIL_VIEW" runat="server" />
        <table id="tblMain" class="tabDetailView" runat="server">
            <tr>
                <th colspan="4" class="dataLabel">
                    <p style="font-size:larger">
                        <asp:Label Text='<%# L10n.Term("Users.LBL_USER_INFORMATION") %>' runat="server" />
                    </p>
                </th>
            </tr>
        </table>
    </div>

    <div style="margin-top:20px;">
        <asp:Table SkinID="tabDetailView" runat="server">
            <asp:TableRow>
                <asp:TableHeaderCell ColumnSpan="3" CssClass="dataLabel"><p style="font-size:larger"><asp:Label Text="管理员设置" runat="server" /></p></asp:TableHeaderCell>
            </asp:TableRow>
            <asp:TableRow Visible='<%# Taoqi.Security.isAdmin %>'>
                <asp:TableCell Width="15%" CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_ADMIN") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:CheckBox ID="chkIS_ADMIN" Enabled="false" runat="server" /><div style="display: inline-block; margin-left: 20px;"><%= L10n.Term("Users.LBL_ADMIN_TEXT") %></div>
                </asp:TableCell>
                <%--<asp:TableCell CssClass="tabDetailViewDF" ColumnSpan="2"></asp:TableCell>--%>
            </asp:TableRow>
            <asp:TableRow Visible='False'>
                <asp:TableCell Width="20%" CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_ADMIN_DELEGATE") %>&nbsp;</asp:TableCell>
                <asp:TableCell Width="15%" CssClass="tabDetailViewDF">
                    <asp:CheckBox ID="chkIS_ADMIN_DELEGATE" Enabled="false" CssClass="checkbox" runat="server" />&nbsp;
                </asp:TableCell>
                <asp:TableCell Width="65%" CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_ADMIN_DELEGATE_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_PORTAL_ONLY") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:CheckBox ID="chkPORTAL_ONLY" Enabled="false" CssClass="checkbox" runat="server" />&nbsp;
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_PORTAL_ONLY_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_RECEIVE_NOTIFICATIONS") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:CheckBox ID="chkRECEIVE_NOTIFICATIONS" Enabled="false" CssClass="checkbox" runat="server" />&nbsp;
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_RECEIVE_NOTIFICATIONS_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_LANGUAGE") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:Label ID="txtLANGUAGE" runat="server" />
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_LANGUAGE_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_DATE_FORMAT") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:Label ID="txtDATEFORMAT" runat="server" />
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_DATE_FORMAT_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_TIME_FORMAT") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:Label ID="txtTIMEFORMAT" runat="server" />
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_TIME_FORMAT_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_TIMEZONE") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:Label ID="txtTIMEZONE" runat="server" />
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_TIMEZONE_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_CURRENCY") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:Label ID="txtCURRENCY" runat="server" />
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_CURRENCY_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_REMINDER") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <!-- 08/05/2006   Remove stub of unsupported code. Reminder is not supported at this time. -->
                    <asp:CheckBox ID="chkREMINDER" Enabled="false" CssClass="checkbox" runat="server" />&nbsp;
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_REMINDER_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>

            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_SAVE_QUERY") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:CheckBox ID="chkSAVE_QUERY" Enabled="false" CssClass="checkbox" runat="server" />&nbsp;
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_SAVE_QUERY_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_GROUP_TABS") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:CheckBox ID="chkGROUP_TABS" Enabled="false" CssClass="checkbox" runat="server" />&nbsp;
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_GROUP_TABS_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_SUBPANEL_TABS") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:CheckBox ID="chkSUBPANEL_TABS" Enabled="false" CssClass="checkbox" runat="server" />&nbsp;
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_SUBPANEL_TABS_TEXT") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Visible="false">
                <asp:TableCell CssClass="tabDetailViewDL"><%= L10n.Term("Users.LBL_SYSTEM_GENERATED_PASSWORD") %>&nbsp;</asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF">
                    <asp:CheckBox ID="chkSYSTEM_GENERATED_PASSWORD" Enabled="false" CssClass="checkbox" runat="server" />&nbsp;
                </asp:TableCell>
                <asp:TableCell CssClass="tabDetailViewDF"><%= L10n.Term("Users.LBL_SYSTEM_GENERATED_PASSWORD") %>&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</div>

<div id="divMailOptions" runat="server" visible="false">
    <p>
        <h4>
            <asp:Label Text='<%# L10n.Term("Users.LBL_MAIL_OPTIONS_TITLE") %>' runat="server" /></h4>
        <Taoqi:DynamicButtons ID="ctlExchangeButtons" Visible="<%# !PrintView %>" runat="Server" />
        <table id="tblMailOptions" class="tabDetailView" runat="server">
        </table>
    </p>
</div>

<asp:Panel ID="pnlGoogleAppsOptions" runat="server" Visible="false">
    <p>
        <h4>
            <asp:Label Text='<%# L10n.Term("Users.LBL_GOOGLEAPPS_OPTIONS_TITLE") %>' runat="server" /></h4>
        <Taoqi:DynamicButtons ID="ctlGoogleAppsButtons" Visible="<%# !PrintView %>" runat="Server" />
        <table id="tblGoogleAppsOptions" class="tabDetailView" runat="server">
        </table>
    </p>
</asp:Panel>

<asp:Panel ID="pnlICloudOptions" runat="server" Visible="false">
    <p>
        <h4>
            <asp:Label Text='<%# L10n.Term("Users.LBL_ICLOUD_OPTIONS_TITLE") %>' runat="server" /></h4>
        <Taoqi:DynamicButtons ID="ctlICloudButtons" Visible="<%# !PrintView %>" runat="Server" />
        <table id="tblICloudOptions" class="tabDetailView" runat="server">
        </table>
    </p>
</asp:Panel>

<div id="divAccessRights" runat="server" visible="false">
    <h4>访问权限</h4>
    <%@ Register TagPrefix="Taoqi" TagName="AccessView" Src="~/Administration/ACLRoles/AccessView.ascx" %>
    <Taoqi:AccessView ID="ctlAccessView" EnableACLEditing="false" runat="Server" />
</div>

<div id="divDetailSubPanel">
    <%@ Register TagPrefix="Taoqi" TagName="Signatures" Src="Signatures.ascx" %>
    <Taoqi:Signatures ID="ctlSignatures" runat="Server" Visible="false" />

    <%@ Register TagPrefix="Taoqi" TagName="Roles" Src="Roles.ascx" %>
    <Taoqi:Roles ID="ctlRoles" runat="Server" Visible="false" />

    <%@ Register TagPrefix="Taoqi" TagName="Teams" Src="Teams.ascx" %>
    <Taoqi:Teams ID="ctlTeams" runat="Server" Visible="false" />

    <%@ Register TagPrefix="Taoqi" TagName="Logins" Src="Logins.ascx" %>
    <Taoqi:Logins ID="ctlLogins" Visible='<%# Taoqi.Security.AdminUserAccess(m_sMODULE, "view") >= 0 %>' runat="Server" />
</div>

<%@ Register TagPrefix="Taoqi" TagName="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" runat="Server" />