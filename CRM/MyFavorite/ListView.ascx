<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQMyFavorite.ListView" %>

<div class="row nav2">
    <div class="col-md-8 listTitle" style="margin-bottom:0.4em;">我的关注</div>
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
                    <asp:TemplateColumn HeaderText="操作" >
                        <ItemTemplate>
                        <asp:LinkButton runat="server" Text="取消关注" ID="btnDelete" OnClientClick="return confirm('提示：确认取消关注吗？');" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                       </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </Taoqi:SplendidGrid>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

