<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TransitSell.ListView" %>

<%@ Register TagPrefix="cc" Assembly="Taoqi" Namespace="Taoqi.TransitSell" %>

<div class="row nav2">
    <div class="col-md-8 listTitle">出售中的在途气</div>
    <div class="col-md-4">
        <a href="edit.aspx">
            <div class="btnRed_Large" style="float: right;">发布新在途气</div>
        </a>
    </div>
</div>
<div class="myorderstatus">
    <a href="default.aspx?scope=0"><div class="orderTab">全部</div></a>
     <a href="default.aspx?scope=1"><div class="orderTab">出售中</div></a>
     <a href="default.aspx?scope=2"><div class="orderTab">等待审核</div></a>
     <a href="default.aspx?scope=3"><div class="orderTab">审核未通过</div></a>
</div>
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

<asp:UpdatePanel ID="updatePanel" runat="server" data-ng-controller="orderController">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="操作" HeaderStyle-Width="85px">
                    <ItemTemplate>
                        <cc:ActionInTransit C_Status='<%# Eval("C_Status").ToString() %>' TransitID='<%# Eval("ID").ToString() %>' 
                            LandingUrl='<%# Eval("C_LandingUrl") %>' Driver='<%# Eval("C_Driver") %>' DriverTel='<%# Eval("C_Tel") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>

        <div class="modal fade" id="TransitSellModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h3>查看抢购</h3>
                    </div>
                    <div class="modal-body">
                        <input type="hidden" id="C_TransitID" runat="server" />
                        <table class="table">
                            <thead>
                                <th>选择</th>
                                <th>公司名称</th>
                                <th>联系人</th>
                                <th>联系电话</th>
                                <th>QQ</th>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rptModal">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <input type="checkbox" id="ckPrice" runat="server" />
                                                <input type="hidden" id="MyTransitID" runat="server" />
                                            </td>
                                            <td><%# Eval("Buyer") %></td>
                                            <td><%# Eval("BuyerName") %></td>
                                            <td><%# Eval("BuyerPhone") %></td>
                                            <td><%# Eval("BuyerQQ") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <button class="btnGray1" data-dismiss="modal">取消</button>
                        <button name="Command" class="btnOrange" value="btnConfirm">确认</button>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

