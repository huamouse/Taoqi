<%@ Page Language="c#" MasterPageFile="~/DefaultView.Master" CodeBehind="edit.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.TQQuote.Edit" %>

<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
    <%@ Register TagPrefix="Taoqi" TagName="EditView" Src="EditView.ascx" %>
    <Taoqi:EditView ID="ctlEditView" Visible='<%# Taoqi.Security.GetUserAccess("TQQuote", "edit") >= 0 %>' Runat="Server" />
    <asp:Label ID="lblAccessError" Text='<%# L10n.Term(".LBL_UNAUTH_ADMIN") %>' Visible="<%# !ctlEditView.Visible %>" CssClass="error" EnableViewState="false" runat="server" />

    <div class="modal fade" id="modalMap" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog container">
            <div class="modal-content">
                <div class="modal-body">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <div class="mapTitle">地图标注</div>
                    <div class="mapBody">
                        <div id="allmap" style="height: 450px;"></div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default btnGray1" data-dismiss="modal" style="margin:0px;">
                        关闭
                    </button>
                    <button type="button" class="btn btnOrange" onclick="SavePosition()" style="margin:0px 30px;">
                        保存
                    </button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=wCp9mFd1bab0ygEVLnhdXAY7"></script>
    <script type="text/javascript" src="/js/map.js"></script>
</asp:Content>


