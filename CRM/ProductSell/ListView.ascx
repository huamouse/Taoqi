<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.ProductSell.ListView" %>
<style>
    .form-control {
        font-size: 13px;
    }

    .form-control.select {
        background: url(/images/select.png) scroll right no-repeat;
    }

    select {
        -moz-appearance: none;
        -webkit-appearance: none;
    }

        select::-ms-expand {
            display: none;
        }
</style>

<div class="row nav2">
    <div class="col-md-8 listTitle">�����е���Դ</div>
    <div class="col-md-4">
        <a href="edit.aspx">
            <div class="btnRed_Large" style="float: right;">��������Դ</div>
        </a>
    </div>
</div>

<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel" runat="server" data-ng-controller="productController">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15"
            AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="����" HeaderStyle-Width="225px">
                    <ItemTemplate>
                        <%--                        <div class="btn btnOrange" data-ng-click="modifyModal('<%# Eval("ID") %>')">�۸����</div>
                        <div class="btn btnSecond btnOrange" data-ng-click="modifyModal('<%# Eval("ID") %>', true)">���۷���</div>
                        <div class="btn btnSecond btnOrange">�۸�����</div>--%>
                        <div class="btn btnOrange" data-toggle="modal" data-ng-click="detailModal('<%# Eval("C_ProductID") %>', '<%# Eval("C_ProvinceID") %>')">�鿴����</div>
                        <asp:LinkButton runat="server" ID="btnDelete" class="btn btnSecond btnGray1" OnClientClick="return layer.confirm('��ʾ��ȷ��ɾ����');"
                            CommandName="Delete" CommandArgument='<%# Eval("ID") %>'>ɾ��</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>

        <div class="modal fade" id="detailModal" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    </div>
                    <div class="modal-body">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th class="col-md-2">��</th>
                                    <th class="col-md-2">�����ۣ�Ԫ/�֣�</th>
                                    <th class="col-md-2">�չ�������</th>
                                    <th class="col-md-2">����ʱ��</th>
                                    <th class="col-md-3">����</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="item in mainArea | filter: deletedFilter">
                                    <td class="textCenter" ng-bind="item.C_CityName"></td>
                                    <td class="textCenter">
                                        <div class="input-group">
                                            <span class="input-group-btn">
                                                <button class="btn btn-default" type="button" ng-click="priceSub($index)">-</button>
                                            </span>
                                            <input type="text" id="price{{$index}}" class="form-control textCenter" ng-model="item.C_Price_Min" maxlength="4" ng-pattern="/^[3-6]\d{3}$/" autocomplete="off" />
                                            <span class="input-group-btn">
                                                <button class="btn btn-default" type="button" ng-click="priceAdd($index)">+</button>
                                            </span>
                                        </div>
                                    </td>
                                    <td class="textCenter">
                                        <div class="input-group">
                                            <span class="input-group-btn">
                                                <button class="btn btn-default" type="button" ng-click="quantitySub($index)">-</button>
                                            </span>
                                            <input type="text" id="quantity{{$index}}" class="form-control textCenter" ng-model="item.C_CarQuantity" maxlength="2" ng-pattern="/^\d{1,2}$/" autocomplete="off" />
                                            <span class="input-group-btn">
                                                <button class="btn btn-default" type="button" ng-click="quantityAdd($index)">+</button>
                                            </span>
                                        </div>
                                    </td>
                                    <td class="textCenter">
                                        <span class="textCenter" ng-bind="item.DATE_MODIFIED | date:'yyyy-MM-dd hh:mm'"></span>
                                    </td>
                                    <td class="textCenter">
                                        <div class="btn btnOrange">�۸�����</div>
                                        <div class="btn btnOrange" ng-click="removeDetail($index)">ɾ��</div>
                                    </td>
                                </tr>
                                <tr ng-show="productCityList.length > 0">
                                    <td class="textCenter">
                                        <select class="form-control select" style="margin-top:5px;padding-right:6px;" ng-model="ProductCity" 
                                            ng-options="item.C_CityName for item in productCityList">
                                            <option value="">ѡ�����</option>
                                        </select>
                                    </td>
                                    <td colspan="4">
                                        <div class="btn btnOrange" ng-click="AddProductCity()">���</div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <div class="detailbtn" style="margin-left: 300px;" ng-click="modifyModal(1)">�۸����</div>
                        <div class="detailbtn" style="margin-left: 50px;" ng-click="modifyModal(0)">���۷���</div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
