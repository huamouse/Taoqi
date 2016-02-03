<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BindingUserEmail_ReBind.ascx.cs" Inherits="Taoqi.Users.BindingEmail.BindingUserEmail_ReBind" %>

<div style="border-bottom: solid; border-bottom-color: rgb(241,241,241); width: 100%; padding-bottom: 10px;">
    <label style="font-size: 14px; margin-left: 20px; color: #3e6378;">绑定邮箱是保护您的账户和隐私安全的重要手段。</label>
</div>

<div class="form-group" style="margin-top: 40px; float: left;">

    <div style="float: left; margin-left: 80px; width: 10%;">
        <img src="/images/ForgetPassword/banner_login_07.png" />
    </div>

    <div style="float: left; width: 70%; text-align: left; margin-left:20px">
        <h3>您已成功绑定邮箱：
                        <label style="font-size: 18px;">
                            <asp:Literal ID="lblUSER_EMail" runat="server"></asp:Literal>
                        </label>
            <a href="BindingUserEmail.aspx?NewReBind=1" style="display: inline; color: rgba(254, 100, 0, 1); margin-left: 100px;">更换</a>
        </h3>

    </div>
</div>
