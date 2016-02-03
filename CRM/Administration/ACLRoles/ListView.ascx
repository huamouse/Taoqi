<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Administration.ACLRoles.ListView" %>



<div class="row nav2">
    <div class="col-md-8 listTitle">角色列表</div>
    <div class="col-md-4">

        <div class="search_bar">
            <input type="text" placeholder="角色名" class="search_box" />

            <div class="btn_search">
                <img src="/member/Include/images/search3.jpg" />
            </div>
        </div>
    </div>
</div>


<div class="container_body">
    <div style="display: none;">
        <asp:PlaceHolder ID="plcSearch" Visible="<%# !PrintView %>" runat="server" />

        <%@ Register TagPrefix="Taoqi" TagName="ListHeader" Src="~/_controls/ListHeader.ascx" %>
        <Taoqi:ListHeader Title="ACLRoles.LBL_ROLE" runat="Server" />

    </div>
    <asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
        <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
    </asp:Panel>

    <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
        <Columns>
            <%--<asp:TemplateColumn HeaderText="" ItemStyle-Width="1%">
                <ItemTemplate><%# grdMain.InputCheckbox(!PrintView, ctlCheckAll.FieldName, Sql.ToGuid(Eval("ID")), ctlCheckAll.SelectedItems) %></ItemTemplate>
            </asp:TemplateColumn>--%>
            <asp:TemplateColumn HeaderText=".LBL_LIST_DEFAULT" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                <ItemTemplate>
                    <div style="display: <%# String.Compare(Sql.ToString(DataBinder.Eval(Container.DataItem, "ID")), Sql.ToString(Application["CONFIG.default_role"]), true) == 0 ? "inline" : "none" %>">
                        <asp:ImageButton Visible='<%# Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0 %>' CommandName="ACLRole.MakeDefault" CommandArgument='' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LNK_CLEAR_DEFAULT") %>' SkinID="check_inline" runat="server" />
                        &nbsp;<%= L10n.Term(".LBL_YES") %>
                    </div>
                    <div style="display: <%# String.Compare(Sql.ToString(DataBinder.Eval(Container.DataItem, "ID")), Sql.ToString(Application["CONFIG.default_role"]), true) != 0 ? "inline" : "none" %>">
                        <asp:ImageButton Visible='<%# Taoqi.Security.AdminUserAccess(m_sMODULE, "edit") >= 0 %>' CommandName="ACLRole.MakeDefault" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LBL_DEFAULT") %>' SkinID="decline_inline" runat="server" />
                        &nbsp;<%= L10n.Term(".LBL_NO") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="操作" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
                <ItemTemplate>
                    <span onclick="return confirm('<%= L10n.TermJavaScript(".NTC_DELETE_CONFIRMATION") %>')">
                        <asp:ImageButton Visible='<%# Taoqi.Security.AdminUserAccess(m_sMODULE, "delete") >= 0 %>' CommandName="ACLRole.Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LNK_DELETE") %>' SkinID="delete_inline" runat="server" />
                        <asp:LinkButton Visible='<%# Taoqi.Security.AdminUserAccess(m_sMODULE, "delete") >= 0 %>' CommandName="ACLRole.Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term(".LNK_DELETE") %>' runat="server" />
                    </span>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </Taoqi:SplendidGrid>

    <div runat="server" visible="false">
        <%@ Register TagPrefix="Taoqi" TagName="CheckAll" Src="~/_controls/CheckAll.ascx" %>
        <Taoqi:CheckAll ID="ctlCheckAll" Visible="<%# !PrintView %>" runat="Server" />

        <%@ Register TagPrefix="Taoqi" TagName="MassUpdate" Src="MassUpdate.ascx" %>
        <Taoqi:MassUpdate ID="ctlMassUpdate" runat="Server" />
    </div>

</div>

<a href="edit.aspx">
    <div class="btnBlue_large">新建角色</div>
</a>



