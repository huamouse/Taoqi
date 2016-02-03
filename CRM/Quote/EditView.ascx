<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.TQQuote.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<style>
    .form-horizontal .control-label {
        padding-top: 9px;
    }

    .form-control {
        font-size: 13px;
        background: url(/images/select.png) scroll right no-repeat;
    }

    select {
        -moz-appearance: none;
        -webkit-appearance: none;
    }

        select::-ms-expand {
            display: none;
        }
</style>
<div data-ng-controller="clientAddressController">
    <div class="tableStyle1">
        <div class="titleBar2">
            <div class="txt2" style="color: black; display: inline-block; width: auto; margin-right: 24px;">发布求购</div>
            <span class="colorRed">*</span><span class="colorGray">为必填项</span>
            <a href="default.aspx">
                <div class="btnRed_Large" style="float: right;">发布中的求购</div>
            </a>
        </div>
        <div class="borderStyle4" style="margin-top: 5px; margin-bottom: 25px; padding: 35px 20px 15px 1px; border-top: 1px solid #ddd;">
            <div class="form-horizontal">
                <div class="row form-group">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>求购报价有效期：</label>
                    <div class="input-group validityTime col-md-4 date">
                        <input class="form-control" style="background-image:none;" type="text" readonly="readonly" ng-model="C_ValidityTime" data-ng-change="updatePreview()" />
                        <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                    </div>
                </div>
                <div class="row form-group" ng-if="C_ValidityTime">
                    <label class="col-md-4 col-md-offset-3" id="diffTime" style="font-size: 14px;"></label>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>类别：</label>
                    <select class="form-control col-md-4" ng-model="entity.C_GasTypeID" style="padding-right:6px;" id="C_GasTypeID" data-ng-change="updatePreview()"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasType">
                    </select>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3">品种：</label>
                    <select class="form-control col-md-4" style="padding-right:6px;" ng-model="entity.C_GasVarietyID" id="C_GasVarietyID" data-ng-change="updatePreview()"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasVariety">
                        <option value="">不限</option>
                    </select>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3">气化率（Nm³/T）：</label>
                    <select class="form-control col-md-4" ng-model="entity.C_GasificationRateRange" style="padding-right:6px;" id="C_GasificationRateRange" data-ng-change="updatePreview()"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasificationRate">
                        <option value="">不限</option>
                    </select>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3">热值（大卡/立方米）：</label>
                    <select class="form-control col-md-4" ng-model="entity.C_CalorificValueRange" style="padding-right:6px;" id="C_CalorificValueRange" data-ng-change="updatePreview()"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.CalorificValue">
                        <option value="">不限</option>
                    </select>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3">液温：</label>
                    <select class="form-control col-md-4" ng-model="entity.C_LiquidTemperature" style="padding-right:6px;" id="C_LiquidTemperature" data-ng-change="updatePreview()"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.LiquidTemperature">
                        <option value="">不限</option>
                    </select>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3">发票要求：</label>
                    <select class="form-control col-md-4" ng-model="entity.C_InvoiceRequestID" style="padding-right:6px;" id="C_InvoiceRequestID" data-ng-change="updatePreview()"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.InvoiceRequest">
                        <option value="">不限</option>
                    </select>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3">发票类型：</label>
                    <select class="form-control col-md-4" ng-model="entity.C_InvoiceTypeID" style="padding-right:6px;" id="C_InvoiceTypeID" data-ng-change="updatePreview()"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.InvoiceType">
                        <option value="">不限</option>
                    </select>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3">备注：</label>
                    <textarea class="form-control col-md-6" ng-model="C_Remark" data-ng-change="updatePreview()" style="background-image: none"></textarea>
                </div>
                <div class="row form-group" data-ng-init="getClientAddress()">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>到岸地：</label>
                    <div class="table-responsive col-md-8 clearPaddingLeft" data-ng-show="addressLoaded">
                        <table class="table" style="border-top: 1px solid #dcdcdc;">
                            <tbody>
                                <tr data-ng-repeat="item in myClientAddress">
                                    <td style="height: 35px">
                                        <label class="fontNormal" style="font-size: 13px;">
                                            <input type="checkbox" data-ng-change="quoteAddressChange()" name="clientAddress" data-ng-model="item.IsChecked" style="width: 15px; height: 13px;" />
                                            {{item.C_StationShortName}}，{{item.UserTypeName}}，{{item.C_ProvinceName}}{{item.C_CityName}}{{item.C_CountyName}}{{item.C_Address}}，{{item.C_ContactName}}，{{item.C_Tel}}
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div data-ng-show="addressAll" class="btnRed" style="width: 105px; padding: 10px; margin-top: 5px;" onclick="$('#divClientAddress').toggle();">+新增到岸地</div>
                                        <a href="#" data-ng-click="showAll()" data-ng-hide="addressAll">显示全部到岸地</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div data-ng-hide="addressLoaded">
                        <img src="/images/loading.gif" />
                    </div>
                </div>

                <div id="divClientAddress" style="display: none;">
                    <h3 style="color: #545453; margin-left:120px;margin-bottom:-8px;">新增到岸地</h3>
                    <hr />
                    <div class="form-horizontal" ng-form="add">
                        <div class="row form-group">
                            <label class="col-md-3 control-label"><span class="colorRed">*</span>收货地址：</label>
                            <div class="form-control col-md-4 dropdown">
                                <div class="overflow" data-toggle="dropdown" data-target="#" style="padding-right:6px;">
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
                            <input id="address" type="text" name="address" class="form-control col-md-8" ng-model="model.C_Address" maxlength="50" style="width:460px;background-image:none;" required />
                            <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="add.address.$dirty && add.address.$valid"></div>
                            <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="add.address.$dirty && add.address.$invalid"></div>
                        </div>
                        <div class="row form-group" style="margin-top:-0.7em;">
                            <div class="col-md-8 col-md-offset-3">
                                <input type="hidden" id="baiduPosition" ng-model="model.C_BaiduPosition" />
                                <div class="icon icon_location"></div>
                                <button data-toggle="modal" data-target="#modalMap" style="color: red; background-color: transparent; border: none;margin-left:-8px;" onclick="return false;">地图标注</button>
                                <span class="colorGray">请标注准确地址位置，便于物流准确交接。</span>
                            </div>
                        </div>
                        <div class="row form-group" ng-class="{ 'has-error': add.station.$dirty && add.station.$invalid }">
                            <label class="col-md-3 control-label"><span class="colorRed">*</span>场站名称：</label>
                            <input type="text" name="station" class="col-md-4 form-control" ng-model="model.C_StationShortName" style="background-image:none;" required />
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
                            <input type="text" name="contact" class="col-md-4 form-control" ng-model="model.C_ContactName" style="background-image:none;" required />
                            <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="add.contact.$dirty && add.contact.$valid"></div>
                            <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="add.contact.$dirty && add.contact.$invalid"></div>
                        </div>
                        <div class="row form-group" ng-class="{ 'has-error': add.phone.$dirty && add.phone.$invalid }">
                            <label class="col-md-3 control-label"><span class="colorRed">*</span>手机：</label>
                            <input type="text" name="phone" class="form-control col-md-4" ng-model="model.C_Tel" style="background-image:none;" maxlength="20" ng-pattern="/^1[3|5|7|8|]\d{9}$/" required />
                            <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="add.phone.$dirty && add.phone.$valid"></div>
                            <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="add.phone.$dirty && add.phone.$invalid"></div>
                        </div>
                        <div class="row form-group">
                            <label class="col-md-3 control-label">场站总罐容（立方）：</label>
                            <input type="text" class="form-control col-md-4" ng-model="model.C_StationCapacity" style="background-image:none;" maxlength="6" />
                        </div>
                        <div class="row form-group">
                            <label class="col-md-3 control-label">LNG日供气量（公斤）：</label>
                            <input type="text" class="form-control col-md-4" ng-model="model.C_DailyConsumption1" style="background-image:none;" maxlength="6" />
                        </div>
                        <div class="row form-group">
                            <label class="col-md-3 control-label">CNG日供气量（方）：</label>
                            <input type="text" class="form-control col-md-4" ng-model="model.C_DailyConsumption2" style="background-image:none;" maxlength="6" />
                        </div>
                        <div class="row form-group">
                            <div class="col-md-offset-3 col-md-4">
                                <div class="btnBlue_large" style="margin-left:-14px;" ng-click="addClientAddress('Quote')">确认添加</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="tableStyle1 marginTop15px">
            <div class="titleBar2">
                <div class="txt2" style="display: inline-block; width: auto; margin-right: 24px;">求购预览</div>
                <span class="colorGray">（数量：每车默认20吨）</span>
            </div>
            <div class="table-responsive">
                <!-- Table -->
                <table class="table blueTable" id="quotelook">
                    <thead>
                        <tr>
                            <th>有效期</th>
                            <th>求购内容</th>
                            <th>到岸地点</th>
                            <th width="6%">数量（车）</th>
                            <th width="23%">到岸时间</th>
                            <th width="7%">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr ng-repeat="item in ParamList">
                            <td class="shopcart_list_style1">{{item.C_ValidityTime}}
                            </td>
                            <td class="shopcart_list_style1">{{item.GasTypeName}}、{{item.GasVarietyName}}、{{item.GasificationRateRange}}
                                、{{item.CalorificValueRange}}、{{item.LiquidTemperatureName}}、{{item.ClientAddressInfo.UserTypeName}}
                            </td>
                            <td class="textCenter">{{item.ClientAddressInfo.C_ProvinceName}}{{item.ClientAddressInfo.C_CityName}}{{item.ClientAddressInfo.C_CountyName}}<br />
                                {{item.ClientAddressInfo.C_Address}}<br />
                            </td>
                            <td class="textCenter">
                                <div class="input-group">
                                    <div class="form-control middle" ng-bind="item.C_Quantity" style="background-image:none;"></div>
                                    <span class="input-group-btn">
                                        <span class="btn btn-default" ng-click="addQuantity($index)">+</span>
                                    </span>
                                </div>
                            </td>
                            <td class="textCenter">
                                <div class="input-group landTime date" onmouseover="setLandTimeePicker();">
                                    <input class="form-control" type="text" readonly="readonly" ng-model="item.C_ArriveTime" style="background-image:none;"/>
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>
                                </div>
                            </td>
                            <td>
                                <div class="btnRed" style="background-color: transparent; color: #0065e6;" ng-click="ParamListRemove($index)">删除</div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="btnBlue_large" ng-click="addQuote()">确认发布</div>
    </div>
