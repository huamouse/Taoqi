<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="ModifyPassword_password.aspx.cs" Inherits="Taoqi.Users.ModifyPassword_password" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cntBody" runat="server">
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

    <div id="modifyPassword_view">
        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 16px;">修改密码</label>
        <div style="border: solid #ddd; padding-top: 40px; float: left; padding: 20px 15px; width: 100%; font-size: small;">

            <div style="width: 100%; border-bottom: solid #ddd; margin-top: 10px; margin-bottom: 40px; height: 40px;">

                <a href="./BindingPhone_InitPage.aspx">
                    <div class="navDiv">绑定手机</div>
                </a>
                <a href="BindingEmail/BindingUserEmail.aspx">
                    <div class=" navDiv">绑定邮箱</div>
                </a>
                <a href="BindingQQ/BindingQQ.aspx">
                    <div class="navDiv">绑定QQ</div>
                </a>
<%--                <a href="SecurityInfromInfo/SecurityInformInfo.aspx">
                    <div class="navDiv">安全提示</div>
                </a>--%>
                <a>
                    <div class=" navBoderColor navDiv">密码修改</div>
                </a>
            </div>
            <div class="form-group">
                <label class="control-label col-md-3" style="text-align: right;"><span style="color: red;">*</span>选择验证身份方式：</label>

                <asp:RadioButton type="radio" ID="Radio_modifyType_phone" GroupName="Radio_modifyType" Style="margin-left: 15px;" AutoPostBack="true" OnCheckedChanged="Radio_modifyType_CheckedChanged" runat="server" />
                <label for="Radio_modifyType_phone" style="margin-left: 5px;">手机验证</label>

                <asp:RadioButton type="radio" ID="Radio_modifyType_password" GroupName="Radio_modifyType" Style="margin-left: 15px;" Checked="true" runat="server" />
                <label for="Radio_modifyType_password" style="margin-left: 5px;">密码验证</label>
            </div>


            <div class="form-group" style="margin-top: 15px;">
                <label for="OldPassword" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>原密码：</label>

                <div class="col-md-9 inputControl">
                    <asp:TextBox ID="OldPassword" MaxLength="16" TextMode="Password" CssClass="form-control" runat="server" placeholder="原密码" />
                </div>
            </div>

            <div class="form-group" style="margin-top: 5px;">
                <div class="col-md-9 col-md-offset-3">
                    <p class="colorDarkGray">请输入您的登录密码</p>
                </div>
            </div>

            <div class="form-group" style="margin-top: 15px;">
                <label for="NewPassword" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>新密码：</label>

                <div class="col-md-9 inputControl">
                    <asp:TextBox ID="NewPassword" MaxLength="16" TextMode="Password" CssClass="form-control" runat="server" placeholder="新密码" />
                </div>
            </div>

            <div class="form-group" style="margin-top: 5px;">
                <div class="col-md-9 col-md-offset-3">
                    <p class="colorDarkGray">密码要求是6-16个字符</p>
                </div>
            </div>

            <div class="form-group" style="margin-top: 15px;">
                <label for="RepeatNewPassword" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>重复新密码：</label>

                <div class="col-md-9 inputControl">
                    <asp:TextBox ID="RepeatNewPassword" MaxLength="16" TextMode="Password" CssClass="form-control" runat="server" placeholder="重复新密码" />
                </div>
            </div>

            <div class="form-group" style="margin-top: 5px;">
                <div class="col-md-9 col-md-offset-3">
                    <p class="colorDarkGray">请再次输入密码</p>
                </div>
            </div>

            <div class="form-group" style="margin-top: 15px;">
                <label for="txtVerification" class="control-label col-md-3" style="text-align: right; margin-top: 8px;"><span style="color: red;">*</span>验证码：</label>

                <div class="col-md-9">
                    <div>
                        <asp:TextBox ID="verificationCode" CssClass="form-control" runat="server" Style="width: 70px; display: inline-block;" />
                        <img id="verificationImg" style="width: 70px; height: 34px; display: inline-block; margin-left: 10px; margin-bottom: 6px;" onclick="refreshRandCode()" runat="server" />
                        <div style="display: inline; margin-left: 10px;">看不清？<a style="color: rgb(255,100,0);" onclick="refreshRandCode()" href="javascript:void(0);">换一张</a></div>
                    </div>
                </div>
            </div>

            <div class="form-group" style="margin-top: 5px;">
                <div class="col-md-9 col-md-offset-3">
                    <p class="colorDarkGray">按右图输入验证码，不区分大小写</p>
                </div>
            </div>

            <div class="form-group" style="margin-top: 15px; margin-bottom: 0px">
                <div class="col-md-9 col-md-offset-3">
                    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

                    <asp:Button ID="btnLogin"
                        CssClass="btn" Text='提交' runat="server" OnClick="btnLogin_Click" Style="background-color: #0065e6; color: white; width: 120px;margin-top: 0; height: 36px;" />
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        //刷新验证码
        function refreshRandCode() {
            $('#<% =verificationImg.ClientID %>').hide().attr('src',
                    '../account/verifycode.aspx?' + Math.floor(Math.random() * 100)).fadeIn();
        }
    </script>
</asp:Content>
