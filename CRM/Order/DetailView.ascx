<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="DetailView.ascx.cs" Inherits="Taoqi.Order.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

<%@ Register Assembly="Taoqi" Namespace="Taoqi.Order" TagPrefix="cc" %>
<style>
    .btnSecond {
        margin-left: 5px;
    }

    .layui-layer-prompt .layui-layer-title {
        display: block;
    }

    .layui-layer-prompt .layui-layer-content {
        padding-top: 22px;
    }

    img {
        max-width: 900px;
        max-height: 600px;
    }

    .layui-layer-content {
        height: auto !important;
    }

    .layui-layer-nobg .layui-layer-setwin {
        width: 32px;
        height: 32px;
        display: block;
        right: -11px;
        top: -16px;
    }

    .layui-layer-nobg .layui-layer-setwin a {
        width: 30px;
        height: 32px;
    }

    .layui-layer-ico {
        background-position: -149px -31px;
    }
</style>
<div class="tableStyle1">
    <div class="titleBar2">
        <div class="txt2" style="color: black;">我的订单</div>

    </div>
    <div class="borderStyle5" style="padding-top: 0px;">
        <div id="divDetailView" runat="server">
            <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
            <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQOrder" EnablePrint="false" HelpName="DetailView" EnableHelp="false" runat="Server" />

            <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>

            <div id="dynamicButtons">
                <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" runat="Server" />
            </div>
            <table id="tblMain" class="tabDetailView" runat="server" visible="false" />

            <asp:Repeater ID="RPOrderInformation" runat="server">
                <ItemTemplate>
                    <table class="formStyle" style="margin-bottom: 20px;">
                        <tr>
                            <td class="dataLabel">订单编号： </td>
                            <td class="dataField"><span class="colorBlack"><%# Eval("C_SN") %></span></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">公司名称： </td>
                            <td class="dataField"><a href="/client.html?id=<%# Eval("SellerClientID") %>"><%# Eval("Seller") %></a></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">联系人： </td>
                            <td class="dataField"><span style="color:#ff6400;"><%# Eval("SellerName") %></span></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">手机： </td>
                            <td class="dataField"><span style="color:#ff6400;"><%# Eval("SellerPhone") %></span></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">QQ： </td>
                            <td class="dataField"><a style="color: #0065e6;" href="http://wpa.qq.com/msgrd?v=3&uin='<%# Eval("SellerQQ") %>'&site=qq&menu=yes"><%# Eval("SellerQQ") %></a></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">订购货物： </td>
                            <td class="dataField"><span class="colorBlack"><%# Eval("ProductText") %></span></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">下单时间： </td>
                            <td class="dataField"><span class="colorBlack"><%# Eval("DATE_ENTERED","{0:yyyy-MM-dd hh:mm}") %></span></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">订单状态： </td>
                            <td class="dataField"><span class="price"><%# Eval("StatusName") %></span></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>


            <div>
                <span style="font-size: 16px; font-weight: bold; color: #ff6400; display: inline-block; height: 1.9em;">详细订单</span>
                <asp:UpdatePanel ID="updatePanel" runat="server" data-ng-controller="orderController">
                    <ContentTemplate>
                        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15"
                            AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
                            <Columns>
                                <asp:TemplateColumn HeaderText="操作" HeaderStyle-Width="170px">
                                    <ItemTemplate>
                                        <cc:ActionOrderDetail Status='<%# Eval("C_Status") %>' OrderDetailID='<%# Eval("ID") %>' CarID='<%# Eval("C_CarID") %>' 
                                            LandingUrl='<%# Eval("C_LandingUrl") %>' Driver='<%# Eval("C_Driver") %>' DriverTel='<%# Eval("C_Tel") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </Taoqi:SplendidGrid>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divDetailSubPanel">
                <asp:PlaceHolder ID="plcSubPanel" runat="server" />
            </div>
        </div>

    </div>
</div>
