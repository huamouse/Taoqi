<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQClientAddress.EditView" %>

<style>
    .form-control {
        font-size:13px;
        background: url(/images/select.png) scroll right no-repeat;
    }
        select {
        -moz-appearance: none;
        -webkit-appearance: none;
    }

        select::-ms-expand {
            display: none;
        }
        .deliveryPanel .nav-pills a:hover {
            background-color:#ff6400;
            color:white;
        }
</style>
<div class="tableStyle1" ng-controller="clientAddressController" ng-init="initClientAddress()">
    <div class="titleBar2">
        <div class="txt2" style="color: black; display: inline-block; width: auto; margin-right: 24px;margin-bottom:0.3em;" ng-if="!model.ID">新增到岸地</div>
        <div class="txt2" style="color: black; display: inline-block; width: auto; margin-right: 24px;margin-bottom:0.3em;" ng-if="model.ID">编辑到岸地</div>
        <span class="colorRed">*</span><span class="colorGray">为必填项</span>
    </div>
    <div class="borderStyle4" style="margin-top: 0px; margin-bottom: 25px; padding: 35px 20px 15px 1px; border-top: 1px solid #ddd;">
        <div class="form-horizontal" ng-form="add">
            <div class="row form-group">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>收货地址：</label>
                <div class="form-control col-md-4 dropdown">
                    <div class="overflow" data-toggle="dropdown" data-target="#">
                        <span ng-if="entity.LandName" ng-bind="entity.LandName"></span>
                        <span ng-if="!entity.LandName" class="colorGray">请选择收货地址</span>
                        <span class="dropDownRight2">
                        </span>
                    </div>
                    <div data-panel='plants' class="dropdown-menu deliveryPanel">
                        <ul class="nav nav-tabs">
                            <li ng-class="{active:tabLand===0}"><a data-toggle="tab" data-target="#" ng-click="getProvinces($event)">省份</a></li>
                            <li ng-class="{active:tabLand===1}"><a data-toggle="tab" data-target="#" ng-click="getCities($event)">城市</a></li>
                            <li ng-class="{active:tabLand===2}"><a data-toggle="tab" data-target="#" ng-click="getCounties($event)">县区</a></li>
                        </ul>
                        <ul class="nav nav-pills">
                            <li ng-repeat="item in baseData.Province" ng-if="tabLand === 0">
                                <a ng-click="provinceSelect($event, item)">{{item.C_ProvinceName}}</a>
                            </li>
                            <li ng-repeat="item in cities" ng-if="tabLand === 1">
                                <a ng-click="citySelect($event, item)">{{item.C_CityName}}</a>
                            </li>
                            <li ng-repeat="item in counties" ng-if="tabLand === 2">
                                <a ng-click="countySelect(item)">{{item.C_CountyName}}</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="row form-group" ng-class="{ 'has-error': add.address.$dirty && add.address.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>详细地址：</label>
                <input id="address" type="text" name="address" class="form-control col-md-8" style="background-image:none;width:460px;" ng-model="model.C_Address" maxlength="50" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="add.address.$dirty && add.address.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="add.address.$dirty && add.address.$invalid"></div>
            </div>
            <div class="row form-group" style="margin-top:-0.7em;">
                <div class="col-md-8 col-md-offset-3">
                    <input type="hidden" id="baiduPosition" value="{{model.C_BaiduPosition}}">
                    <div class="icon icon_location"></div>
                    <button data-toggle="modal" data-target="#modalMap" style="color: red; background-color: transparent; border: none;margin-left:-8px;" onclick="return false;">地图标注</button>
                    <span class="colorGray">请标注准确地址位置，便于物流准确交接。</span>
                </div>
            </div>
            <div class="row form-group" ng-class="{ 'has-error': add.station.$dirty && add.station.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>场站名称：</label>
                <input type="text" name="station" class="col-md-4 form-control" style="background-image:none;" ng-model="model.C_StationShortName" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="add.station.$dirty && add.station.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="add.station.$dirty && add.station.$invalid"></div>
            </div>
            <div class="row form-group">
                <label class="col-md-3 control-label">场站类型：</label>
                <select class="form-control col-md-4" ng-model="model.C_UserType" style="padding-right:6px;" id="C_UserType"
                    ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.UserType">
                    <option value="">不限</option>
                </select>
            </div>
            <div class="row form-group" ng-class="{ 'has-error': add.contact.$dirty && add.contact.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>站内联系人：</label>
                <input type="text" name="contact" class="col-md-4 form-control"  style="background-image:none;" ng-model="model.C_ContactName" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="add.contact.$dirty && add.contact.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="add.contact.$dirty && add.contact.$invalid"></div>
            </div>
            <div class="row form-group" ng-class="{ 'has-error': add.phone.$dirty && add.phone.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>手机：</label>
                <input type="text" name="phone" class="form-control col-md-4" style="background-image:none;" ng-model="model.C_Tel" maxlength="20" ng-pattern="/^1[3|5|7|8|]\d{9}$/" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="add.phone.$dirty && add.phone.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="add.phone.$dirty && add.phone.$invalid"></div>
            </div>
            <div class="row form-group">
                <label class="col-md-3 control-label">场站总罐容（立方）：</label>
                <input type="text" class="form-control col-md-4" style="background-image:none;" ng-model="model.C_StationCapacity" maxlength="6" />
            </div>
            <div class="row form-group">
                <label class="col-md-3 control-label">LNG日供气量（公斤）：</label>
                <input type="text" class="form-control col-md-4" style="background-image:none;" ng-model="model.C_DailyConsumption1" maxlength="6" />
            </div>
            <div class="row form-group">
                <label class="col-md-3 control-label">CNG日供气量（方）：</label>
                <input type="text" class="form-control col-md-4" style="background-image:none;" ng-model="model.C_DailyConsumption2" maxlength="6" />
            </div>
            <div class="row form-group">
                <div class="col-md-offset-3 col-md-4">
                    <div class="btnBlue_large" style="margin-left:-14px;" ng-click="addClientAddress('Quote')">确认保存</div>
                </div>
            </div>
        </div>
    </div>
</div>

