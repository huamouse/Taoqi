<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ForgetPassword1.ascx.cs" Inherits="Taoqi.Users.ForgetPassword1" %>

<script type="text/javascript">
    //刷新验证码
    function refreshRandCode() {
        $('#<% =verificationImg.ClientID %>').hide().attr('src',
                    '../account/verifycode.aspx?' + Math.floor(Math.random() * 100)).fadeIn();
        }

</script>

<div>
    <div style="border-bottom: solid; border-bottom-width: thin; border-bottom-color: rgb(228,228,228);">
        <p style="color: #ff6400; font-size: 28px; font-weight: bold;">找回密码</p>

        <table style="margin-top: 25px; margin-bottom: 20px; width: 755px; height: 64px; margin-left: auto; margin-right: auto; background: url('/images/ForgetPassword/wjmm1_03.png') no-repeat;">
            <tr style="font-size: medium; font-weight: bold;">
                <td valign="bottom" align="left" class="colorOrange" style="width: 234px; font-size: inherit; font-weight: inherit;">确认账户</td>
                <td valign="bottom" class="colorDarkGray" style="width: 236px; font-size: inherit; font-weight: inherit;">验证身份</td>
                <td valign="bottom" class="colorDarkGray" style="width: 246px; font-size: inherit; font-weight: inherit;">设置密码</td>
                <td valign="bottom" align="right" class="colorDarkGray" style="font-size: inherit; font-weight: inherit;">完成</td>
            </tr>
        </table>
    </div>

    <div style="margin-top: 46px">
        <div style="width: 504px; margin-left: 200px;">
            <div id="divLoginView">
                <form id="frmMain" method="post" runat="server" class="form-horizontal">

                    <div class="form-group" style="margin-bottom: 3px;">
                        <label for="txtUSERNAME" class="col-md-3 control-label">手机号：</label>

                        <div class="col-md-9 ">
                            <asp:TextBox ID="txtUSERNAME" MaxLength="20" CssClass="form-control" runat="server" placeholder="手机号" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-9 col-md-offset-3">
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 5px; line-height: 20px;">
                        <label for="txtVerification" class="col-md-3 control-label">验证码：</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="verificationCode" CssClass="form-control" runat="server" Style="width: 70px; display: inline-block;" />
                            <img id="verificationImg" style="width: 70px; height: 34px; display: inline-block; margin-left: 10px; margin-bottom: 6px;" onclick="refreshRandCode()" runat="server"></img>
                            <div style="display: inline; margin-left: 15px;">看不清？<a href="javascript:void(0);" onclick="refreshRandCode()" style="color: rgb(255,100,0);">换一张</a></div>
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px;">
                        <div class="col-md-9 col-md-offset-3">
                            <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

                            <asp:Button ID="btnLogin"
                                CssClass="btn" Text='下一步' runat="server" OnClick="btnFG_Click" Style="background-color: #e17f00; color: white; width: 150px; height: 46px;" />
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>