﻿<%@ Page Title="" Language="C#" MasterPageFile="~/App_MasterPages/Atlantic/UserRegister.Master" AutoEventWireup="true" CodeBehind="Invitation.aspx.cs" Inherits="Taoqi.Account.RegisterInvitation" %>



<asp:Content ContentPlaceHolderID="RegisterContent" runat="server">


        <div class="invitationbox">
            <div class="invitationboxtop">
                <img class="IRimg" src="/images/RegisterInvitation/zc_09.png" />
                <span class="IRtext">邀请你加入</span>
                <span class="IRCompany">南京正略信息技术有限公司</span>
            </div>
            <div class="invitationboxbottom">
                <div class="row form-group" data-ng-class="{'has-error':comForm.name.$dirty && comForm.name.$invalid}">
                    <!--<label class="col-md-3 control-label">真实姓名：</label>-->
                    <input type="text" name="name" placeholder="真实姓名" class="form-control col-md-8 col-md-offset-2" data-ng-model="model.RealName" data-ng-pattern="/^[\u4E00-\u9FA5]{2,4}$/" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.name.$dirty && comForm.name.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.name.$dirty && comForm.name.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.phone.$dirty && comForm.phone.$invalid}">
                    <!--<label class="col-md-3 control-label">手机号码：</label>-->
                    <input type="text" name="phone" class="form-control col-md-8 col-md-offset-2" placeholder="请输入手机号" data-ng-model="model.Mobile" data-ng-pattern="/^1[3|5|7|8|]\d{9}$/" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.phone.$dirty && comForm.phone.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.phone.$dirty && comForm.phone.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.code.$dirty && comForm.code.$invalid}">
                    <!--<label class="col-md-3 control-label">验证码：</label>-->
                    <input type="text" name="code" placeholder="验证码" class="form-control col-md-3 col-md-offset-2" data-ng-model="model.Code" data-ng-change="errorSmsMessage=''" required />
                    <img id="verificationImg1" class="regControl" src="verifycode.aspx" onclick="refreshRandCode(1)" />
                    <div class="regControl">看不清？<a style="color: rgb(255,100,0);" onclick="refreshRandCode(1)" href="javascript:void(0);">换一张</a></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.smsCode.$dirty && comForm.smsCode.$invalid}">
                    <!--<label class="col-md-3 control-label">短信验证码：</label>-->
                    <div class="row col-md-8">
                        <input type="text" name="smsCode" placeholder="短信验证码" class="form-control col-md-7" style="margin-left:69px;" data-ng-model="model.SmsCode" required />
                        <button class="btn btnOrange" data-ng-click="getSmsCode()" style="width: 100px; margin: -31px -88px 0px 70px;float:right;">获取短信验证码</button>
                        <div data-ng-bind="errorSmsMessage" style="color: red; float: left;"></div>
                    </div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.pass.$dirty && comForm.pass.$invalid}">
                    <!--<label class="col-md-3 control-label">输入密码：</label>-->
                    <input type="password" name="pass" placeholder="输入密码" class="form-control col-md-8 col-md-offset-2" data-ng-model="model.Password" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.pass.$dirty && comForm.pass.$valid"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.pass.$dirty && comForm.pass.$invalid"></div>
                </div>
                <div class="row form-group" data-ng-class="{'has-error':comForm.pass2.$dirty && (comForm.pass2.$invalid || Password2 != model.Password)}">
                    <!--<label class="col-md-3 control-label">确认密码：</label>-->
                    <input type="password" name="pass2" placeholder="确认密码" class="form-control col-md-8 col-md-offset-2" data-ng-model="Password2" required />
                    <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="comForm.pass2.$dirty && comForm.pass2.$valid && Password2 == model.Password"></div>
                    <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="comForm.pass2.$dirty && (comForm.pass2.$invalid || Password2 != model.Password)"></div>
                </div>
                <button class="form-control col-md-offset-2" style="width:280px;margin-left:59px;background-color:#ff6400;text-align:center;border:none;border-radius:5px;color:white;">加入企业</button>
                <span class="IRbottomtext">进入淘气网<a>www.517taoqi.com</a>或下载淘气网APP<br />登录此账号即可进行在线交易</span>
            </div>
        </div>

</asp:Content>
