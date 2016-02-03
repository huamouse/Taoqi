<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQClientAddress.ListView" %>

<div class="row nav2">
    <div class="col-md-8 listTitle" style="margin-top:0.26em;">到岸地管理</div>
    <div class="col-md-4">
        <a href="edit.aspx">
            <div class="btnRed_Large" style="float: right;margin-bottom:0.3em;">新增到岸地</div>
        </a>
    </div>
</div>

<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel2" runat="server">
    <ContentTemplate>
        <div class="div_grid" id="dadgl">
            <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15"
                AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
                <Columns>
                    <asp:TemplateColumn HeaderText="操作">
                        <ItemTemplate>
                            <asp:HyperLink Text='编辑' NavigateUrl='<%# "edit.aspx?ID=" + DataBinder.Eval(Container.DataItem, "ID") %>' runat="server" />
                            <asp:LinkButton runat="server" Text="删除" ID="btnDelete" OnClientClick="return confirm('提示：确认删除吗？');" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </Taoqi:SplendidGrid>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

