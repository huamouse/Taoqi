<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="DetailView.ascx.cs" Inherits="Taoqi.OrderSell.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

<%@ Register Assembly="Taoqi" Namespace="Taoqi.OrderSell" TagPrefix="RB" %>
<style>
    .layui-layer-prompt .layui-layer-title {
        display: block;
    }

    .layui-layer-prompt .layui-layer-content {
        padding-top: 22px;
    }

    .layui-layer-prompt .layui-layer-btn {
        bottom: -1px;
    }
    .layui-layer-prompt .layui-layer-setwin a {
        display:none;
    }

    input::-webkit-outer-spin-button,
    input::-webkit-inner-spin-button {
        -webkit-appearance: none !important;
        margin: 0;
    }

    input[type="number"] {
        -moz-appearance: textfield;
    }
    
    .layui-layer-content> img {
        max-width: 900px;
        max-height: 600px;
    }

    .layui-layer-content {
        height: auto !important;
    }

    .layui-layer-setwin {
        width: 32px;
        height: 32px;
        right: -11px;
        top: -16px;
    }

    .layui-layer-setwin a {
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
                            <td class="dataField"><%# Eval("C_SN") %></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">公司名称： </td>
                            <td class="dataField"><a href="/client.html?id=<%# Eval("BuyerClientID") %>"><%# Eval("Buyer") %></a></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">联系人： </td>
                            <td class="dataField"><span style="color:#ff6400;"><%# Eval("BuyerName") %></span></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">手机： </td>
                            <td class="dataField"><span style="color:#ff6400;"><%# Eval("BuyerPhone") %></span></td>
                        </tr>
                        <tr>
                            <td class="dataLabel">QQ： </td>
                            <td class="dataField"><a style="color: #0065e6;" href="http://wpa.qq.com/msgrd?v=3&uin='<%# Eval("BuyerQQ") %>'&site=qq&menu=yes"><%# Eval("BuyerQQ") %></a></td>
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
                <span style="font-size: 16px; font-weight: bold; color: #ff6400; margin-left: 2px;">详细订单</span>
                <asp:UpdatePanel ID="updatePanel" runat="server" data-ng-controller="orderController">
                    <ContentTemplate>
                        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15"
                            AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
                            <Columns>
                                <asp:TemplateColumn HeaderText="操作" HeaderStyle-Width="170px">
                                    <ItemTemplate>
                                        <RB:ActionOrderDetail Status='<%# Eval("C_Status") %>' OrderDetailID='<%# Eval("ID") %>' CarID='<%# Eval("C_CarID") %>' 
                                            ShippingUrl='<%# Eval("C_ShippingUrl") %>' Driver='<%# Eval("C_Driver") %>' DriverTel='<%# Eval("C_Tel") %>' runat="server" />
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

<div class="modal fade" id="fahuoModal" style="overflow:hidden;" tabindex="-1" role="dialog">
    <div class="modal-content">
        <div class="modal-header">
            <button class="close" type="button" data-dismiss="modal">×</button>
            <h3 id="fahuoModalLabel">选择车辆</h3>
        </div>
        <div class="modal-body" style="min-height: 200px;">
            <div id="fahuord_left">
                <asp:Repeater ID="RPSelectCar" runat="server">
                    <ItemTemplate>
                        <div style="font-size: 14px; margin-top: 10px; margin-left: 30px;">
                            <input type="radio" name="RDCar" value="<%# Eval("ID") %>" />
                            <span><%# Eval("C_PlateNumber") %>，<%# Eval("C_Tonnage", "{0:f2}") %>，<%# Eval("C_Driver") %>，<%# Eval("C_Tel") %>，<%# Eval("C_Driver2") %>，<%# Eval("C_Tel2") %></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="modal-footer">
            <div style="margin-top: 13px;">
                <a style="width: 100px;" class="btnOrange" id="fhbtnadd" data-toggle="modal" data-target="#fahuoaddModal">+ 新增车辆</a>
                <button type="submit" class="btn fhbtn" name="SendCars" value="SendCars" style="background-color: #0065e6;height:32px;margin-top:-3px;">确认</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="fahuoaddModal" tabindex="-1" style="overflow:hidden;" role="dialog" data-ng-controller="carController">
    <div class="modal-header">
        <button class="close" type="button" data-dismiss="modal">×</button>
        <h3 id="quoteModalLabel">添加车辆</h3>
    </div>
    <div class="modal-body">
        <div ng-form="car" class="form-horizontal">
            <div class="row form-group" data-ng-class="{ 'has-error': car.plateNumber.$dirty && car.plateNumber.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>车牌号：</label>
                <input type="text" name="plateNumber" class="form-control col-md-4" data-ng-model="model.C_PlateNumber" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-show="car.plateNumber.$dirty && car.plateNumber.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-show="car.plateNumber.$dirty && car.plateNumber.$invalid"></div>
            </div>
            <div class="row form-group" data-ng-class="{ 'has-error': car.tonnage.$dirty && car.tonnage.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>罐容：</label>
                <input type="number" name="tonnage" class="form-control col-md-4" data-ng-model="model.C_Tonnage" min="45" max="60" required />
                <label class="control-label">（立方米）</label>
                <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-show="car.tonnage.$dirty && car.tonnage.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-show="car.tonnage.$dirty && car.tonnage.$invalid"></div>
                <div class="col-md-offset-3" data-ng-show="car.tonnage.$dirty && car.tonnage.$invalid"><span class="colorRed">罐容正常范围值：45-60（立方米）</span></div>
            </div>
            <div class="row form-group" data-ng-class="{ 'has-error': car.driver.$dirty && car.driver.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>驾驶员：</label>
                <input type="text" name="driver" class="form-control col-md-4" data-ng-model="model.C_Driver" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-show="car.driver.$dirty && car.driver.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-show="car.driver.$dirty && car.driver.$invalid"></div>
            </div>
            <div class="row form-group" data-ng-class="{ 'has-error': car.phone.$dirty && car.phone.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>驾驶员手机：</label>
                <input type="text" name="phone" class="form-control col-md-4" data-ng-model="model.C_Tel" data-ng-pattern="/^1[3|5|7|8|]\d{9}$/" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-show="car.phone.$dirty && car.phone.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-show="car.phone.$dirty && car.phone.$invalid"></div>
            </div>
            <div class="row form-group">
                <label class="col-md-3 control-label">押运员：</label>
                <input type="text" name="driver2" class="form-control col-md-4" data-ng-model="model.C_Driver2" />
                <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-show="car.driver2.$dirty && car.driver2.$valid"></div>
            </div>
            <div class="row form-group" data-ng-class="{ 'has-error': car.phone2.$dirty && car.phone2.$invalid }">
                <label class="col-md-3 control-label">押运员手机：</label>
                <input type="text" name="phone2" class="form-control col-md-4" data-ng-model="model.C_Tel2" data-ng-pattern="/^1[3|5|7|8|]\d{9}$/" />
                <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-show="car.phone2.$dirty && car.phone2.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-show="car.phone2.$dirty && car.phone2.$invalid"></div>
            </div>
<%--            <div class="btn" ng-click="SaveCar()"
                style="margin-left: 186px; margin-top: 10px; height: 36px; width: 100px; color: white; background-color: #016ffc;">
                确认添加
            </div>--%>
            <button name="AddCar" value="AddCar" class="btn" 
                style="margin-left: 186px; margin-top: 10px; height: 36px; width: 100px; color: white; background-color: #016ffc;">
                确认添加
            </button>
        </div>
    </div>
</div>

<script type="text/javascript">
    $(function () {
        function fhad() {
            $('#fahuoaddModal').modal('show');
        }

        function addCar() {

        }
    })
</script>
