<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQCar.ListView" %>


<div class="row nav2">
    <div class="col-md-8 listTitle">��������</div>
    <div class="col-md-4">
        <a href="edit.aspx">
            <div class="btnRed_Large" style="float: right;">�½���������</div>
        </a>
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
                <asp:TemplateColumn HeaderText="����">
                    <ItemTemplate>
                        <asp:HyperLink Text='�༭' NavigateUrl='<%# "edit.aspx?ID=" + DataBinder.Eval(Container.DataItem, "ID") %>' runat="server" />
                        <asp:LinkButton runat="server" Text="ɾ��" ID="btnDelete" OnClientClick="return confirm('��ʾ��ȷ��ɾ����');" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
         </Taoqi:SplendidGrid>
    </ContentTemplate>
</asp:UpdatePanel>
