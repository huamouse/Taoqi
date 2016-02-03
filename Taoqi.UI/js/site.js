//定义一个module
var taoqiApp = angular.module("taoqiApp", ['ngCookies', 'ngSanitize']);

//基础数据
var GetBaseDataUrl = "/Member/api/home/GetBaseData";
var GetCityByProvinceUrl = "/Member/api/home/GetCityByProvince/";
var GetCountyByCityUrl = "/Member/api/home/GetCountyByCity/";
var GetUserInfoUrl = "/Member/api/home/UserInfo/";
var GetCompanyUrl = "/Member/api/home/Company/";

//产品--现货
var GetProductListTopUrl = "/Member/api/Product/GetProductListTop";
var GetProductListUrl = "/Member/api/Product/GetProductList";
var GetProductSearchUrl = "/Member/api/Product/Search/";
var AddProductAreaUrl = "/Member/api/Product/AddProductArea";
var ModifyProductPriceUrl = "/Member/api/Product/ModifyPrice";
var GetProdctCityUrl = '/Member/api/Product/GetCity';

//产品--在途气
var GetTransitListTopUrl = "/Member/api/Transit/GetTransitListTop";
var GetTransitListUrl = "/Member/api/Transit/GetTransitList";
var AddTransitUrl = "/Member/api/Transit/AddTransit";
var AddTransitMyUrl = "/Member/api/Transit/AddTransitMy";
var GetProductInTransit_ClientInformationUrl = "/Member/api/Transit/GetClientInformationById/";

//求购
var GetQuoteListTopUrl = "/Member/api/Quote/GetQuoteListTop";
var GetQuoteListUrl = "/Member/api/Quote/GetQuoteList";
var AddQuotePriceUrl = "/Member/api/Quote/AddQuotePrice";
var GetQuotePriceUrl = "/Member/api/Quote/GetQuotePriceByID/";
var RemoveQuotePriceUrl = "/Member/api/Quote/RemoveQuotePrice/"

// 到岸地
var AddClientAddressUrl = "/Member/api/ClientAddress/Add";
var GetClientAddressListUrl = "/Member/api/ClientAddress/GetList/";
var GetClientAddressByIDUrl = "/Member/api/ClientAddress/GetByID/";

//评价
var GetAllEstimateDetailUrl = "/Member/api/Estimate/GetAllEstimateDetail/";

//客户
var Client_GetClientInformation = "/Member/api/Client/GetClient/";
var Client_GetAllEstimateDetailUrl = "/Member/api/Client/Client_GetAllEstimateDetail/";

//行情数据信息
var GetMarketDataUrl = "/Member/api/MarketData/GetMarketDataInfo/";

//交易行情信息
var GetFreeQuotationInformationUrl = "/Member/api/freeQuotation/GetFreeQuotationList";

//交易行情/订单
var GetOrderListTop10Url = "/Member/api/Order/GetOrderListTop10";
var GetOrderListUrl = "/Member/api/Order/GetOrderList";
var GetOrderListTop25Url = "/Member/api/Order/GetOrderListTop25";
var ModifyOrderPriceUrl = "/Member/api/Order/ModifyPrice";
var ModifyOrderQuantityUrl = "/Member/api/Order/ModifyQuantity";
var AddOrderUrl = "/Member/api/Order/AddOrder";

// 车辆
var GetCarListUrl = '/Member/api/Car/GetList/';
var GetCarUrl = '/Member/api/Car/GetByID/';
var AddCarUrl = '/Member/api/Car/Add';
var GetCarInfoUrl = "/Member/api/Car/Info/";
var GetCarPositionUrl = '/Member/api/Car/GetPosition/';

// 购物车
var GetShopCartUrl = "/Member/api/ShopCart/Get/";
var AddShopCartUrl = "/Member/api/ShopCart/Add/";
var RemoveShopCartUrl = "/Member/api/ShopCart/Remove/";

// 消息
var GetUnreadMessageUrl = "/member/api/message/Unread";
var GetAllMessageUrl = "/member/api/message/All";
var ReadMessageUrl = "/member/api/message/Read/";

//资讯
var GetMInfoListUrl = "/Member/api/MarketInfo/GetMInfoList";

// 账户注册及企业认证
var RegisterUrl = "/Member/api/Account/Register";
var GetSmsCodeUrl = "/Member/api/Account/GetSmsCode";
var GetClientUrl = "/Member/api/Account/GetClient/";
var SaveClientUrl = "/Member/api/Account/SaveClient";
var AddClientUrl = "/Member/api/Account/AddClient";

//每页记录数
var PAGESIZE = 15;

// HTML输出
taoqiApp.filter("asHtmlFilter", function ($sce) {
    return function (input) {
        return $sce.trustAsHtml(input);
    };
});

// 企业星级
taoqiApp.filter("rankingFilter", function ($sce) {
    return function (input) {
        var number = Math.round(input);

        if (number == 0) {
            return "";
        }

        var result = "<img src='../images/rank" + number + ".jpg' />";

        return $sce.trustAsHtml(result);
    };
});

// 隐藏名字部分字符
taoqiApp.filter("markFullNameFilter", function ($sce) {
    return function (input) {
        var result = '***';

        if (input != null && input.length > 1) {
            result = input.substring(0, 1) + '**';
        }

        return result;
    };
});

// 剩余多少小时
taoqiApp.filter("diffHourFilter", function ($sce) {
    return function (input) {
        var current = new Date();
        var diffTime = new Date(input);
        var diff = diffTime.getTime() - current.getTime() - 8 * 3600 * 1000;
        var hour = Math.floor(diff / (3600 * 1000));
        
        return hour;
    };
});

