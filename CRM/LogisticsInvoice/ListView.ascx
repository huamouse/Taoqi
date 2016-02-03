<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQLogisticsInvoice.ListView" %>


<div class="row nav2">
    <div class="col-md-8 listTitle">我的发票</div>
    <div class="col-md-4">

        <div class="search_bar">
            <input type="text" placeholder="发货企业名称" class="search_box" />

            <div class="btn_search">
                <img src="/member/Include/images/search3.jpg" />
            </div>
        </div>
    </div>
</div>

<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel2" runat="server">

    <ContentTemplate>

        <div class="div_grid">
            <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="10"
                AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
                <Columns>
                </Columns>
            </Taoqi:SplendidGrid>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
