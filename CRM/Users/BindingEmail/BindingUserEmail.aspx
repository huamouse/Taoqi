<%@ Page Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="BindingUserEmail.aspx.cs" Inherits="Taoqi.Users.BindingUserEmail" Async="true" %>

<%@ Register Src="BindingUserEmail_New.ascx" TagName="Email_New" TagPrefix="TB" %>
<%@ Register Src="BindingUserEmail_ReBind.ascx" TagName="Email_ReBind" TagPrefix="TB" %>
<%@ Register Src="BindingUserEmail_Sucess.ascx" TagName="Email_Sucess" TagPrefix="TB" %>

<asp:Content ID="cntBodydas" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        .navDiv {
            color: #777474;
            font-size: 15px;
            height: 100%;
            margin-left: 30px;
            display: inline-block;
            line-height: 37px;
            width: 90px;
            text-align: center;
            position:relative;
            bottom:-3px;
        }

        .navBoderColor {
            border-bottom: solid #ff6400;
        }
        .form-control {
            font-size:13px;
        }
    </style>
    <div id="BindingUserEmail_View">
        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 16px;">账户安全</label>
        <div style="border: solid #ddd; float: left; padding: 20px 15px; width: 100%; font-size: small;">

            <div style="width: 100%; border-bottom: solid #ddd; margin-top: 10px; margin-bottom: 40px; height: 40px;">

                <a href="../BindingPhone_InitPage.aspx">
                    <div class="navDiv">绑定手机</div>
                </a>
                <a >
                    <div class="navBoderColor navDiv">绑定邮箱</div>
                </a>
                <a href="../BindingQQ/BindingQQ.aspx">
                    <div class="navDiv">绑定QQ</div>
                </a>
<%--                <a href="../SecurityInfromInfo/SecurityInformInfo.aspx">
                    <div class="navDiv">安全提示</div>
                </a>--%>
                <a href="../ModifyPassword_password.aspx">
                    <div class="navDiv">密码修改</div>
                </a>
            </div>
            <div>
                <asp:PlaceHolder ID="PLBindingEmail" runat="server"></asp:PlaceHolder>
            </div>

        </div>

    </div>

</asp:Content>