// 父类控制器
taoqiApp.controller("baseController", function ($cookies, $scope, $http) {
    $scope.baseLoaded = false;
    $scope.isLoginIn = false;

    //header模板
    $scope.headerHtml = 'header.html';
    //footer模板
    $scope.footerHtml = 'footer.html';

    //$scope.regions = ['热门', '西北', '华北', '华中', '西南', '东北'];
    $scope.regions = ['西北', '华北', '华中', '西南', '东北'];

    $scope.entity = {};
    $scope.entity.C_GasTypeID = '1';    // 气源类型ID

    $scope.entity.C_ProcessOfGasDifference = '1';    // 气差处理
    $scope.entity.C_StandardOfYaChe = '4';    // 押车标准
    $scope.entity.C_TypeOfCarFuel = '1';    // 车辆燃料类型
    //$scope.entity.C_TypeOfPay = '1';    // 付款方式

    $scope.UserInfo = {};

    $scope.orderBy = "";
    $scope.orderDirection = "";
    $scope.currentPageIndex = 1;
    $scope.tabGasSource = 0;    // 气源地Tab
    $scope.tabLand = 0;         // 到岸地Tab
    $scope.tabLand2 = 0;

    $scope.selectChanged_color = function (id) {
        var $target = $("#" + id);
        $target.removeClass("selectInitColor").addClass("selectChangedColor");
    }

    $scope.GetBaseData = function () {
        //气源分类、省份数据、用户信息
        $http.get(GetBaseDataUrl).success(function (data) {
            $scope.baseData = data;
            $scope.Percent = data.Percent;
            $scope.Company = data.Company;
            $cookies.putObject('Company', data.Company, { 'path': '/' });
            $cookies.put('Percent', data.Percent, { 'path': '/' });

            if ($scope.baseData.hasOwnProperty("UserInfo")) {
                $scope.isLoginIn = true;
                $scope.UserInfo = data.UserInfo;
                $cookies.put('isLoginIn', true, { 'path': '/' });
                $cookies.putObject('UserInfo', data.UserInfo, { 'path': '/' });
            } else {
                $scope.isLoginIn = false;
                $scope.UserInfo = {};
                $cookies.put('isLoginIn', false, { 'path': '/' });
                $cookies.putObject('UserInfo', {}, { 'path': '/' });
            }

            $scope.baseLoaded = true;
            $scope.$broadcast('baseLoaded');
            
            $scope.initFreeQuotationSearch();

            //$("[data-panel='plants']").each(function () {
            //    $(this).find("a:first").tab('show');
            //});

            $scope.currentTab = 0;
            $scope.GetPlants(null, $scope.regions[0]);
        });
    };
    
    $scope.InitByCookie = function () {
        $scope.isLoginIn = $cookies.get('isLoginIn');
        $scope.Percent = $cookies.get('Percent');
        if ($cookies.getObject('Company')) $scope.Company = $cookies.getObject('Company');
        if ($cookies.getObject('UserInfo')) $scope.UserInfo = $cookies.getObject('UserInfo');
        if ($cookies.getObject('messageList')) $scope.messageList = $cookies.getObject('messageList');
    }

    $scope.ChangeAccount = function (item) {
        $scope.UserInfo.AccountID = item.ID;
        $scope.UserInfo.CompanyName = item.C_ClientName;
        $scope.getUserInfo();
    }

    $scope.getUserInfo = function () {
        var index = layer.load();
        $http.get(GetUserInfoUrl + $scope.UserInfo.AccountID)
            .success(function (data) {
                layer.close(index);
                $scope.UserInfo = data;
                $cookies.putObject('UserInfo', data, { 'path': '/' });

                $scope.$broadcast('refresh');
            })
             .error(function (data) {
                 layer.close(index);
             });
    }

    $scope.getCompany = function () {
        $http.get(GetCompanyUrl).success(function (data) {
            $scope.Company = data;
            $cookies.putObject('Company', data, { 'path': '/' });
        });
    }

    $scope.CheckLogin = function () {
        if (!$scope.isLoginIn) {
            layer.alert("提示：请先登录。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return false;
        }

        return true;
    }
    
    $scope.headerA_shopcart_step = function () {
        if ($scope.UserInfo.isBuyer != 1) {
            layer.alert("提示：您无权限执行此操作。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            return false;
        }
        else {
            window.location = "/shopcart_step1.html";
        }
    }

    //根据地址栏参数的现货搜索的查询
    $scope.AddressSearch = function () {
        if (window.getQueryString("GasTypeID")) {
            $scope.entity.C_GasTypeID = window.getQueryString("GasTypeID");
        }

        if (window.getQueryString("GasVarietyID")) {
            $scope.entity.C_GasVarietyID = window.getQueryString("GasVarietyID");
        }

        if (window.getQueryString("GasificationRateRange")) {
            $scope.entity.GasificationRateRange = window.getQueryString("GasificationRateRange");
        }

        if (window.getQueryString("ClientID")) {
            $scope.entity.C_ClientID = window.getQueryString("ClientID");
        }

        if (window.getQueryString("ProductID")) {
            $scope.entity.C_ProductID = window.getQueryString("ProductID");
        }

        $scope.$broadcast('search');
    }

    $scope.paging = function (pageIndex) {
        $scope.currentPageIndex = pageIndex;
        $scope.$broadcast('search');
    }
    
    //交易行情的搜索
    $scope.initFreeQuotationSearch = function () {
        if (window.location.pathname == "/freequotation.html") {
            if (window.getQueryString("GasVariety")) {
                for (var i = 0; i < $scope.baseData.GasVariety.length; i++) {
                    if ($scope.baseData.GasVariety[i].NAME == window.getQueryString("GasVariety")) {
                        $scope.entity.C_GasVarietyID = $scope.baseData.GasVariety[i];
                        break;
                    }
                }
            }

            if (window.getQueryString("GasificationRate")) {
                for (var i = 0; i < $scope.baseData.GasificationRate.length; i++) {
                    if ($scope.baseData.GasificationRate[i].NAME == window.getQueryString("GasificationRate")) {
                        $scope.entity.C_GasificationRate = $scope.baseData.GasificationRate[i];
                        break;
                    }
                }
            }

            if (window.getQueryString("ClientType1")) {
                $scope.entity.C_ProductID = window.getQueryString("ClientType1");
            }

            $scope.search();
        }
    }

    $scope.search = function (pageIndex, orderBy, pageTotal) {
        //分页
        if (pageIndex) {
            $scope.currentPageIndex += pageIndex;
        }
        else {
            $scope.currentPageIndex = 1;
        }

        if ($scope.currentPageIndex < 1) {
            $scope.currentPageIndex = 1;
            return;
        }

        if (pageTotal) {
            if ($scope.currentPageIndex > pageTotal) {
                $scope.currentPageIndex = pageTotal;
                return;
            }
        }

        //排序
        if (orderBy) {
            if ($scope.orderBy != orderBy) {
                $scope.orderDirection = "desc";
            }
            else {
                if ($scope.orderDirection == "asc" || $scope.orderDirection == "") {
                    $scope.orderDirection = "desc";
                } else {
                    $scope.orderDirection = "asc";
                }
            }

            $scope.orderBy = orderBy;
        }

        //通知子控制器执行search方法
        $scope.$broadcast('search');
    };

    $scope.gasChange = function(isSearch) {
        $scope.entity.C_ProductID = '';
        $scope.entity.DeliveryName = '';

        if (isSearch) {
            $scope.search();
        }
    }

    $scope.cancelSearch = function()
    {
        $scope.entity = {};
        $scope.entity.C_GasTypeID = '1';    // 气源类型ID

        $scope.$broadcast('search');
    }
    
    $scope.deletedFilter = function (item) {
        return item.Deleted != 1;
    }

    //根据省份绑定气源地
    //$scope.province2_onChange = function () {
    //    var provinceId = $scope.QuotePriceInfo.C_ProvinceID;
    //    //切换省份时，清除工厂选中值
    //    $scope.QuotePriceInfo.C_ProductID = '';
    //    $scope.QuotePriceInfo.C_ProductID_DisplayName = "";

    //    $http.get("/Member/api/home/GetParentClientByProvince/" + provinceId).success(function (data) {
    //        $scope.parentClientList = data;

    //        if ($("#C_ProductID").find("option:selected").val() == "") {
    //            $("#others").removeClass("hide");
    //        }
    //        else {
    //            $("#others").addClass("hide");
    //        }
    //    });
    //}

    //发布气源估算到岸价
    //$scope.calcPrice = function () {
    //    if ($("#C_ProductID").find("option:selected").val() == "") {
    //        $("#others").removeClass("hide");
    //    }
    //    else {
    //        $("#others").addClass("hide");
    //    }

    //    if ($scope.mainArea) {  //有主营区域城市列表
    //        if ($("#C_ProductID").length > 0) {
    //            var parentClientPoint = $("#C_ProductID").find("option:selected").attr("BaiduPosition");

    //            if (parentClientPoint != "") { //有坐标点
    //                for (var i = 0; i < $scope.mainArea.length; i++) {
    //                    window.getDistance(parentClientPoint, $scope.mainArea[i].C_CityName, "#price" + i, "#help" + i);
    //                }
    //            }
    //            else {
    //                layer.alert("提示：该工厂没有坐标信息，无法自动计算到岸价。");
    //            }
    //        }
    //    }

    //    $scope.$broadcast('RB_BringInformation');
    //}

    $scope.GetShopCart = function () {
        $http.get(GetShopCartUrl).success(function (data) {
            $scope.ShopCartList = data;
        });
    }

    $scope.RemoveShopCart = function (id) {
        if (!$scope.CheckLogin()) return;

        layer.confirm('提示：确认删除吗？', {
            btn: ['确认', '取消'],
            area: ['385px', '178px'],
            offset:['195px','500px'],
        }, function (index) {
            layer.close(index);
            for (var i = 0; i < $scope.ShopCartList.length; i++) {
                if ($scope.ShopCartList[i].ID == id) {   // 删除购物车商品
                    $http.get(RemoveShopCartUrl + id).success(function (data) {
                        if (data == "OK") {
                            $scope.GetShopCart();
                        }
                        else {
                            layer.alert(data);
                        }
                    });
                    break;
                }
            }
        });
    }

    $scope.Settlement = function (productAreaID) {
        if (!$scope.CheckLogin()) return;

        if ($scope.ShopCartList) {
            window.location = "shopcart_step2.html?id=" + productAreaID;
        }
    }
        
    var currentDate = new Date();
    var currentWeek = currentDate.getDay();
    var weeks = new Array("周日", "周一", "周二", "周三", "周四", "周五", "周六");

    $scope.currentDay = (currentDate.getMonth() + 1) + "/" + currentDate.getDate() + " " + weeks[currentWeek];

    $scope.btnAssistantClick = function () {
        var currenthr = new Date();
        currenthr1 = currenthr.getHours();
        if (currenthr1 < 6 || currenthr1 > 22) {
            layer.alert("抱歉：现在不在工作时间范围之内！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            return false;
        }

        if (!$scope.CheckLogin()) return;

        if ($scope.entity.C_CityID &&
            $scope.entity.C_GasVarietyID &&
            $scope.entity.GasificationRateRange &&
            $scope.entity.C_UserType) {
            var data = {
                C_CityID: $scope.entity.C_CityID,
                C_GasVarietyID: $scope.entity.GasVarietyNAME,
                C_GasificationRateRange: $scope.entity.GasificationRateRange,
                C_UserType: $scope.entity.C_UserType
            };

            $http.post("/Member/api/Home/AddAssistantInfo",
               data).success(function (data) {
                   if (data == "OK") {
                       layer.alert("提示：您的求购信息已经记录到系统，我们会尽快联系您！", {
                           area: ['385px', '178px'],
                           offset: ['195px', '500px'],
                       });
                   }
                   else {
                       layer.alert(data);
                   }
               });
        }
        else {
            layer.alert("提示：到岸地点、气源品种、气化率、用户类型都要选择。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
        }
    };

    //搜索
    $scope.btnSearch_click = function () {
        if ($scope.entity.keyword) {
            window.location = "product.html?keyword=" + encodeURI($scope.entity.keyword);
        }
    }
    
    $scope.showWin = function () {
        if (!$scope.CheckLogin()) return;

        $('#myModal').modal();
    }

    $scope.GetPlants = function ($event, target) {
        if ($event) {
            $event.stopPropagation();
            $scope.tabGasSource = this.$index;
        }

        if ($scope.baseData.Plant[target]) {
            $scope.plants = $scope.baseData.Plant[target];
        }
    }

    $scope.plantSelect = function (item) {
        if (item) {
            $scope.entity.C_ProductID = item.ID;
            $scope.entity.DeliveryName = item.C_GasSourceName;
            $scope.entity.C_GasSourceName = item.C_GasSourceName;
            $scope.entity.C_GasificationRate = item.C_GasificationRate;
            $scope.entity.C_CalorificValue = item.C_CalorificValue;
            $scope.entity.C_LiquidTemperature = item.C_LiquidTemperature.toString();
            $scope.entity.C_GasZoneID = item.C_GasZoneID;
        }
        else {
            $scope.entity.C_ProductID = '';
            $scope.entity.DeliveryName = '其他';
            $scope.entity.C_GasSourceName = '';
            $scope.entity.C_GasificationRate = '';
            $scope.entity.C_CalorificValue = '';
            $scope.entity.C_LiquidTemperature = '';
            $scope.entity.C_GasZoneID = 0;

            $scope.tabGasSource = -1;
            $scope.plants = '';
        }
        
        var isSearch = arguments[1] ? !arguments[1] : $scope.isSearch;
        if (isSearch) {
            $scope.search();
        }

        if (window.location.pathname.toLowerCase().indexOf('/member/marketdata/md_editview.aspx') != -1) {
            $("input.ID_HDNC_ProductID").attr("value", $scope.entity.C_ProductID);
        }
    }

    $scope.provinceSelect = function ($event, item, isLandTo) {
        if (isLandTo) {
            $scope.entity.C_ProvinceID2 = item.C_ProvinceID;
            $scope.entity.ProvinceName2 = item.C_ProvinceName;
            $scope.entity.C_CityID2 = '';
            $scope.entity.C_CountyID2 = '';

            $scope.entity.LandName2 = $scope.entity.ProvinceName2 + ' - ';
        } else {
            $scope.entity.C_ProvinceID = item.C_ProvinceID;
            $scope.entity.ProvinceName = item.C_ProvinceName;
            $scope.entity.C_CityID = '';
            $scope.entity.C_CountyID = '';

            $scope.entity.LandName = $scope.entity.ProvinceName + ' - ';
        }
        
        if (window.location.pathname.toLowerCase().indexOf('/member/users/ClientInfo.aspx') != -1) {
            $("input.ID_HDNProvinceID").attr("value", item.C_ProvinceID); 
        }

        $scope.getCities($event, isLandTo);
    }

    $scope.citySelect = function ($event, item, isLandTo) {
        if (isLandTo) {
            $scope.entity.C_CityID2 = item.C_CityID;
            $scope.entity.CityName2 = item.C_CityName;
            $scope.entity.C_CountyID2 = '';

            $scope.entity.LandName2 = $scope.entity.ProvinceName2 + ' - ' + $scope.entity.CityName2 + '-';
        } else {
            $scope.entity.C_CityID = item.C_CityID;
            $scope.entity.CityName = item.C_CityName;
            $scope.entity.C_CountyID = '';

            $scope.entity.LandName = $scope.entity.ProvinceName + ' - ' + $scope.entity.CityName + '-';
        }

        if (window.location.pathname.toLowerCase().indexOf('/member/users/ClientInfo.aspx') != -1) {
            $("input.ID_HDNCityID").attr("value", item.C_CityID);
        }

        $scope.getCounties($event, isLandTo);
    }

    $scope.citySelect2 = function (item, isLandTo) {
        if (isLandTo) {
            $scope.entity.C_CityID2 = item.C_CityID;
            $scope.entity.CityName2 = item.C_CityName;
            $scope.entity.LandName2 = $scope.entity.ProvinceName2 + ' - ' + item.C_CityName;
        } else {
            $scope.entity.C_CityID = item.C_CityID;
            $scope.entity.CityName = item.C_CityName;
            $scope.entity.LandName = $scope.entity.ProvinceName + ' - ' + item.C_CityName;
        }
        

        if ($scope.isSearch) {
            $scope.search();
        }
    }

    $scope.countySelect = function (item, isLandTo) {
        if (isLandTo) {
            $scope.entity.C_CountyID2 = item.C_CountyID;
            $scope.entity.C_CountyName2 = item.C_CountyName;
            $scope.entity.LandName2 = $scope.entity.ProvinceName2 + ' - ' + $scope.entity.CityName2 + ' - ' + item.C_CountyName;
        } else {
            $scope.entity.C_CountyID = item.C_CountyID;
            $scope.entity.C_CountyName = item.C_CountyName;
            $scope.entity.LandName = $scope.entity.ProvinceName + ' - ' + $scope.entity.CityName + ' - ' + item.C_CountyName;
        }

        if ($scope.isSearch) {
            $scope.search();
        }

        if (window.location.pathname.toLowerCase().indexOf('/member/users/ClientInfo.aspx') != -1) {
            $("input.ID_HDNCountyID").attr("value", item.C_CountyID);
        }
    }

    $scope.getProvinces = function ($event, isLandTo) {
        $event.stopPropagation();

        if (isLandTo) {
            $scope.tabLand2 = 0;
        } else {
            $scope.tabLand = 0;
        }
    }

    $scope.getCities = function ($event, isLandTo) {
        $event.stopPropagation();

        var provinceID = isLandTo ? $scope.entity.C_ProvinceID2 : $scope.entity.C_ProvinceID;
        if (provinceID == null || provinceID == '') {
            layer.alert("请先选择省份！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            return false;
        }

        $http.get(GetCityByProvinceUrl + provinceID).success(function (data) {
            if (isLandTo) {
                $scope.cities2 = data;
                $scope.tabLand2 = 1;
            } else {
                $scope.cities = data;
                $scope.tabLand = 1;
            }

            if ($scope.isSearch) $scope.search();
        });
    }

    $scope.getCounties = function ($event, isLandTo) {
        $event.stopPropagation();

        var cityID = isLandTo ? $scope.entity.C_CityID2 : $scope.entity.C_CityID;
        if (cityID == null || cityID == '') {
            layer.alert("请先选择城市！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            return false;
        }

        $http.get(GetCountyByCityUrl + cityID).success(function (data) {
            if (isLandTo) {
                $scope.counties2 = data;
                $scope.tabLand2 = 2;
            } else {
                $scope.counties = data;
                $scope.tabLand = 2;
            }

            if ($scope.isSearch) $scope.search();
        })
    }

    $scope.entrustLogistics = function () {
        if (!$scope.CheckLogin()) return;

        if ($scope.entity.DeliveryName === '') {
            layer.alert("提示", "请先选择提货地！", "error", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
        } else if ($scope.entity.LandName === '') {
            layer.alert("提示", "请先选择提货地！", "error", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
        } else {
            layer.alert("提示：您的委托信息已经记录到系统，我们会尽快联系您！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
        }
    }

    $scope.$on('refresh', function (e) {
        $scope.GetShopCart();

        if (location.pathname.toLowerCase().indexOf('/member/') != -1) {
            if (location.href.toLowerCase().indexOf('clientinfo.aspx') == -1)
                location.reload();
        }
    });
});

// 产品
taoqiApp.controller("productController", function ($scope, $http) {
    $scope.$on('search', function (e) {
        $scope.GetProductSellList();
    });
    
    $scope.$on('refresh', function (e) {
        //$scope.GetProductSellList();
    });

    // 根据省份绑定城市（目标地）
    $scope.ProvinceChange = function () {
        var provinceId = $scope.entity.C_ProvinceID;

        $http.get(GetCityByProvinceUrl + provinceId).success(function (data) {
            $scope.mainArea = data;

            for (var i = 0; i < $scope.mainArea.length; i++) {
                for (var j = 0; j < $scope.ProductAreaList.length; j++) {
                    if ($scope.mainArea[i].C_CityID == $scope.ProductAreaList[j].C_CityID) {
                        $scope.mainArea[i].C_Price_Min = $scope.ProductAreaList[j].C_Price_Min;
                        $scope.mainArea[i].C_CarQuantity = $scope.ProductAreaList[j].C_CarQuantity;
                        break;
                    }
                }
            }
            //for (var j = $scope.ProductAreaList.length - 1; j >= 0; j--) {
            //    if ($scope.ProductAreaList[j].C_ProvinceName == $("#C_ProvinceID").find("option:selected").text()) {
            //        $scope.removeProductArea(j);
            //    }
            //}
        });
    }

    // 现货
    $scope.GetProductListTop = function () {
        $http.get(GetProductListTopUrl).success(function (data) {
            $scope.ProductList = data;
        });
    };

    $scope.GetProductSellList = function (id) {
        var keyword = window.getQueryString("keyword");
        if (keyword) {
            var config = {
                params: {
                    keyword: keyword
                }
            };

            $http.get(GetProductSearchUrl, config).success(function (data) {
                $scope.ProductSellList = data;
            });
            return;
        }

        var config = {
            params: {
                id: id,
                C_ProvinceID: $scope.entity.C_ProvinceID,
                C_CityID: $scope.entity.C_CityID,
                C_ProductID: $scope.entity.C_ProductID,
                C_GasTypeID: $scope.entity.C_GasTypeID,
                C_GasVarietyID: $scope.entity.C_GasVarietyID,
                C_GasSourceName: $scope.entity.C_GasSourceName,
                GasificationRateRange: $scope.entity.GasificationRateRange,
                CalorificValueRange: $scope.entity.CalorificValueRange,
                C_LiquidTemperature: $scope.entity.C_LiquidTemperature,
                SellerClientID: $scope.entity.C_ClientID,
                pageIndex: $scope.currentPageIndex,
                orderBy: $scope.orderBy,
                orderDirection: $scope.orderDirection
            }
        };

        $scope.ProductSellList = [];
        $("#loading").show();
        $http.get(GetProductListUrl, config)
            .success(function (data) {
                $scope.ProductSellList = data.items;

                //分页
                $scope.pageTotal = Math.ceil(data.total / PAGESIZE);
                $scope.pageList = [];

                var j = 0;
                for (var pageNumber = $scope.currentPageIndex - 2; pageNumber <= $scope.pageTotal; pageNumber++) {

                    if (pageNumber < 1) {
                        continue;
                    }

                    if (pageNumber > $scope.pageTotal) {
                        continue;
                    }

                    if (j >= 5) {
                        break;
                    }

                    $scope.pageList.push(pageNumber);
                    j++;
                }

                $("#loading").hide();
            })
            .error(function (data) {
                ("#loading").hide();
            });
            
    }

    // 购物车
    $scope.AddShopCart = function (index) {
        if (!$scope.CheckLogin()) return;
        if ($scope.ProductSellList == null) return false;

        if ($scope.UserInfo.isBuyer != 1) {
            layer.alert("提示：您无权限执行此操作。", {             
                area: ['385px', '178px'],
                offset:['195px','500px'],
            });
            return;
        }

        // 检测是否已添加
        var hasAdd = false;
        for (var i = 0; i < $scope.ShopCartList.length; i++) {
            if ($scope.ShopCartList[i].C_ProductAreaID === $scope.ProductSellList[index].ID) {
                hasAdd = true;
                break;
            }
        }

        if (!hasAdd) {
            $http.get(AddShopCartUrl + $scope.ProductSellList[index].ID).success(function (data) {
                if (data == "OK") {
                    $scope.GetShopCart();
                    $('#modalShopCart').modal('show');
                }
                else {
                    layer.alert(data);
                }
            });
        }
        else
            layer.alert("提示：该商品已在购物车中。",{
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
    }

    $scope.ProductAreaList = [];

    $scope.addProductPreview = function (index) {
        if (!$scope.productValid()) {
            return false;
        }

        var Product = {};
        Product.C_GasTypeID = $scope.entity.C_GasTypeID;
        Product.GasTypeName = $("#C_GasTypeID").find("option:selected").text();
        Product.C_GasVarietyID = $scope.entity.C_GasVarietyID;
        Product.GasVarietyName = $("#C_GasVarietyID").find("option:selected").text();
        Product.C_GasSourceName = $scope.entity.C_GasSourceName;
        Product.C_GasificationRate = $scope.entity.C_GasificationRate;
        Product.C_CalorificValue = $scope.entity.C_CalorificValue;
        Product.C_LiquidTemperature = $scope.entity.C_LiquidTemperature;
        Product.LiquidTemperatureName = $("#C_LiquidTemperature").find("option:selected").text();
        Product.C_GasZoneID = $scope.entity.C_GasZoneID;

        for (var j = $scope.ProductAreaList.length - 1; j >= 0; j--) {
            if ($scope.ProductAreaList[j].C_ProvinceName == $("#C_ProvinceID").find("option:selected").text()) {
                $scope.removeProductArea(j);
            }
        }

        for (var i = 0; i < $scope.mainArea.length; i++) {
            if (!$scope.mainArea[i].C_Price_Min || !$scope.mainArea[i].C_CarQuantity
                || $scope.mainArea[i].C_Price_Min == '' || $scope.mainArea[i].C_CarQuantity == '0') {
                continue;
            }

            var ProductArea = {
                Product: Product,
                C_ProvinceID: $scope.entity.C_ProvinceID,
                C_ProvinceName: $("#C_ProvinceID").find("option:selected").text(),
                C_CityID: $scope.mainArea[i].C_CityID,
                C_CityName: $scope.mainArea[i].C_CityName,
                C_Price_Min: $scope.mainArea[i].C_Price_Min,
                C_CarQuantity: $scope.mainArea[i].C_CarQuantity
            };

            $scope.ProductAreaList.push(ProductArea);
        }
    };

    $scope.addProductArea = function () {
        if ($scope.ProductAreaList.length == 0) {
            layer.alert("提示：请添加气源。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }

        var config = {
            params: {}
        };

        var index = layer.load();
        $http.post(AddProductAreaUrl, $scope.ProductAreaList, config)
            .success(function (data) {
                layer.close(index);
                if (data == "OK") {
                    layer.alert('操作成功！',{
                        type: 0,
                        area: ['385px', '178px'],
                        offset: ['195px', '500px']
                    },
                    function () {
                        window.location.href = "/Member/ProductSell/";
                    });
                }
            })
            .error(function (data) {
                layer.close(index);
            });
    }

    $scope.removeProductArea = function (index) {
        if ($scope.ProductAreaList && $scope.ProductAreaList.length > 0) {
            $scope.ProductAreaList.splice(index, 1);
        }
    }

    $scope.updatePreview = function () {
        for (var i = 0; i < $scope.ProductAreaList.length; i++) {
            var Product = {};
            Product.C_GasTypeID = $scope.entity.C_GasTypeID;
            Product.GasTypeName = $("#C_GasTypeID").find("option:selected").text();
            Product.C_GasVarietyID = $scope.entity.C_GasVarietyID;
            Product.GasVarietyName = $("#C_GasVarietyID").find("option:selected").text();
            Product.C_GasSourceName = $scope.entity.C_GasSourceName;
            Product.C_GasificationRate = $scope.entity.C_GasificationRate;
            Product.C_CalorificValue = $scope.entity.C_CalorificValue;
            Product.C_LiquidTemperature = $scope.entity.C_LiquidTemperature;
            Product.LiquidTemperatureName = $("#C_LiquidTemperature").find("option:selected").text();
            Product.C_GasZoneID = $scope.entity.C_GasZoneID;

            $scope.ProductAreaList[i].Product = Product;
            $scope.ProductAreaList[i].C_ProvinceID = $scope.entity.C_ProvinceID;
            $scope.ProductAreaList[i].C_ProvinceName = $("#C_ProvinceID").find("option:selected").text();
            $scope.ProductAreaList[i].C_CityID = $scope.mainArea[i].C_CityID;
            $scope.ProductAreaList[i].C_CityName = $scope.mainArea[i].C_CityName;
            $scope.ProductAreaList[i].C_Price_Min = $scope.mainArea[i].C_Price_Min;
            $scope.ProductAreaList[i].C_CarQuantity = $scope.mainArea[i].C_CarQuantity;
        }
    }

    $scope.RemovePreview = function (index) {
        if ($scope.ProductAreaList[index].C_ProvinceID == $scope.entity.C_ProvinceID) {
            for (var i = 0; i < $scope.mainArea.length; i++) {
                if ($scope.mainArea[i].C_CityID == $scope.ProductAreaList[index].C_CityID) {
                    $scope.mainArea[i].C_CarQuantity = 0;
                    break;
                }
            }
        }

        $scope.removeProductArea(index);
    }

    $scope.productValid = function () {
        if ($scope.gas.variety.$invalid || $scope.gas.rate.$invalid || $scope.gas.calorific.$invalid || $scope.gas.temperature.$invalid) {
            $scope.gas.variety.$dirty = true;
            $scope.gas.rate.$dirty = true;
            $scope.gas.calorific.$dirty = true;
            $scope.gas.temperature.$dirty = true;

            return false;
        }

        return true;
    };

    $scope.priceSub = function (index, isPreview) {
        var price = parseInt($scope.mainArea[index].C_Price_Min)
        if (!isNaN(price) && price >= 3010) {
            $scope.mainArea[index].C_Price_Min = price - 10;
        }

        if (isPreview) {
            $scope.priceValid(index, isPreview);
        }
    };

    $scope.priceAdd = function (index, isPreview) {
        var price = parseInt($scope.mainArea[index].C_Price_Min)
        if (!isNaN(price) && price <= 5990) {
            $scope.mainArea[index].C_Price_Min = price + 10;
        }

        if (isPreview) {
            $scope.priceValid(index, isPreview);
        }
    }

    $scope.priceValid = function (index, isPreview) {
        for (var i = 0; i < $scope.mainArea.length; i++)
        {
            if ($scope.mainArea[index].C_Price_Min && $scope.mainArea[i].C_Price_Min == null) {
                $scope.mainArea[i].C_Price_Min = $scope.mainArea[index].C_Price_Min;
            }
        }

        if (isPreview) {
            $scope.addProductPreview();
        }
    }

    $scope.quantitySub = function (index, isPreview) {
        var quantity = parseInt($scope.mainArea[index].C_CarQuantity)
        if (quantity > 0) {
            $scope.mainArea[index].C_CarQuantity = quantity - 1;
        }

        if (isPreview) {
            $scope.quantityValid(index, isPreview);
        }
    };

    $scope.quantityAdd = function (index, isPreview) {
        var quantity = parseInt($scope.mainArea[index].C_CarQuantity)
        if (quantity < 99) {
            $scope.mainArea[index].C_CarQuantity = quantity + 1;
        }

        if (isPreview) {
            $scope.quantityValid(index, isPreview);
        }
    }

    $scope.quantityValid = function (index, isPreview) {
        for (var i = 0; i < $scope.mainArea.length; i++) {
            if ($scope.mainArea[index].C_CarQuantity && $scope.mainArea[i].C_CarQuantity == null) {
                $scope.mainArea[i].C_CarQuantity = $scope.mainArea[index].C_CarQuantity;
            }
        }

        if (isPreview) {
            $scope.addProductPreview();
        }
    }

    $scope.modifyModal = function (mode) {
        var config = {
            params: {
                mode: mode
            }
        };

        $http.post(ModifyProductPriceUrl, $scope.mainArea, config).success(function (data) {
            layer.alert("操作成功！",{
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            },
            function () {
                window.location.reload();
            });
        });
    }

    $scope.detailModal = function (productID, provinceID) {
        var config = {
            params: {
                'productID': productID,
                'provinceID': provinceID
            }
        };

        var index = layer.load();
        $http.get(GetCityByProvinceUrl + provinceID)
            .success(function (data) {
                $scope.productCityList = data;
                $http.get(GetProdctCityUrl, config)
                    .success(function (data) {
                        layer.close(index);
                        $scope.mainArea = data;
                        for (var i = 0; i < $scope.mainArea.length; i++) {
                            $scope.removeProductCity($scope.mainArea[i].C_CityID);
                        }
                        $('#detailModal').modal('show');
                    })
                    .error(function (data) {
                        layer.close(index);
                    });
            })
            .error(function (data) {
                layer.close(index);
            });
        
        return false;
    }

    $scope.removeDetail = function (index) {
        if ($scope.mainArea && $scope.mainArea.length > 0) {
            if ($scope.mainArea[index] && $scope.mainArea[index].ID) {
                var productCity = {};
                productCity = angular.copy($scope.mainArea[index]);
                $scope.mainArea[index].Deleted = 1;
                $scope.productCityList.push(productCity);
            }
            else
                $scope.mainArea.splice(index, 1);
        }
    }

    $scope.AddProductCity = function () {
        if ($scope.ProductCity == null || $scope.ProductCity == '') {
            layer.alert("提示：请先选择需添加的城市。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }

        var productArea = {};
        if ($scope.ProductCity.Deleted == null || $scope.ProductCity.Deleted == '') { // 标识未删除城市
            productArea = angular.copy($scope.mainArea[0]);
            productArea.C_CityID = $scope.ProductCity.C_CityID;
            productArea.C_CityName = $scope.ProductCity.C_CityName;
            productArea.ID = '';
            productArea.DATE_MODIFIED = '';
        } else { // 标识已删除城市
            for (var i = 0; i < $scope.mainArea.length; i++) {
                if ($scope.mainArea[i].C_CityID == $scope.ProductCity.C_CityID) {
                    $scope.mainArea.splice(i, 1);
                }
                productArea = angular.copy($scope.ProductCity);
                break;
            }
        }
        productArea.Deleted = 0;
        $scope.removeProductCity(productArea.C_CityID);
        $scope.mainArea.push(productArea);
    }

    $scope.removeProductCity = function (cityID) {
        for (var j = 0; j < $scope.productCityList.length; j++) {
            if (cityID == $scope.productCityList[j].C_CityID) {
                $scope.productCityList.splice(j, 1);
                break;
            }
        }
    }
});

// 在途气
taoqiApp.controller("transitController", function ($scope, $http) {
    $scope.TransitList = [];

    $scope.$on('search', function (e) {
        $scope.GetTransitList();
    });

    $scope.$on('refresh', function (e) {
        //$scope.GetTransitList();
    });
    
    //在途气
    $scope.GetTransitListTop = function () {
        $http.get(GetTransitListTopUrl).success(function (data) {
            $scope.TransitTopList = data;
        });
    };

    $scope.GetTransitList = function (id) {
        var config = {
            params: {
                id: id,
                TargetProvinceId: $scope.entity.C_ProvinceID,
                TargetCityId: $scope.entity.C_CityID,
                C_GasTypeID: $scope.entity.C_GasTypeID,
                C_GasVarietyID: $scope.entity.C_GasVarietyID,
                C_ProductID: $scope.entity.C_ProductID,
                GasificationRateRange: $scope.entity.GasificationRateRange,
                CalorificValueRange: $scope.entity.CalorificValueRange,
                C_LiquidTemperature: $scope.entity.C_LiquidTemperature,
                pageIndex: $scope.currentPageIndex,
                orderBy: $scope.orderBy,
                orderDirection: $scope.orderDirection
            }
        };

        $scope.TransitSellList = [];
        $("#loading").show();
        $http.get(GetTransitListUrl, config).success(function (data) {
            $scope.transitSellList = data.items;

            //分页
            $scope.pageTotal = Math.ceil(data.total / PAGESIZE);
            $scope.pageList = [];

            var j = 0;
            for (var pageNumber = $scope.currentPageIndex - 2; pageNumber <= $scope.pageTotal; pageNumber++) {

                if (pageNumber < 1) {
                    continue;
                }

                if (pageNumber > $scope.pageTotal) {
                    continue;
                }

                if (j >= 5) {
                    break;
                }

                $scope.pageList.push(pageNumber);
                j++;
            }

            $("#loading").hide();
        });
    }
    
    //收藏我的在途气
    $scope.addTransitMy = function (index) {
        if (!$scope.CheckLogin()) return;

        if ($scope.UserInfo.isBuyer != 1) {
            layer.alert("提示：您无权限执行此操作。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }

        if ($scope.transitSellList) {
            var config = {
                params: {
                    C_TransitID: $scope.transitSellList[index].ID
                }
            };

            $http.post(AddTransitMyUrl, {}, config).success(function (data) {
                if (data == "OK") {
                    layer.confirm('已抢购成功！现在就去查看抢购单吗！', {
                        btn: ['是，现在就去', '不， 以后再说！'], //按钮
                        area: ['385px', '178px'],
                        offset: ['195px', '500px'],
                    }, function () {
                        window.location.href = "/member/Transit/";
                    }, function () {
                        $scope.GetTransitList();
                    });
               }
               else {
                   layer.alert(data);
               }
           });
        }
    }

    $scope.addTransitPreview = function () {
        if ($scope.C_ValidityTime == null || $scope.C_ValidityTime == '') {
            layer.alert("提示：请先选择有效期。",
                {
                    area: ['385px', '178px'],
                    offset: ['195px', '500px']
                });
        }else if ($scope.entity.C_GasVarietyID == null || $scope.entity.C_GasVarietyID == '') {
            layer.alert("提示：请先选择气源品种。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if ($scope.entity.C_GasSourceName == null || $scope.entity.C_GasSourceName == '') {
            layer.alert("提示：请先选择气源地。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if ($scope.entity.C_GasificationRate == null || $scope.entity.C_GasificationRate == '') {
            layer.alert("提示：请先输入气化率。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if (!parseInt($scope.entity.C_GasificationRate)) {
            layer.alert("提示：气化率应该是整数。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if ($scope.entity.C_LiquidTemperature == null || $scope.entity.C_LiquidTemperature == '') {
            layer.alert('提示：请先选择液温。', {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if ($scope.entity.C_GasSourceName.length > 7) {
            layer.alert('提示：气源地名称请勿超过七个字。', {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if ($scope.entity.Car.ID == null || $scope.entity.Car.ID == '') {  // 在途气字段验证
            layer.alert("提示：请选择车辆。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if ($scope.entity.C_Quantity == null || $scope.entity.C_Quantity == '') {
            layer.alert("提示：请输入装车量。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if ($scope.entity.C_CityID == null || $scope.entity.C_CityID == '') {
            layer.alert("提示：请选择出发地。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else if ($scope.entity.C_CityID2 == null || $scope.entity.C_CityID2 == '') {
            layer.alert("提示：请选择目标地。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        }
        else { 
            var Product = {};
            Product.C_GasTypeID = $scope.entity.C_GasTypeID;
            Product.GasTypeName = $("#C_GasTypeID").find("option:selected").text();
            Product.C_GasVarietyID = $scope.entity.C_GasVarietyID;
            Product.GasVarietyName = $("#C_GasVarietyID").find("option:selected").text();
            Product.C_GasSourceName = $scope.entity.C_GasSourceName;
            Product.C_GasificationRate = $scope.entity.C_GasificationRate;
            Product.C_CalorificValue = $scope.entity.C_CalorificValue;
            Product.C_LiquidTemperature = $scope.entity.C_LiquidTemperature;
            Product.LiquidTemperatureName = $("#C_LiquidTemperature").find("option:selected").text();
            var Transit = {
                Product: Product,
                C_CarID: $scope.entity.Car.ID,
                C_PlateNumber: $scope.entity.Car.C_PlateNumber,
                C_Tonnage: $scope.entity.Car.C_Tonnage,
                C_Driver: $scope.entity.Car.C_Driver,
                C_Tel: $scope.entity.Car.C_Tel,

                C_Quantity: $scope.entity.C_Quantity,
                C_FromCityID: $scope.entity.C_CityID,
                C_TargetCityID: $scope.entity.C_CityID2,
                FromProvinceName: $scope.entity.ProvinceName,
                FromCityName: $scope.entity.CityName,
                TargetProvinceName: $scope.entity.ProvinceName2,
                TargetCityName: $scope.entity.CityName2,

                C_ValidityTime: $scope.C_ValidityTime
            };

            $scope.TransitList = [];
            $scope.TransitList.push(Transit);
        }
    }

    $scope.removeTransit = function (index) {
        if ($scope.TransitList && $scope.TransitList.length > 0) {
            $scope.TransitList.splice(index, 1);
        }
    }

    $scope.addTransit = function () {
        if ($scope.TransitList.length == 0) {
            layer.alert("提示：请添加在途气。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }

        var config = {
            params: {
            }
        };

        $http.post(AddTransitUrl, $scope.TransitList[0], config).success(function (data) {
            if (data == "OK") {
                layer.alert("提交成功，请耐心等待管理员审核。", {
                    area: ['385px', '178px'],
                    offset: ['195px', '500px']
                }, 
                function (index) {
                    layer.close(index);
                    window.location.href = "/Member/TransitSell/";
                });
            }
            else {
                layer.alert(data);
            }
        });
    }

    $scope.viewLanding = function (url, driver, tel) {
        if (url == null || url == "") {
            layer.alert("此车LNG水单尚未上传，请联系司机：" + driver + "，" + tel, {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });

            return false;
        }

        layer.open({
            type: 1,
            skin: 'layui-layer-nobg', //没有背景色
            shadeClose: true,
            content: "<img src='/member/Data/Upload/Images/" + url + "' />",
            area: ['auto', 'auto'],
            offset: ['5%', '20%'],
            closeBtn: [0, true]
        });
    }

    $scope.modifyModal = function (gid, isPub) {
        //layer.confirm('请输入新价格：', {
        //    btn: ['重要','奇葩'], //按钮
        //    title:[,true]
        //    }, function(){
        //        layer.msg('的确很重要', {icon: 1});
        //    }, function(){
        //        layer.msg('也可以这样', {
        //            time: 20000, //20s后自动关闭
        //            btn: ['明白了', '知道了']
        //        });
        //    });


    //        title: isPub ? "调价并重新发布" : "价格更正",
    //        text: "请输入新价格：",
    //        type: "input",
    //        showCancelButton: true,
    //        closeOnConfirm: false,
    //        animation: "slide-from-top",
    //        cancelButtonText: "取消",
    //        confirmButtonText: isPub ? "调价" : "更正",
    //        showLoaderOnConfirm: true,
    //        area: ['385px', '178px'],
    //        offset: ['195px', '500px'],
    //    }, function (inputValue) {
    //        var price = window.Number(inputValue);
    //        if (window.isNaN(price) || price > 6000 || price < 3000) {
    //            layer.alert.showInputError("价格范围：（3000-6000）", {
    //                area: ['385px', '178px'],
    //                offset: ['195px', '500px'],
    //            });
    //            return false;
    //        }

        //    var config = {
        //        params: {
        //            id: gid,
        //            price: price,
        //            mode: isPub ? 1 : 0
        //        }
        //    };

        //    $http.get(ModifyProductPriceUrl, config).success(function (data) {
        //        layer.alert("操作成功！",{
        //            type: 0,
        //            area: ['385px', '178px'],
        //            offset: ['195px', '500px'],
        //        },
        //        function () {
        //            window.location.reload();
        //        });
        //    });
        //});
    }
});

// 求购
taoqiApp.controller("quoteController", function ($scope, $http) {

    $scope.InitData = function () {
        $scope.model = {};

        $scope.model.C_ProcessOfGasDifference = $scope.entity.C_ProcessOfGasDifference;    // 气差处理
        $scope.model.C_StandardOfYaChe = $scope.entity.C_StandardOfYaChe;    // 押车标准
        $scope.model.C_TypeOfCarFuel = $scope.entity.C_TypeOfCarFuel;    // 车辆燃料类型

        $scope.model.C_Tonnage = '';

        $scope.model.AddOrEdit = '+ 添加';
        $scope.model.AddOrEdit_index = -1;
    }

    $scope.$on('search', function (e) {
        $scope.getQuoteList();
    });
    
    $scope.$on('refresh', function (e) {
        $scope.getQuoteList();
    });

    //报价弹出窗1
    $scope.QuotePriceList = [];
    $scope.InitData();
    
    $scope.GetPlants = function ($event, target) {
        if ($event) {
            $event.stopPropagation();
            $scope.tabGasSource = this.$index;
        }

        if ($scope.baseData.Plant[target]) {
            $scope.plants = $scope.baseData.Plant[target];
        }
    }

    $scope.plantSelect2 = function (item) {
        if (item) {
            $scope.model.C_ProductID = item.ID;
            $scope.model.DeliveryName = item.C_GasSourceName;
            $scope.model.C_GasSourceName = item.C_GasSourceName;
            $scope.model.C_GasificationRate = item.C_GasificationRate;
            $scope.model.C_CalorificValue = item.C_CalorificValue;
            $scope.model.C_LiquidTemperature = item.C_LiquidTemperature.toString();;
            $scope.model.C_GasZoneID = item.C_GasZoneID;
        }
        else {
            $scope.model.C_ProductID = '';
            $scope.model.DeliveryName = '其他';
            $scope.model.C_GasSourceName = '';
            $scope.model.C_GasificationRate = '';
            $scope.model.C_CalorificValue = '';
            $scope.model.C_LiquidTemperature = '';
            $scope.model.C_GasZoneID = 0;

            $scope.tabGasSource = -1;
            $scope.plants = '';
        }
    }
    
    $scope.addQuotePrice = function (index) {
        $scope.ChooseIndex = index;
        if (!$scope.CheckLogin()) return;
        
        if ($scope.UserInfo.isSeller != 1) {
            layer.alert("提示：您无权限执行此操作。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }

        $scope.C_QuoteID = $scope.QuoteList[index].ID;
        $scope.model.C_GasVarietyID = $scope.QuoteList[index].C_GasVarietyID.toString();
        $scope.QuotePriceList = [];
        $("#quoteModal").modal();
    }

    $scope.editQuotePrice = function (index) {
        $scope.ChooseIndex = index;
        if (!$scope.CheckLogin()) return;

        $scope.C_QuoteID = $scope.QuoteList[index].ID;
        $scope.model.C_GasVarietyID = $scope.QuoteList[index].C_GasVarietyID.toString();
        
        $http.get(GetQuotePriceUrl + $scope.C_QuoteID).success(function (data) {
            $scope.QuotePriceList = data.quotePrice;
            
            $.each($scope.QuotePriceList, function (i, n) {
                $scope.QuotePriceList[i].C_LiquidTemperature_Name = $scope.QuotePriceList[i].LiquidTemperatureName;
            //    if ($scope.QuotePriceList[i].C_TypeOfPay != 0)
            //        $scope.QuotePriceList[i].C_TypeOfPay_Name = $scope.baseData.TypeOfPay[$scope.QuotePriceList[i].C_TypeOfPay].DISPLAY_NAME;

            //    if ($scope.QuotePriceList[i].C_TypeOfCarFuel != 0)
            //        $scope.QuotePriceList[i].C_TypeOfCarFuel_Name = $scope.baseData.TypeOfCarFuel[$scope.QuotePriceList[i].C_TypeOfCarFuel].DISPLAY_NAME;

            //    if ($scope.QuotePriceList[i].C_StandardOfYaChe != 0)
            //        $scope.QuotePriceList[i].C_StandardOfYaChe_Name = $scope.baseData.StandardOfYaChe[$scope.QuotePriceList[i].C_StandardOfYaChe].DISPLAY_NAME;

            //    if ($scope.QuotePriceList[i].C_ProcessOfGasDifference != 0)
            //        $scope.QuotePriceList[i].C_ProcessOfGasDifference_Name = $scope.baseData.ProcessOfGasDifference[$scope.QuotePriceList[i].C_ProcessOfGasDifference].DISPLAY_NAME;

            //    if ($scope.QuotePriceList[i].C_LiquidTemperature != 0)
            //        $scope.QuotePriceList[i].C_LiquidTemperature_Name = $scope.baseData.LiquidTemperature[$scope.QuotePriceList[i].C_LiquidTemperature].DISPLAY_NAME;
            });
            
            $("#quoteModal").modal();
        });
    }

    $scope.addQuotePriceToList = function () {
        var quotePrice = {};
        quotePrice.ID = $scope.model.ID;
        quotePrice.C_ProductID = $scope.model.C_ProductID;
        quotePrice.C_GasSourceName = $scope.model.C_GasSourceName;
        quotePrice.C_Price = $scope.model.C_Price;
        quotePrice.DeliveryName = $scope.model.DeliveryName;

        quotePrice.C_Tonnage = $scope.model.C_Tonnage;
        
        quotePrice.C_GasTypeID = $scope.QuoteList[$scope.ChooseIndex].C_GasTypeID;
        quotePrice.C_GasVarietyID   =   $scope.QuoteList[$scope.ChooseIndex].C_GasVarietyID;
        quotePrice.C_GasificationRate = $scope.model.C_GasificationRate;
        quotePrice.C_CalorificValue = $scope.model.C_CalorificValue;
        quotePrice.C_LiquidTemperature = $scope.model.C_LiquidTemperature;
        quotePrice.C_GasZoneID = $scope.model.C_GasZoneID;


        //数组是从0开始计数的，而基础数据表里的基础数据时从1开始计数的 by RouBai
        quotePrice.C_TypeOfPay = $scope.model.C_TypeOfPay;
        quotePrice.C_TypeOfCarFuel = $scope.model.C_TypeOfCarFuel;
        quotePrice.C_StandardOfYaChe = $scope.model.C_StandardOfYaChe;
        quotePrice.C_ProcessOfGasDifference = $scope.model.C_ProcessOfGasDifference;

        if (quotePrice.C_TypeOfPay == null || quotePrice.C_TypeOfCarFuel == null || quotePrice.C_StandardOfYaChe == null || quotePrice.C_ProcessOfGasDifference == null || quotePrice.C_LiquidTemperature == null) {
            layer.alert("提示：请将信息填写完整。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }

        
        quotePrice.C_LiquidTemperature_Name = $scope.baseData.LiquidTemperature[quotePrice.C_LiquidTemperature - 1].DISPLAY_NAME;

        quotePrice.C_TypeOfPay_Name = $scope.baseData.TypeOfPay[quotePrice.C_TypeOfPay - 1].DISPLAY_NAME;
        quotePrice.C_TypeOfCarFuel_Name = $scope.baseData.TypeOfCarFuel[quotePrice.C_TypeOfCarFuel - 1].DISPLAY_NAME;
        quotePrice.C_StandardOfYaChe_Name = $scope.baseData.StandardOfYaChe[quotePrice.C_StandardOfYaChe - 1].DISPLAY_NAME;
        quotePrice.C_ProcessOfGasDifference_Name = $scope.baseData.ProcessOfGasDifference[quotePrice.C_ProcessOfGasDifference - 1].DISPLAY_NAME;
        

        if ((quotePrice.C_ProductID == null || quotePrice.C_ProductID == '') && (quotePrice.C_GasSourceName == '' || quotePrice.C_GasSourceName == null || quotePrice.C_GasZoneID == '')) {
            layer.alert("提示：请正确输入气源工厂信息。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }

        if (quotePrice.C_Price == null || quotePrice.C_Price == '') {
            layer.alert("提示：请输入有效报价,报价范围：<br/>3000-6000！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }
        
        if (quotePrice.C_Tonnage == '' || quotePrice.C_Tonnage < 45 || quotePrice.C_Tonnage > 60) {
            layer.alert("提示：请输入有效车辆罐容,车辆罐容范围：<br/>45-60！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return;
        }
        
        for (var i = 0; i < $scope.QuotePriceList.length; i++) {
            if ($scope.QuotePriceList[i].C_ProductID == quotePrice.C_ProductID && $scope.model.AddOrEdit_index == -1) {
                $scope.QuotePriceList[i].Deleted = 0;
                $scope.QuotePriceList[i].C_Price = $scope.model.C_Price;
                return;
            }
        }
        
        if ($scope.model.AddOrEdit_index == -1)
            $scope.QuotePriceList.push(quotePrice);
        else
            $scope.QuotePriceList[$scope.model.AddOrEdit_index] = quotePrice;
            
        $scope.InitData();
    }

    $scope.removeQuotePrice = function (index) {
        if ($scope.QuotePriceList && $scope.QuotePriceList.length > 0) {
            if ($scope.QuotePriceList[index] && $scope.QuotePriceList[index].ID) {
                $scope.QuotePriceList[index].Deleted = 1;
            }
            else 
                $scope.QuotePriceList.splice(index, 1);
        }
    }

    $scope.editQuote = function (index) {
        if ($scope.QuotePriceList[index]) {
            $scope.model.AddOrEdit = '+ 保存';
            $scope.model.AddOrEdit_index = index;
            
            $scope.model.ID = $scope.QuotePriceList[index].ID;
            $scope.model.C_ProductID = $scope.QuotePriceList[index].C_ProductID;
            $scope.model.C_GasSourceName = $scope.QuotePriceList[index].C_GasSourceName;
            $scope.model.C_Price = $scope.QuotePriceList[index].C_Price;
            $scope.model.DeliveryName = $scope.QuotePriceList[index].C_GasSourceName;

            $scope.model.C_Tonnage = $scope.QuotePriceList[index].C_Tonnage;
            
            $scope.model.C_GasificationRate = $scope.QuotePriceList[index].C_GasificationRate;
            $scope.model.C_CalorificValue = $scope.QuotePriceList[index].C_CalorificValue;
            $scope.model.C_LiquidTemperature = $scope.QuotePriceList[index].C_LiquidTemperature.toString();
            
            $scope.model.C_TypeOfPay = $scope.QuotePriceList[index].C_TypeOfPay.toString();
            $scope.model.C_TypeOfCarFuel = $scope.QuotePriceList[index].C_TypeOfCarFuel.toString();
            $scope.model.C_StandardOfYaChe = $scope.QuotePriceList[index].C_StandardOfYaChe.toString();
            $scope.model.C_ProcessOfGasDifference = $scope.QuotePriceList[index].C_ProcessOfGasDifference.toString();

        }
    }
    
    $scope.saveQuotePrice = function () {
        var config = {
            params: {
                quoteID: $scope.C_QuoteID
            }
        }

        if ($scope.QuotePriceList.length > 0) {
            $http.post(AddQuotePriceUrl, $scope.QuotePriceList, config).success(function (data) {
                if (data == "OK") {
                    layer.alert("操作成功！",{
                        area: ['385px', '178px'],
                        offset: ['195px', '500px']
                    },
                    function (index) {
                        layer.close(index);
                        // 不同页面访问不同数据源
                        if (window.location.pathname.toLowerCase().indexOf('/quote.html') != -1)
                            $scope.getQuoteList();
                        else
                            $scope.getQuoteListTop();
                    });
                }
            });
        }

        //关闭悬浮窗
        $("#quoteModal").modal("hide");
        $scope.InitData();
    }
    
    $scope.getQuoteListTop = function () {
        $http.get(GetQuoteListTopUrl).success(function (data) {
            $scope.QuoteList = data;
        });
    };

    $scope.getQuoteList = function (id) {
        var config = {
            params: {
                id: id,
                C_ProvinceID: $scope.entity.C_ProvinceID,
                C_CityID: $scope.entity.C_CityID,
                C_CountyID: $scope.entity.C_CountyID,

                C_GasTypeID: $scope.entity.C_GasTypeID,
                C_GasVarietyID: $scope.entity.C_GasVarietyID,
                GasificationRateRange: $scope.entity.GasificationRateRange,
                CalorificValueRange: $scope.entity.CalorificValueRange,
                C_LiquidTemperature: $scope.entity.C_LiquidTemperature,
                C_UserType: $scope.UserType ? $scope.UserType.NAME : "",

                pageIndex: $scope.currentPageIndex,
                orderBy: $scope.orderBy,
                orderDirection: $scope.orderDirection
            }
        };

        $("#loading").show();
        $http.get(GetQuoteListUrl, config).success(function (data) {
            $scope.QuoteList = data.items;

            //分页
            $scope.pageTotal = Math.ceil(data.total / PAGESIZE);
            $scope.pageList = [];

            var j = 0;
            for (var pageNumber = $scope.currentPageIndex - 2; pageNumber <= $scope.pageTotal; pageNumber++) {

                if (pageNumber < 1) {
                    continue;
                }

                if (pageNumber > $scope.pageTotal) {
                    continue;
                }

                if (j >= 5) {
                    break;
                }

                $scope.pageList.push(pageNumber);
                j++;
            }

            $("#loading").hide();
        });
    }
});

// 订单
taoqiApp.controller("orderController", function ($scope, $http) {
    $scope.GetMarketData = function () {
        $http.get(GetMarketDataUrl).success(function (data) {
            $scope.MarketData = data;

            $scope.marketLength = 0;
            for (var item in data) {
                $scope.marketLength++;
            }
        });
    };

    $scope.GetClientInformationMyJqueryInit = function () {
        $(function () {
            $("#carousel-marketData > div.carousel-inner > div.item:first").addClass("active");
            $("#carousel-marketData > .carousel-inner > .item").each(function () {
                $(this).find("table:first tr.tr_detail").removeClass("disabled");
                $(this).find("table:first tr.tr_short").addClass("disabled");

                //    $(this).find("tr.tr_detail").each(function () {
                //        if ($(this).find("div.left > div.greenFont2 > span:eq(0)").text() != "{{item2.SUBTRACT}}")
                //        {
                //            $(this).find("div.left > div.greenFont2 > span:eq(1)").addClass("carousel-marketData_greenFont2-span2");
                //        }

                //        if ($(this).find("div.left > div.silverFont > span:eq(0)").text() != "{{item2.C_Category_Name}}")
                //        {
                //            $(this).find("div.left > div.silverFont > span:eq(1)").addClass("carousel-marketData_silverFont-span2");
                //        }

                //    });
            });

        });
    };

    $scope.GetOrderListTop10 = function () {
        $http.get(GetOrderListTop10Url).success(function (data) {
            $scope.orderList = data;
        });
    };

    $scope.GetOrderListTop25 = function () {
        $http.get(GetOrderListTop25Url).success(function (data) {
            $scope.orderList = data;
        });
    };

    $scope.GetOrderList = function () {
        $http.get(GetOrderListUrl).success(function (data) {
            $scope.orderList = data;
        });
    }

    $scope.GetDemo = function () {
        var items = [];

        for (var i = 0; i < 15; i++) {
            var item = {
                c1: "进口气",
                c2: "1000-1050",
                c3: "1000-1500",
                c4: "黄冈",
                c5: "2240",
                c6: "5%",
                c7: "icon_rise"
            };
            items.push(item);
        }

        $scope.demoList = items;
    }

    $scope.modifyModal = function (gid) {
        layer.prompt({
            title:['请输入修改价格'],
            area: ['385px', '178px'],
            offset: ['195px', '500px']
        }, function (val) {
            var price = window.Number(val);
            if (window.isNaN(price) || price > 6000 || price < 3000) {
                layer.alert("价格范围：（3000-6000）", {
                    area: ['385px', '178px'],
                    offset: ['195px', '500px']
                });
                return false;
            }

            var config = {
                params: {
                    id: gid,
                    price: price
                }
            };
            var index = layer.load();
            $http.get(ModifyOrderPriceUrl, config)
                .success(function (data) {
                    layer.close(index);
                    layer.alert("操作成功！", {
                        area: ['385px', '178px'],
                        offset: ['195px', '500px']
                    }, function (index) {
                        layer.close(index);
                        window.location.reload();
                    });
                })
                .error(function (data) {
                    layer.close(index);
                });
        });
    }

    $scope.deliverModal = function (gid) {
        layer.prompt({
            title: ['请输入实际到货量（单位：吨）'],
            area: ['385px', '178px'],
            offset: ['195px', '500px']
        }, function (val) {
            var quantity = window.Number(val);
            var config = {
                params: {
                    id: gid,
                    quantity: quantity
                }
            };
            var index = layer.load();
            $http.get(ModifyOrderQuantityUrl, config)
                .success(function (data) {
                    layer.close(index);
                    layer.alert("操作成功！",{
                        area: ['385px', '178px'],
                        offset: ['195px', '500px']
                    }, function (index) {
                        layer.close(index);
                        window.location.reload();
                    });
                })
                .error(function (data) {
                    layer.close(index);
                });
        });
    }

    $scope.viewManifest = function (url, driver, tel) {
        if (url == null || url == "") {
            layer.alert("此车LNG货单尚未上传，请联系司机：" + driver + "，" + tel, {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });

            return false;
        }

        layer.open({
            type:1,
            skin: 'layui-layer-nobg', //没有背景色
            shadeClose: true,
            content: "<img src='/member/Data/Upload/Images/" + url + "' />",
            area: ['auto', 'auto'],
            offset: ['5%', '20%'],
            closeBtn: [0, true]
        });
    }

    $scope.viewLanding = function (url, driver, tel) {
        if (url == null || url == "") {
            layer.alert("此车LNG水单尚未上传，请联系司机：" + driver + "，" + tel, {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });

            return false;
        }

        layer.open({
            type: 1,
            skin: 'layui-layer-nobg', //没有背景色
            shadeClose: true,
            content: "<img src='/member/Data/Upload/Images/" + url + "' />",
            area: ['auto', 'auto'],
            offset: ['5%', '20%'],
            closeBtn: [0, true]
        });
    }
});

// 交易行情
taoqiApp.controller("freeQuotationController", function ($scope, $http) {
    $scope.$on('search', function (e) {
        if ($scope.freeQuotationList) {
            $scope.GetFreeQuotationList();
        }
    });

    $scope.GetFreeQuotationList = function () {
        var config = {
            params: {
                C_GasTypeID: $scope.entity.C_GasTypeID,
                C_GasVarietyID: $scope.entity.C_GasVarietyID ? $scope.entity.C_GasVarietyID.NAME : "",
                GasificationRateRange: $scope.entity.C_GasificationRate ? $scope.entity.C_GasificationRate.NAME : "",
                C_ProductID: $scope.entity.C_ProductID,
                pageIndex: $scope.currentPageIndex,
                orderBy: $scope.orderBy,
                orderDirection: $scope.orderDirection
            }
        };


        $scope.freeQuotationList = [];
        $("#loading").show();
        $http.get(GetFreeQuotationInformationUrl, config).success(function (data) {
            $scope.freeQuotationList = data.items;

            //分页
            $scope.pageTotal = Math.ceil(data.total / PAGESIZE);
            $scope.pageList = [];

            var j = 0;
            for (var pageNumber = $scope.currentPageIndex - 2; pageNumber <= $scope.pageTotal; pageNumber++) {

                if (pageNumber < 1) {
                    continue;
                }

                if (pageNumber > $scope.pageTotal) {
                    continue;
                }

                if (j >= 5) {
                    break;
                }

                $scope.pageList.push(pageNumber);
                j++;
            }

            $("#loading").hide();
        });

    };
});

// 客户
taoqiApp.controller("ClientController", function ($scope, $http) {
    $scope.model = {};
    $scope.model.C_Estimate_1 = '0.00%';
    $scope.model.C_Estimate_2 = '0.00%';
    $scope.model.C_Estimate_3 = '0.00%';
    $scope.model.C_Estimate_4 = '0.00%';
    $scope.model.C_Estimate_5 = '0.00%';

    $scope.model.C_Estimate = 0;

    $scope.DataInit = function () {
        
        var config = {
            params: {
                id: window.getQueryString("id")
            }
        };

        $("#loading1").show();
        $http.get(Client_GetClientInformation, config).success(function (data) {
            $scope.model = data[0];

            $scope.model.C_Estimate_1 = data[0]["C_EstimateLogisticsAvg_1"];
            $scope.model.C_Estimate_2 = data[0]["C_EstimateLogisticsAvg_2"];
            $scope.model.C_Estimate_3 = data[0]["C_EstimateLogisticsAvg_3"];
            $scope.model.C_Estimate_4 = data[0]["C_EstimateLogisticsAvg_4"];
            $scope.model.C_Estimate_5 = data[0]["C_EstimateLogisticsAvg_5"];
            
            $scope.model.C_Estimate = data[0]["C_EstimateLogisticsAvg"];
            $scope.model.C_Estimate_Count = data[0]["C_EstimateLogisticsCount"];

            $("#loading1").hide();

            $("#clienteva_C_EstimateLogistics").addClass("RB_ClientGray");
        });
    };

    $scope.changeSelect = function (name) {
        $scope.model.C_Estimate_1 = $scope.model[name + "Avg_1"];
        $scope.model.C_Estimate_2 = $scope.model[name + "Avg_2"];
        $scope.model.C_Estimate_3 = $scope.model[name + "Avg_3"];
        $scope.model.C_Estimate_4 = $scope.model[name + "Avg_4"];
        $scope.model.C_Estimate_5 = $scope.model[name + "Avg_5"];

        $scope.model.C_Estimate = $scope.model[name + "Avg"];
        $scope.model.C_Estimate_Count = $scope.model[name + "Count"];
        
        $("#clienteva_C_EstimateLogistics").removeClass("RB_ClientGray");
        if (name == "C_EstimateLogistics")
            $("#clienteva_C_EstimateLogistics").addClass("RB_ClientGray");
    };

    $scope.$on('search', function (e) {
        if ($scope.AllEstimateList) {
            $scope.GetOrderEstimateList();
        }
    });

    $scope.GetOrderEstimateList = function () {
        var config = {
            params: {
                clientId: window.getQueryString("id"),
                pageIndex: $scope.currentPageIndex,
            }
        };


        $scope.AllEstimateList = [];
        $("#loading2").show();
        $("#Div_GetToData").hide();
        
        $http.get(Client_GetAllEstimateDetailUrl, config).success(function (data) {
            $scope.AllEstimateList = data.items;

            //总数据
            $scope.dataSum = data.total;

            //每页显示的数量
            $scope.pageNum = 2;

            //分页
            $scope.pageTotal = Math.ceil(data.total / $scope.pageNum);
            $scope.pageList = [];

            var j = 0;
            for (var pageNumber = $scope.currentPageIndex - 2; pageNumber <= $scope.pageTotal; pageNumber++) {

                if (pageNumber < 1) {
                    continue;
                }

                if (pageNumber > $scope.pageTotal) {
                    continue;
                }

                if (j >= 5) {
                    break;
                }

                $scope.pageList.push(pageNumber);
                j++;
            }

            $("#loading2").hide();
        });

    };
});

// 订单评价
taoqiApp.controller("OrderEstimate", function ($scope, $http) {
    $scope.$on('search', function (e) {
        if ($scope.AllEstimateList) {
            $scope.GetOrderEstimateList();
        }
    });

    $scope.GetOrderEstimateList = function () {
        var config = {
            params: {
                orderDetailId: window.getQueryString("id"),
                pageIndex: $scope.currentPageIndex,
            }
        };


        $scope.AllEstimateList = [];
        $("#loading").show();
        $("#Div_GetToData").hide();
        $http.get(GetAllEstimateDetailUrl, config).success(function (data) {
            $scope.AllEstimateList = data.items;

            //总数据
            $scope.dataSum = data.total;

            //每页显示的数量
            $scope.pageNum = 2;

            //分页
            $scope.pageTotal = Math.ceil(data.total / $scope.pageNum);
            $scope.pageList = [];

            var j = 0;
            for (var pageNumber = $scope.currentPageIndex - 2; pageNumber <= $scope.pageTotal; pageNumber++) {

                if (pageNumber < 1) {
                    continue;
                }

                if (pageNumber > $scope.pageTotal) {
                    continue;
                }

                if (j >= 5) {
                    break;
                }

                $scope.pageList.push(pageNumber);
                j++;
            }

            $("#loading").hide();
        });

    };
});

// 资讯
taoqiApp.controller("MarketInfoController", function ($scope, $http) {
    $scope.$on('search', function (e) {
        if ($scope.MInfoList) {
            $scope.GetMInfoList();
        }
    });

    $scope.GetMInfoList = function () {
        var config = {
            params: {
                id: window.getQueryString("ID"),
                pageIndex: $scope.currentPageIndex,
            }
        };

        $("#loading").show();
        $http.get(GetMInfoListUrl, config).success(function (data) {
            $scope.MInfoList = data.items;

            //分页
            $scope.pageTotal = Math.ceil(data.total / PAGESIZE);
            $scope.pageList = [];

            var j = 0;
            for (var pageNumber = $scope.currentPageIndex - 2; pageNumber <= $scope.pageTotal; pageNumber++) {

                if (pageNumber < 1) {
                    continue;
                }

                if (pageNumber > $scope.pageTotal) {
                    continue;
                }

                if (j >= 5) {
                    break;
                }

                $scope.pageList.push(pageNumber);
                j++;
            }

            $("#loading").hide();
        });
    };

    $scope.GetMInfoCover = function (total) {
        var config = {
            params: {
                total: total
            }
        };

        $http.get("/Member/api/MarketInfo/GetMInfoCover", config).success(function (data) {
            $scope.MInfoPicture = data;
            //$("#ad-generic").children(".carousel-inner").children("div:first").addClass("active");
        });
    };

    $scope.GetMInfoListTop1_5 = function () {
        var config = {
            params: {
                pageIndex: 1
            }
        };

        $http.get(GetMInfoListUrl, config).success(function (data) {
            $scope.MInfoListTop5 = data.items;
        });
    };

    $scope.GetMInfoListTop5_10 = function () {
        var config = {
            params: {
                pageIndex: 2
            }
        };

        $http.get(GetMInfoListUrl, config).success(function (data) {
            $scope.MInfoListTop5_10 = data.items;
        });
    };
});

// 消息
taoqiApp.controller("MessageController", function ($cookies, $scope, $http) {

    $scope.GetUnreadMessage = function () {
        $http.get(GetUnreadMessageUrl).success(function (data) {
            $scope.messageList = data;
            $cookies.putObject('messageList', data, { 'path': '/' });
            $scope.entity.UnreadCount = data.length;
            if ($scope.messageList.length == 0) {
                $scope.messageError = true;
            }
        });
    };

    $scope.GetAllMessage = function () {
        $http.get(GetAllMessageUrl).success(function (data) {
            $scope.messageList = data;
            if ($scope.messageList.length == 0) {
                $scope.messageError = true;
            }
        });
    };

    $scope.ReadMessage = function (id) {
        $http.get(ReadMessageUrl + id).success(function (data) {
            $scope.GetUnreadMessage();
        });
    };
});

// 到岸地
taoqiApp.controller("clientAddressController", function ($scope, $http) {
    $scope.model = {};
    $scope.model.C_UserType = 0;
    $scope.model.C_StationCapacity = 0;
    $scope.model.C_DailyConsumption1 = 0;
    $scope.model.C_DailyConsumption2 = 0;

    $scope.myClientAddress = [];    //到岸地
    $scope.ParamList = [];

    var config = {
        params: {
            total: 5
        }
    };
    
    //$scope.$on('addressRemove', function (event, addressId) {
    //    for (var i = 0; i < $scope.myClientAddress.length; i++) {
    //        if ($scope.myClientAddress[i].ID == addressId) {
    //            $scope.myClientAddress[i].IsChecked = false;
    //            break;
    //        }
    //    }
    //})

    $scope.getClientAddress = function (owner, newStationName) {
        var index = layer.load();
        $http.get(GetClientAddressListUrl, config)
            .success(function (data) {
                layer.close(index);
                $scope.myClientAddress = data;
                $scope.addressLoaded = true;
                if ($scope.myClientAddress.length <= 5) $scope.addressAll = true;

                for (var i = 0; i < $scope.myClientAddress.length; i++) {
                    if ($scope.myClientAddress[i].C_StationShortName == newStationName) {
                        $scope.myClientAddress[i].IsChecked = true;
                        if (owner == 'Quote') { // 选中新地址（求购）
                            $scope.quoteAddressChange();
                        } else if (owner == 'Cart') {   // 选中新地址（购物车）
                            $scope.cardAddressChange();
                        }
                        break;
                    }
                }
            })
            .error(function (data) {
                layer.close(index);
            });
    };

    $scope.initClientAddress = function () {
        $scope.model.ID = window.getQueryString("ID");
        if ($scope.model.ID == null || $scope.model.ID == '') {
            return;
        }
        
        var index = layer.load();
        $http.get(GetClientAddressByIDUrl + $scope.model.ID)
            .success(function (data) {
                layer.close(index);
                $scope.model = data[0];
                $scope.entity.C_ProductID = $scope.model.C_ProvinceID;
                $scope.entity.C_CountyID = $scope.model.C_CountyID;
                $scope.entity.LandName = $scope.model.C_ProvinceName + ' - ' + $scope.model.C_CityName + ' - ' + $scope.model.C_CountyName;
                $scope.model.C_UserType = $scope.model.C_UserType.toString();
            })
            .error(function (data) {
                layer.close(index);
            });
    };

    $scope.showAll = function () {
        config = {
            params: {
                total: 0
            }
        };

        $scope.getClientAddress();
        $scope.addressAll = true;
        $('#btnShowAll').hide();
    }

    $scope.updatePreview = function ()
    {
        for (var i = 0; i < $scope.ParamList.length; i++) {
            $scope.ParamList[i].C_ValidityTime = $scope.C_ValidityTime;
            $scope.ParamList[i].C_GasTypeID = $scope.entity.C_GasTypeID;
            $scope.ParamList[i].GasTypeName = $("#C_GasTypeID").find("option:selected").text();
            $scope.ParamList[i].C_GasVarietyID = $scope.entity.C_GasVarietyID;
            $scope.ParamList[i].GasVarietyName = $("#C_GasVarietyID").find("option:selected").text();
            $scope.ParamList[i].C_GasificationRateRange = $scope.entity.C_GasificationRateRange;
            $scope.ParamList[i].GasificationRateRange = $("#C_GasificationRateRange").find("option:selected").text();
            $scope.ParamList[i].C_CalorificValueRange = $scope.entity.C_CalorificValueRange;
            $scope.ParamList[i].CalorificValueRange = $("#C_CalorificValueRange").find("option:selected").text();
            $scope.ParamList[i].C_LiquidTemperature = $scope.entity.C_LiquidTemperature;
            $scope.ParamList[i].LiquidTemperatureName = $("#C_LiquidTemperature").find("option:selected").text();
            $scope.ParamList[i].C_InvoiceRequestID = $scope.entity.C_InvoiceRequestID;
            $scope.ParamList[i].InvoiceRequestName = $("#C_InvoiceRequestID").find("option:selected").text();
            $scope.ParamList[i].C_InvoiceTypeID = $scope.entity.C_InvoiceTypeID;
            $scope.ParamList[i].InvoiceTypeName = $("#C_InvoiceTypeID").find("option:selected").text();
            $scope.ParamList[i].C_Remark = $scope.C_Remark;
        }
    }

    //添加到岸地
    $scope.addClientAddress = function (owner) {
        if ($scope.add.$invalid) {
            return;
        }

        if ($scope.entity.C_CountyID == null || $scope.entity.C_CountyID == '') {
            layer.alert("提示：请先选择收货地址。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
        } else {
            $scope.model.C_CountyID = $scope.entity.C_CountyID;

            var index = layer.load();
            $http.post(AddClientAddressUrl, $scope.model)
                .success(function (data) {
                    layer.close(index);
                    if (data == "OK") {
                        layer.alert('提示：到岸地址添加成功！', {
                            area: ['385px', '178px'],
                            offset: ['195px', '500px']
                        },
                        function (index) {
                            layer.close(index);
                            if (window.location.pathname.toLowerCase().indexOf("/member/clientaddress/") != -1) {
                                window.location = "/Member/ClientAddress/";
                            } else {
                                $scope.getClientAddress(owner, $scope.model.C_StationShortName);
                                $("#divClientAddress").hide();
                            }
                        });
                    }
                })
                .error(function (data) {
                    layer.close(index);
                });
        }
    }

    // 选择求购单到岸地
    $scope.quoteAddressChange = function () {
        var isValid = true;

        if ($scope.C_ValidityTime == null || $scope.C_ValidityTime == '') {
            layer.alert("提示：请先选择求购报价有效期。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            isValid = false;
        }

        for (var i = 0; i < $scope.myClientAddress.length; i++) {
            var clientAddressInfo = $scope.myClientAddress[i];

            if (!isValid) {
                $scope.myClientAddress[i].IsChecked = false;
                continue;
            }

            if (clientAddressInfo.IsChecked) {
                var quoteInfo = {
                    C_ValidityTime: $scope.C_ValidityTime,

                    C_GasTypeID: $scope.entity.C_GasTypeID,
                    GasTypeName: $("#C_GasTypeID").find("option:selected").text(),
                    C_GasVarietyID: $scope.entity.C_GasVarietyID,
                    GasVarietyName: $("#C_GasVarietyID").find("option:selected").text(),
                    C_GasificationRateRange: $scope.entity.C_GasificationRateRange,
                    GasificationRateRange: $("#C_GasificationRateRange").find("option:selected").text(),
                    C_CalorificValueRange: $scope.entity.C_CalorificValueRange,
                    CalorificValueRange: $("#C_CalorificValueRange").find("option:selected").text(),
                    C_LiquidTemperature: $scope.entity.C_LiquidTemperature,
                    LiquidTemperatureName: $("#C_LiquidTemperature").find("option:selected").text(),

                    C_ClientAddressID: clientAddressInfo.ID,
                    C_Quantity: 1,
                    C_ArriveTime: '',
                    C_InvoiceRequestID: $scope.entity.C_InvoiceRequestID,
                    InvoiceRequestName: $("#C_InvoiceRequestID").find("option:selected").text(),
                    C_InvoiceTypeID: $scope.entity.C_InvoiceTypeID,
                    InvoiceTypeName: $("#C_InvoiceTypeID").find("option:selected").text(),
                    C_Remark: $scope.C_Remark,

                    ClientAddressInfo: clientAddressInfo
                };

                var isExist = false;
                for (var j = 0; j < $scope.ParamList.length; j++) {
                    if ($scope.ParamList[j].C_ClientAddressID == clientAddressInfo.ID) {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist) $scope.ParamList.push(quoteInfo);
            } else {
                for (var k = $scope.ParamList.length - 1; k >= 0; k--) {
                    if ($scope.ParamList[k].C_ClientAddressID == clientAddressInfo.ID) {
                        $scope.ParamList.splice(k, 1);
                    }
                }
            }
        }
    }

    // 选择订单到岸地
    $scope.cardAddressChange = function () {
        for (var i = 0; i < $scope.myClientAddress.length; i++) {
            var clientAddressInfo = $scope.myClientAddress[i];

            if ($scope.myClientAddress[i].IsChecked) {
                var productAreaID = window.getQueryString("id");
                var productArea = {};

                for (var j = 0; j < $scope.ShopCartList.length; j++) {
                    if ($scope.ShopCartList[j].C_ProductAreaID == productAreaID) {
                        productArea = $scope.ShopCartList[j];
                        break;
                    }
                }

                if (productArea) {
                    var orderDetailInfo = {
                        Product: productArea,
                        C_ClientAddressID: clientAddressInfo.ID,
                        ClientAddressInfo: clientAddressInfo,

                        C_Quantity: 1,
                        C_Price: productArea.C_Price_Min,
                        C_ArriveTime: ''
                    };

                    var isExist = false;
                    for (var j = 0; j < $scope.ParamList.length; j++) {
                        if ($scope.ParamList[j].ClientAddressInfo.ID == clientAddressInfo.ID) {
                            isExist = true;
                            break;
                        }
                    }

                    if (!isExist) {
                        $scope.ParamList.push(orderDetailInfo);
                    }
                }
            } else {
                for (var k = $scope.ParamList.length - 1; k >= 0; k--) {
                    if ($scope.ParamList[k].C_ClientAddressID == clientAddressInfo.ID) {
                        $scope.ParamList.splice(k, 1);
                    }
                }
            }
        }
    }

    $scope.ParamListRemove = function (index) {
        if ($scope.ParamList && $scope.ParamList.length > 0) {
            $scope.ParamList.splice(index, 1);
        }
    }

    $scope.addQuantity = function (index) {
        if ($scope.ParamList) {
            var parm = {};
            angular.extend(parm, $scope.ParamList[index]);
            $scope.ParamList.splice(index, 0, parm);
        }
    }

    //添加订单
    $scope.addOrder = function () {
        if (!$scope.CheckLogin()) return;

        var productAreaID = window.getQueryString("id");

        var config = {
            params: {
                id: productAreaID
            }
        };

        if (!config.params.id) {
            layer.alert("提示：请先选择商品。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            return;
        }

        if ($scope.ParamList.length == 0) {
            layer.alert("提示：请选择或添加收货地址。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            return;
        }

        for (var i = 0; i < $scope.ParamList.length; i++) {
            if ($scope.ParamList[i].C_ArriveTime == "") {
                layer.alert("提示：请选择到岸时间。", {
                    area: ['385px', '178px'],
                    offset: ['195px', '500px'],
                });
                return;
            }
        }

        $http.post(AddOrderUrl, $scope.ParamList, config).success(function (data) {
            if (data.indexOf("OK") != -1) {
                layer.alert("操作成功！",{
                    type: 0,
                    area: ['385px', '178px'],
                    offset: ['195px', '500px'],
                },
                function () {
                    var result = data.split(":");
                    if (result.length == 2) {
                        window.location = "shopcart_step3.html?SN=" + result[1];
                    } else {
                        window.location = "shopcart_step3.html";
                    }
                });
            }
            else {
                layer.alert(data);
            }
        });
    }

    //添加求购
    $scope.addQuote = function () {
        if (!$scope.CheckLogin()) return;
        
        if ($scope.ParamList.length == 0) {
            layer.alert("提示：请添加求购信息。", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            return;
        }

        for (var i = 0; i < $scope.ParamList.length; i++) {
            if ($scope.ParamList[i].C_ArriveTime == "") {
                layer.alert("提示：请选择到岸时间。", {
                    area: ['385px', '178px'],
                    offset: ['195px', '500px'],
                });
                return;
            }
        }

        $http.post("/Member/api/Quote/AddQuote",
            $scope.ParamList
            ).success(function (data) {
                if (data.indexOf("OK") != -1) {
                    layer.alert("成功提交审核，30分钟之内将发布您的求购！",{
                        type: 0,
                        area: ['385px', '178px'],
                        offset: ['195px', '500px'],
                    },
                    function () {
                        window.location.href = "/Member/Quote/";
                    });
                }
                else {
                    layer.alert(data);
                }
            });
    }
});

// 车辆管理
taoqiApp.controller("carController", function ($scope, $http) {
    $scope.model = {};
    $scope.CarInfo = {};

    var config = {
        params: {
            total: 5
        }
    };

    $scope.GetCarList = function (newPlateNumber) {
        $http.get(GetCarListUrl, config).success(function (data) {
            $scope.CarList = data;
            $scope.carLoaded = true;
            if ($scope.CarList.length <= 5) $scope.carAll = true;
        });
    };

    $scope.showAll = function () {
        config = {
            params: {
                total: 0
            }
        };

        $scope.GetCar();
        $("#btnShowAll").text("已全部显示");
    }

    $scope.InitCar = function () {
        $scope.model.ID = window.getQueryString("ID");
        if ($scope.model.ID == null || $scope.model.ID == '') {
            return;
        }

        var index = layer.load();
        $http.get(GetCarUrl + $scope.model.ID, config)
            .success(function (data) {
                layer.close(index);
                $scope.model = data[0];
            })
            .error(function (data) {
                layer.close(index);
            });
    };

    $scope.SaveCar = function () {
        if ($scope.car.$invalid) {
            $scope.car.tonnage.$dirty = true;
            $scope.car.phone.$dirty = true;
            return false;
        }

        var index = layer.load();
        $http.post(AddCarUrl, $scope.model)
            .success(function (data) {
                layer.close(index);
                if (data == "OK" || data == 1) {
                    layer.alert("操作成功！", {
                        area: ['385px', '178px'],
                        offset: ['195px', '500px']
                    },
                    function (index) {
                        layer.close(index);
                        if (window.location.href.toLowerCase().indexOf("/car/") != -1) {
                            window.location = '/member/car/';
                        } else {
                            $scope.GetCarList();
                            $scope.entity.Car = $scope.CarList[$scope.CarList.length - 1]; // 选中最新添加的记录
                            $("#carModal").modal('hide');
                        }
                    });
                }
            })
            .error(function (data) {
                layer.close(index);
            });
    }

    $scope.getCarInfo = function () {
        $http.get(GetCarInfoUrl + window.getQueryString("id")).success(function (data) {
            $scope.CarInfo = data[0];
            if (data[0].C_BaiduPosition == null) {
                layer.alert("提示：暂时没有该车辆实时位置，您有权要求卖方链接车辆的实时位置，以便您随时了解车辆位置。", {
                    area: ['385px', '178px'],
                    offset: ['195px', '500px'],
                });
            } else {
                window.showMarker(data[0].C_BaiduPosition);
            }
        });
    }
});

// 账户管理
taoqiApp.controller("accountController", function ($scope, $http) {
    $scope.model = {};
    $scope.isShow = false;
    $scope.isEdit = false;

    $scope.$on('baseLoaded', function (e) {
        if ($scope.initFailed) $scope.initClient();
    });

    $scope.$on('refresh', function (e) {
        $scope.initClient();
    });

    $scope.$on('update', function (e) {
        switch ($scope.UserInfo.CompanyStatus) {
            case '0':
                $scope.isShow = true;
                if ($scope.isEdit) {
                    $scope.StatusText = "企业变更";
                } else {
                    $scope.StatusText = "您保存后将进入审核阶段，保存以后将无法修改";
                }
                break;
            case '1':
                $scope.StatusText = "请等待审核";
                $scope.isShow = false;
                break;
            case '2':
                $scope.StatusText = "企业已认证";
                $scope.isShow = false;
                break;
            case '3':
                $scope.StatusText = "审核未通过";
                $scope.isShow = true;
                break;
        }

        $scope.$apply();
    });

    $scope.initClient = function () {
        if (!$scope.UserInfo.ClientID) {
            $scope.initFailed = true;
            return;
        }

        var index = layer.load();
        $http.get(GetClientUrl + $scope.UserInfo.ClientID)
            .success(function (data) {
                layer.close(index);
                $scope.model = data[0];
                $scope.entity.C_ProvinceID = $scope.model.C_ProvinceID;
                $scope.entity.C_CityID = $scope.model.C_CityID;
                $scope.entity.C_CountyID = $scope.model.C_CountyID;
                if ($scope.model.C_CountyID) {
                    $scope.entity.LandName = $scope.model.C_ProvinceName + " - " + $scope.model.C_CityName + " - " + $scope.model.C_CountyName;
                } else {
                    $scope.entity.LandName = "";
                }
                $scope.isEdit = false;

                $scope.$broadcast("update");
            })
            .error(function (data) {
                layer.close(index);
            });
    }

    $scope.getClient = function () {
        $http.get(GetClientUrl + window.getQueryString("id")).success(function (data) {
            $scope.model = data[0];
        });
    };

    $scope.getSmsCode = function () {
        var config = {
            params: {
                mobile: $scope.model.Mobile,
                code: $scope.model.Code
            }
        };
        
        var index = layer.load();
        $http.post(GetSmsCodeUrl, {}, config)
            .success(function (data) {
                layer.close(index);
                if (data == "成功") {
                    $scope.errorSmsMessage = "短信验证已发送（10分钟内有效）。";
                } else if (data == "0") {
                    $scope.errorSmsMessage = "图片验证码错误，请重新输入！";
                }
                else {
                    $scope.errorSmsMessage = data;
                }
            })
            .error(function (data) {
                layer.close(index);
            });
    }

    $scope.register = function () {
        var arg = arguments[0];
        if (arg == 'personal') {
            if ($scope.perForm.$invalid) {
                $scope.perForm.phone.$dirty = true;
                $scope.perForm.code.$dirty = true;
                $scope.perForm.smsCode.$dirty = true;
                $scope.perForm.name.$dirty = true;
                $scope.perForm.pass.$dirty = true;
                $scope.perForm.pass2.$dirty = true;

                if ($scope.perForm.name.$error) {
                    $scope.errorMessage = '请重新输入您的真实姓名！<br>';
                }

                return false;
            }
        } else {
            if ($scope.comForm.$invalid) {
                $scope.comForm.companyName.$dirty = true;
                $scope.comForm.phone.$dirty = true;
                $scope.comForm.code.$dirty = true;
                $scope.comForm.smsCode.$dirty = true;
                $scope.comForm.name.$dirty = true;
                $scope.comForm.pass.$dirty = true;
                $scope.comForm.pass2.$dirty = true;

                if ($scope.comForm.companyName.$error) {
                    $scope.errorMessage  = '公司名称非法，请重新输入！公司名称需与营业执照一致！<br>';
                }

                if ($scope.comForm.name.$error) {
                    $scope.errorMessage = '请重新输入您的真实姓名！<br>';
                }

                return false;
            }
        }

        var index = layer.load();
        $http.post(RegisterUrl, $scope.model)
            .success(function (data) {
                layer.close(index);
                if (data == "OK") {
                    $(".tab-pane").each(function () {
                        $(this).removeClass("active");
                    });
                    if (arg == 'personal')
                        $("#perFinish").addClass("active");
                    else
                        $("#comFinish").addClass("active");
                } else if (data == '1') {
                    $scope.errorMessage = "短信验证码错误，请重新输入！<br>";
                } else {
                    $scope.errorMessage = data;
                }
            })
            .error(function (data) {
                layer.close(index);
            });
    }

    $scope.saveClient = function () {
        $scope.model.C_CountyID = $scope.entity.C_CountyID;
        var index = layer.load();
        $http.post(SaveClientUrl, $scope.model)
            .success(function (data) {
                layer.close(index);
                if (data == "OK") {
                    layer.alert("企业资料已保存！", {
                        area: ['385px', '178px'],
                        offset: ['195px', '500px']
                    });
                }
                else {
                    layer.alert(data, {
                        area: ['385px', '178px'],
                        offset: ['195px', '500px']
                    });
                }
            })
            .error(function (data) {
                layer.close(index);
            });
    };

    $scope.submitClient = function (status) {
        if ($scope.entity.C_CountyID == null || $scope.entity.C_CountyID == '') {
            layer.alert("请选择公司所在地！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return false;
        }
        if ($scope.model.C_Address == null || $scope.model.C_Address == '') {
            layer.alert("请填写公司详细地址！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return false;
        }
        var imgPath = '/member/Data/Upload/Images/';
        if ($('#ImgUp1').attr('src') == imgPath || $('#ImgUp2').attr('src') == imgPath || $('#ImgUp3').attr('src') == imgPath
            || $('#ImgUp4').attr('src') == imgPath || $('#ImgUp5').attr('src') == imgPath || $('#ImgUp6').attr('src') == imgPath) {
            layer.alert("请先上传企业证件！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return false;
        }

        var config = {
            params: {
                status: $scope.isEdit ? 4 : 1
            }
        };

        layer.confirm('点击确认后将进入审核阶段，信息将无法修改', {
            btn: ['确认', '取消'],
            area: ['385px', '178px'],
            offset: ['195px', '500px'],
        }, function (index) {
            $scope.model.C_CountyID = $scope.entity.C_CountyID;
            var index = layer.load();
            $http.post(SaveClientUrl, $scope.model, config)
                .success(function (data) {
                    layer.close(index);
                    if (data == "OK") {
                        layer.alert("企业资料已提交，请等待管理员审核！", {
                            area: ['385px', '178px'],
                            offset: ['195px', '500px']
                        }, function (index) {
                            layer.close(index);
                            $scope.getUserInfo();
                        });
                    }
                    else {
                        layer.alert(data, {
                            area: ['385px', '178px'],
                            offset: ['195px', '500px']
                        });
                    }
                })
                .error(function (data) {
                    layer.close(index);
                });
        });
    };

    $scope.addClient = function () {
        if ($scope.addForm.company.$invalid) {
            $scope.addForm.company.$dirty = true;
            layer.alert("请输入公司中文全称，需跟营业执照一致", {
                area: ['385px', '178px'],
                offset: ['195px', '500px']
            });
            return false;
        }

        var config = {
            params: {
                companyName: $scope.model.Company
            }
        };

        var index = layer.load();
        $http.get(AddClientUrl, config)
            .success(function (data) {
                layer.close(index);
                if (data.indexOf("OK") != -1) {
                    $scope.getCompany();
                    var result = data.split(":");
                    if (result.length == 3) {
                        if (result[1] == '2') {
                            layer.alert('同名企业已认证，你的申请将提交公司管理员审核！', {
                                area: ['385px', '178px'],
                                offset: ['195px', '500px']
                            });
                        } else {
                            layer.alert('操作成功！', {
                                area: ['385px', '178px'],
                                offset: ['195px', '500px']
                            });
                        }
                        $scope.UserInfo.AccountID = result[2];
                        $scope.getUserInfo();
                    }
                    $("#addClientModel").modal('hide');
                } else {
                    layer.alert(data, {
                        area: ['385px', '178px'],
                        offset: ['195px', '500px']
                    });
                }
            })
            .error(function (data) {
                layer.close(index);
                alert(data);
            });
    };

    $scope.modifyClient = function () {
        layer.confirm('企业变更后所有资料需重新审核，企业用户已有权限可能会丢失，是否继续？', {
            btn: ['继续', '取消'], 
            area: ['385px', '178px'],
            offset: ['195px', '500px'],
        }, function (index) {
            layer.close(index);
            $scope.UserInfo.CompanyStatus = '0';
            $scope.isEdit = true;
            $scope.$broadcast("update");
        });
    }
});

// 指令
//taoqiApp.directive('modal', function () {
//    return {
//        restrict: 'E',
//        template: '<div class="modal fade">' +
//                        '<div class="modal-dialog">' +
//                            '<div class="modal-content">' +
//                                '<div class="modal-header">' +
//                                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
//                                    '<h4 class="modal-title">{{ title }}</h4>' +
//                                '</div>' +
//                                '<div class="modal-body" ng-transclude></div>' +
//                            '</div>' +
//                        '</div>' +
//                    '</div>',
//        transclude: true,
//        replace: true,
//        scope: true,
//        link: function postLink(scope, element, attrs) {
//            scope.title = attrs.title;

//            scope.$watch(attrs.visible, function (value) {
//                if (value == true)
//                    $(element).modal('show');
//                else
//                    $(element).modal('hide');
//            });

//            $(element).on('shown.bs.modal', function () {
//                scope.$apply(function () {
//                    scope.$parent[attrs.visible] = true;
//                });
//            });

//            $(element).on('hidden.bs.modal', function () {
//                scope.$apply(function () {
//                    scope.$parent[attrs.visible] = false;
//                });
//            });
//        }
//    };
//});

function initMenu() {
    var pathName = window.location.pathname;

    if (pathName == "/") {
        pathName = "/index.html";
    }

    //菜单
    $("ul#menus > li >a").each(function (index, item) {
        if ($(item).attr("href") == pathName) {
            $(item).addClass("active");
        }
    });

    //气源分类子菜单
    if ($('#categoryList').length > 0) {
        $('#categoryList > div').on('shown.bs.dropdown', function () {
            $("#categoryList > div > a").removeClass("active");
            $(this).find("a").addClass("active");
        })
    }

    //点击气源分类
    $("#categoryMenu").bind("click", function () {
        if (pathName == "/index.html") {
            return false;
        }

        $("#categoryList").toggle();
    });

    if (pathName == "/index.html") {
        $("#categoryList").show();
    }
}

$(function () {
    $("#tabRanking").click(function () {
        $("#tabRanking").addClass("active");
        $("#tabRecord").removeClass("active");
    });

    $("#tabRecord").click(function () {
        $("#tabRecord").addClass("active");
        $("#tabRanking").removeClass("active");
    });

    var pathName = window.location.pathname;

    if (pathName == "/") {
        pathName = "/index.html";
    }

    if (pathName == "/index.html") {
        $("#btnLogin").click(function () {
            $("#loginForm").submit();
        });

        $("#txtUSER_NAME,#txtPASSWORD").keydown(function () {
            if (event.keyCode == 13) {
                $("#loginForm").submit();
            }
        });
    }
});

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}

function toLoginPage() {
    var url = encodeURIComponent(window.location.pathname + window.location.search);

    if (window.location.pathname.indexOf("/member/") >= 0) {
        url = "/member/Users/Login.aspx?Redirect=~/" + url;
    }
    else {
        url = "/member/Users/Login.aspx?Redirect=~/../" + url;
    }

    window.location = url;
}

function setBaiduPosition(position) {
    $("#baiduPosition").val(position);

    var scope = angular.element($("#baiduPosition")).scope();
    scope.$apply(function () {
        scope.model.C_BaiduPosition = position;
    });
}

$.fn.extend({
    "slideUp": function (value) {
        var docthis = this;
        //默认参数
        value = $.extend({
            "li_h": "30",
            "time": 2000,
            "movetime": 600
        }, value)

        //向上滑动动画
        function autoani() {
            $(docthis).animate({ "margin-top": -value.li_h }, value.movetime, function () {
                $(this).css("margin-top", 0);
                $("tr:first", docthis).appendTo(docthis);
            })
        }

        //自动间隔时间向上滑动
        var anifun = setInterval(autoani, value.time);

        //悬停时停止滑动，离开时继续执行
        $("tr", docthis).hover(function () {
            clearInterval(anifun);			//清除自动滑动动画
        }, function () {
            anifun = setInterval(autoani, value.time);	//继续执行动画
        })
    }
})

function setDatetimePicker() {
    //设置日期控件
    $('.form_time').datetimepicker({
        language: 'zh-CN',
        weekStart: 7,
        todayBtn: 0,
        autoclose: 1,
        todayHighlight: 0,
        minView: 1, //最小选择小时
        forceParse: 0,
        format: 'yyyy-mm-dd hh:00'
    });
}

function addDays(interval) {
    var dateNow = new Date();
    return dateNow.getFullYear() + '-' + (dateNow.getMonth() + 1) + '-' + (dateNow.getDate() + interval) + ' ' + dateNow.getHours() + ':00';
}

function modalQuit(modalID, success) {
    //设置日期控件
    $('#' + modalID).modal('hide');
    $('div.modal-backdrop').remove();

    if (success) {
        layer.confirm('您已成功下单！现在就去查看订单吗？', {
            btn: ['是，现在就去', '不， 以后再说！'], //按钮
            area: ['385px', '178px'],
            offset: ['195px', '500px'],
        }, function () {
            window.location.href = "/member/Order/";
        });
    }
}

function modal_ProductInTransit(modalID, success) {
    //设置日期控件
    $('#' + modalID).modal('hide');
    $('div.modal-backdrop').remove();

    if (success) {
        layer.alert("您的在途气已成功选定买家",{
            title: ["恭喜！",true],
            type: 0,
            area: ['385px', '178px'],
            offset: ['195px', '500px'],
        },
        function () {
            window.location.href = "/member/TransitSell/";
        });
    }
}

function modal_OrderSell(modalID, success) {
    //设置日期控件
    $('#' + modalID).modal('hide');
    $('div.modal-backdrop').remove();

    if (success) {
        layer.alert("您已成功发货。",{
            title: ["恭喜！",true],
            type: 0,
            area: ['385px', '178px'],
            offset: ['195px', '500px'],
        },
        function () {
            window.location.reload();
        });
    }
}


var Plants;
//为首页和报价页面的报价功能服务
function JSFC_Quote(e) {

    if (Plants == null) {
        //获取气源工厂的信息
        $.ajax({
            method: "GET",
            async: false,
            url: "/member/api/home/getbasedata",
            success: function (data) {
                Plants = data['Plants'];
            }
        });
    }

    var rowNum = $("#" + e.id).attr("rowNum");

    var appendHtml = "<tr><td colspan=\"" + rowNum + "\" style=\"background-color: rgb(241,241,241);\">" +
                        "<form method=\"POST\" action=\"/member/Quote/QuoteForm.ashx\" id=\"QuoteForm\">" +
                            "<div class=\"Quote_div\" style=\"margin:5px; background-color: white; padding: 10px;\">" +

                                "<div class=\"Quote_item\">" +

                                    "<div class=\"delivery dropdown\" style=\"float:left; margin-left:20px; margin-right:50px; height: 30px;\">" +
                                        "<div class=\"deliveryBorder\" data-toggle=\"dropdown\" data-target=\"#\"  style=\"text-align: center; height: inherit;\">" +
                                            "<span id=\"TXT_ClientShortName\"></span>" +
                                            "<input type=\"hidden\" name=\"" + e.id + "Quote_Factory1\"></input>"+
                                            "<div class=\"btn floatRight\">" +
                                                "<span class=\"caret\"></span>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div id=\"delivery\" class=\"dropdown-menu panel\" data-panel='plants'>" +
                                            "<ul class=\"nav nav-tabs\">";

                                                $.each(Plants, function (key, value) {
                                                    if (key == '热门')
                                                        var str_active = " class=\"active\" ";
                                                    else
                                                        var str_active = "";
                                                    appendHtml += "<li" + str_active + "><a class=\"a_C_Client_type\" data-toggle=\"tab\" data-target=\"#\">" + key + "</a></li>";
                               
                                                });

                                                appendHtml += "</ul><ul class=\"nav nav-pills\">";

                                                $.each(Plants['热门'], function (key, value) {
                                                    appendHtml += "<li><a class=\"a_C_Client\" value=\"" + value['ID'] + "\">" + value['C_ClientShortName'] + "</a></li>";

                                                });

                            appendHtml += "</ul>" +
                                        "</div>"+
                                    "</div>"+

                                    "<div style=\"display:inline-block; margin-right:70px;\">" +
                                        "<label for=\"" + e.id + "Quote_Quote1\">报价：</label>" +
                                        "<input id=\"" + e.id + "Quote_Quote1\" name=\"" + e.id + "Quote_Quote1\" type=\"text\" class=\"Quote_Quote\" style=\"width:100px;\"/>" +
                                        "元/吨" +
                                    "</div>" +

                                    "<a href=\"#\" id=\"" + e.id + "a1\" style=\"color:#ee6409; display:inline-block;\"  onclick=\"JSFC_Quote_add(this)\">+添加</a>" +

                                    "<button type=\"submit\" value=\"" + e.id + "\" name=\"btn_quote\" style=\"background-color:#ee6409; color:white; display:inline-block; width:70px; height:26px; float:right;\">确认提交</button>" +
                                "</div>" +

                            "</div>" +
                        "</form>" +
                    "</td></tr>";

    $("#" + e.id).parents("tr").after(appendHtml);

    $("#" + e.id).text("取消报价");
    $("#" + e.id).attr("onclick", "JSFC_CancelQuote(this)");
    $("#" + e.id).css("color", "black");
}

function JSFC_Quote_add(e) {


    if (e.id == null) {
        return 0;
        layer.alert("请重试！", {
            area: ['385px', '178px'],
            offset: ['195px', '500px'],
        });
    }
    else {
        var Quote_Factory = $("#" + e.id).parents("div.Quote_item").find(".deliveryBorder > input[type='hidden']").attr("name");
        var Quote_Quote = $("#" + e.id).parents("div.Quote_item").find("input.Quote_Quote").attr("id");

        var eId = e.id + "1";
        Quote_Factory = Quote_Factory + "1";
        Quote_Quote = Quote_Quote + "1";

        var buttonValue = $("#" + e.id).next().val();
    }

    var appendHtml = "<div class=\"Quote_item\" style=\"margin-top:10px;\">" +

                        "<div class=\"delivery dropdown\" style=\"float:left; margin-left:20px; margin-right:50px; height: 30px;\">" +
                            "<div class=\"deliveryBorder\" data-toggle=\"dropdown\" data-target=\"#\"  style=\"text-align: center; height: inherit;\">" +
                                "<span id=\"TXT_ClientShortName\"></span>" +
                                "<input type=\"hidden\" name=\"" + Quote_Factory + "\"></input>" +
                                "<div class=\"btn floatRight\">" +
                                    "<span class=\"caret\"></span>" +
                                "</div>" +
                            "</div>" +
                            "<div id=\"delivery\" class=\"dropdown-menu panel\" data-panel='plants'>" +
                                "<ul class=\"nav nav-tabs\">";

                                    $.each(Plants, function (key, value) {
                                        if (key == '热门')
                                            var str_active = " class=\"active\" ";
                                        else
                                            var str_active = "";
                                        appendHtml += "<li" + str_active + "><a class=\"a_C_Client_type\" data-toggle=\"tab\" data-target=\"#\">" + key + "</a></li>";
                               
                                    });

                                    appendHtml += "</ul><ul class=\"nav nav-pills\">";

                                    $.each(Plants['热门'], function (key, value) {
                                        appendHtml += "<li><a class=\"a_C_Client\" value=\"" + value['ID'] + "\">" + value['C_ClientShortName'] + "</a></li>";

                                    });

                appendHtml += "</ul>" +
                            "</div>"+
                        "</div>"+

                        "<div style=\"display:inline-block; margin-right:70px;\">" +
                            "<label for=\"" + Quote_Quote + "\">报价：</label>" +
                            "<input id=\"" + Quote_Quote + "\" name=\"" + Quote_Quote + "\" class=\"Quote_Quote\" type=\"text\" style=\"width:100px;\"/>" +
                            "元/吨" +
                        "</div>" +

                        "<a href=\"#\" id=\"" + eId + "\"  style=\"color:#ee6409; display:inline-block;\"  onclick=\"JSFC_Quote_add(this)\">+添加</a>" +

                        "<button type=\"submit\" value=\"" + buttonValue + "\" name=\"btn_quote\" style=\"background-color:#ee6409; color:white; display:inline-block; width:70px; height:26px; float:right;\">确认提交</button>" +
                    "</div>";

    $("#" + e.id).parents("div.Quote_item").after(appendHtml);

    $("#" + e.id).next().remove();

    $("#" + e.id).text("-取消");
    $("#" + e.id).css("color", "black");
    $("#" + e.id).attr("onclick", "JSFC_Quote_subtract(this)");
}

function JSFC_CancelQuote(e) {
    $("#" + e.id).parents("tr").next().remove();

    $("#" + e.id).text("报价");
    $("#" + e.id).attr("onclick", "JSFC_Quote(this)");
    $("#" + e.id).css("color", "white");
}

function JSFC_Quote_subtract(e) {
    $("#" + e.id).parents("div.Quote_item").remove();
}

$(function () {
    //延迟显示
    $("#mainBody").show();
});

//评价页面为选择星级评价服务
$(function () {
    $('.img_EstimateQuality').click(function () {
        var estimateStar = $(this).attr('index');
        $('#EstimateQuality').val = estimateStar;
    });
});

$(function () {
    //鼠标移动至首页的行情数据时操作
    $("#carousel-marketData > div.carousel-inner").delegate('tr.tr_short', 'mouseover', function () {

        $(this).parent("tbody").parent("table").parent("div.item").children("table").each(function () {
            $(this).find("tr.tr_detail").addClass("disabled");
            $(this).find("tr.tr_short").removeClass("disabled");
        });

        $(this).prev("tr.tr_detail").removeClass("disabled");
        $(this).addClass("disabled");
    });

    //鼠标离开首页的行情数据时操作
    $("#carousel-marketData > div.carousel-inner").delegate('div.item', 'mouseleave ', function () {

        $(this).children("table").each(function () {
            $(this).find("tr.tr_detail").addClass("disabled");
            $(this).find("tr.tr_short").removeClass("disabled");
        });

        $(this).find("table:first tr.tr_detail").removeClass("disabled");
        $(this).find("table:first tr.tr_short").addClass("disabled");
    });


    //操作合并的级联气源场
    $('body').on('click', '.a_C_Client_type', function () {
        //获取数据值
        var appendHtml = "";
        $.each(Plants[$(this).text()], function (key, value) {
            appendHtml += "<li><a class=\"a_C_Client\" value=\"" + value + "\">" + value['C_ClientShortName'] + "</a></li>";
        });

        $(this).parentsUntil("div.Quote_item").find("ul.nav-pills").html(appendHtml);

        return false;
    });

    $(".delivery").blur(function () { alert('as');});

    $('body').on('click', '.a_C_Client', function () {
        var C_ClientID = $(this).attr('value');
        var C_ClientShortName = $(this).text();
        $(this).parentsUntil("div.Quote_div").children("div.deliveryBorder").children('#TXT_ClientShortName').text(C_ClientShortName);
        $(this).parentsUntil("div.Quote_div").children("div.deliveryBorder").children("input[type='hidden']").attr('value', C_ClientID);
    });
    //根据品种不同，起源地变化

});
