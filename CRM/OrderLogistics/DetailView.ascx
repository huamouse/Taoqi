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

<%-- ����������  --%>
<div id="nav">
    <div class="nav_left">
        <span class="title">������ַ
        </span>
        <span class="site_Path">��ǰλ�ã��ͻ� > <span class="current1">������ַ</span>

        </span>
    </div>
    <div class="nav_right">


        <a href="default.aspx">
            <img align="absmiddle" border="0" src="../include/images/back.png" />
            ����</a>

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
                    �˻���Ԫ����<span style="color: #018ccd; font-size: 14px; margin-right: 20px;"><%# Eval("C_StationShortName") %></span>
                    ��֤���ܶԪ����<span style="color: #018ccd; font-size: 14px;"><%# Eval("C_StationCapacity") %></span>
                </div>
                <div class="detail_nav_right" id="dynamicButtons_placeholder"></div>

            </div>


            <div class="tabGroup" style="margin-top: 15px;">

                <div class="tab active" onclick="activeTab(this,'#clientInfo')">�ͻ���Ϣ</div>



            </div>



            <div class="container_body">
                <div id="clientInfo" class="tabDetails">

                    <div class="grid" style="margin-top: 5px;">
                        <div>
                            <div class="title">
                                ������Ϣ
                            </div>
                            <div class="title_expand">
                                <img src="/include/images/grid_collapse.png" align="absmiddle" />
                                չ��/����
                            </div>
                        </div>



                        <table class="formStyle">
                            <tr>
                                <td class="dataLabel">��վ���ƣ� </td>
                                <td class="dataField"><%# Eval("C_StationName") %></td>
                                <td class="dataLabel">��վ��ƣ� </td>
                                <td class="dataField"><%# Eval("C_StationShortName") %></td>
                            </tr>
                            <tr>
                                <td class="dataLabel">��վ���ݣ��֣��� </td>
                                <td class="dataField"><%# Eval("C_StationCapacity") %></td>

                                <td class="dataLabel">�����ص��ʱ��ַ�� </td>
                                <td class="dataField"><%# Eval("C_Address") %></td>
                            </tr>
                            <tr>
                                <td class="dataLabel">������ϵ�ˣ� </td>
                                <td class="dataField"><%# Eval("C_ContactName") %></td>
                                <td class="dataLabel">������ϵ�绰�� </td>
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



