<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.ProductSell.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<style>
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

    input::-webkit-outer-spin-button,
    input::-webkit-inner-spin-button {
        -webkit-appearance: none !important;
        margin: 0;
    }

    input[type="number"] {
        -moz-appearance: textfield;
    }
</style>
<div ng-controller="productController">
    <div class="tableStyle1" ng-form="gas">
        <div class="titleBar2">
            <div class="txt2" style="color: black; display: inline-block; width: auto; margin-right: 24px;">发布气源</div>
            <span class="colorRed">*</span><span class="colorGray">为必填项</span>
            <a href="default.aspx">
                <div class="btnRed_Large" style="float: right;">查看出售中的气源</div>
            </a>
        </div>
        <div class="borderStyle4" style="margin-top: 5px; margin-bottom: 25px; padding: 35px 20px 15px 1px; border-top: 1px solid #ddd;">
            <div class="row form-group">
                <label class="control-label col-md-3"><span class="colorRed">*</span>类别：</label>
                <select class="form-control col-md-4" style="padding-right: 6px;" ng-model="entity.C_GasTypeID" id="C_GasTypeID" data-ng-change="updatePreview()"
                    ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasType">
                </select>
            </div>
            <div class="row form-group" ng-class="{'has-error':gas.variety.$dirty && gas.variety.$invalid}">
                <label class="control-label col-md-3"><span class="colorRed">*</span>品种：</label>
                <select class="form-control col-md-4" style="padding-right: 6px;" ng-model="entity.C_GasVarietyID" id="C_GasVarietyID" name="variety" data-ng-change="updatePreview()"
                    ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.GasVariety" required>
                    <option value="">请选择</option>
                </select>
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="gas.variety.$dirty && gas.variety.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="gas.variety.$dirty && gas.variety.$invalid"></div>
            </div>
            <div class="row form-group" ng-if="entity.C_GasVarietyID" id="gaspublish">
                <label class="control-label col-md-3"><span class="colorRed">*</span>气源地：</label>
                <div class="form-control col-md-4 dropdown" style="padding-right: 6px;">
                    <div class="overflow" data-toggle="dropdown" data-target="#" style="padding-right: 6px;">
                        <span ng-if="entity.DeliveryName" ng-bind="entity.DeliveryName"></span>
                        <span ng-if="!entity.DeliveryName" class="colorGray">请选择气源地</span>
                        <span class="dropDownRight2"></span>
                    </div>
                    <div data-panel='plants' class="dropdown-menu deliveryPanel" ng-if="entity.C_GasVarietyID != '2'">
                        <ul class="nav nav-tabs">
                            <li ng-repeat="item in regions" ng-class="{active:tabGasSource===$index}"><a ng-click="GetPlants($event, item)" data-toggle="tab" data-target="#">{{item}}</a></li>
                            <li ng-class="{active:tabGasSource === -1}"><a ng-click="plantSelect()" style="color:#ff3e0a;">其他</a></li>
                        </ul>
                        <ul class="nav nav-pills">
                            <li ng-repeat="item in plants"><a ng-click="plantSelect(item);updatePreview()">{{item.C_GasSourceName}}</a></li>
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
                <select class="form-control col-md-4" ng-model="entity.C_GasZoneID" style="padding-right: 6px;" id="C_GasZoneID"
                    ng-options="item.C_ZoneID as item.C_ZoneName for item in baseData.Zone">
                    <option value="">请选择</option>
                </select>
            </div>
            <div class="row form-group" ng-if="entity.C_GasVarietyID && entity.DeliveryName === '其他'">
                <label class="control-label col-md-3"><span class="colorRed">*</span>其他气源名称：</label>
                <input class="form-control col-md-4" ng-model="entity.C_GasSourceName" style="background-image: none;" data-ng-change="updatePreview()" />
            </div>
            <div class="row form-group" ng-class="{'has-error':gas.rate.$dirty && gas.rate.$invalid}">
                <label class="control-label col-md-3"><span class="colorRed">*</span>气化率（Nm³/T）：</label>
                <input type="text" name="rate" class="form-control col-md-4" style="background-image: none;" data-ng-change="updatePreview()"
                     ng-model="entity.C_GasificationRate" maxlength="5" ng-pattern="/^\d+$/" required />
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="gas.rate.$dirty && gas.rate.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="gas.rate.$dirty && gas.rate.$invalid"></div>
            </div>
            <div class="row form-group" ng-class="{'has-error':gas.calorific.$dirty && gas.calorific.$invalid}">
                <label class="control-label col-md-3">热值（大卡/立方米）：</label>
                <input type="text" name="calorific" class="form-control col-md-4" style="background-image: none;" data-ng-change="updatePreview()" 
                     ng-model="entity.C_CalorificValue" maxlength="5" ng-pattern="/^\d+$/">
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="gas.calorific.$dirty && gas.calorific.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="gas.calorific.$dirty && gas.calorific.$invalid"></div>
            </div>
            <div class="row form-group" ng-class="{'has-error':gas.temperature.$dirty && gas.temperature.$invalid}">
                <label class="control-label col-md-3"><span class="colorRed">*</span>液温：</label>
                <select class="form-control col-md-4" ng-model="entity.C_LiquidTemperature" style="padding-right: 6px;" data-ng-change="updatePreview()" 
                    id="C_LiquidTemperature" name="temperature" ng-options="item.NAME as item.DISPLAY_NAME for item in baseData.LiquidTemperature" required>
                    <option value="">请选择</option>
                </select>
                <div class="glyphicon glyphicon-ok form-glyphicon" ng-show="gas.temperature.$dirty && gas.temperature.$valid"></div>
                <div class="glyphicon glyphicon-remove form-glyphicon" ng-show="gas.temperature.$dirty && gas.temperature.$invalid"></div>
            </div>
            <div class="row form-group">
                <label class="control-label col-md-3"><span class="colorRed">*</span>经营区域：</label>
                <div class="col-md-8 clearPaddingLeft">
                    <div>
                        <select class="searchBar" ng-change="ProvinceChange()" ng-model="entity.C_ProvinceID" id="C_ProvinceID"
                            ng-options="item.C_ProvinceID as item.C_ProvinceName for item in baseData.Province" style="width: 150px; background: url(/images/select.png) scroll right no-repeat;">
                            <option value="">请选择</option>
                        </select>
                    </div>
                    <table class="table marginTop15px" id="mainArea" style="border-top: 3px solid #cccccc;">
                        <thead>
                            <tr>
                                <th class="col-md-3">市</th>
                                <th class="col-md-5">到岸价（元/吨）</th>
                                <th class="col-md-4">日供（车）</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr ng-repeat="item in mainArea">
                                <td class="textCenter" ng-bind="item.C_CityName"></td>
                                <td class="textCenter has-feedback">
                                    <div class="input-group col-md-11">
                                        <span class="input-group-btn">
                                            <button class="btn btn-default" type="button" ng-click="priceSub($index, true)">-</button>
                                        </span>
                                        <input type="number" name="price{{$index}}" class="form-control textCenter" style="background-image: none;padding-right:0;" ng-model="item.C_Price_Min" maxlength="4"
                                            min="3000" max="6000" ng-change="priceValid($index, true)" autocomplete="off" />
                                        <%--                                        <input type="text" id="price{{$index}}" class="form-control textCenter" ng-model="item.C_Price_Min" maxlength="4" 
                                            ng-blur="priceValid($index)" ng-pattern="/^[3-6]\d{3}$/" autocomplete="off" />--%>
                                        <span class="input-group-btn">
                                            <button class="btn btn-default" type="button" ng-click="priceAdd($index, true)">+ </button>
                                        </span>
                                    </div>
                                    <span class="glyphicon glyphicon-ok form-control-feedback" ng-show="gas.price{{$index}}.$dirty && gas.price{{$index}}.$valid"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback" ng-show="gas.price{{$index}}.$dirty && gas.price{{$index}}.$invalid"></span>
                                </td>
                                <td class="textCenter has-feedback">
                                    <div class="input-group col-md-10">
                                        <span class="input-group-btn">
                                            <button class="btn btn-default" type="button" ng-click="quantitySub($index, true)">-</button>
                                        </span>
                                        <input type="number" name="quantity{{$index}}" class="form-control textCenter" ng-model="item.C_CarQuantity" maxlength="2"
                                            min="0" max="99" ng-change="quantityValid($index, true)" autocomplete="off" style="background-image: none;padding-right:0;" />
                                        <span class="input-group-btn">
                                            <button class="btn btn-default" type="button" ng-click="quantityAdd($index, true)">+ </button>
                                        </span>
                                    </div>
                                    <span class="glyphicon glyphicon-ok form-control-feedback" ng-show="gas.quantity{{$index}}.$dirty && gas.quantity{{$index}}.$valid"></span>
                                    <span class="glyphicon glyphicon-remove form-control-feedback" ng-show="gas.quantity{{$index}}.$dirty && gas.quantity{{$index}}.$invalid"></span>
                                </td>
                            </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div class="tableStyle1 marginTop15px">
        <div class="titleBar2">
            <div class="txt2">气源预览</div>
        </div>
        <div class="table-responsive">
            <!-- Table -->
            <table class="table blueTable">
                <thead>
                    <tr>
                        <th rowspan="2">类型</th>
                        <th rowspan="2">品种</th>
                        <th rowspan="2">气源地</th>
                        <th rowspan="2">气化率</th>
                        <th rowspan="2">热值</th>
                        <th rowspan="2">液温</th>
                        <th colspan="2">主营区域</th>
                        <th rowspan="2">到岸价（元/吨）</th>
                        <th rowspan="2">日供（车）</th>
                        <th rowspan="2">操作</th>
                    </tr>
                    <tr>
                        <th>省</th>
                        <th>市</th>
                    </tr>
                </thead>
                <tbody>
                    <tr ng-repeat="item in ProductAreaList">
                        <td class="textCenter" ng-bind="item.Product.GasTypeName"></td>
                        <td class="textCenter" ng-bind="item.Product.GasVarietyName"></td>
                        <td class="textCenter" ng-bind="item.Product.C_GasSourceName"></td>
                        <td class="textCenter" ng-bind="item.Product.C_GasificationRate"></td>
                        <td class="textCenter" ng-bind="item.Product.C_CalorificValue"></td>
                        <td class="textCenter" ng-bind="item.Product.LiquidTemperatureName"></td>
                        <td class="textCenter" ng-bind="item.C_ProvinceName"></td>
                        <td class="textCenter" ng-bind="item.C_CityName"></td>
                        <td class="textCenter price" ng-bind="item.C_Price_Min"></td>
                        <td class="textCenter" ng-bind="item.C_CarQuantity"></td>
                        <td class="textCenter">
                            <div class="btnRed" ng-click="RemovePreview($index)">删除</div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <div class="btnBlue_large" ng-click="addProductArea()">确认发布</div>
