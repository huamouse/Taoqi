<%@ Page Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="estimate.aspx.cs" Inherits="Taoqi.Order.estimate" %>

<%@ Register TagPrefix="Estimate" TagName="First" Src="~/Order/FirstEstimate.ascx" %>
<%@ Register TagPrefix="Estimate" TagName="Add" Src="~/Order/AddEstimate.ascx" %>

<asp:Content ID="Estimate" ContentPlaceHolderID="cntBody" runat="server">

    <style>
        div#Estimate_view table.listView th {
            border-right: solid thin #cccccc;
            border-left: solid thin #cccccc;
        }

        div#Estimate_view table.listView th, div#Estimate_view table.listView td {
            text-align: center;
            padding: 0px;
            font-size: 13px;
        }

        div#historyEstimate > #topTool > div.item{
            font-size:large; 
            text-align:center;
            padding:5px;
            float:left;
            line-height: 28px;
            height: 100%;
        }

        div#historyEstimate > #topTool > div.select{
            border-bottom-color: #ff6400;
        }


        div#historyEstimate > #otherEsitmate > div > div.item > div.left_item{
            float:left;
            border-right: dashed #cccccc;
            width:332px;
            min-height:80px;
        }

        div#historyEstimate > #otherEsitmate > div >  div.item > div.left_item > div.item1{
            font-size:larger;
            font-weight:bold;
            float:left;
            width:100%;
        }
        div#historyEstimate > #otherEsitmate > div >  div.item > div.left_item > div.item2{
            height:20px;
            width:100%;
            margin-top:6px;
            float:left;
        }
        div#historyEstimate > #otherEsitmate > div >  div.item > div.left_item > div.item3{
            margin-top:10px;
            float:left;
            width:100%;
        }
        div#historyEstimate > #otherEsitmate > div >  div.item > div.left_item > div.item3 > div.context{
            display: inline;
            font-weight: bold;
            margin-left:2px;
        }

        div#historyEstimate > #otherEsitmate > div >  div.item > div.right_item{
            float:left;
            margin-left:2%;
            width:556px;
            min-height:80px;
        }

        div#historyEstimate > #otherEsitmate > div >  div.item > div.right_item > div.item1 > div.title,   div#historyEstimate > #otherEsitmate > div.item > div.right_item > div.item2 > div.title,   div.MyHistoryEstimate > div.item > div.title{
            display:inline-block;
            width:100px;
        }
        div#historyEstimate > #otherEsitmate > div >  div.item > div.right_item > div.item1 > div.context, div#historyEstimate > #otherEsitmate > div.item > div.right_item > div.item2 > div.context,   div.MyHistoryEstimate > div.item > div.context{
            display:inline-block; 
            width:450px;
            font-weight: bold;
        }

        div#historyEstimate > #otherEsitmate > div >  div.item > div.right_item > div.item2{
            margin-top:10px;
        }

        div#historyEstimate > #otherEsitmate > div >  div.item > div.right_item > div.item1{
            margin-top:14px;
        }

       div.MyHistoryEstimate > div.HistoryItem{
           float:left;
           width:100%;
       }
       div.MyHistoryEstimate > div.HistoryItem > div.MyIcon{
           width: 20px; 
           height: 50px; 
           float: left;
       }
       div.MyHistoryEstimate > div.HistoryItem > div.item{
           margin-left:20px;
           float: left;
       }
       div.MyHistoryEstimate > div.HistoryItem > div.item > div.title{
            float: left;
            width:70px;
        }
       div.MyHistoryEstimate > div.HistoryItem > div.item > div.context{
            float: left;
            width:450px;
        }

       #Estimate_view .select_div{
           float:left;
           margin-left: 20px;
           border: 2px solid; 
           padding:5px; 
           border-radius: 4px;
           min-width: 80px;
           text-align: center;
       }
    </style>

    <script>
        $(function () {

            var star_avg_C_EstimateQuality = <%: star_avg_C_EstimateQuality %>;
            $('img.avg_C_EstimateQuality').each(function () {
                if ($(this).attr("data-RB-index") > star_avg_C_EstimateQuality) {
                    $(this).attr("src", "");
                }
            });

            $("input.RDOHistoryEstimateType").change(function () {
                $("form").trigger("submit");
            });

            $(".select_div").click(function () {
                <%--var MostEstimate = $("#<%: TXTMostEstimate.ClientID %>").val();
                var index = $(this).attr('data-rb-index');
                var sum = $(this).children('.sum').text();

                if (MostEstimate.indexOf(index) != -1) {
                    //反选
                    MostEstimate = MostEstimate.replace(index, '');
                    $(this).children('.sum').text(parseInt(sum) - 1);
                }
                else {
                    //选中
                    MostEstimate = MostEstimate + index;
                    $(".textarea_estimate").text($(".textarea_estimate").text() + ' ' + $(this).children('.title').text());
                    $(this).children('.sum').text(parseInt(sum) + 1);
                }

                $("#<%: TXTMostEstimate.ClientID %>").val(MostEstimate);--%>

                $(".textarea_estimate").text($(".textarea_estimate").text() + ' ' + $(this).children('.title').text());
                return false;
            });
        });
    </script>

    <div id="Estimate_view" style="float:left; width:100%;" ng-controller="OrderEstimate">

        <label style="float:left; width:100%; font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 0px;">我的订单</label>

        <div style="padding-top: 10px;float:left; width:100%;">

            <table class="listView">
                <thead>
                    <tr>
                        <th>订单编号</th>
                        <th>公司名称</th>
                        <th>订单货物</th>
                        <th>成交单价</th>
                        <th>下单时间</th>
                        <th>订单状态</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><%: MO_SN %></td>
                        <td style="color:#0065e6;"><%: MO_C_ClientShortName %></td>
                        <td><%: MO_ProductText %></td>
                        <td style="color:#ff6400; font-weight: bold;"><%: MO_C_Price %></td>
                        <td><%: MO_DATE_ENTERED %></td>
                        <td><%: MO_StatusName %></td>
                    </tr>
                </tbody>
            </table>

            <%--<div style="float: left; width: 100%; margin-top: 20px; border: solid 2px #f5f5f5;">
                <div style="float: left; width: 100%; background-color: #f5f5f5;">
                    <div style="float: left; margin-left: 20px; font-size: medium; padding: 5px; font-weight: bold;">
                        <span>累计评价</span>
                        <span style="margin-left:15px; color:#ff6400;">{{dataSum}}</span>
                    </div>
                </div>

                <div style="float: left; width: 100%; padding: 10px 15px;">
                    <div style="float: left; width: 140px;">
                        <div style="float : left; width: 100%; text-align: center; font-size: larger;">与描述相符</div>
                        <div style="float : left; width: 100%; text-align: center; font-size: xx-large; color: #ff6400;"><%: avg_C_EstimateQuality %></div>
                        <div style="float : left; width: 100%; text-align: center;">
                            <div>
                                <img class="avg_C_EstimateQuality" data-RB-index="1" src="/images/Estimate/SStar2.jpg" />
                                <img class="avg_C_EstimateQuality" data-RB-index="2" src="/images/Estimate/SStar2.jpg" />
                                <img class="avg_C_EstimateQuality" data-RB-index="3" src="/images/Estimate/SStar2.jpg" />
                                <img class="avg_C_EstimateQuality" data-RB-index="4" src="/images/Estimate/SStar2.jpg" />
                                <img class="avg_C_EstimateQuality" data-RB-index="5" src="/images/Estimate/SStar2.jpg" />
                            </div>
                        </div>
                    </div>

                    <div style="float: left; width: 40px; color: #cccccc; border-left: dashed thin; border-right: dashed thin; padding: 0px 10px; text-align: center;">
                        大家都写到
                    </div>

                    
                </div>

            </div>--%>

            <Estimate:First ID="Estimate_First" runat="server" Visible="true" />
            <Estimate:Add ID="Estimate_Add" runat="server" Visible="false" />
            

            <div id="historyEstimate" style="float:left; width:100%;">
                <div id="topTool" style="float:left; width:100%; height:40px;border: 1px solid #ccc;; background-color:#f5f5f5; box-shadow: inset; margin-top:20px;">
                    <div class="item" style="width: 1%;">
                    </div>
                    <div class="item select" style="width:14%;color:#ff6400;font-weight:bold;">
                        全部（{{dataSum}}）
                    </div>
                    <%--<div class="item" style="width:14%;">
                        好评
                    </div>
                    <div class="item" style="width:14%;">
                        中评
                    </div>
                    <div class="item" style="width:14%;">
                        差评
                    </div>--%>
                    <div class="item" style="width: 84.9%;">
                    </div>

                    <%--<div style="float:right; margin-right:16px; height:100%; line-height:50px;">
                        <select style="min-width:100px; min-height:26px;"></select>
                    </div>--%>
                </div>

                <div id="otherEsitmate" style="width:100%; float:left;">
                    <%--<div id="Div_GetToData">
                        <asp:Repeater ID="RPOtherEsitmate" runat="server">
                            <ItemTemplate>
                                <div class="item" style="width:100%; float:left; min-height:100px; padding:10px; border-bottom:solid thin; border-bottom-color:#cccccc;">
                                    <div class="left_item">
                                        <div class="item1"><%# Eval("Seller") %></div>
                                        <div class="item2">
                                            <img src="/images/Estimate/SStar2.jpg" />
                                            <img src="/images/Estimate/SStar2.jpg" />
                                            <img src="/images/Estimate/SStar2.jpg" />
                                            <img src="/images/Estimate/SStar2.jpg" />
                                            <img src="/images/Estimate/SStar2.jpg" />
                                        </div>
                                        <div class="item3">购买产品：<div class="context"><%# Eval("GasTypeName") %>：<%# Eval("GasVarietyName") %>；<%# Eval("C_GasificationRate") %>；<%# Eval("C_GasSourceName") %>；</div></div>
                                    </div>

                                    <div class="right_item">
                                        <div class="item1">
                                            <div class="title">
                                                <div>评价：</div>
                                                <div><%# string.Format("{0:M}",Eval("DATE_ENTERED")) %></div>
                                            </div>
                                            <div class="context">
                                                <%# Eval("C_EstimateContext") %>
                                            </div>
                                        </div>
                                        <div class="item2">
                                            <div class="title">
                                                <div>收货后7天追加：</div>
                                            </div>
                                            <div class="context">
                                                发货速度很快，气还不错！
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>--%>
                    
                    <div id="loading"><img src="/images/loading.gif" /></div>
                    

                    <div id="Div_PostToData" ng-init="GetOrderEstimateList()">
                        <div class="item" ng-repeat="item in AllEstimateList" style="width:100%; float:left; min-height:100px; padding:10px; border-bottom:solid thin; border-bottom-color:#cccccc;">
                            <div class="left_item">
                                <%--<div class="item1" ng-bind="item.Seller"></div>--%>
                                <div class="item2">
                                    <span ng-bind="item.Seller" style="font-size:15px;font-weight:bold;color:#0065e6;"></span>
                                    <img src="/images/Estimate/SStar2.jpg" />
                                    <img src="/images/Estimate/SStar2.jpg" />
                                    <img src="/images/Estimate/SStar2.jpg" />
                                    <img src="/images/Estimate/SStar2.jpg" />
                                    <img src="/images/Estimate/SStar2.jpg" />
                                </div>
                                <div class="item3">购买产品：<div class="context">{{item.GasTypeName}}：{{item.GasVarietyName}}；{{item.C_GasificationRate}}；{{item.C_GasSourceName}}；</div></div>
                            </div>

                            <div class="right_item">
                                <div class="item1">
                                    <div class="title">
                                        <div>初次评价：</div>
                                        <div ng-bind="item.DATE_ENTERED | date:'MM-dd' " ></div>
                                    </div>
                                    <div class="context" style="vertical-align:top;" ng-bind="item.C_EstimateContext"></div>
                                </div>
