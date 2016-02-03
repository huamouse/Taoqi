// 百度地图初始化
var map = new BMap.Map("allmap", { enableMapClick: false });
map.centerAndZoom(new BMap.Point(118.742218, 31.988125), 15);
map.enableScrollWheelZoom(true);
var marker;

// 设置地图方法
function enableMapClick() {
    map.addEventListener("click", function (e) {
        updatePosition(e.point);
    });
}

function Geocoder(address, city) {
    var mapGeo = new BMap.Geocoder();
    mapGeo.getPoint(address, function (point) {
        if (point) updatePosition(point);
    }, city); 
}

function updatePosition(point) {
    if ("undefined" == typeof marker) {
        marker = new BMap.Marker(point);

        // 设置infoWindow
        var info = arguments[1] || '请点击地图或拖动红色图标更改位置';
        marker.addEventListener("click", function () {
            var infoWindow = new BMap.InfoWindow(info);
            this.openInfoWindow(infoWindow);
        });
        map.addOverlay(marker)

        var disableDrag = arguments[2] || false;
        if (!disableDrag) {
            marker.enableDragging()
        }
    }
    else {
        marker.setPosition(point);
    }
}

function updatePosition2(point) {
    if ("undefined" == typeof marker) {
        marker = new BMap.Marker(point);
        map.addOverlay(marker);
    }
    else {
        marker.setPosition(point);
    }
}

function resolvePosition(stringPosition) {
    var positions = stringPosition.split(',');
    var lng = positions[0];
    var lat = positions[1];
    return new BMap.Point(lng, lat);
}

// 绑定模式框事件（地图标注或显示）
$('#modalMap').on('show.bs.modal', function () {
    // 解析地址
    var baiduPosition = $("#baiduPosition").val();
    if (baiduPosition) {
        var point = resolvePosition(baiduPosition);
        updatePosition(point);
    }
    else {
        var address = $('#address').val();
        var scope = angular.element($("#baiduPosition")).scope();
        var city = scope.$parent.entity.ProvinceName + scope.$parent.entity.CityName;
        Geocoder(address, city);
    }

    enableMapClick();
})

$('#modalMap').on('shown.bs.modal', function () {
    if ("undefined" != typeof marker) map.centerAndZoom(marker.getPosition(), 15);
})

function SavePosition() {
    if ("undefined" != typeof marker) {
        var point = marker.getPosition();
        setBaiduPosition(point.lng + "," + point.lat);
        $('#modalMap').modal('hide');
    }
    else {
        alert("地址未标注！");
    }
}
//$('#modalMap').on('hide.bs.modal', function () {
//    if ("undefined" != typeof marker) {
//        var point = marker.getPosition();
//        setBaiduPosition(point.lng + "," + point.lat);
//    }
//    else {
//        alert("地址未标注！");
//    }
//})

// 地图显示（非模式框）
function showMap() {
    var baiduPosition = getQueryString("position");
    //var carID = getQueryString("car");
    var id = getQueryString('id');
    var point;
    var info = $("#car").clone().css("visibility", "visible").get(0);

    if (baiduPosition) {   // 百度坐标模式
        point = resolvePosition(baiduPosition);
        updatePosition(point, info, true);
        map.centerAndZoom(point, 15);
    //} else if (id) {   // 车辆检索模式
    //    $.getJSON("/Member/api/Car/GetPosition/" + id, function (data) {
    //        if (data.length > 0) {
    //            if (data[0].C_BaiduPosition) {
    //                point = resolvePosition(data[0].C_GPS);
    //                updatePosition(point, info, true);
    //                map.centerAndZoom(point, 15);
    //            }
    //            else {
    //                alert("提示：暂时没有该车辆的实时位置");
    //            }
    //        } else {
    //            alert("提示：找不到该车辆相关信息");
    //        }
    //    })
    } else if (id) {    // 订单模式
        $.getJSON(GetCarPositionUrl + id, function (data) {
            if (data) {
                point = resolvePosition(data);
                updatePosition2(point);
                map.centerAndZoom(point, 15);
            }
            else {
                layer.alert("提示：暂时没有该车辆的实时位置");
            }
        })
    }
}

function showMarker(data)
{
    if (data) {
        point = resolvePosition(data);
        updatePosition2(point);
        map.centerAndZoom(point, 15);
    }
}
