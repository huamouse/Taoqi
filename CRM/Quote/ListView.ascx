<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQQuote.ListView" %>

<%@ Register TagPrefix="cc" Assembly="Taoqi" Namespace="Taoqi.TQQuote" %>
<style>
    .layui-layer-btn a{
        width:135px;
    }
</style>
<div class="row nav2">
    <div class="col-md-8 listTitle" style="margin-top: 0.17em;">我的求购</div>
    <div class="col-md-4" style="margin-bottom: 0.2em;">
        <a href="../Quote/edit.aspx">
            <div class="btnRed_Large" style="float: right;">发布新求购</div>
        </a>
    </div>
</div>

<div class="myorderstatus">
    <a href="default.aspx?scope=0">
        <div class="orderTab">全部</div>
    </a>
    <a href="default.aspx?scope=1">
        <div class="orderTab">求购中</div>
    </a>
    <a href="default.aspx?scope=2">
        <div class="orderTab">等待审核</div>
    </a>
    <a href="default.aspx?scope=3">
        <div class="orderTab">审核未通过</div>
    </a>
</div>
<style>
    a#ctl00_cntBody_ctlListView_btnConfirm:hover {
        background-color: transparent;
    }
    /*#quoteModal a:hover{
        background-color:transparent;
    }*/
    .layui-layer-btn0 {
        width: 116px;
    }

    .layui-layer-btn1 {
        width: 121px;
    }
    #pricequote:hover{
        background-color:transparent !important;
    }
    .gasInformation {
        margin-right: 4px;
        float:left;
    }
</style>
<script>
    var scope = "<%=Request.QueryString["scope"] %>";

    $(".orderTab").each(function (index, item) {
        if (scope == index) {
            $(item).addClass("actived");
        }
    });
</script>

<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15"
            AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="状态" HeaderStyle-Width="85px">
                    <ItemTemplate>
                        <cc:QuoteButton runat="server" ID="btnConfirm" CommandName="QuoteView" CommandArgument='<%# Eval("ID") %>' Status='<%# Eval("C_Status") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <Columns>
                <asp:TemplateColumn HeaderText="操作" HeaderStyle-Width="85px">
                    <ItemTemplate>
                        <cc:ActionQuote QuoteCheck='<%# Eval("C_Status") %>' QuoteId='<%# Eval("ID") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>

        <div class="modal fade" id="quoteModal" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <% if (IsView)
                           { %>
                        <h3>查看报价</h3>
                        <% }
                           else
                           { %>
                        <h3>查看成交</h3>
                        <% } %>
                    </div>
                    <div class="modal-body">
                        <asp:Repeater runat="server" ID="rptModal">
                            <ItemTemplate>
                                <div class="tableStyle1">
                                    <span class="bjgsxx" style="width: auto">公司名称：<span style="color:#0065e6;"><%# Eval("Seller") %></span></span>
                                    <span class="bjgsxx" style="width: auto; margin-left:20px;">联系人：<span style="color:#ff6400;"><%# Eval("SellerName") %></span></span>
                                    <span class="bjgsxx" style="width: auto; margin-left:20px;">手机：<span style="color:#ff6400;"><%# Eval("SellerPhone") %></span></span>
                                    <%--<span class="bjgsxx" style="width: auto; margin-left:20px;">QQ：<span style="color:#0065e6;"><%# Eval("SellerQQ") %></span></span>--%>

                                    <div class="table-responsive" style="overflow-x: visible;margin-top:10px;">
                                        <table class="table blueTable addright">
                                            <thead>
                                                <tr>
                                                    <th style="width:46px;">操作</th>
                                                    <th style="width:401px;">气源信息</th>
                                                    <th style="width:110px;">报价（元/吨）</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater runat="server" ID="rptPrice">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="textCenter">
                                                                <asp:CheckBox ID="ckPrice" runat="server" Checked='<%# Eval("C_IsConfirm") %>' Enabled="<%# IsView %>" />
                                                            </td>
                                                            <td class="textCenter">
                                                                <span class="bjgsxx" style="margin-left: 5px; width: auto;float:left;color:#a09c96;">
                                                                    <span class="gasInformation" style="color:#595959;"><%# Eval("C_GasSourceName") %>;</span><span class="gasInformation" style="color:#595959;"><%# Eval("C_GasificationRate") %>;</span><span class="gasInformation" style="color:#595959;"><%# Eval("C_CalorificValue") %>;</span><span class="gasInformation" style="color:#595959;"><%# Eval("LiquidTemperatureName") %>;</span><br />
                                                                    <span class="gasInformation"><%# Eval("C_TypeOfPay_Name") %>;</span><span class="gasInformation"><%# Eval("C_TypeOfCarFuel_Name") %>;<%# Eval("C_Tonnage") %>;</span><span class="gasInformation"><%# Eval("C_StandardOfYaChe_Name") %>;</span><br />
                                                                    <span class="gasInformation"><%# Eval("C_ProcessOfGasDifference_Name") %></span>
                                                                </span>
                                                            </td>
                                                            <td class="textCenter">
                                                                ¥<a class="bjgsgc" id="pricequote" style="color: #ff6400;">&nbsp;<%# Eval("C_Price") %>&nbsp;</a>
                                                                <asp:TextBox Visible="false" ID="quotePriceID" Text='<%# Eval("ID") %>' runat="server" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
