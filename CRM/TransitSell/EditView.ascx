<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.TransitSell.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<style>
    #purposeland1 a {
        color: #595959;
    }

        #purposeland1 a:hover {
            color: white;
            background-color: #ff6400;
        }

    #purposeland a {
        color: #595959;
    }

        #purposeland a:hover {
            color: white;
            background-color: #ff6400;
        }

    .form-control {
        font-size: 13px;
        background: url(/images/select.png) scroll right no-repeat;
    }

    select {
        appearance: none;
        -ms-appearance: none;
        -moz-appearance: none;
        -webkit-appearance: none;
    }

        select::-ms-expand {
            display: none;
        }
</style>
<div ng-controller="transitController">
    <div class="tableStyle1">
        <div class="titleBar2">
            <div class="txt2" style="color: black; display: inline-block; width: auto; margin-right: 24px;">发布在途气</div>
            <span class="colorRed">*</span><span class="colorGray">为必填项</span>
            <a href="default.aspx">
                <div class="btnRed_Large" style="float: right;">查看出售中的在途气</div>
            </a>
        </div>
        <div class="borderStyle4" style="margin-top: 5px; margin-bottom: 25px; padding: 35px 20px 15px 1px; border-top: 1px solid #ddd;">
            <div class="form-horizontal">
                <div class="row form-group">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>在途气发布有效期：</label>
                    <input class="form-control validityTime col-md-4" type="text" style="padding-right:6px;" readonly="readonly" ng-model="C_ValidityTime" />
                </div>
                <div class="row form-group" ng-if="C_ValidityTime">
                    <label class="col-md-4 col-md-offset-3" id="diffTime" style="font-size: 14px;"></label>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>类别：</label>
                    <select class="form-control col-md-4" ng-change="gasChange()" style="padding-right:6px;" ng-model="entity.C_GasTypeID" id="C_GasTypeID"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasType">
                    </select>
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>品种：</label>
                    <select class="form-control col-md-4" ng-change="gasChange()" style="padding-right:6px;" ng-model="entity.C_GasVarietyID" id="C_GasVarietyID"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasVariety">
                        <option value="">请选择</option>
                    </select>
                </div>
                <div class="row form-group" ng-if="entity.C_GasVarietyID" id="gaspublish">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>气源地：</label>
                    <div class="form-control col-md-4 dropdown">
                        <div class="overflow" data-toggle="dropdown" data-target="#" style="padding-right:6px;">
                            <span ng-if="entity.DeliveryName" ng-bind="entity.DeliveryName"></span>
                            <span ng-if="!entity.DeliveryName" class="colorGray">请选择气源地</span>
                            <span class="dropDownRight2"></span>
                        </div>
                        <div data-panel='plants' class="dropdown-menu deliveryPanel" ng-if="entity.C_GasVarietyID != '2'">
                            <ul class="nav nav-tabs">
                                <li ng-repeat="item in regions" ng-class="{active:tabGasSource===$index}"><a ng-click="GetPlants($event, item)" data-toggle="tab" data-target="#">{{item}}</a></li>
                                <li ng-class="{active:tabGasSource === -1}"><a ng-click="plantSelect()"><span class="colorRed">其他</span></a></li>
                            </ul>
                            <ul class="nav nav-pills">
                                <li ng-repeat="item in plants"><a ng-click="plantSelect(item)">{{item.C_GasSourceName}}</a></li>
                            </ul>
                        </div>
                        <div class="dropdown-menu deliveryPanel" ng-if="entity.C_GasVarietyID === '2'">
                            <ul class="nav nav-pills">
                                <li ng-repeat="item in baseData.Wharf"><a ng-click="plantSelect(item)">{{item.C_GasSourceName}}</a></li>
                                <li><a ng-click="plantSelect()"><span class="colorRed">其他</span></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="row form-group" ng-if="entity.C_GasVarietyID && entity.C_GasVarietyID != '2' && entity.DeliveryName === '其他'">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>其他气源所在区域：</label>
                    <select class="form-control col-md-4" ng-model="entity.C_GasZoneID" id="C_GasZoneID"
                        ng-options="item.C_ZoneID as item.C_ZoneName for item in baseData.Zone">
                        <option value="">请选择</option>
                    </select>
                </div>
                <div class="row form-group" ng-if="entity.C_GasVarietyID && entity.DeliveryName === '其他'">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>其他气源名称：</label>
                    <input class="form-control col-md-4" ng-model="entity.C_GasSourceName" style="background-image: none;" />
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>气化率（Nm³/T）：</label>
                    <input type="text" class="form-control col-md-4" ng-model="entity.C_GasificationRate" maxlength="5" style="background-image: none;" />
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3">热值（大卡/立方米）：</label>
                    <input type="text" class="form-control col-md-4" ng-model="entity.C_CalorificValue" maxlength="5" style="background-image: none;" />
                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>液温：</label>
                    <select class="form-control col-md-4" ng-model="entity.C_LiquidTemperature" style="padding-right:6px;" id="C_LiquidTemperature"
                        ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.LiquidTemperature">
                        <option value="">请选择</option>
                    </select>
                </div>
                <div class="row form-group" ng-controller="carController" data-ng-init="GetCarList()">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>车辆：</label>
                    <div class="table-responsive col-md-7 clearPaddingLeft" data-ng-show="carLoaded">
                        <table class="table" style="border-top:1px solid #ddd;margin-bottom:0;border-collapse:separate;border-radius:4px;">
                            <tbody>
                                <tr data-ng-repeat="item in CarList">
                                    <td height="35" style="border-top:none;">
                                        <label class="fontNormal">
                                            <input type="radio" ng-model="entity.Car" ng-value="item" style="width: 15px; height: 15px; border-color:white;" />
                                            {{item.C_PlateNumber}}，{{item.C_Tonnage | number:2}}，{{item.C_Driver}}/{{item.C_Tel}}，{{item.C_Driver2}}/{{item.C_Tel2}}
                                        </label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-top:none;">
                                        <div data-ng-show="carAll" class="btnRed" data-toggle="modal" data-target="#carModal"
                                            style="width: 85px; padding: 5px; margin-top: 5px;">
                                            +新增车辆
                                        </div>
                                        <a href="#" data-ng-click="showAll()" data-ng-hide="carAll">显示全部车辆</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div data-ng-hide="carLoaded">
                        <img src="/images/loading.gif" />
                    </div>
                    <div class="modal fade" id="carModal" tabindex="-1" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title">添加车辆</h4>
                                </div>
                                <div class="modal-body" ng-form="car">
                                    <div class="row form-group" ng-class="{ 'has-error': car.plateNumber.$dirty && car.plateNumber.$invalid }">
                                        <label class="col-md-3 control-label"><span class="colorRed">*</span>车牌号：</label>
                                        <input type="text" name="plateNumber" class="form-control col-md-4" style="background-image: none;" ng-model="model.C_PlateNumber" required />
                                        <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.plateNumber.$dirty && car.plateNumber.$valid"></div>
                                        <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.plateNumber.$dirty && car.plateNumber.$invalid"></div>
                                    </div>
                                    <div class="row form-group" ng-class="{ 'has-error': car.tonnage.$dirty && car.tonnage.$invalid }">
                                        <label class="col-md-3 control-label"><span class="colorRed">*</span>罐容（立方米）：</label>
                                        <input type="number" name="tonnage" class="form-control col-md-4" style="background-image: none;" ng-model="model.C_Tonnage" min="45" max="60" required />
                                        <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.tonnage.$dirty && car.tonnage.$valid"></div>
                                        <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.tonnage.$dirty && car.tonnage.$invalid"></div>
                                        <div class="inline" ng-show="car.tonnage.$dirty && car.tonnage.$invalid"><span class="colorRed">罐容正常范围值：45-60（立方米）</span></div>
                                    </div>
                                    <div class="row form-group" ng-class="{ 'has-error': car.driver.$dirty && car.driver.$invalid }">
                                        <label class="col-md-3 control-label"><span class="colorRed">*</span>驾驶员：</label>
                                        <input type="text" name="driver" class="form-control col-md-4" style="background-image: none;" ng-model="model.C_Driver" required />
                                        <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.driver.$dirty && car.driver.$valid"></div>
                                        <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.driver.$dirty && car.driver.$invalid"></div>
                                    </div>
                                    <div class="row form-group" ng-class="{ 'has-error': car.phone.$dirty && car.phone.$invalid }">
                                        <label class="col-md-3 control-label"><span class="colorRed">*</span>驾驶员手机：</label>
                                        <input type="text" name="phone" class="form-control col-md-4" style="background-image: none;" ng-model="model.C_Tel" ng-pattern="/^1[3|5|7|8|]\d{9}$/" required />
                                        <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.phone.$dirty && car.phone.$valid"></div>
                                        <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.phone.$dirty && car.phone.$invalid"></div>
                                    </div>
                                    <div class="row form-group">
                                        <label class="col-md-3 control-label">押运员：</label>
                                        <input type="text" class="form-control col-md-4" style="background-image: none;" ng-model="model.C_Driver2" />
                                    </div>
                                    <div class="row form-group" ng-class="{ 'has-error': car.phone2.$dirty && car.phone2.$invalid }">
                                        <label class="col-md-3 control-label">押运员手机：</label>
                                        <input type="text" name="phone2" class="form-control col-md-4" style="background-image: none;" ng-model="model.C_Tel2" ng-pattern="/^1[3|5|7|8|]\d{9}$/" />
                                        <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="car.phone2.$dirty && car.phone2.$valid"></div>
                                        <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="car.phone2.$dirty && car.phone2.$invalid"></div>
                                    </div>
                                </div>
                                <div class="modal-footer" style=" background-color: #f4f4f4;">
                                    <div class="col-md-4" style="margin-left: 10em;">
                                        <div class="btnBlue_large" ng-click="SaveCar()">保存车辆信息</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row form-group">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>装车量（吨）：</label>
                    <input class="form-control col-md-4" ng-model="entity.C_Quantity" style="background-image: none;" />
                </div>
                <div class="row form-group" id="purposeland">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>出发地：</label>
                    <div class="form-control col-md-4 dropdown" style="padding-right:6px;">
                        <div class="overflow" data-toggle="dropdown" data-target="#">
                            <span ng-if="entity.LandName" ng-bind="entity.LandName"></span>
                            <span ng-if="!entity.LandName" class="colorGray">请选择出发地</span>
                            <span class="dropDownRight2"></span>
                        </div>
                        <div data-panel='plants' class="dropdown-menu deliveryPanel">
                            <ul class="nav nav-tabs">
                                <li ng-class="{active:tabLand===0}"><a data-toggle="tab" data-target="#" ng-click="getProvinces($event)">省份</a></li>
                                <li ng-class="{active:tabLand===1}"><a data-toggle="tab" data-target="#" ng-click="getCities($event)">城市</a></li>
                            </ul>
                            <ul class="nav nav-pills">
                                <li ng-repeat="item in baseData.Province" ng-if="tabLand === 0">
                                    <a ng-click="provinceSelect($event, item)">{{item.C_ProvinceName}}</a>
                                </li>
                                <li ng-repeat="item in cities" ng-if="tabLand === 1">
                                    <a ng-click="citySelect2(item)">{{item.C_CityName}}</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="row form-group" id="purposeland1">
                    <label class="control-label col-md-3"><span class="colorRed">*</span>目标地：</label>
                    <div class="form-control col-md-4 dropdown" style="padding-right:6px;">
                        <div class="overflow" data-toggle="dropdown" data-target="#">
                            <span ng-if="entity.LandName2" ng-bind="entity.LandName2"></span>
                            <span ng-if="!entity.LandName2" class="colorGray">请选择目标地</span>
                            <span class="dropDownRight2"></span>
                        </div>
                        <div data-panel='plants' class="dropdown-menu deliveryPanel">
                            <ul class="nav nav-tabs">
                                <li ng-class="{active:tabLand2===0}"><a data-toggle="tab" data-target="#" ng-click="getProvinces($event, true)">省份</a></li>
                                <li ng-class="{active:tabLand2===1}"><a data-toggle="tab" data-target="#" ng-click="getCities($event, true)">城市</a></li>
                            </ul>
                            <ul class="nav nav-pills">
                                <li ng-repeat="item in baseData.Province" ng-if="tabLand2 === 0">
                                    <a ng-click="provinceSelect($event, item, true)">{{item.C_ProvinceName}}</a>
                                </li>
                                <li ng-repeat="item in cities2" ng-if="tabLand2 === 1">
                                    <a ng-click="citySelect2(item, true)">{{item.C_CityName}}</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="clearBoth" style="margin-left: 223px;">
                    <div class="btnBlue_large" ng-click="addTransitPreview()">生成预览</div>
                </div>

            </div>
            <div class="tableStyle1 marginTop15px">
                <div class="titleBar2">
                    <div class="txt2">在途气预览</div>
                </div>
                <div class="table-responsive">
                    <!-- Table -->
                    <table class="table blueTable">
                        <thead>
                            <tr>
                                <th>类型</th>
                                <th>品种</th>
                                <th>气源地</th>
                                <th>气化率</th>
                                <th>热值</th>
                                <th>液温</th>
                                <th>出发地</th>
                                <th>目标地</th>
                                <th>车牌号</th>
                                <th>车辆<br />
                                    吨位（吨）</th>
                                <th>装车量<br />
                                    （吨）
                                </th>
                                <th>联系人/联系方式 </th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="item in TransitList">
                                <td class="textCenter" ng-bind="item.Product.GasTypeName"></td>
                                <td class="textCenter" ng-bind="item.Product.GasVarietyName"></td>
                                <td class="textCenter" ng-bind="item.Product.C_GasSourceName"></td>
                                <td class="textCenter" ng-bind="item.Product.C_GasificationRate"></td>
                                <td class="textCenter" ng-bind="item.Product.C_CalorificValue"></td>
                                <td class="textCenter" ng-bind="item.Product.LiquidTemperatureName"></td>
                                <td class="textCenter">{{item.FromProvinceName}}<br />
                                    {{item.FromCityName}}</td>
                                <td class="textCenter">{{item.TargetProvinceName}}<br />
                                    {{item.TargetCityName}}</td>
                                <td class="textCenter" ng-bind="item.C_PlateNumber"></td>
                                <td class="textCenter" ng-bind="item.C_Tonnage"></td>
                                <td class="textCenter" ng-bind="item.C_Quantity"></td>
                                <td class="textCenter">{{item.C_Driver}}<br />
                                    {{item.C_Tel}}</td>
                                <td class="textCenter">
                                    <div class="btnRed" ng-click="removeTransit($index)">删除</div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

            </div>
            <div class="btnBlue_large" ng-click="addTransit()">确认发布</div>
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
    </script>
