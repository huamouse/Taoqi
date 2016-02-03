<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.OrderSell.ListView" %>

<div class="row nav2">
    <div class="col-md-8 listTitle">���۶���</div>
</div>

<div class="myorderstatus">
     <a href="default.aspx?scope=0"><div class="orderTab">ȫ��</div></a>
     <a href="default.aspx?scope=1"><div class="orderTab">�¶���</div></a>
     <a href="default.aspx?scope=2"><div class="orderTab">��;</div></a>
     <a href="default.aspx?scope=3"><div class="orderTab">�����</div></a>
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

<asp:HiddenField ID="LAYOUT_LIST_VIEW"  runat="server" />

<asp:UpdatePanel ID="updatePanel"  runat="server">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="����" HeaderStyle-Width="85px">
                    <ItemTemplate>
                        <%# ActionForStatus( Eval("C_Status").ToString(), Eval("ID").ToString() ) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>
    </ContentTemplate>
</asp:UpdatePanel>

<script>
    $("Button[name=Delete]").click(function () {
        if(!confirm('��ȷ��ɾ����?'))
            return false;
    });
    $("Button[name=Received]").click(function () {
        if (!confirm('��ȷ���ջ���?'))
            return false;
    });
</script>







