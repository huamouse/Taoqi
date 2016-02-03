<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="Taoqi.Users.BossView_EditUser" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">

    <%--<script>
        $(document).ready(function () {
            $("#<%: A_uploadClientImg.ClientID %>").click(function(){
                $("#UploadClientImg").trigger("click");
            });
            $("#UploadClientImg").change(function(){
                $("#btn_UploadClientImg").trigger("click");
            });
        })
    </script>--%>
    <style>
        #BossViewEdit_view .left .form-group{
            float: left;
            width: 100%;
        }
        #BossViewEdit_view .left .form-group > label{
            width: 30%;
        }
        #BossViewEdit_view .left .form-group > div{
            width: 360px;
        }
        #BossViewEdit_view .checkbox_group{
            display: inline-block;
            margin-right:10px;
        }
    </style>

    <div id="BossViewEdit_view">

        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 16px;">员工信息</label>

        <div style="border: solid #ddd; min-height: 2380px; font-size: small; padding: 30px;">

            <div style="float: left; width: 100%;">
                <div class="left" style="float: left; width: 100%">

                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtRealName" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>真实姓名：</label>

                        <div class="col-md-9 inputControl">
                            <input type="text" id="txtRealName" class="form-control" required="required" runat="server" placeholder="真实姓名" />
                        </div>
                    </div>


                    <div class="form-group" style="min-height: 23px; margin-top: 20px; margin-bottom: 5px;">
                        <label for="txtUSER_PHONE" class="control-label col-md-3 " style="text-align: right;"><span style="color: red;">*</span>绑定手机号：</label>

                        <div class="col-md-9 inputControl">
                            <input type="text" id="txtUSER_PHONE" class="form-control" required="required" runat="server" placeholder="手机号" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 5px;">
                        <div class="col-md-9 col-md-offset-3" style="margin-left:30%;">
                            <p class="colorDarkGray">修改手机号，对应的用户名也会改变。</p>
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtEMail" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">E-mail：</label>

                        <div class="col-md-9 inputControl">
                            <input type="email" id="txtEMail" class="form-control" runat="server" placeholder="E-mail" />
                        </div>
                    </div>


                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtQQ" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>绑定QQ号码：</label>

                        <div class="col-md-9 inputControl">
                            <input type="text" id="txtQQ" class="form-control" required="required" runat="server" placeholder="QQ" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtWeixin" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">微信号：</label>

                        <div class="col-md-9 inputControl">
                            <input type="text" id="txtWeixin" class="form-control" runat="server" placeholder="微信号" />
                        </div>
                    </div>

                    <div class="form-group" id="div_UserType" style="margin-top: 20px; height: 50px; float: left;" runat="server">
                        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px; float: left;"><span style="color: red;">*</span>职位：</label>

                        <div class="col-md-9 inputControl" style="margin-top: 10px;">
                            <div class="checkbox_group" id="div_CGY" runat="server">
                                <input type="checkbox" id="CGY" runat="server" />
                                <label for="CGY" class="LABEL_SLCBTNUserType">采购员</label>
                            </div>

                            <div class="checkbox_group" id="div_XSY" runat="server">
                                <input type="checkbox" id="XSY" runat="server" />
                                <label for="XSY" class="LABEL_SLCBTNUserType">销售员</label>
                            </div>

                            <div class="checkbox_group" id="div_CZYG" runat="server">
                                <input type="checkbox" id="CZYG" runat="server" />
                                <label for="CZYG" class="LABEL_SLCBTNUserType">场站员工</label>
                            </div>

                            <div class="checkbox_group" id="div_JSY" runat="server">
                                <input type="checkbox" id="JSY" runat="server" />
                                <label for="JSY" class="LABEL_SLCBTNUserType">驾驶员</label>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 15px; margin-bottom: 5px;">
                        <label for="NewPassword" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">新密码：</label>

                        <div class="col-md-9 inputControl">
                            <input type="password" id="NewPassword" maxlength="16" class="form-control" runat="server" placeholder="新密码" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 5px;">
                        <div class="col-md-9 col-md-offset-3" style="margin-left:30%;">
                            <p class="colorDarkGray">密码要求是6-16个字符。（您可以选择不修改密码）</p>
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 15px; margin-bottom: 5px;">
                        <label for="RepeatNewPassword" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">重复新密码：</label>

                        <div class="col-md-9 inputControl">
                            <input type="password" id="RepeatNewPassword" maxlength="16" class="form-control" runat="server" placeholder="重复新密码" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 5px;">
                        <div class="col-md-9 col-md-offset-3" style="margin-left:30%;">
                            <p class="colorDarkGray">请再次输入密码。（您可以选择不修改密码）</p>
                        </div>
                    </div>
                </div>

                <%--<div style="float: right; margin-right: 20px;">
                    <div style="width: 120px; text-align: center;">
                        <img src="../Include/images/userPhoto.jpg" id="ClientImg" style="height: 120px; width: inherit;" runat="server" />
                        <a style="color: blue; margin-top: 10px;" id="A_uploadClientImg" href="#" runat="server">上传头像</a>
                        <input type="file" style="visibility: hidden" name="btnFile8" id="UploadClientImg" />
                        <label id="error8" class="error" runat="server"></label>
                        <button type="submit" id="btn_UploadClientImg" name="btn_UploadClientImg" value="btn_UploadClientImg" style="visibility: hidden" />
                    </div>
                </div>--%>
            </div>

                
            <div class="form-group" style="margin-top: 20px; margin-left: 15%; width: 800px; float: left;">
                <div class="col-md-10 col-md-offset-2">
                    <button name="btnLogin" value="save" class="btn" style="background-color: rgb(1,111,252); color: white; width: 100px; height: 46px;" >保存</button>
                    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
                </div>
            </div>
        </div>

    </div>
</asp:Content>