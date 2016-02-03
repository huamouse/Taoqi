<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="MD_EditView.aspx.cs" Inherits="Taoqi.Users.MD_EditView" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        #GasSourceGroup > .form-group{
            float: left;
            margin-left: 30px;
        }
        #MarketData_EditView .BigGroup{
            float: left;
            width: 100%;
        }

        .BigGroup > button{
            width: 100px;
        }
        .control-label{
            padding-top:9px;
        }
        .form-control li a {
            color:grey;
        }
        .form-control li a:hover {
            color:white !important;
            background-color:#ff6400 !important;
        }
    </style>

    <div id="MarketData_EditView">
        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 16px;">增加行情数据</label>
        <div style="border: solid #ddd; padding-top: 40px; float: left; padding: 20px 15px; width: 100%; font-size: small;">
            
            <div id="GasSourceGroup" class="BigGroup">
                <label class="control-label col-md-3">气源地：</label>
                <div class="form-group" style="margin-left: 15px;">
                    <label>类别</label>
                    <select class="searchBar" ng-change="gasChange(true)" style="height:34px;" ng-model="entity.C_GasTypeID"
                            ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasType">
                        <option value="">不限</option>
                    </select>
                </div>
                <div class="form-group">
                    <label>品种</label>
                    <select class="searchBar" ng-change="gasChange(true)" style="height:34px;" ng-model="entity.C_GasVarietyID"
                            ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasVariety">
                        <option value="">不限</option>
                    </select>
                </div>
                <div class="form-group" ng-show="entity.C_GasVarietyID">
                    <label class="overflow" style="float: left; margin-top: 10px;">气源地</label>
                    <div class="form-control col-md-4 dropdown" style="margin-left: 5px; width:200px;">
                        <div class="overflow" data-toggle="dropdown" data-target="#">
                            <span ng-if="entity.DeliveryName" ng-bind="entity.DeliveryName"></span>
                            <span ng-if="!entity.DeliveryName" class="colorGray">请选择气源地</span>
                            <span class="dropDownRight2">
                                <span class="caret"></span>
                            </span>
                        </div>
                        <div data-panel='plants' class="dropdown-menu deliveryPanel" ng-if="entity.C_GasVarietyID != '2'">
                            <ul class="nav nav-tabs">
                                <li ng-repeat="item in regions" ng-class="{active:tabGasSource===$index}"><a ng-click="GetPlants($event, item)" data-toggle="tab" data-target="#">{{item}}</a></li>
                                <%--<li ng-class="{active:tabGasSource === -1}"><a ng-click="plantSelect()"><span class="colorRed">其他</span></a></li>--%>
                            </ul>
                            <ul class="nav nav-pills">
                                <li ng-repeat="item in plants"><a ng-click="plantSelect(item)">{{item.C_GasSourceName}}</a></li>
                            </ul>
                        </div>
                        <div class="dropdown-menu deliveryPanel" ng-if="entity.C_GasVarietyID === '2'">
                            <ul class="nav nav-pills">
                                <li ng-repeat="item in baseData.Wharf"><a ng-click="plantSelect(item)">{{item.C_GasSourceName}}</a></li>
                                <%--<li><a ng-click="plantSelect()"><span class="colorRed">其他</span></a></li>--%>
                            </ul>
                        </div>
                    </div>
                    <input type="hidden" runat="server" class="ID_HDNC_ProductID" id="HDNC_ProductID" />
                </div>
            </div>
            
            <div class="BigGroup" style="margin-top: 30px;">
                <label for="TXT_C_GuidePrice" class="control-label col-md-3">指导价格：</label>
                <div class="col-md-9" style="width: 220px;">
                    <input type="text" id="TXT_C_GuidePrice" class="form-control" runat="server" />
                </div>
            </div>

            <div class="BigGroup" style="margin-left: 27%; margin-top: 30px;">
                <button name="btn_save" value="save" class="btnBlue_large">保存</button>
                <button name="btn_cancel" value="cancel" class="btnBlue_large" style="margin-left: 20px;">返回</button>
                <label id="lbl_error" class="error" runat="server"></label>
            </div>
        </div>
    </div>
</asp:Content>
