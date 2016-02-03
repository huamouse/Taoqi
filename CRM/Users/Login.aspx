<%@ Page Language="c#" CodeBehind="Login.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.Users.Login" %>

<%@ Register TagPrefix="Taoqi" TagName="LoginView" Src="LoginView.ascx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1,user-scalable=no">
    <title>请登录</title>

    <link href="/css/bootstrap.min.css" rel="stylesheet">
    <link href="/css/site.css" rel="stylesheet">

      <!--[if lt IE 9]>
      <script src="/js/html5shiv.min.js" defer async="async"></script>
      <script src="/js/respond.min.js" defer async="async"></script>
    <![endif]-->

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
                            <li><a href="/member/">
                                <div class="icon icon_my"></div>
                                我的淘气网</a></li>
                            <li><a href="/member/Order/">
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
                            <img src="/images/logo.png" class="logo" /></a><img src="/images/description.png" class="small480" style="vertical-align:bottom;margin-bottom:11px;margin-left:10px;" />
                    </div>

                    <div class="col-md-6">
                    </div>
                </div>
            </div>
        </div>



    </div>

    <div class="container" style="margin-top: 40px; min-height: 429px;">
        <div class="row">


            <div class="col-md-6" style="text-align: center;">
                <img src="../include/images/login_img.jpg" />
            </div>

            <div class="col-md-6">

                <div style="background: url(../include/images/login_bg.jpg) no-repeat; width: 413px; height: 360px; padding: 35px;">



                    <form id="frmMain" method="post" runat="server" class="form-horizontal">
                        <Taoqi:LoginView ID="ctlLoginView" runat="Server" />

                       
                    </form>
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
                    COPYRIGHT &copy; www.517taoqi.com 京ICP备15058548号
                </div>

            </div>
        </div>
    </div>
</body>
</html>

