<%@ Control Language="c#" AutoEventWireup="false" Codebehind="DetailView.ascx.cs" Inherits="Taoqi.Administration.ACLRoles.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>



<div id="nav">
    <div class="nav_left">
        <span class="title">角色
        </span>
        <span class="site_Path">当前位置：角色 > <span class="current1">查看角色</span>

        </span>
    </div>
    <div class="nav_right">

        <a href="default.aspx">
            <img align="absmiddle" border="0" src="../../include/images/back.png" />返回</a>

    </div>
</div>

<div id="divDetailView" runat="server" style="margin-top:20px;">
	<%@ Register TagPrefix="Taoqi" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Roles" EnablePrint="false" HelpName="DetailView" EnableHelp="false" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
    <div style="margin-bottom:10px;">
        <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" Runat="Server" />
    </div>

	<table ID="tblMain" class="tabDetailView" runat="server">
	</table>
	<br />

	<%@ Register TagPrefix="Taoqi" Tagname="AccessView" Src="AccessView.ascx" %>
	<Taoqi:AccessView ID="ctlAccessView" EnableACLEditing="false" Runat="Server" />
	
	<div id="divDetailSubPanel" style="margin-top:10px;">
		<asp:PlaceHolder ID="plcSubPanel" Runat="server" />
	</div>
</div>

<%@ Register TagPrefix="Taoqi" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />



