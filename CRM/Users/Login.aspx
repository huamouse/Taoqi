<%@ Page Language="c#" CodeBehind="Login.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.Users.Login" %>

<%@ Register TagPrefix="Taoqi" TagName="LoginView" Src="LoginView.ascx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1,user-scalable=no">
    <title>���¼</title>

    <link href="/css/bootstrap.min.css" rel="stylesheet">
    <link href="/css/site.css" rel="stylesheet">

      <!--[if lt IE 9]>
      <script src="/js/html5shiv.min.js" defer async="async"></script>
      <script src="/js/respond.min.js" defer async="async"></script>
    <![endif]-->

</head>
<body>
    <div class="header">
        <!--����������-->
        <div class="header_top">
            <div class="container">
                <div class="row">
                    <div class="col-md-6">
                        <ul class="nav nav-pills" id="navLinksLeft">
                            <li><a>Hi����ӭ��������</a></li>
                            <li><a href="/member/" class="colorOrange"><span class="colorOrange">���¼</span></a></li>
                            <li><a href="/member/account/register.aspx">ע���˺�</a></li>
                        </ul>
                    </div>
                    <div class="col-md-6 hidden480">
                        <ul class="nav nav-pills pull-right" id="navLinksRight">
                            <li><a href="/member/">
                                <div class="icon icon_my"></div>
                                �ҵ�������</a></li>
                            <li><a href="/member/Order/">
                                <div class="icon icon_order"></div>
                                �ҵĶ���</a></li>
                            <li><a href="/mall.html">
                                <div class="icon icon_mall"></div>
                                �����̳�</a></li>
                            <li><a target="_blank" href="http://sighttp.qq.com/authd?IDKEY=15d1e15e6424078777701aaab26d0f81df5320ec28c72c53">
                                <div class="icon icon_support"></div>
                                ���߿ͷ�</a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!--LOGO����������-->
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
                    <a href="/Member/About/about6.aspx" data-parent="#menu2">��������</a>
                 | ����Ӣ��
                 | <a href="/Member/About/about4.aspx" data-parent="#menu2">��ϵ����</a>
                 | <a href="/Member/About/about5.aspx" data-parent="#menu2">��������</a>
                </div>
                <div class="textCenter2">
                    COPYRIGHT &copy; www.517taoqi.com ��ICP��15058548��
                </div>

            </div>
        </div>
    </div>
</body>
</html>

