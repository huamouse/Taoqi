<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQClient.ListView" %>
<div class="row nav2">
    <div class="col-md-8 listTitle">��ҵ�б�</div>
    <div class="col-md-4" style="display:none;">

        <div class="search_bar">
            <input type="text" id="clientname" placeholder="��ҵ���/��ҵ����/��ϵ��" class="search_box" runat="server" />
            <asp:LinkButton runat="server" ID="btnSearch" CommandName="Search">
                <div class="btn_search">
                    <img src="/member/Include/images/search3.jpg" />
                </div>
            </asp:LinkButton>
        </div>
    </div>
</div>

<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="10"
            AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="����" ItemStyle-Width="60px">
                    <ItemTemplate>
                        <asp:LinkButton CommandName="Edit" ID="btnEdit" Text="���" Visible='<%# Eval("C_Status").ToString() == "1" %>' runat="server" CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command"> </asp:LinkButton>
                        <asp:LinkButton CommandName="View" ID="btnView" Text="�鿴" Visible='<%# Eval("C_Status").ToString() == "2" %>' runat="server" CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command"> </asp:LinkButton>
<%--                        <asp:LinkButton CommandName="Delete" ID="btnDelete" Text='ɾ��' Visible='<%# Eval("C_Status").ToString() == "2" %>' runat="server" CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command"
                                OnClientClick="return confirm('��ʾ��ȷ��ɾ����')"> </asp:LinkButton>--%>
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
</script>