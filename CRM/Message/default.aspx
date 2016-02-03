<%@ Page language="c#" MasterPageFile="~/DefaultView.Master" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.TQMessage.Default" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
	<%@ Register TagPrefix="Taoqi" Tagname="ListView" Src="ListView.ascx" %>
	<Taoqi:ListView ID="ctlListView" Visible='<%# Taoqi.Security.GetUserAccess("TQMessage", "list") >= 0 %>' Runat="Server" />
	<asp:Label ID="lblAccessError" Text='<%# L10n.Term(".LBL_UNAUTH_ADMIN") %>' Visible="<%# !ctlListView.Visible %>" CssClass="error" EnableViewState="false" Runat="server" />
</asp:Content>


