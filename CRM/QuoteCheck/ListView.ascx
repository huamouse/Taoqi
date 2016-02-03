<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQuoteCheck.ListView" %>

<div class="row nav2">
    <div class="col-md-8 listTitle">求购发布审核</div>
    <div class="col-md-4" style="display:none;">

        <div class="search_bar">

            <asp:TextBox runat="server" ID="txtKeyword" placeholder="审核人" CssClass="search_box"></asp:TextBox>
            

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
    });
</script>

<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel2" runat="server">
    <ContentTemplate>
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15"
                AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="详情" >
                    <ItemTemplate>
                        <asp:HyperLink Text='查看详情' NavigateUrl='<%# "view.aspx?ID=" + DataBinder.Eval(Container.DataItem, "ID") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="操作" ItemStyle-Width="160px">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnEdit" Visible='<%# Eval("C_Status").ToString() == "0" %>' OnClientClick="return confirm('提示：确认通过吗？');" CommandName="QuotePass" CommandArgument='<%# Eval("ID") %>'>
                            <div class="btnOrange">审核通过</div>
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnDelete" Visible='<%# Eval("C_Status").ToString() == "0" %>'  OnClientClick="return confirm('提示：确认不予通过吗？');" CommandName="QuoteNoPass" CommandArgument='<%# Eval("ID") %>'>
                            <div class="btnSecond btnGray1">不予通过</div>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>
    </ContentTemplate>
</asp:UpdatePanel>