<%--                                <div class="item2">
                                    <div class="title"style="width:100px;float:left;">
                                        <div>收货后7天追加：</div>
                                    </div>
                                    <div class="context" style="width:450px;float:right;">
                                        发货速度很快，气还不错！
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>


                    <div>
                        <ul class="pagination">
                            <li>
                                <a href="#" ng-click="paging(1)">
                                    <span aria-hidden="true">首页</span>
                                </a>
                            </li>
                            <li ng-class="{'disabled':currentPageIndex == 1}">
                                <a href="#" aria-label="Previous" ng-click="search(-1,null,pageTotal)">
                                    <span aria-hidden="true">上一页</span>
                                </a>
                            </li>
                            <li ng-repeat="item in pageList" ng-class="{'active':item == currentPageIndex}"><a href="#" ng-click="paging(item)">{{item}}</a></li>
                            <li ng-class="{'disabled':currentPageIndex == pageTotal}">
                                <a href="#" aria-label="Next" ng-click="search(1,null,pageTotal)">
                                    <span aria-hidden="true">下一页</span>
                                </a>
                            </li>
                            <li>
                                <a href="#" ng-click="paging(pageTotal)">
                                    <span aria-hidden="true">末页</span>
                                </a>
                            </li>
                        </ul>

                        <div class="pagingTotal">共{{pageTotal}}页</div>
                    </div>
                </div>

            </div>

        </div>
    </div>
</asp:Content>