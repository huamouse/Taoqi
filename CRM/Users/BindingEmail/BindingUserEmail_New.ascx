<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BindingUserEmail_New.ascx.cs" Inherits="Taoqi.Users.BindingEmail.BindingUserEmail_New" EnableViewState="true" %>

<div class="form-horizontal" role="form">
    <div style="border-bottom: solid; border-bottom-color: rgb(241,241,241); width: 100%; padding-bottom: 10px;">
        <label style="font-size: 14px; margin-left: 20px; color: grey;">绑定邮箱后，可用邮箱直接登录，绑定邮箱是保护您的账户和隐私安全的重要手段。</label>
    </div>

    <div style="margin-top: 20px; float: left; width: 100%;">
        <div class="form-group">
            <label for="txtEmail" class="control-label col-md-3" style="text-align: right;">邮箱帐号：</label>
            <div class="col-md-6" style="width: 40%;">
                <asp:TextBox ID="txtEmail" MaxLength="100" CssClass="form-control" runat="server" placeholder="请输入真实存在的邮箱帐号"    type="email"/>
            </div>
            <asp:Button ID="btnGetCode1" CssClass="btnGetCode" runat="server" class="btn btn-default col-md-3"
                Style="background-color: #ff6400; color: white; margin: 0px; padding: 7px 15px; border: 0px"
                Text="获取邮箱验证码" OnClick="btnGetCode1_Click" OnClientClick="SetButtonDisable()" ></asp:Button>
            <asp:Label ID="lbErrorForCode" CssClass="error" EnableViewState="false" runat="server" />
        </div>
        <div class="form-group">
            <label for="txtEmailCode" class="control-label col-md-3">验证码：</label>
            <div class="col-md-9" style="width: 200px;">
                <asp:TextBox ID="txtEmailCode" MaxLength="6" CssClass="form-control requried" runat="server" />
            </div>
        </div>
        <div class="form-group" style="margin-top: 20px; margin-bottom: 0px">
            <div class="col-md-9 col-md-offset-3">
                <asp:Label ID="lblError1" CssClass="error" EnableViewState="false" runat="server" />
                <asp:Button ID="btnConfirmBind"
                    CssClass="btn" Text='提交绑定邮箱' runat="server" Style="background-color: #0065e6; color: white; width: 120px; height: 36px;margin-top: 0;" OnClick="btnConfirmBind_Click" />
            </div> 
        </div>
    </div>
   
</div>
<script>
    function SetButtonDisable()
    {
        $(".btnGetCode").attr("disabled", false);
        $(".btnGetCode").attr("background-color", "grey");
    }

    var tick = <% =Tick %>;
    $(document).ready(function () {
  
        if (tick <= 0) return;  // tick > 0 启动定时器
        $(".btnGetCode").attr("disabled", true);
        setTimeout(function () {
            if (tick > 0) {
                tick--;

                $(".btnGetCode").attr("disabled", true);
                $(".btnGetCode").val("(" + tick + ")秒后可重发");
                setTimeout(arguments.callee, 1000);
            }
            else {
                $(".btnGetCode").val("重新发送");
                $(".btnGetCode").attr("disabled", false);
            }
        }, 1000)

    });

</script>
