<%@ Page language="c#" MasterPageFile="~/DefaultView.Master" Codebehind="edit.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.TQClientAddress.Edit" %>

<%--
<asp:Content ID="cntSidebar" ContentPlaceHolderID="cntSidebar" runat="server">
	<%@ Register TagPrefix="Taoqi" Tagname="Shortcuts" Src="~/_controls/Shortcuts.ascx" %>
	<Taoqi:Shortcuts ID="ctlShortcuts" SubMenu="Modules" Runat="Server" />
</asp:Content>
--%>

<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
	<%@ Register TagPrefix="Taoqi" Tagname="EditView" Src="EditView.ascx" %>
	<Taoqi:EditView ID="ctlEditView" Visible='<%# Taoqi.Security.GetUserAccess("TQClientAddress", "edit") >= 0 %>' Runat="Server" />
	<asp:Label ID="lblAccessError" Text='<%# L10n.Term(".LBL_UNAUTH_ADMIN") %>' Visible="<%# !ctlEditView.Visible %>" CssClass="error" EnableViewState="false" Runat="server" />

    <div class="modal fade" id="modalMap" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog container">
            <div class="modal-content">
                <div class="modal-body">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <div class="mapTitle">µØÍ¼±ê×¢</div>
                    <div class="mapBody">
                        <div id="allmap" style="height:500px;"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=wCp9mFd1bab0ygEVLnhdXAY7"></script>
    <script type="text/javascript" src="/js/map.js"></script>
</asp:Content>


