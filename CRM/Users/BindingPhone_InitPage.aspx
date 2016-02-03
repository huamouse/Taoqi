<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="BindingPhone_InitPage.aspx.cs" Inherits="Taoqi.Users.BindingPhone_InitPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <div id="BindingPhone_InitPage_View">
        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 16px;">账户安全</label>

        <div style="border: solid #ddd; padding-top: 20px; float: left; padding-bottom: 20px; font-size: small; padding-left: 15px; padding-right: 15px;">
            <div style="width: 100%; border-bottom: solid #ddd; margin-top: 10px; margin-bottom: 17px; height: 40px;">
                <a>
                    <div style="color: #777474; font-size: 15px; height: 100%; margin-left: 15px; display: inline-block; border-bottom: 3px solid #ff6400;position:relative;bottom:-3px; line-height: 37px; width: 90px; text-align: center;">绑定手机</div>
                </a>
                <a href="./BindingEmail/BindingUserEmail.aspx">
                    <div style="color: #777474; font-size: 15px; height: 100%; margin-left: 30px; display: inline-block; line-height: 37px; width: 90px; text-align: center;position:relative;bottom:-3px;">绑定邮箱</div>
                </a>

                <a href="./BindingQQ/BindingQQ.aspx">
                    <div style="color: #777474; font-size: 15px; height: 100%; margin-left: 30px; display: inline-block; line-height: 37px; width: 90px; text-align: center;position:relative;bottom:-3px;">绑定qq</div>
                </a>
<%--                <a href="./SecurityInfromInfo/SecurityInformInfo.aspx">
                    <div style="color: #777474; font-size: 15px; height: 100%; margin-left: 30px; display: inline-block; line-height: 37px; width: 90px; text-align: center;position:relative;bottom:-3px;">安全提示</div>
                </a>--%>
                <a href="./ModifyPassword_password.aspx">
                    <div style="color: #777474; font-size: 15px; height: 100%; margin-left: 30px; display: inline-block; line-height: 37px; width: 90px; text-align: center;position:relative;bottom:-3px;">密码修改</div>
                </a>
            </div>
            <div style="border-bottom: 1px solid ; border-bottom-color: rgb(241,241,241); width: 100%; padding-bottom: 10px;">
                <label style="font-size: 14px; margin-left: 20px; color: grey;">绑定手机是保护您的账户和隐私安全的重要手段。</label>
            </div>

            <div class="form-group" style="margin-top: 40px; float: left;">

                <div style="float: left; margin-left: 95px; width: 10%;">
                    <img src="/images/ForgetPassword/banner_login_07.png" />
                </div>

                <div style="float: left; width: 70%; text-align: left;">
                    <h3 style="color:grey">您已成功绑定手机：
                        <label style="font-size: 18px;color:black;">
                            <asp:Literal ID="lblUSER_PHONE" runat="server"></asp:Literal>
                        </label>
                        <a href="BindingNewPhone.aspx" style="display: inline; color: #ff6400; float:right;">更换绑定手机>></a>
                    </h3>
                    <h3 style="color:grey; font-size:13px;">淘气网提示：如果您更换了绑定手机，登录时需要新绑定的手机号进行登录；如果您之前进行过手机短信订阅，绑定手机号将默认为您订阅时的手机号</h3>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
