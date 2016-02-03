<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="ModifyPassword_phone.aspx.cs" Inherits="Taoqi.Users.ModifyPassword_phone" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cntBody" runat="server">
    <script>
        function showTimeOut(times) {
           // alert("dadas");
            $("#btnGetCodeNew").attr("disabled", true);
            setTimeout(function() {
                if (times > 0) {
                    times--;

                    $("#btnGetCodeNew").attr("disabled", true);
                    $("#btnGetCodeNew").val("(" + times + ")秒后重新发送");
                    setTimeout(arguments.callee, 1000);
                }
                else {
                    $("#btnGetCodeNew").val("重新发送");
                    $("#btnGetCodeNew").attr("disabled", false);
                }
            }, 1000)
    
        } 
     
        function SenSMSCode()
        {
            var validcode=$('#<%=this.verificationCode.ClientID%>').val();
            if (validcode==null||validcode==undefined||validcode=="") {
                layer.alert("请先输入验证码!");
            }
            else
            {  
                $.ajax({
                    type: 'Get',
                    datatype:Text,
                    url: "/Member/api/Account/Get/"+validcode,
                    success: function (results) {
                      
                        if (results=="0") {
                            layer.alert("图片验证码不正确");
    
                        }
                        else if (results=='成功') {
                            layer.alert("手机验证码发送成功，请查收！");
                            showTimeOut(5);
                        }
                        else{
                            alert(results);
                        }
                   
                    }
               
                });
            }
       
        }
        
    </script>
    <div id="modifyPassword_view">

        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 16px;">修改密码</label>

        <div style="border: solid #ddd; padding-top: 40px; float: left; padding-bottom: 20px; width: 100%; font-size: small;">

            <div class="form-group" style="margin-bottom: 20px;">
                <label class="control-label col-md-3" style="text-align: right;"><span style="color: red;">*</span>选择验证身份方式：</label>

                <asp:RadioButton type="radio" ID="Radio_modifyType_phone" GroupName="Radio_modifyType" Style="margin-left: 15px;" runat="server" Checked="true" />
                <label for="Radio_modifyType_phone" style="margin-left: 5px;">手机验证</label>

                <asp:RadioButton type="radio" ID="Radio_modifyType_password" GroupName="Radio_modifyType" Style="margin-left: 15px;" runat="server" AutoPostBack="true" OnCheckedChanged="Radio_modifyType_CheckedChanged" />
                <label for="Radio_modifyType_password" style="margin-left: 5px;">密码验证</label>
            </div>

            <div class="form-group" style="min-height: 23px; margin-bottom: 26px;">
                <label for="lblUSER_PHONE" class="col-md-3 control-label" style="text-align: right;"><span style="color: red;">*</span>手机号码：</label>

                <div class="col-md-9" style="text-align: left; line-height: 18px;">
                    <label>
                        <asp:Literal ID="lblUSER_PHONE" runat="server"></asp:Literal></label>
                </div>
            </div>


            <div class="form-group" style="height: 40px;">
                <label for="txtVerification" class="control-label col-md-3" style="text-align: right; margin-top: 8px;"><span style="color: red;">*</span>验证码：</label>

                <div class="col-md-9">
                    <div>
                        <asp:TextBox ID="verificationCode" CssClass="form-control" runat="server" Style="width: 70px; display: inline-block;" AutoComplete="off" />
                        <img id="verificationImg" style="width: 70px; height: 34px; display: inline-block; margin-left: 10px; margin-bottom: 6px;" onclick="refreshRandCode()" runat="server" />
                        <div style="display: inline; margin-left: 10px;">看不清？<a style="color: rgb(255,100,0);" onclick="refreshRandCode()" href="javascript:void(0);">换一张</a></div>
                    </div>
                </div>
            </div>

            <div class="form-group" style="margin-top: 5px;">
                <div class="col-md-9 col-md-offset-3">
                    <p class="colorDarkGray">请输入四位短信验证码</p>
                </div>
            </div>

            <div class="form-group" style="margin-top: 20px;">
                <label for="txtPASSWORD" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>短信验证码：</label>
                <div class="col-md-9" style="width: 200px;">
                    <asp:TextBox ID="smsCode" MaxLength="6" CssClass="form-control" runat="server" />
                </div>
                <asp:Button ID="btnGetCode" runat="server" class="btn btn-default"
                    Style="float: left; background-color: #ff6400; color: white; height: 34px;"
                    Text="获取短信验证码" OnClick="btnGetCode_Click" Visible="false"></asp:Button>
                <input id="btnGetCodeNew" type="button" onclick="SenSMSCode()" value="获取验证码" style="float: left; background-color: #ff6400; color: white; height: 34px;" />
                <asp:Label ID="lbSMS" CssClass="error" EnableViewState="false" runat="server" />
            </div>

            <div class="form-group" style="margin-top: 5px;">
                <div class="col-md-9 col-md-offset-3">
                    <p class="colorDarkGray">密码要求是6-16个字符</p>
                </div>
            </div>

            <div class="form-group" style="margin-top: 20px;">
                <label for="NewPassword" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>密码：</label>

                <div class="col-md-9 inputControl">
                    <asp:TextBox ID="NewPassword" MaxLength="16" TextMode="Password" CssClass="form-control" runat="server" placeholder="密码" />
                </div>
            </div>

            <div class="form-group" style="margin-top: 5px;">
                <div class="col-md-9 col-md-offset-3">
                    <p class="colorDarkGray">请再次输入密码</p>
                </div>
            </div>

            <div class="form-group" style="margin-top: 20px;">
                <label for="RepeatNewPassword" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>再次输入密码：</label>

                <div class="col-md-9 inputControl">
                    <asp:TextBox ID="RepeatNewPassword" MaxLength="16" TextMode="Password" CssClass="form-control" runat="server" placeholder="再次输入密码" />
                </div>
            </div>

            <div class="form-group" style="margin-top: 5px;">
                <div class="col-md-9 col-md-offset-3">
                    <p class="colorDarkGray">按右图输入验证码，不区分大小写</p>
                </div>
            </div>

            <div class="form-group" style="margin-top: 20px; margin-bottom: 0px">
                <div class="col-md-9 col-md-offset-3">
                    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

                    <asp:Button ID="btnLogin"
                        CssClass="btn" Text='提交' runat="server" Style="background-color: #0065e6; color: white; width: 120px; height: 36px;margin-top:0;" OnClick="btnLogin_Click" />
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

        var tick = <% =Tick %>;

        $(document).ready(function () {
            if (tick <= 0) return;  // tick > 0 启动定时器

            $("#<% =btnGetCode.ClientID %>").attr("disabled", true);
            setTimeout(function() {
                if (tick > 0) {
                    tick--;

                    $("#<% =btnGetCode.ClientID %>").attr("disabled", true);
                    $("#<% =btnGetCode.ClientID %>").val("(" + tick + ")秒后重新发送");
                    setTimeout(arguments.callee, 1000);
                }
                else {
                    $("#<% =btnGetCode.ClientID %>").val("重新发送");
                    $("#<% =btnGetCode.ClientID %>").attr("disabled", false);
                }
            }, 1000)
        })
    </script>
</asp:Content>
