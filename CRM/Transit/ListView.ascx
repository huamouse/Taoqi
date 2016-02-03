<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQTransitMy.ListView" %>

<div class="nav2">
    <div class="listTitle" style="margin-bottom:0.5em;">�ҵ���;��</div>
</div>

<asp:Panel ID="Panel" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel" runat="server" data-ng-controller="orderController">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15"
            AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="״̬/����" HeaderStyle-Width="175px">
                    <ItemTemplate>
                        <%# ActionFor(Eval("C_Status"), Eval("ID"), Eval("C_CarID"), Eval("C_LandingUrl"), Eval("C_Driver"), Eval("C_Tel")) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>
    </ContentTemplate>
</asp:UpdatePanel>

<script>
    $("button[name=Delete]").click(function () {
        return confirm('��ʾ��ȷ��ȡ��������');
    });
    $("button[name=Finish]").click(function () {
        return confirm('��ʾ��ȷ���ջ���');
    });
</script>