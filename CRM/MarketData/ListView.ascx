<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQMarketData.ListView" %>


<div class="row nav2">
    <div class="col-md-8 listTitle">行情数据</div>
    <div class="col-md-4">

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

 <a href="MD_EditView.aspx" style="width:142px;"><div class="btnBlue_large">新建</div></a>