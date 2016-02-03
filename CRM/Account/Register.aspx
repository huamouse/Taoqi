<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Atlantic/UserRegister.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Taoqi.Account.Register" %>

<asp:Content ContentPlaceHolderID="RegisterContent" runat="server">

    <div style="border: 1px solid #b5c1d7;">
        <ul class="nav">
            <li class="col-md-6 active">
                <a href="#company" data-toggle="tab" onclick="refreshRandCode(1)">我是企业用户</a>
            </li>
            <li class="col-md-6">
                <a href="#personal" data-toggle="tab" onclick="refreshRandCode(2)">我是个人用户</a>
            </li>
        </ul>
        <div class="tab-content ng-cloak" data-ng-controller="accountController">
            <div class="tab-pane fade in active" id="company" data-ng-form="comForm">
                <div class="row form-group">
                    <label class="control-label"><strong>填写注册信息</strong></label>
                    <label style="color: #939393;">--只有经过认证后企业用户或其子账号才能在淘气网进行在线交易</label>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.companyName.$dirty && comForm.companyName.$invalid}">
                    <label class="col-md-3 control-label">公司名称：</label>
                    <input type="text" name="companyName" class="form-control col-md-8" placeholder="公司中文全称，需跟营业执照一致"
                        data-ng-model="model.CompanyName" data-ng-pattern="/^[\u4E00-\u9FA5]{5,}$/" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.companyName.$dirty && comForm.companyName.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.companyName.$dirty && comForm.companyName.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.phone.$dirty && comForm.phone.$invalid}">
                    <label class="col-md-3 control-label">手机号码：</label>
                    <input type="text" name="phone" class="form-control col-md-8" placeholder="请输入正确的手机号" data-ng-model="model.Mobile" data-ng-pattern="/^1[3|5|7|8|]\d{9}$/" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.phone.$dirty && comForm.phone.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.phone.$dirty && comForm.phone.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.code.$dirty && comForm.code.$invalid}">
                    <label class="col-md-3 control-label">验证码：</label>
                    <input type="text" name="code" class="form-control col-md-3" data-ng-model="model.Code" data-ng-change="errorSmsMessage=''" required />
                    <img id="verificationImg1" class="regControl" src="verifycode.aspx" onclick="refreshRandCode(1)" />
                    <div class="regControl">看不清？<a style="color: rgb(255,100,0);" onclick="refreshRandCode(1)" href="javascript:void(0);">换一张</a></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.smsCode.$dirty && comForm.smsCode.$invalid}">
                    <label class="col-md-3 control-label">短信验证码：</label>
                    <div class="row col-md-8">
                        <input type="text" name="smsCode" class="form-control col-md-7" data-ng-model="model.SmsCode" required />
                        <button class="btn btnOrange" data-ng-click="getSmsCode()" style="width: 100px; margin: 1px 13px 1px 0px;">获取短信验证码</button>
                        <div data-ng-bind="errorSmsMessage" style="color: red; float: left;"></div>
                    </div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.name.$dirty && comForm.name.$invalid}">
                    <label class="col-md-3 control-label">真实姓名：</label>
                    <input type="text" name="name" class="form-control col-md-8" data-ng-model="model.RealName" data-ng-pattern="/^[\u4E00-\u9FA5]{2,4}$/" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.name.$dirty && comForm.name.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.name.$dirty && comForm.name.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.pass.$dirty && comForm.pass.$invalid}">
                    <label class="col-md-3 control-label">输入密码：</label>
                    <input type="password" name="pass" class="form-control col-md-8" data-ng-model="model.Password" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.pass.$dirty && comForm.pass.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.pass.$dirty && comForm.pass.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.pass2.$dirty && (comForm.pass2.$invalid || Password2 != model.Password)}">
                    <label class="col-md-3 control-label">确认密码：</label>
                    <input type="password" name="pass2" class="form-control col-md-8" data-ng-model="Password2" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.pass2.$dirty && comForm.pass2.$valid && Password2 == model.Password"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.pass2.$dirty && (comForm.pass2.$invalid || Password2 != model.Password)"></div>
                </div>
                <div class="row form-group" style="margin-top: -15px; margin-bottom: 10px; text-align: left;">
                    <div class="col-md-offset-3">
                        <div>
                            <input type="checkbox" data-ng-model="agreement" />
                            <span>《淘气网用户协议》</span>
                        </div>
                        <div data-ng-bind-html="errorMessage" style="color: red;"></div>
                    </div>
                </div>
                <div class="row form-group">
                    <button type="button" class="btn form-control col-md-8 col-md-offset-3" style="background-color: #ff6600; color: white;"
                        data-ng-click="register()" data-ng-disabled="!agreement">
                        提交</button>
                </div>
                <div style="border-top: 1px dashed #dcdcdc; min-height: 70px">
                    <div style="text-align: left; float: left; width: 310px;">
                        <div class="colorDarkGray" style="font-size: 14px; font-weight: bold; padding-top: 10px;">
                            已有淘气网账号?
                        </div>
                        <div class="colorDarkGray" style="padding-top: 5px;">
                            成为会员，淘气网将人工免费帮您找最便宜的天然气！并且可以免费发布、下载资源单！
                        </div>
                    </div>
                    <div style="padding-top: 10px; float: left; margin-top: 20px; width: 118px;">
                        <a href="/member/">
                            <div class="btn" style="background-color: #ff6400; color: white; padding-left: 30px; padding-right: 30px; margin-right: 13px;">立即登录</div>
                        </a>
                    </div>
                </div>
            </div>
            <div class="tab-pane fade in" id="personal" data-ng-form="perForm">
                <div class="row form-group">
                    <label class="control-label"><strong>填写注册信息</strong></label>
                    <label style="color: #939393;">--个人用户可先查看淘气网资源，但不能进行在线交易</label>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':perForm.phone.$dirty && perForm.phone.$invalid}">
                    <label class="col-md-3 control-label">手机号码：</label>
                    <input type="text" name="phone" class="form-control col-md-8" placeholder="请输入正确的手机号"
                        data-ng-model="model.Mobile" data-ng-pattern="/^1[3|5|7|8|]\d{9}$/" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="perForm.phone.$dirty && perForm.phone.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="perForm.phone.$dirty && perForm.phone.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':perForm.code.$dirty && perForm.code.$invalid}">
                    <label class="col-md-3 control-label">验证码：</label>
                    <input type="text" name="code" class="form-control col-md-3" data-ng-model="model.Code" data-ng-change="errorSmsMessage=''" required />
                    <img id="verificationImg2" class="regControl" onclick="refreshRandCode(2)" />
                    <div class="regControl">看不清？<a style="color: rgb(255,100,0);" onclick="refreshRandCode(2)" href="javascript:void(0);">换一张</a></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':perForm.smsCode.$dirty && perForm.smsCode.$invalid}">
                    <label class="col-md-3 control-label">短信验证码：</label>
                    <div class="col-md-8" style="padding: 0px;">
                        <input type="text" name="smsCode" class="form-control col-md-7" data-ng-model="model.SmsCode" required />
                        <button class="btn btnOrange" data-ng-click="getSmsCode()" style="width: 100px; margin: 1px 13px 1px 0px;">获取短信验证码</button>
                        <div data-ng-bind="errorSmsMessage" style="color: red; float: left;"></div>
                    </div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':perForm.name.$dirty && perForm.name.$invalid}">
                    <label class="col-md-3 control-label">真实姓名：</label>
                    <input type="text" name="name" class="form-control col-md-8" placeholder="请输入您的真实姓名"
                        data-ng-model="model.RealName" data-ng-pattern="/^\W{2,4}$/" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="perForm.name.$dirty && perForm.name.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="perForm.name.$dirty && perForm.name.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':perForm.pass.$dirty && perForm.pass.$invalid}">
                    <label class="col-md-3 control-label">输入密码：</label>
                    <input type="password" name="pass" class="form-control col-md-8" data-ng-model="model.Password" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="perForm.pass.$dirty && perForm.pass.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="perForm.pass.$dirty && perForm.pass.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':perForm.pass2.$dirty && (perForm.pass2.$invalid || model.Password2 != model.Password)}">
                    <label class="col-md-3 control-label">确认密码：</label>
                    <input type="password" name="pass2" class="form-control col-md-8" data-ng-model="model.Password2" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="perForm.pass2.$dirty && perForm.pass2.$valid && model.Password2 == model.Password"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="perForm.pass2.$dirty && (perForm.pass2.$invalid || model.Password2 != model.Password)"></div>
                </div>
                <div class="row form-group" style="margin-top: -15px; margin-bottom: 10px; text-align: left;">
                    <div class="col-md-offset-3">
                        <div>
                            <input type="checkbox" data-ng-model="agreement" />
                            <span>《淘气网用户协议》</span>
                        </div>
                        <div data-ng-bind-html="errorMessage" style="color: red;"></div>
                    </div>
                </div>
                <div class="row form-group">
                    <button type="button" class="btn form-control col-md-8 col-md-offset-3" style="background-color: #ff6600; color: white;"
                        data-ng-click="register('personal')" data-ng-disabled="!agreement">
                        提交</button>
                </div>
                <div style="border-top: 1px dashed #dcdcdc; min-height: 70px">
                    <div style="text-align: left; float: left; width: 310px;">
                        <div class="colorDarkGray" style="font-size: 14px; font-weight: bold; padding-top: 10px;">
                            已有淘气网账号?
                        </div>
                        <div class="colorDarkGray" style="padding-top: 5px;">
                            成为会员，淘气网将人工免费帮您找最便宜的天然气！并且可以免费发布、下载资源单！
                        </div>
                    </div>
                    <div style="padding-top: 10px; float: left; margin-top: 20px; width: 118px;">
                        <a href="/member/">
                            <div class="btn" style="background-color: #ff6400; color: white; padding-left: 30px; padding-right: 30px; margin-right: 13px;">立即登录</div>
                        </a>
                    </div>
                </div>
            </div>
            <div class="tab-pane" style="padding: 20px 30px; min-height: 240px;" id="comFinish">
                <div style="width: 350px; height: 75px; margin-left: 50px;">
                    <img style="float: left; width: 60px; height: 60px;" src="/images/Register/zccg_07.png" />
                    <div style="">
                        <span style="display: inline-block; color: #0065e6; font-size: 20px; font-weight: bold; padding-top: 2px; float: left; padding-left: 18px;">恭喜您，注册成功！
                        </span>
                        <span style="display: inline-block; color: grey; font-size: 14px; padding-top: 3px;">欢迎您加入淘气网，网上交易从此开始！
                        </span>
                    </div>
                </div>
                <button onclick="companyinfo()" style="background-color: #ff6400; color: white; width: 78%; height: 50px; text-align: center; border-radius: 4px; font-size: 16px; border-style: none;">
                    继续完善企业信息
                </button>
                <a href="/" style="width: 40%; color: #ff6400; float: left; font-size: 14px; margin-left: 135px; margin-top: 20px;">返回淘气网首页
                </a>
                <span style="display: inline-block; margin-top: 10px; color: grey; font-size: 14px;">进入淘气商城需获得正式交易资格的会员才能操作。<br />
                    请先完善您的企业信息，另有积分赠送。
                </span>
            </div>
            <div class="tab-pane" style="padding: 20px 30px; min-height: 240px;" id="perFinish">
                <div style="width: 350px; height: 75px; margin-left: 50px;">
                    <img style="float: left; width: 60px; height: 60px;" src="/images/Register/zccg_07.png" />
                    <div style="">
                        <span style="display: inline-block; color: #0065e6; font-size: 20px; font-weight: bold; padding-top: 2px; float: left; padding-left: 18px;">恭喜您，注册成功！
                        </span>
                        <span style="display: inline-block; color: grey; font-size: 14px; padding-top: 3px;">欢迎您加入淘气网，网上交易从此开始！
                        </span>
                    </div>
                </div>
                <button onclick="companyinfo2()" style="background-color: #ff6400; color: white; width: 78%; height: 50px; text-align: center; border-radius: 4px; font-size: 16px; border-style: none;">
                    继续完善个人信息
                </button>
                <a href="/" style="width: 40%; color: #ff6400; float: left; font-size: 14px; margin-left: 135px; margin-top: 20px;">返回淘气网首页
                </a>
                <span style="display: inline-block; margin-top: 10px; color: grey; font-size: 14px;">进入淘气商城需获得正式交易资格的会员才能操作。<br />
                    请先完善您的企业信息，另有积分赠送。
                </span>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        //刷新验证码
        function refreshRandCode(index) {
            $('#verificationImg' + index).hide().attr('src',
                'verifycode.aspx?' + Math.floor(Math.random() * 100)).fadeIn();
        }

        function companyinfo() {
            window.location = "/Member/Users/ClientInfo.aspx";
        }

        function companyinfo2() {
            window.location = "/Member/Users/PersonalInfo.aspx";
        }
        <%--var tick = <% =Tick %>;

        $(document).ready(function () {
            if (tick <= 0) return;  // tick > 0 启动定时器

            $("#<%= btnGetCode.ClientID %>").attr("disabled", true);
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
        })--%>
    </script>
</asp:Content>
