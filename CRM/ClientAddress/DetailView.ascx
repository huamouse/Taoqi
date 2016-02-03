<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="DetailView.ascx.cs" Inherits="Taoqi.TQClientAddress.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

<script>
    window.onload = function () {
        $("#ctl00_cntBody_ctlDetailView_ctlDynamicButtons_btnEDIT").hide();
        $("#ctl00_cntBody_ctlDetailView_ctlDynamicButtons_btnDELETE").hide();

    }


    </script>

<div class="tableStyle1" >
    <div class="titleBar2">
        <div class="txt2" style="color: black;">收货地址列表</div>

    </div>
    <div class="borderStyle5">

<div id="divDetailView" runat="server">
    <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
    <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQClientAddress" EnablePrint="false" HelpName="DetailView" EnableHelp="false" runat="Server" />

    <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>

    <div id="dynamicButtons">
        <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" runat="Server" />
    </div>
    <table id="tblMain" class="tabDetailView" runat="server" />

    
    <div id="divDetailSubPanel">
        <asp:PlaceHolder ID="plcSubPanel" runat="server" />
    </div>
</div>

        </div></div>

