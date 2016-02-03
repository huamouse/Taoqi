<%@ Page Language="c#" CodeBehind="ForgetPassword.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.Users.ForgetPassword" %>

<%@ Register TagPrefix="Taoqi" TagName="ForgetPassword1" Src="~/Users/ForgetPassword1.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="ForgetPassword2" Src="~/Users/ForgetPassword2.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="ForgetPassword3" Src="~/Users/ForgetPassword3.ascx" %>
<%@ Register TagPrefix="Taoqi" TagName="ForgetPassword4" Src="~/Users/ForgetPassword4.ascx" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1,user-scalable=no">
    <title>找回密码</title>

    <link href="/css/bootstrap.min.css" rel="stylesheet">
    <link href="/css/site.css" rel="stylesheet">

    <script src="/js/jquery.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="header">
        <!--顶部工具栏-->
        <div class="header_top">
            <div class="container">
                <div class="row">
                    <div class="col-md-6">
                        <ul class="nav nav-pills" id="navLinksLeft">
                            <li><a>Hi，欢迎来淘气！</a></li>
                            <li><a href="/member/" class="colorOrange"><span class="colorOrange">请登录</span></a></li>
                            <li><a href="/member/account/register.aspx">注册账号</a></li>
                        </ul>
                    </div>
                    <div class="col-md-6 hidden480">
                        <ul class="nav nav-pills pull-right" id="navLinksRight">
                            <li><a href="/">
                                <div class="icon icon_my"></div>
                                我的淘气网</a></li>
                            <li><a href="/member/">
                                <div class="icon icon_order"></div>
                                我的订单</a></li>
                            <li><a href="/mall.html">
                                <div class="icon icon_mall"></div>
                                积分商城</a></li>
                            <li><a target="_blank" href="http://sighttp.qq.com/authd?IDKEY=15d1e15e6424078777701aaab26d0f81df5320ec28c72c53">
                                <div class="icon icon_support"></div>
                                在线客服</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!--LOGO和搜索区域-->
        <div class="header_middle">
            <div class="container">
                <div class="row">
                    <div class="col-md-6">
                        <a href="/">
                            <img src="/images/logo.png" class="logo" /></a><img src="/images/description.png" class="small480" />
                    </div>

                    <div class="col-md-6">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="container" style="margin-top: 40px; min-height: 436px;">
        <div class="row">

            <div style="background: url(/images/ForgetPassword/wjmm_bg_03.jpg) no-repeat; width: 1170px; min-height: 450px; padding: 30px 35px 20px 0px; margin-left: auto; margin-right: auto;">

                <div style="width: 1000px; margin-left: auto; margin-right: auto;">
                    <Taoqi:ForgetPassword1 ID="FP1" runat="server" Visible="true" />
                    <Taoqi:ForgetPassword2 ID="FP2" runat="server" Visible="false"/>
                    <Taoqi:ForgetPassword3 ID="FP3" runat="server" Visible="false"/>
                    <Taoqi:ForgetPassword4 ID="FP4" runat="server" Visible="false"/>

                </div>
            </div>
        </div>
    </div>

    <div class="footer" style="height: auto; padding-top: 0px;">
        <div class="container">

            <div class="foot_txt">
                <div class="textCenter2">
                    <a href="/Member/About/about6.aspx" data-parent="#menu2">法律声明</a>
                 | 诚招英才
                 | <a href="/Member/About/about4.aspx" data-parent="#menu2">联系我们</a>
                 | <a href="/Member/About/about5.aspx" data-parent="#menu2">友情链接</a>
                </div>
                <div class="textCenter2">
                    COPYRIGHT &copy; www.517taoqi.com 沪ICP备13013915号-3 ICP证：沪B2-20140148
                </div>
            </div>
        </div>
    </div>
</body>
</html>