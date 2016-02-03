<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQOrder.ListView" %>
<%@ Register Src="~/_controls/DatePicker.ascx" TagPrefix="uc1" TagName="DatePicker" %>
<%@ Register TagPrefix="Taoqi" TagName="SearchView" Src="~/_controls/SearchView.ascx" %>
<style>
    #ctl00_cntBody_ctlListView_ctlSearchView_btnSearch {
        background-color: #ff6400;
        position:absolute;
        top:14.8em;
        right:1em;
    }

    #ctl00_cntBody_ctlListView_ctlSearchView_tblSearch {
        grid-row-span: 1;
    }

    #ctl00_cntBody_ctlListView_ctlSearchView_SN {
        position: absolute;
        left: 6em;
    }

    #ctl00_cntBody_ctlListView_ctlSearchView_Seller {
        position: absolute;
        left: 57em;
    }

    .marginleft {
        position: relative;
        left: -8em;
        top: 0.3em;
    }

    .line1 {
        position: absolute;
        left: 23.4em;
    }

    .line2 {
        position: absolute;
        left: 36.4em;
    }

    .dptclass {
    }

    .dptd10 {
        position: relative;
        left: 2.3em;
        top: 0.3em;
    }

    .dptd0 {
        padding-top: 0.3em;
        padding-left: 0.4em;
    }

    .dptd7 {
        font-size: 13px;
        position: absolute;
        left: 0.4em;
        top: 0.3em;
    }

    .dptd4 {
        font-size: 13px;
        position: absolute;
        left: -1.2em;
        top: 0.3em;
    }

    .dptd5 {
        position: absolute;
        left: 0.7em;
    }

    .dptd8 {
        position: absolute;
        left: 2.2em;
    }

    #ctl00_cntBody_ctlListView_ctlSearchView_SN, #ctl00_cntBody_ctlListView_ctlSearchView_DATE_ENTERED_AFTER_txtDATE, #ctl00_cntBody_ctlListView_ctlSearchView_DATE_ENTERED_BEFORE_txtDATE, #ctl00_cntBody_ctlListView_ctlSearchView_Seller {
        height: 2em;
        width:10em;
    }

    .formStyle .dataLabel {
        height: 29px;
    }
</style>
<div id="ordertop">
    <div class="ordertopleft">
        <div class="headportrait">
            <div class="headportrait1">
                <img src="../include/images/userPhoto.jpg" width="98" />
            </div>
        </div>
        <div class="personalinfo">
            <div class="telno">
                <label class="telnumber"><%= Security.FULL_NAME %></label>
                <label class="telgreet">，您好！</label>
            </div>
            <div class="datacompeletion" ng-switch="baseData.Percent">
                <label class="datacomleft">资料完整度：</label>
                <div ng-switch-when="3">
                    <div class="completitionline">
                        <div class="completitionline1" style="width:110px;">60%</div>
                        <div class="completitionline2" style="width:80px;"></div>
                    </div>
                    <a class="complete" href="../users/PersonalInfo.aspx">立即完善</a>
                </div>
                <div ng-switch-when="4">
                    <div class="completitionline">
                        <div class="completitionline1" style="width:150px;">80%</div>
                        <div class="completitionline2" style="width:40px;"></div>
                    </div>
                    <a class="complete" href="../users/PersonalInfo.aspx">立即完善</a>
                </div>
                <div ng-switch-when="5">
                    <div class="completitionline">
                        <div class="completitionline1" style="width:190px;">100%</div>
                    </div>
                </div>
                <div ng-switch-default>
                    <div class="completitionline">
                        <div class="completitionline1" style="width:80px;">40%</div>
                        <div class="completitionline2" style="width:110px;"></div>
                    </div>
                    <a class="complete" href="../users/PersonalInfo.aspx">立即完善</a>
                </div>
            </div>
            <div class="accountsecurity">
                <label class="securitylevel">账户安全级别：</label>
                <div class="securitylevelright">
                    <label class="securityleveltxt">中</label>
                    <div class="securitylevelright2">
                        <div class="securitylevelimg">
                            <a href="../Users/BindingPhone_InitPage.aspx"><img class="securitylevelimg1" src="../App_Themes/Atlantic/images/securitylevelimg1.png" /></a>
                            <a href="../Users/ModifyPassword_password.aspx"><img class="securitylevelimg2" src="../App_Themes/Atlantic/images/securitylevelimg2.png" /></a>
                            
                        </div>
                        <a class="securitylevela" href="../users/ModifyPassword_password.aspx">立即提升</a>
                    </div>
                </div>
            </div>
        </div>
        <div>
        </div>
    </div>

    <div class="ordertopright">
        <div class="ordertoprightsmall" ng-controller="MessageController" ng-init="GetAllMessage()">
            <div class="infotitle">
                <label class="myinfo">我的未读消息</label>

                <span class="badge" style="background-color: #f90a0d; margin-top: 5px;" ng-bind="entity.UnreadCount"></span>
                <label class="allinfo"><a href="/member/Message/">全部消息</a></label>
            </div>
            <div class="concreteinfo">
                <div class="concreteinfo1" ng-repeat="item in messageList | limitTo:4">
                    <div>
                        <div class="concreteinfo1left" style="color: grey" ng-if="item.C_Flag == true" ng-bind="item.C_Content"></div>
                        <div class="concreteinfo1left" ng-if="item.C_Flag == false" ng-bind="item.C_Content"></div>
                        <div class="concreteinfo1right" ng-bind="item.DATE_ENTERED | date: 'yyyy-MM-dd'"></div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</div>

