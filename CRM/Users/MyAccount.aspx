<%@ Page language="c#" MasterPageFile="~/DefaultView.Master" Codebehind="MyAccount.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.Users.MyAccount" %>

<%--
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
	<%@ Register TagPrefix="Taoqi" Tagname="Shortcuts" Src="~/_controls/Shortcuts.ascx" %>
	<Taoqi:Shortcuts ID="ctlShortcuts" SubMenu="Users" Visible='<%# Taoqi.Security.AdminUserAccess("Users", "access") >= 0 %>' Runat="Server" />
</asp:Content>
--%>

<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>
	
	<%@ Register TagPrefix="Taoqi" Tagname="DetailView" Src="DetailView.ascx" %>
	<Taoqi:DetailView ID="ctlDetailView" MyAccount="true" Runat="Server" />
</asp:Content>


