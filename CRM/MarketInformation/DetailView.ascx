<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="DetailView.ascx.cs" Inherits="Taoqi.TQMarketInformation.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>






<asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

<div id="divDetailView" runat="server">
    <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
    <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQMarketInformation" EnablePrint="false" HelpName="DetailView" EnableHelp="false" runat="Server" />

    <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>


    

    <div id="dynamicButtons">
        <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" runat="Server" />
    </div>
    <table id="tblMain" class="tabDetailView" runat="server" />


    <div id="divDetailSubPanel">
        <asp:PlaceHolder ID="plcSubPanel" runat="server" />
    </div>
</div>



