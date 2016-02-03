<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQMessage.ListView" %>

<style>
    table, td {
        font-size: 13px;
        color: black;
        border:none;
    }

    .gridView1  tr.oddListRowS1,
    .gridView1  tr.evenListRowS1 {
        padding: 2px 8px 4px 5px;
        border-bottom: 1px dotted #cccccc;
        vertical-align: middle;
        height: 35px;
    }

    .gridView1 tr > td {
        height: 30px;
        padding-left: 13px;
        border:none;
    }
    #ctl00_cntBody_ctlListView_updatePanel2 tr.oddListRowS1, tr.evenListRowS1 {
    padding: 2px 8px 4px 5px;
    border-bottom: 1px dashed #cccccc;
    vertical-align: middle;
    height: 35px;
    text-align: left;
}
    #ctl00_cntBody_ctlListView_btnDelete {
        margin-left:15px;
    }

    #<%= checkAll.ClientID %>{
        float: left;
        margin-top: 24px;
    }

    #lb_checkAll{
        float: left;
        margin-top: 22px;
        margin-left: 5px;
    }
    #ctl00_cntBody_ctlListView_grdMain {
        margin-top:-10px;
    }
    #div_grid {
        margin-top:-10px;
    }
    .datestyle {
        text-align:right;
        padding-right:1.2em;
    }

</style>

<div class="row nav2">
    <div class="col-md-8 listTitle">我的消息</div>
    <div class="col-md-4">
    </div>
</div>

<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:UpdatePanel ID="updatePanel2" runat="server" data-ng-controller="MessageController">
    <ContentTemplate>
        <asp:CheckBox ID="checkAll" runat="server" AutoPostBack="True" OnCheckedChanged="checkAll_CheckedChanged"/>
        <label for="<%= checkAll.ClientID %>" id="lb_checkAll">全选</label>
<%--            <asp:Button ID="btnRead" runat="server" Font-Size="9pt" Text="设为已读" OnClick="btnRead_Click" />--%>
        <asp:Button ID="btnDelete" runat="server" Font-Size="9pt" Text="批量删除" OnClick="btnDelete_Click" />
        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="15" AutoGenerateColumns="false" DataKeyField="ID"
            AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server" >
            <Columns>
                <asp:TemplateColumn HeaderText="操作" ItemStyle-Width="3%">
                    <ItemTemplate>
                        <asp:CheckBox ID="check" runat="server" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="消息" ItemStyle-Width="82%">
                    <ItemTemplate>
                        <a href='<%# Eval("URL") %>' ng-click='<%# Eval("ID", "ReadMessage(\"{0}\")") %>'><span style="color:black;"><%# Eval("Body") %></span></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
<%--                    <asp:BoundColumn DataField="Body" ItemStyle-Width="82%"></asp:BoundColumn>--%>
                <asp:BoundColumn DataField="DATE_ENTERED" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-CssClass="datestyle" ItemStyle-Width="15%"></asp:BoundColumn>
            </Columns>
        </Taoqi:SplendidGrid>
    </ContentTemplate>
</asp:UpdatePanel>
