<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="DetailView.ascx.cs" Inherits="Taoqi.TQMarketData.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>





<asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

<div id="divDetailView" runat="server">
    <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
    <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQMarketData" EnablePrint="false" HelpName="DetailView" EnableHelp="false" runat="Server" />

    <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>

    <div id="dynamicButtons">
        <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" runat="Server" />
    </div>
    <table id="tblMain" class="tabDetailView" runat="server" />


    <div id="divDetailSubPanel">
        <asp:PlaceHolder ID="plcSubPanel" runat="server" />
    </div>
</div>
<style>
    .sua {
        background-color:rebeccapurple;
    }
</style>
<script>
    $copy = $('#ctl00_cntBody_ctlDetailView_ctlDynamicButtons_btnDUPLICATE');
    $copy.hide();
    $span = $('#ctl00_cntBody_ctlEditView_ctlDynamicButtons_tdButtons');
    $span.addClass('sua');

    $(function () {
        var id = window.getQueryString("id");
        var btn_edit = $('#dynamicButtons').children('table:first').find('tr:first').children('td:first').children('div:first').children('input:first');
        btn_edit.attr("onclick", "window.location.href='MD_EditView.aspx?ID=" + id + "'; return false;");
    });
</script>