</div>
<script type="text/javascript">
    //设置日期控件
    $('.validityTime').datetimepicker({
        language: 'zh-CN',
        weekStart: 7,
        //todayBtn: 1,
        autoclose: 1,
        todayHighlight: 0,
        minView: 1, //最小选择小时
        forceParse: 0,
        pickerPosition: 'bottom-left',
        format: 'yyyy-mm-dd hh:00',
        initialDate: addDays(1),
        startDate: addDays(1),
        endDate: addDays(7)
    }).on('changeDate', function (ev) {
        var current = new Date();
        var diff = ev.date.getTime() - current.getTime() - 8 * 3600 * 1000;
        var day = Math.floor(diff / (3600 * 1000 * 24));
        var hour = Math.floor((diff - day * 3600 * 1000 * 24) / (3600 * 1000));
        $('#diffTime').text('离现在大约' + day + '天' + hour + '小时');
    });

    function setLandTimeePicker() {
        $('.landTime').datetimepicker({
            language: 'zh-CN',
            weekStart: 7,
            //todayBtn: 1,
            autoclose: 1,
            todayHighlight: 0,
            minView: 1, //最小选择小时
            forceParse: 0,
            pickerPosition: 'bottom-left',
            format: 'yyyy-mm-dd hh:00',
            initialDate: addDays(2),
            startDate: addDays(2)
        });
    }
</script>