</div>

<%--<script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=wCp9mFd1bab0ygEVLnhdXAY7"></script>

<script>
    var map = new BMap.Map();

    function getDistance(point, address, target,helpTxt) {

        if (!point) {
            console.log("提示：没有坐标点!");
            return;
        }

        var baiduPoint = point.split(',');

        if (baiduPoint.length != 2) {
            console.log("提示：格式不正确!");
            return;
        }

        var pointA = new BMap.Point(baiduPoint[0], baiduPoint[1]);
        //地址解析
        var myGeo = new BMap.Geocoder();

        myGeo.getPoint(address, function (pointB) {
            if (pointB) {
                //获取的原始距离是米，转为公里          
                var spacing = (map.getDistance(pointA, pointB)).toFixed(2) / 1000;
                console.log(spacing + "米");

                var price = 0;

                //各区间报价
                if (spacing<300) {
                    price = spacing * 1;
                }
                else if (spacing >= 300 && spacing < 500) {
                    price = 300 * 2 + (spacing-300) * 1;
                }
                else if (spacing >= 500 && spacing < 1000) {
                    price = 300 * 2 + 200 * 1 + (spacing-500) * 1;
                }
                else if (spacing >= 1000) {
                    price = 300 * 2 + 200 * 1 + 500 * 1 + (spacing - 1000) * 1;
                }

                //取50的倍数
                price = (price/10).toFixed()*10;

                if (price > 6000)
                    price = 6000;
                else if (price < 3000)
                    price = 3000;

                //下方更新angularjs model时，会更新文本框的值
                //$(target).val(price);

                //通知angularjs更新model
                var scope = angular.element($(target)).scope();
                scope.$apply(function () {
                    //更新绑定到文本框的到岸价

                    scope.item.C_Price_Min = price;
                    //console.log(scope.item.C_Price_Min);
                });

                //$(helpTxt).html("");
              
            } else {
                console.log("提示：您选择的地址没有解析到结果!");
                $(helpTxt).html("无法计算");
            }
        }, "");
       
    }

    //getDistance("106.581515,29.615467", "南京市");
</script>--%>
