<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ForgetPassword2.ascx.cs" Inherits="Taoqi.Users.ForgetPassword2" %>

<div>
    <div style="border-bottom: solid; border-bottom-width: thin; border-bottom-color: rgb(228,228,228);">
        <p style="color: #ff6400; font-size: 28px; font-weight: bold;">找回密码</p>

        <table style="margin-top: 25px; margin-bottom: 20px; width: 755px; height: 64px; margin-left: auto; margin-right: auto; background: url('/images/ForgetPassword/wjmm2_03.png') no-repeat;">
            <tr style="font-size: medium; font-weight: bold;">
                <td valign="bottom" align="left" class="colorOrange" style="width: 234px; font-size: inherit; font-weight: inherit;">确认账户</td>
                <td valign="bottom" class="colorOrange" style="width: 236px; font-size: inherit; font-weight: inherit;">验证身份</td>
                <td valign="bottom" class="colorDarkGray" style="width: 246px; font-size: inherit; font-weight: inherit;">设置密码</td>
                <td valign="bottom" align="right" class="colorDarkGray" style="font-size: inherit; font-weight: inherit;">完成</td>
            </tr>
        </table>
    </div>

    <div style="margin-top: 46px">
        <div style="width: 504px; margin-left: 200px;">
            <div id="divLoginView">
                <form id="frmMain" method="post" runat="server" class="form-horizontal">
                    <div class="form-group">
                        <label for="txtUSER_PHONE" class="col-md-3 control-label">手机号码：</label>

                        <div class="col-md-9 " style="text-align: left;">
                            <label>
                                <asp:Literal ID="lblUSER_PHONE" runat="server"></asp:Literal></label>
                            <asp:Button ID="btnGetCode" runat="server" class="btn btn-default"
                                Style="background-color: #ff6400; color: white; display: inline-block; margin-left: 15px; height:100%;"
                                OnClick="btnGetCode_Click"
                                Text="获取短信验证码"></asp:Button>
                            <asp:Label ID="lbSMS" CssClass="error" EnableViewState="false" runat="server" />
                        </div>
                    </div>

                    <div class="form-group">
                        <label for="txtPASSWORD" class="control-label col-md-3">短信验证码：</label>
                        <div class="col-md-9">
                            <asp:TextBox ID="txtCode" MaxLength="6" CssClass="form-control" runat="server" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px; width: 900px;">
                        <div class="col-md-9 col-md-offset-3" style="margin-left: 134px;">
                            <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

                            <asp:Button ID="Button1"
                                CssClass="btn" Text='上一步' OnClick="btnFG_Pre" runat="server" Style="background-color: rgb(228,228,228); color: white; width: 150px; height: 46px;" />
                            <asp:Button ID="btnLogin"
                                CssClass="btn" Text='下一步' OnClick="btnFG_Next" runat="server" Style="background-color: #e17f00; color: white; width: 150px; height: 46px; margin-left:15px;" />

                        </div>
                    </div>

                </form>
            </div>
        </div>
    </div>
</div>