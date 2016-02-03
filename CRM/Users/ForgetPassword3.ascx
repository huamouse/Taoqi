<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ForgetPassword3.ascx.cs" Inherits="Taoqi.Users.ForgetPassword3" %>

<div>
    <div style="border-bottom: solid; border-bottom-width: thin; border-bottom-color: rgb(228,228,228);">
        <p style="color: #ff6400; font-size: 28px; font-weight: bold;">找回密码</p>

        <table style="margin-top: 25px; margin-bottom: 20px; width: 755px; height: 64px; margin-left: auto; margin-right: auto; background: url('/images/ForgetPassword/wjmm3_03.png') no-repeat;">
            <tr style="font-size: medium; font-weight: bold;">
                <td valign="bottom" align="left" class="colorOrange" style="width: 234px; font-size: inherit; font-weight: inherit;">确认账户</td>
                <td valign="bottom" class="colorOrange" style="width: 236px; font-size: inherit; font-weight: inherit;">验证身份</td>
                <td valign="bottom" class="colorOrange" style="width: 246px; font-size: inherit; font-weight: inherit;">设置密码</td>
                <td valign="bottom" align="right" class="colorDarkGray" style="font-size: inherit; font-weight: inherit;">完成</td>
            </tr>
        </table>
    </div>

    <div style="margin-top: 46px">
        <div style="width: 504px; margin-left: 200px;">
            <div id="divLoginView">
                <form id="frmMain" method="post" runat="server" class="form-horizontal">

                    <div class="form-group">
                        <label for="txtUSER_PASSWORD" class="col-md-3 control-label">密码：</label>

                        <div class="col-md-9">
                            <asp:TextBox ID="RegisterPassword" MaxLength="16" TextMode="Password" CssClass="form-control" runat="server" placeholder="密码" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 30px;">
                        <label for="txtPassword2" class="col-md-3 control-label">再次输入密码：</label>

                        <div class="col-md-9">
                            <asp:TextBox ID="RegisterPassword2" MaxLength="16" TextMode="Password" CssClass="form-control" runat="server" placeholder="再次输入密码" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 30px; margin-bottom: 0px">
                        <div class="col-md-9 col-md-offset-3">
                            <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

                            <asp:Button ID="btnLogin"
                                CssClass="btn" Text='下一步' OnClick="btnFP_Next" runat="server" Style="background-color: #e17f00; color: white; width: 150px; height: 46px;" />
                        </div>
                    </div>

                </form>
            </div>
        </div>
    </div>
</div>