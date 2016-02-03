<%@ Page Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="BindingNewPhone.aspx.cs" Inherits="Taoqi.Users.BindingNewPhone" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">

    <script type="text/javascript">
        //刷新验证码
        function refreshRandCode() {
            $('.verificationImg').hide().attr('src',
                    '../account/verifycode.aspx?' + Math.floor(Math.random() * 100)).fadeIn();
        }

        var tick = <% =Tick %>;

        $(document).ready(function () {
            if (tick <= 0) return;  // tick > 0 启动定时器

            $(".btnGetCode").attr("disabled", true);
            setTimeout(function() {
                if (tick > 0) {
                    tick--;

                    $(".btnGetCode").attr("disabled", true);
                    $(".btnGetCode").val("(" + tick + ")秒后重新发送");
                    setTimeout(arguments.callee, 1000);
                }
                else {
                    $(".btnGetCode").val("重新发送");
                    $(".btnGetCode").attr("disabled", false);
                }
            }, 1000)
        })
    </script>


    <div id="BindingNewPhone_View">
        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 16px;">绑定手机</label>

        <div style="border: solid #ddd; padding-top: 20px; float: left; width: 100%; padding-bottom: 20px; font-size: small; padding-left: 15px; padding-right: 15px;">
            <div style="border-bottom: solid; border-bottom-color: rgb(241,241,241); width: 100%; padding-bottom: 10px;">
                <label style="font-size: 14px; margin-left: 20px; color: grey;">绑定手机后，可用手机号直接登录，绑定手机是保护您的账户和隐私安全的重要手段。</label>
            </div>

            <div style="margin-top: 20px; float: left; width: 100%;">

                <asp:PlaceHolder ID="PlaceHolder1" Visible="true" runat="server">
                    <div class="form-group" style="min-height: 23px; margin-bottom: 26px;">
                        <label for="lblUSER_PHONE1" class="col-md-3 control-label" style="text-align: right;">手机号码：</label>

                        <div class="col-md-9" style="text-align: left; line-height: 18px;">
                            <label>
                                <asp:Literal ID="lblUSER_PHONE1" runat="server"></asp:Literal></label>
                        </div>
                    </div>

                    <div class="form-group" style="height: 40px;">
                        <label for="txtVerification" class="control-label col-md-3" style="text-align: right; margin-top: 8px;">验证码：</label>

                        <div class="col-md-9">
                            <div>
                                <asp:TextBox ID="verificationCode1" CssClass="form-control" runat="server" Style="width: 70px; display: inline-block;" />
                                <img id="verificationImg1" class="verificationImg" style="width: 70px; height: 34px; display: inline-block; margin-left: 10px; margin-bottom: 6px;" onclick="refreshRandCode()" runat="server" />
                                <div style="display: inline; margin-left: 10px; font-size:13px;">看不清？<a style="color: rgb(255,100,0);" onclick="refreshRandCode()" href="javascript:void(0);">换一张</a></div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px; min-height: 40px;">
                        <label for="txtPASSWORD" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">短信验证码：</label>
                        <div class="col-md-9" style="width: 200px;">
                            <asp:TextBox ID="smsCode1" MaxLength="6" CssClass="form-control" runat="server" />
                        </div>
                        <asp:Button ID="btnGetCode1" CssClass="btnGetCode" runat="server" class="btn btn-default"
                            Style="float: left; background-color: #ff6400; color: white; height: 34px; margin-top:0;"
                            Text="获取短信验证码" OnClick="btnGetCode1_Click"></asp:Button>
                        <asp:Label ID="lbSMS1" CssClass="error" EnableViewState="false" runat="server" />
                    </div>

                    <div class="form-group" style="margin-top: 20px; margin-bottom: 0px">
                        <div class="col-md-9 col-md-offset-3">
                            <asp:Label ID="lblError1" CssClass="error" EnableViewState="false" runat="server" />

                            <asp:Button ID="btnLogin1"
                                CssClass="btn" Text='提交验证' runat="server" Style="background-color: #0065e6; color: white; width: 120px; height: 36px;" OnClick="btnLogin1_Click" />
                        </div>
                    </div>
                </asp:PlaceHolder>




                <asp:PlaceHolder ID="PlaceHolder2" Visible="false" runat="server">
                    <div class="form-group" style="min-height: 23px; margin-bottom: 26px;">
                        <label for="lblUSER_PHONE2" class="col-md-3 control-label" style="text-align: right;">已绑定手机号：</label>

                        <div class="col-md-9" style="text-align: left; line-height: 18px;">
                            <label>
                                <asp:Literal ID="lblUSER_PHONE2" runat="server"></asp:Literal></label>
                        </div>
                    </div>

                    <div class="form-group" style="min-height: 140px;">
                        <label for="TXTNewPhone2" class="control-label col-md-3">新手机号码：</label>

                        <div class="col-md-9" style="width: 440px;">
                            <asp:TextBox ID="TXTNewPhone2" MaxLength="11" CssClass="form-control" runat="server" />

                            <div style="margin-bottom: 4px; margin-top: 10px;">
                                <label for="txtVerification2">验证码：</label>
                                <asp:TextBox ID="verificationCode2" CssClass="form-control" runat="server" Style="margin-left: 10px; width: 70px; display: inline-block;" />
                                <img id="verificationImg2" class="verificationImg" style="width: 70px; height: 34px; display: inline-block; margin-left: 10px; margin-bottom: 6px;" onclick="refreshRandCode()" runat="server" />
                                <div style="display: inline; margin-left: 10px;">看不清？<a style="color: rgb(255,100,0);" onclick="refreshRandCode()" href="javascript:void(0);">换一张</a></div>
                            </div>

                            <asp:Button ID="btnGetCode2" CssClass="btnGetCode" runat="server" class="btn btn-default"
                                Style="float: left; background-color: #ff6400; color: white; margin-top: 5px; min-height: 34px;"
                                Text="获取短信验证码" OnClick="btnGetCode2_Click"></asp:Button>

                            <asp:Label ID="lbSMS2" CssClass="error" EnableViewState="false" runat="server" />
                        </div>
                    </div>

                    <div class="form-group" style="min-height: 40px;">
                        <label for="smsCode2" class="control-label col-md-3">短信验证码：</label>
                        <div class="col-md-9" style="width: 220px;">
                            <asp:TextBox ID="smsCode2" MaxLength="6" CssClass="form-control" runat="server" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px; margin-bottom: 0px">
                        <div class="col-md-9 col-md-offset-3">
                            <asp:Label ID="lblError2" CssClass="error" EnableViewState="false" runat="server" />

                            <asp:Button ID="btnLogin2"
                                CssClass="btn" Text='提交绑定手机' runat="server" Style="background-color: #0065e6; color: white; width: 150px; height: 46px;" OnClick="btnLogin2_Click" />
                        </div>
                    </div>
                </asp:PlaceHolder>




                <asp:PlaceHolder ID="PlaceHolder3" Visible="false" runat="server">

                    <div class="form-group" style="margin-top: 40px; float: left; width: 100%;">

                        <div style="float: left; margin-left: 80px; width: 10%;">
                            <img src="/images/ForgetPassword/banner_login_07.png" />
                        </div>

                        <div style="float: left; width: 80%; text-align: left;">
                            <h3>恭喜您成功绑定手机号：</h3>
                            <label style="font-size: 18px;">
                                <asp:Literal ID="TXTNewPhone3" runat="server"></asp:Literal>
                            </label>
                            <h3>绑定手机后，可用手机号直接登录，绑定手机是保护您的账户和隐私安全的重要手段。</h3>
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px; margin-bottom: 0px; float: left;">
                        <div class="col-md-9 col-md-offset-3" style="margin-left: 160px;">
                            <asp:Label ID="error3" CssClass="error" EnableViewState="false" runat="server" />

                            <a href="/index.html">
                                <div id="btnLogin3" class="btn" style="background-color: #0065e6; color: white; width: 150px; height: 46px; line-height: 28px;">返回淘气网首页</div>
                            </a>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
</asp:Content>