<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="DetailView.ascx.cs" Inherits="Taoqi.TQOrderLogistics.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>


<script type="text/javascript">
    function activeTab(obj, targetId) {
        $("div.tab").removeClass("active");
        $(obj).addClass("active");

        $("div.tabDetails").hide();
        $(targetId).show();

    }

    $(function () {
        $("#dynamicButtons").appendTo("#dynamicButtons_placeholder");
    });
</script>

<%-- 导航栏区域  --%>
<div id="nav">
    <div class="nav_left">
        <span class="title">到岸地址
        </span>
        <span class="site_Path">当前位置：客户 > <span class="current1">到岸地址</span>

        </span>
    </div>
    <div class="nav_right">


        <a href="default.aspx">
            <img align="absmiddle" border="0" src="../include/images/back.png" />
            返回</a>

    </div>

</div>
<asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

<div id="divDetailView" runat="server">
    <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
    <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQOrderLogistics" EnablePrint="false" HelpName="DetailView" EnableHelp="false" runat="Server" />

    <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>

    <div id="dynamicButtons">
        <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# !PrintView %>" runat="Server" />
    </div>
    <table id="tblMain" class="tabDetailView" runat="server" visible="false" />

    <asp:Repeater runat="server" ID="rptShow">
        <ItemTemplate>


            <div class="chartTitle" style="margin-top: 20px;">

                <div class="detail_nav">
                    <div class="txt" style="width: auto;"><%# Eval("C_StationName") %></div>
                </div>
                <div class="detail_nav" style="margin-left: 20px;">
                    账户余额（元）：<span style="color: #018ccd; font-size: 14px; margin-right: 20px;"><%# Eval("C_StationShortName") %></span>
                    保证金总额（元）：<span style="color: #018ccd; font-size: 14px;"><%# Eval("C_StationCapacity") %></span>
                </div>
                <div class="detail_nav_right" id="dynamicButtons_placeholder"></div>

            </div>


            <div class="tabGroup" style="margin-top: 15px;">

                <div class="tab active" onclick="activeTab(this,'#clientInfo')">客户信息</div>



            </div>



            <div class="container_body">
                <div id="clientInfo" class="tabDetails">

                    <div class="grid" style="margin-top: 5px;">
                        <div>
                            <div class="title">
                                基本信息
                            </div>
                            <div class="title_expand">
                                <img src="/include/images/grid_collapse.png" align="absmiddle" />
                                展开/收起
                            </div>
                        </div>



                        <table class="formStyle">
                            <tr>
                                <td class="dataLabel">场站名称： </td>
                                <td class="dataField"><%# Eval("C_StationName") %></td>
                                <td class="dataLabel">场站简称： </td>
                                <td class="dataField"><%# Eval("C_StationShortName") %></td>
                            </tr>
                            <tr>
                                <td class="dataLabel">场站罐容（吨）： </td>
                                <td class="dataField"><%# Eval("C_StationCapacity") %></td>

                                <td class="dataLabel">到岸地点邮编地址： </td>
                                <td class="dataField"><%# Eval("C_Address") %></td>
                            </tr>
                            <tr>
                                <td class="dataLabel">到岸联系人： </td>
                                <td class="dataField"><%# Eval("C_ContactName") %></td>
                                <td class="dataLabel">到岸联系电话： </td>
                                <td class="dataField"><%# Eval("C_Tel") %></td>
                            </tr>


                        </table>





                    </div>



                </div>
                </div>
        </ItemTemplate>
    </asp:Repeater>

    <div id="divDetailSubPanel">
        <asp:PlaceHolder ID="plcSubPanel" runat="server" />
    </div>
</div>