<%--                        <asp:Repeater runat="server" ID="rptModal">
                            <ItemTemplate>
                                <div class="bjgs" style="margin-bottom: 5px;">
                                    <div>
                                        <span class="bjgsxx" style="width:auto">公司名称：<a class="bjgsjt"><%# Eval("Seller") %></a></span>
                                        <span class="bjgsxx" style="margin-left: 15px;">联系人：<a class="bjgsjt"><%# Eval("SellerUserName") %></a></span>
                                        <span class="bjgsxx" style="margin-left: 15px;">手机：<a class="bjgsjt"><%# Eval("SellerPhone") %></a></span>
                                        <span class="bjgsxx" style="margin-left: 15px;">QQ：<a class="bjgsjt"><%# Eval("SellerQQ") %></a></span>
                                    </div>
                                    <div>
                                        <asp:Repeater runat="server" ID="rptPrice">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:CheckBox id="ckPrice" runat="server" Checked='<%# Eval("C_IsConfirm") %>' Enabled="<%# IsView %>" />
                                                    <span class="bjgsxx" style="margin-left: 5px; width: auto;">气源地：<a class="bjgsgc">
                                                        <span class="gasInformation"><%# Eval("C_GasSourceName") %>;</span><span class="gasInformation"><%# Eval("C_GasificationRate") %>;</span><span class="gasInformation"><%# Eval("C_CalorificValue") %>;</span><span class="gasInformation"><%# Eval("LiquidTemperatureName") %>;</span>
                                                        <span class="gasInformation"><%# Eval("C_TypeOfPay_Name") %>;</span><span class="gasInformation"><%# Eval("C_TypeOfCarFuel_Name") %>;</span><span class="gasInformation"><%# Eval("C_StandardOfYaChe_Name") %>;</span><span class="gasInformation"><%# Eval("C_ProcessOfGasDifference_Name") %></span>
                                                    </a>
                                                    </span>
                                                    <span class="bjgsxx" style="margin-left: 10px;">报价：¥<a class="bjgsgc" style="color: #ff6a00;">&nbsp;<%# Eval("C_Price") %>&nbsp;</a>元/吨</span>
                                                    <asp:TextBox Visible="false" ID="quotePriceID" Text='<%# Eval("ID") %>' runat="server" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>--%>
                    </div>
                    <% if (IsView)
                       { %>
                    <div class="modal-footer">
                        <div class="btnGray1" data-dismiss="modal">取消</div>
                        <asp:LinkButton runat="server" ID="btnConfirm">
                            <div class="btnOrange">确认</div>
                        </asp:LinkButton>
                    </div>
                    <% } %>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

