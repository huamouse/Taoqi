<%@ Page language="c#" MasterPageFile="~/DefaultView.Master" Codebehind="edit.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.TQClient.Edit" %>

<%--
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
	<%@ Register TagPrefix="Taoqi" Tagname="Shortcuts" Src="~/_controls/Shortcuts.ascx" %>
	<Taoqi:Shortcuts ID="ctlShortcuts" SubMenu="Modules" Runat="Server" />
</asp:Content>
--%>

<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
	<%@ Register TagPrefix="Taoqi" Tagname="EditView" Src="EditView.ascx" %>
	<Taoqi:EditView ID="ctlEditView" Visible='<%# Taoqi.Security.GetUserAccess("TQClient", "edit") >= 0 %>' Runat="Server" />
	<asp:Label ID="lblAccessError" Text='<%# L10n.Term(".LBL_UNAUTH_ADMIN") %>' Visible="<%# !ctlEditView.Visible %>" CssClass="error" EnableViewState="false" Runat="server" />
</asp:Content>


