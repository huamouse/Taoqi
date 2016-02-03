<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="PersonalInfo.aspx.cs" Inherits="Taoqi.Users.PersonalInfo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <link href="../Include/javascript/Upload/webuploader.css" rel="stylesheet" />
    <script src="../Include/javascript/Upload/webuploader.js"></script>
    <script src="../Include/javascript/Upload/UploadJS.js"></script>
    <style>
        .webuploader-pick {
            background-color:#ff6400;
        }
        .form-control {
            font-size:13px;
        }
    </style>
    <script>
        jQuery(function () {
            CreateUploader('#filePicker', '#fileList', 'Users', 'C_Icon', '#<%=ClientImg.ClientID %>');
        });
    </script>

    <div id="myAccount_view" data-ng-controller="accountController">

        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 6px;">账户信息</label>
        <a href="#" style="margin-left: 20px; margin-top: 13px; display: inline-block;" data-ng-if="UserInfo.isCompany == 0"
            data-toggle="modal" onClick="$('#addClientModel').modal('show');">升级为企业用户</a>

        <div style="border: 1px solid #ddd; padding-bottom: 20px; float: left; font-size: small;">
            <div id="person" style="height: 420px;">
                <div style="float: left; width: 100%; padding-left: 15px; padding-right: 15px;">
                    <div style="float: left;">
                        <div class="form-group" style="margin-top: 20px;">
                            <label for="txtRealName" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>真实姓名：</label>
                            <div class="col-md-9 inputControl" style="width: 260px;">
                                <input type="text" id="txtRealName" class="form-control" runat="server" placeholder="真实姓名" />
                                <asp:RegularExpressionValidator ID="RequiredFieldValidator1" runat="server" ValidationExpression="^[\u4e00-\u9fa5]{2,4}$"
                                    ErrorMessage="请输入真实姓名！" ControlToValidate="txtRealName" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                    ErrorMessage="姓名不能为空！" ControlToValidate="txtRealName" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group" style="min-height: 23px; margin-top: 20px;">
                            <label for="lblUSER_PHONE" class="control-label col-md-3 " style="text-align: right;">绑定手机号：</label>
                            <div class="col-md-9" style="text-align: left; line-height: 18px; margin-top: 1px;">
                                <label>
                                    <asp:Literal ID="lblUSER_PHONE" runat="server"></asp:Literal></label>
                                <a class="editphone" href="BindingNewPhone.aspx">修改手机号</a>
                            </div>
                        </div>

                        <div class="form-group" style="margin-top: 20px;">
                            <label for="txtEMail" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">E-mail：</label>
                            <div class="col-md-9 inputControl" style="width: 260px;">
                                <input type="text" id="txtEMail" class="form-control" runat="server" placeholder="E-mail" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                                    ErrorMessage="邮箱地址非法！" ControlToValidate="txtEMail" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <div class="form-group" style="margin-top: 20px;">
                            <label for="txtQQ" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>绑定QQ号码：</label>
                            <div class="col-md-9 inputControl" style="width: 260px;">
                                <input type="text" id="txtQQ" class="form-control" runat="server" placeholder="QQ" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ValidationExpression="^([1-9][0-9]*)$"
                                    ErrorMessage="QQ号非法！" ControlToValidate="txtQQ" Display="Dynamic"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ErrorMessage="QQ号不能为空！" ControlToValidate="txtQQ" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group" style="margin-top: 20px;">
                            <label for="txtWeixin" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">微信号：</label>
                            <div class="col-md-9 inputControl" style="width: 260px;">
                                <input type="text" id="txtWeixin" class="form-control" runat="server" placeholder="微信号" />
                            </div>
                        </div>
                    </div>

                    <div style="float: left;margin-top:10px;">
                        <div style="width: 120px; text-align: center;">
                            <img src="/images/Information/hp_03.png" id="ClientImg" style="height: 120px; width: inherit;" runat="server" />
                            <div>
                                <div id="fileList" class="uploader-list"></div>
                                <div id="filePicker" style="float: left; margin-left: 19px; margin-top: 8px;">上传头像</div>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="form-group" style="margin-top: 20px; margin-bottom: 1px; margin-left: 10px; float: left;">
                    <div class="col-md-10 col-md-offset-2">
                        <button name="btnLogin" value="save" class="btn" style="background-color: #0065e6; color: white; width: 120px; height: 36px;margin-left:2.1em;">保存</button>
                        <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
                    </div>
                </div>
            </div>

        </div>

        <div class="modal fade" id="addClientModel" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">新增公司</h4>
                    </div>
                    <div class="modal-body" data-ng-form="addForm">
                        <div class="row form-group" data-ng-class="{'has-error':addForm.company.$dirty && comForm.company.$invalid}">
                            <label class="col-md-3 control-label" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>公司名称：</label>
                            <input type="text" name="company" class="form-control col-md-8" placeholder="公司中文全称，需跟营业执照一致"
                                data-ng-model="model.Company" data-ng-pattern="/^[\u4E00-\u9FA5]{5,}$/" required />
                            <div class="glyphicon glyphicon-ok form-glyphicon" data-ng-if="addForm.company.$dirty && addForm.company.$valid"></div>
                            <div class="glyphicon glyphicon-remove form-glyphicon" data-ng-if="addForm.company.$dirty && addForm.company.$invalid"></div>
                        </div>
                    </div>
                    <div class="modal-footer" style="border-top: none;">
                        <button type="button" class="btn btnRed" name="saveNewClient" value="saveNewClient" ng-click="addClient()" style="width: 80px;">确认提交</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">取消</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
