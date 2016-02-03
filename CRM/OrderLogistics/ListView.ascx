<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQOrderLogistics.ListView" %>

<div class="row nav2">
    <div class="col-md-8 listTitle">我的物流</div>
    <div class="col-md-4" style="height:35px;display:none;">

        <div class="search_bar">
            <asp:TextBox runat="server" ID="txtKeyword" placeholder="驾驶员/联系电话" CssClass="search_box"></asp:TextBox>

            <asp:LinkButton runat="server" ID="btnSearch" CommandName="Search">
                    <div class="btn_search">
                        <img src="/member/Include/images/search3.jpg" />
                    </div>
            </asp:LinkButton>
        </div>
    </div>
</div>
<script type="text/javascript">
    $("#<%= this.txtKeyword.ClientID %>").keypress(function (e) {
        if (e.keyCode == 13) {
            document.getElementById("<%= this.btnSearch.ClientID %>").click();
        }
    })
</script>

<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel2" runat="server">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="10"
            AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
            </Columns>
        </Taoqi:SplendidGrid>
    </ContentTemplate>
</asp:UpdatePanel>
