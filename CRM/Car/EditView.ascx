<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.TQCar.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<style>
    input::-webkit-outer-spin-button,
    input::-webkit-inner-spin-button {
        -webkit-appearance: none !important;
        margin: 0;
    }

    input[type="number"] {
        -moz-appearance: textfield;
    }
</style>
<div class="tableStyle1" ng-controller="carController" ng-init="InitCar()">
    <div class="titleBar2">
        <div class="txt2" style="color: black; display: inline-block; width: auto; margin-right: 24px;" ng-if="!model.ID">新增车辆信息</div>
        <div class="txt2" style="color: black; display: inline-block; width: auto; margin-right: 24px;" ng-if="model.ID">编辑车辆信息</div>
        <span class="colorRed">*</span><span class="colorGray">为必填项</span>
    </div>
    <div class="borderStyle4" style="margin-top: 0px; margin-bottom: 25px; padding: 35px 20px 15px 1px; border-top: 1px solid #ddd;">
        <div ng-form="car" class="form-horizontal">
            <div class="row form-group" ng-class="{ 'has-error': car.plateNumber.$dirty && car.plateNumber.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>车牌号：</label>
                <input type="text" name="plateNumber" class="form-control col-md-4" ng-model="model.C_PlateNumber" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.plateNumber.$dirty && car.plateNumber.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.plateNumber.$dirty && car.plateNumber.$invalid"></div>
            </div>
            <div class="row form-group" ng-class="{ 'has-error': car.tonnage.$dirty && car.tonnage.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>罐容（立方米）：</label>
                <input type="number" name="tonnage" class="form-control col-md-4" ng-model="model.C_Tonnage" min="45" max="60" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.tonnage.$dirty && car.tonnage.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.tonnage.$dirty && car.tonnage.$invalid"></div>
                <div class="inline" ng-show="car.tonnage.$dirty && car.tonnage.$invalid"><span class="colorRed">罐容正常范围值：45-60（立方米）</span></div>
            </div>
            <div class="row form-group" ng-class="{ 'has-error': car.driver.$dirty && car.driver.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>驾驶员：</label>
                <input type="text" name="driver" class="form-control col-md-4" ng-model="model.C_Driver" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.driver.$dirty && car.driver.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.driver.$dirty && car.driver.$invalid"></div>
            </div>
            <div class="row form-group" ng-class="{ 'has-error': car.phone.$dirty && car.phone.$invalid }">
                <label class="col-md-3 control-label"><span class="colorRed">*</span>驾驶员手机：</label>
                <input type="text" name="phone" class="form-control col-md-4" ng-model="model.C_Tel" ng-pattern="/^1[3|5|7|8|]\d{9}$/" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.phone.$dirty && car.phone.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.phone.$dirty && car.phone.$invalid"></div>
            </div>
            <div class="row form-group">
                <label class="col-md-3 control-label">押运员：</label>
                <input type="text" class="form-control col-md-4" ng-model="model.C_Driver2" />
            </div>
            <div class="row form-group" ng-class="{ 'has-error': car.phone2.$dirty && car.phone2.$invalid }">
                <label class="col-md-3 control-label">押运员手机：</label>
                <input type="text" name="phone2" class="form-control col-md-4" ng-model="model.C_Tel2" ng-pattern="/^1[3|5|7|8|]\d{9}$/" />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.phone2.$dirty && car.phone2.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.phone2.$dirty && car.phone2.$invalid"></div>
            </div>
            <div class="row form-group">
                <div class="col-md-offset-3">
                    <div class="btnBlue_large" ng-click="SaveCar()">保存车辆信息</div>
                </div>
            </div>
        </div>
    </div>
</div>