<div id="myordertitle">
    我的订单
</div>

<div id="searchdiv">&nbsp
    <table border="0" style="width:100%;">
        <tr>
            <td class="searchlist">订单编号：</td>
            <td class="searchlist"><asp:TextBox runat="server" ID="txtSN"></asp:TextBox>&nbsp;</td>
            <td class="searchlist"">下单日期：</td>
            <td class="searchlist">
                <uc1:DatePicker runat="server" ID="dateFrom" />
            </td>
            <td class="searchlist">至</td>
            <td class="searchlist">
                <uc1:DatePicker runat="server" ID="dateTo" />
            </td>
            <td class="searchlist">公司名称：</td>
            <td class="searchlist"><asp:TextBox runat="server" ID="txtCompanyName"></asp:TextBox>&nbsp;</td>
            <td class="searchlist"><button name="search" class="btn btnOrange">搜索</button></td>
<%--            <td class="searchlist"><asp:button CommandName="btnSearch" runat="server" class="btn btnOrange"  text="搜索"></asp:button></td>--%>
        </tr>
    </table>
</div>

<%--<div id="searchview" style="margin-top: 15px">
    <Taoqi:SearchView ID="ctlSearchView" Module="TQOrder" Visible="<%# !PrintView %>" ShowSearchTabs="false" ShowSearchViews="false" runat="Server" />
</div>--%>

<div class="myorderstatus">
    <a href="default.aspx?scope=0">
        <tab class="orderTab">全部</tab>
    </a>
    <a href="default.aspx?scope=1">
        <tab class="orderTab">新订单</tab>
    </a>
    <a href="default.aspx?scope=2">
        <tab class="orderTab">在途</tab>
    </a>
    <a href="default.aspx?scope=3">
        <tab class="orderTab">已完成</tab>
    </a>
</div>

<script>
    var scope = "<%=Request.QueryString["scope"] %>";

    $(".orderTab").each(function (index, item) {
        if (scope == index) {
            $(item).addClass("actived");
        }
    });

    var $datepick = $('#ctl00_cntBody_ctlListView_ctlSearchView_tblSearch td:eq(3)');
    var $date = $('#ctl00_cntBody_ctlListView_ctlSearchView_tblSearch td:eq(2)');
    var $datepicktd10 = $('#ctl00_cntBody_ctlListView_ctlSearchView_tblSearch td:eq(10)');
    var $datepicktd0 = $('#ctl00_cntBody_ctlListView_ctlSearchView_tblSearch td:eq(0)');
    var $datepicktd7 = $('#ctl00_cntBody_ctlListView_ctlSearchView_tblSearch td:eq(7)');
    var $datepicktd4 = $('#ctl00_cntBody_ctlListView_ctlSearchView_tblSearch td:eq(4)');
    var $datepicktd5 = $('#ctl00_cntBody_ctlListView_ctlSearchView_tblSearch td:eq(5)');
    var $datepicktd8 = $('#ctl00_cntBody_ctlListView_ctlSearchView_tblSearch td:eq(8)');
    $datepicktd7.addClass('dptd7');
    $datepicktd8.addClass('dptd8');
    $datepicktd5.addClass('dptd5');
    $datepicktd4.addClass('dptd4');
    $datepicktd0.addClass('dptd0');
    $datepicktd10.addClass('dptd10');
    $date.addClass('marginleft');
    var $datepicktable = $datepick.find('table');
    $datepicktable.addClass('dptclass');
    var $datepick1 = $datepick.find('tr:eq(0)');
    var $datepick2 = $datepick.find('tr:eq(2)');
    $datepick1.addClass('line1');
    $datepick2.addClass('line2');

</script>


<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel2" runat="server" data-ng-controller="transitController">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="操作" ItemStyle-Width="170px" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# ActionForStatus( Eval("C_Status").ToString(), Eval("ID").ToString() ) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>
    </ContentTemplate>
</asp:UpdatePanel>

<script>
    $("#ctl00_cntBody_ctlListView_ctlSearchView_btnSearch").val("搜索")
    $("ctl00_cntBody_ctlListView_DatePicker1_txtDATE")
    $("ctl00_cntBody_ctlListView_DatePicker2_txtDATE")
    $("Button[name=Delete]").click(function () {
        return confirm('您确定删除吗?');
    });
</script>







