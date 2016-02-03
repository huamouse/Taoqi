<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Users.ListView" %>
<%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="SearchView" Src="~/_controls/SearchView.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="ExportHeader" Src="~/_controls/ExportHeader.ascx" %>

<div class="row nav2" style="height:35px;">
    <div class="col-md-8 listTitle">用户列表</div>
    <div class="col-md-4" style="display:none;">
        <div class="search_bar">
            <input type="text" id="username" placeholder="真实姓名/手机号" class="search_box" runat="server"/>

            <asp:LinkButton runat="server" ID="btnSearch" CommandName="Search">
                    <div class="btn_search">
                        <img src="/member/Include/images/search3.jpg" />
                    </div>
            </asp:LinkButton>
        </div>
    </div>
</div>
<div>
    <div class="container_body">

          <div style="display:none">
        <Taoqi:SearchView  ID="ctlSearchView" Module="Users" Visible="<%# !PrintView %>" ShowSearchTabs="false" ShowSearchViews="false" runat="Server" />

      
            <Taoqi:ExportHeader ID="ctlExportHeader" Module="Users" Title="Users.LBL_LIST_FORM_TITLE" runat="Server" />
        </div>


        <asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
            <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
        </asp:Panel>

        <asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
            <Columns>
                <%--<asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" Visible="false">
                    <ItemTemplate>
                        <asp:HyperLink NavigateUrl='<%# "~/" + m_sMODULE + "/edit.aspx?id=" + Eval("ID") %>' ToolTip='<%# L10n.Term(".LNK_EDIT") %>' runat="server">
						<asp:Image SkinID="edit_inline" Runat="server" />
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Users.LBL_LIST_ADMIN" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                        <div style="DISPLAY: <%# Sql.ToInteger(DataBinder.Eval(Container.DataItem, "IS_ADMIN")) == 1 ? "inline" : "none" %>">
                            <asp:Image SkinID="check_inline" runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" Visible="False">
                    <ItemTemplate>
                        <asp:ImageButton Visible='<%# Taoqi.Security.isAdmin %>' OnClientClick="return confirm('确认继续吗？');" CommandName="Impersonate" CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText="模拟用户" SkinID="users" runat="server" />
                        <asp:LinkButton Visible='<%# Taoqi.Security.isAdmin %>' OnClientClick="return confirm('确认继续吗？');" CommandName="Impersonate" CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text="模拟用户" runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>--%>
                <asp:TemplateColumn HeaderText="操作">
                    <ItemTemplate>
                    <asp:LinkButton CommandName="Agree"  Text='同意加入' Runat="server" ID="btnAgree" CommandArgument='<%# Eval("ID") %>' Visible='<%# !bool.Parse(Eval("C_Status").ToString() == "0" ? "false" : "true") %>'></asp:LinkButton>
                    <asp:LinkButton CommandName="Edit" ID="btnEdit" runat="server" Text="编辑信息" CommandArgument='<%# Eval("ID") %>'  Visible='<%# bool.Parse(Eval("C_Status").ToString() == "0" ? "false" : "true") %>'></asp:LinkButton>
                        &nbsp
                    <asp:LinkButton runat="server" Text="移除" ID="btnDelete" OnClientClick="return confirm('提示：确认移除吗？');" CommandName="Delete" CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>

        <%@ Register TagPrefix="Taoqi" TagName="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
        <Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" runat="Server" />

    </div>
</div>



