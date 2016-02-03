<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Users.ListView2" %>
<%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="SearchView" Src="~/_controls/SearchView.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="ExportHeader" Src="~/_controls/ExportHeader.ascx" %>

<div class="row nav2">
    <div class="col-md-8 listTitle">用户列表</div>
    <div class="col-md-4" style="display:none;">
        <div class="search_bar">
            <input type="text" id="username" placeholder="用户名/真实姓名" class="search_box" runat="server"/>
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
           <asp:TemplateColumn HeaderText="操作">
                    <ItemTemplate>                   
                    <asp:LinkButton CommandName="Edit" ID="btnEdit" runat="server" Text="编辑" CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command"> </asp:LinkButton>
                        <span id="userdelete" OnClick="return confirm('提示：确认删除吗？')">
                    <asp:LinkButton CommandName="Delete"  Text='删除' Runat="server" ID="btnDelete"  CommandArgument='<%# Eval("ID") %>' OnCommand="Page_Command"> </asp:LinkButton>
                         </span>
                    </ItemTemplate>
                </asp:TemplateColumn>
                </Columns>
        </Taoqi:SplendidGrid>

        <%@ Register TagPrefix="Taoqi" TagName="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
        <Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" runat="Server" />
    </div>
</div>
<%--<a href="edit.aspx"><div class="btnBlue_large">新建用户</div></a>--%>


