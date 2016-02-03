<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ForgetPassword4.ascx.cs" Inherits="Taoqi.Users.ForgetPassword4" %>

<div>
    <div style="border-bottom: solid; border-bottom-width: thin; border-bottom-color: rgb(228,228,228);">
        <p style="color: #ff6400; font-size: 28px; font-weight: bold;">找回密码</p>

        <table style="margin-top: 25px; margin-bottom: 20px; width: 755px; height: 64px; margin-left: auto; margin-right: auto; background: url('/images/ForgetPassword/wjmm4_03.png') no-repeat;">
            <tr style="font-size: medium; font-weight: bold;">
                <td valign="bottom" align="left" class="colorOrange" style="width: 234px; font-size: inherit; font-weight: inherit;">确认账户</td>
                <td valign="bottom" class="colorOrange" style="width: 236px; font-size: inherit; font-weight: inherit;">验证身份</td>
                <td valign="bottom" class="colorOrange" style="width: 246px; font-size: inherit; font-weight: inherit;">设置密码</td>
                <td valign="bottom" align="right" class="colorOrange" style="font-size: inherit; font-weight: inherit;">完成</td>
            </tr>
        </table>
    </div>

    <div style="margin-top: 46px">
        <div style="width: 504px; margin-left: 200px;">
            <div id="divLoginView">
                <form id="frmMain" method="post" runat="server" class="form-horizontal">

                    <div class="form-group" style="margin-top: 60px;">

                        <div style="float:left;margin-left:80px; margin-top:13px">
                            <img src="/images/ForgetPassword/banner_login_07.png" />
                        </div>

                        <div style="float:left;margin-left: 10px; text-align:left;">
                            <h4>您的新登录密码已设置成功。请记住她哦！</h4>
                            <h4><a class="colorOrange" href="<%: GetFromPath() %>">点击此处</a>立即返回之前的网页</h4>
                            <h4>5秒后自动返回</h4>
                        </div>
                    </div>

                </form>
            </div>
        </div>
    </div>
</div>