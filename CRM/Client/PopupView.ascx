<%@ Control CodeBehind="PopupView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQClient.PopupView" %>


<%@ Register TagPrefix="Taoqi" TagName="SearchView" Src="~/_controls/SearchView.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="ListHeader" Src="~/_controls/ListHeader.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="CheckAll" Src="~/_controls/CheckAll.ascx" %>

<div id="divPopupView">



    <div class="tabGroup" style="margin-top: 15px;">
        <a href="default.aspx">
            <div class="tab active">客户信息查询</div>
        </a>

        <%--<a href="edit.aspx?isDlg=1">
            <div class="button">
                <img src="/include/images/add.png" border="0" align="absmiddle" />
                新建收款人
            </div>
        </a>--%>


    </div>

    <div class="container_body">

        <Taoqi:SearchView ID="ctlSearchView" Module="TQClient"
            IsPopupSearch="true" ShowSearchTabs="false"
            Visible="<%# !PrintView %>" runat="Server" />



        <Taoqi:DynamicButtons ID="ctlDynamicButtons" runat="Server" />

        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdPopupView" EnableViewState="true" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="" ItemStyle-Width="2%" Visible="false">
                    <ItemTemplate><%# grdMain.InputCheckbox(!PrintView && bMultiSelect, ctlCheckAll.FieldName, Sql.ToGuid(Eval("ID")), ctlCheckAll.SelectedItems) %></ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </Taoqi:SplendidGrid>

        <Taoqi:CheckAll ID="ctlCheckAll" Visible="<%# !PrintView && bMultiSelect %>" runat="Server" />


    </div>
</div>


<script type="text/javascript">

    function SelectClient(sPARENT_ID, sPARENT_NAME) {
        if (window.opener != null && window.opener.ChangeTQClient != null) {
            window.opener.ChangeTQClient(sPARENT_ID, sPARENT_NAME);
            window.close();
        }
        else {
            alert('Original window has closed.  User cannot be assigned.' + '\n' + sPARENT_ID + '\n' + sPARENT_NAME);
        }
    }
    function SelectChecked() {
        if (window.opener != null && window.opener.ChangeTQClient != null) {
            var sSelectedItems = document.getElementById('<%= ctlCheckAll.SelectedItems.ClientID %>').value;
            window.opener.ChangeTQClient(sSelectedItems, '');
            window.close();
        }
        else {
            alert('Original window has closed.  User cannot be assigned.');
        }
    }
    function Clear() {
        if (window.opener != null && window.opener.ChangeTQClient != null) {
            window.opener.ChangeTQClient('', '');
            window.close();
        }
        else {
            alert('Original window has closed.  User cannot be assigned.');
        }
    }
    function Cancel() {
        window.close();
    }

</script>






