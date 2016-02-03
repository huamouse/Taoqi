<%@ Page Title="" Language="C#" MasterPageFile="~/DefaultView1.master" AutoEventWireup="true" CodeBehind="ClientInfo.aspx.cs" Inherits="Taoqi.Users.ClientInfo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cntBody" runat="server">
    <link href="../Include/javascript/Upload/webuploader.css" rel="stylesheet" />
    <script src="../Include/javascript/Upload/webuploader.js"></script>
    <script src="../Include/javascript/Upload/UploadJS.js"></script>
    <style>
        .webuploader-pick {
            background-color: transparent;
            color: #0065e6;
        }

        #company .webuploader-pick {
            background-color: #ff6400;
            color: white;
        }

        #company .webuploader-pick {
            width:175px;
            border-radius:5px;
            height:35px;
            padding-top:9px;
        }
        .form-control {
            font-size: 13px;
        }

        img {
            max-width: 900px;
            max-height: 600px;
        }

        .layui-layer-content {
            height: auto !important;
        }

        .layui-layer-nobg .layui-layer-setwin {
            width: 32px;
            height: 32px;
            display: block;
            right: -11px;
            top: -16px;
        }

            .layui-layer-nobg .layui-layer-setwin a {
                width: 30px;
                height: 32px;
            }

        .layui-layer-ico {
            background-position: -149px -31px;
        }
    </style>
    <script>
        // 图片上传
        jQuery(function () {
            //第一个参数是  “按钮触发弹框上传文件”，第二个是存保存的图片地址,第四个是存储的表明 第五个是字段
            CreateUploader('#filepickerIcon', '#textUpImgIcon', 'TQClient', 'C_imgIcon', '#ImgIcon');
            CreateUploader('#filepickerCP', '#textUpImgClient', 'TQClient', 'C_imgClient', '#ImgClient');
            CreateUploader('#filepicker1', '#fileList1', 'TQClient', 'C_Attachment1', '#ImgUp1');
            CreateUploader('#filepicker2', '#fileList2', 'TQClient', 'C_Attachment2', '#ImgUp2');
            CreateUploader('#filepicker3', '#fileList3', 'TQClient', 'C_Attachment3', '#ImgUp3');
            CreateUploader('#filepicker4', '#fileList4', 'TQClient', 'C_Attachment4', '#ImgUp4');
            CreateUploader('#filepicker5', '#fileList5', 'TQClient', 'C_Attachment5', '#ImgUp5');
            CreateUploader('#filepicker6', '#fileList6', 'TQClient', 'C_Attachment6', '#ImgUp6');

            CreateUploader('#filepicker7', '#fileList7', 'TQClient', 'C_Attachment7', '#ImgUp7');
            CreateUploader('#filepicker8', '#fileList8', 'TQClient', 'C_Attachment8', '#ImgUp8');
            CreateUploader('#filepicker9', '#fileList9', 'TQClient', 'C_Attachment9', '#ImgUp9');
            CreateUploader('#filepicker10', '#fileList10', 'TQClient', 'C_Attachment10', '#ImgUp10');
        });
        $(function () {
            $('.imgUpload').click(function () {
                if (this.src == null || this.src == '' || this.src.split("/member/Data/Upload/Images/")[1] == '') return false;

                layer.open({
                    type: 1,
                    skin: 'layui-layer-nobg', //没有背景色
                    shadeClose: true,
                    content: "<img src='" + this.src + "' />",
                    area: ['auto', 'auto'],
                    offset: ['5%', '20%'],
                    closeBtn: [0, true]
                })
            })
        })
    </script>
    <style type="text/css">
        div#company .form-group p {
            color: rgba(198,196,188,1);
            margin-top: 10px;
        }

        div#company .form-group label {
            margin-top: -5px !important;
        }

        .clearfix :after {
            display: block;
            content: ",";
            height: 0;
            clear: both;
            visibility: hidden;
        }

        .redstar {
            color: red;
            position: relative;
            top: 1.4em;
        }

        .margintop10 {
            margin-top: -10px;
        }

        #btnLogin_save.change {
            background-color: #ddd !important;
            color: black !important;
        }
    </style>
    <div id="myAccount_view" data-ng-controller="accountController" data-ng-init="initClient()">
        <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 6px;">企业认证</label>
        <div style="display: inline-block; margin-left: 10px; color: gray;" data-ng-bind="StatusText"></div>

        <button class="btnOrange" style="float: right; width: 90px;" data-toggle="modal" onclick="$('#addClientModel').modal('show');">增加企业</button>


        <div style="border: 1px solid #ddd; float: left; padding-bottom: 20px; font-size: small; display: table;">
            <div style="padding-left: 15px; padding-right: 15px;">
                <div id="person" style="float: left; position: relative;">
                    <div style="float: left; width: 100%; padding-left: 15px; padding-right: 15px;">
                        <div style="float: left; width: 60%;">
                            <div class="form-group" style="margin-top: 20px;">
                                <label for="txtCompanyName" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>公司名称：</label>
                                <div class="col-md-9 inputControl">
                                    <input type="text" class="form-control" placeholder="公司名称" data-ng-disabled="!isEdit"
                                        data-ng-model="model.C_ClientName" style="width: 260px; display: inline-block;" />
                                    <a href="#" style="margin-left: 20px; margin-top: 13px; display: inline-block;" data-toggle="modal" data-ng-click="modifyClient()"
                                        data-ng-if="UserInfo.CompanyStatus == 1 || (UserInfo.CompanyStatus == 2 && UserInfo.isCompanyAdmin == 1)">
                                        <div class="btn btnOrange">企业变更</div>
                                    </a>
                                </div>
                            </div>
                            <div class="form-group" style="margin-top: 18px; float: left;" data-ng-if="UserInfo.CompanyStatus == 0 || UserInfo.CompanyStatus == 3">
                                <label for="txtCompanyAddress" class="control-label col-md-3" style="text-align: right; margin-top: 6px; width: 20%;"><span style="color: red;">*</span>公司地址：</label>
                                <div class="delivery dropdown" style="float: left; margin-left: 14px;">
                                    <div class="deliveryBorder" data-toggle="dropdown" data-target="#" style="padding-left: 14px;">
                                        <span data-ng-if="entity.LandName" data-ng-bind="entity.LandName"></span>
                                        <span data-ng-if="!entity.LandName" class="colorGray">省/市/区（县）</span>
                                        <span class="btn floatRight" style="padding: 6px 4px">
                                            <img src="../App_Themes/Atlantic/images/select.png" style="margin-top: -0.3em; margin-right: -0.28em;" />
                                        </span>
                                    </div>
                                    <div id="land" class="dropdown-menu panel" data-panel='plants'>
                                        <ul class="nav nav-tabs">
                                            <li data-ng-class="{active:tabLand===0}"><a data-toggle="tab" data-target="#" data-ng-click="getProvinces($event)">省份</a></li>
                                            <li data-ng-class="{active:tabLand===1}"><a data-toggle="tab" data-target="#" data-ng-click="getCities($event)">城市</a></li>
                                            <li data-ng-class="{active:tabLand===2}"><a data-toggle="tab" data-target="#" data-ng-click="getCounties($event)">县区</a></li>
                                        </ul>
                                        <ul class="nav nav-pills">
                                            <li data-ng-repeat="item in baseData.Province" data-ng-if="tabLand === 0">
                                                <a data-ng-click="provinceSelect($event, item)">{{item.C_ProvinceName}}</a>
                                            </li>
                                            <li data-ng-repeat="item in cities" data-ng-if="tabLand === 1">
                                                <a data-ng-click="citySelect($event, item)">{{item.C_CityName}}</a>
                                            </li>
                                            <li data-ng-repeat="item in counties" data-ng-if="tabLand === 2">
                                                <a data-ng-click="countySelect(item)">{{item.C_CountyName}}</a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div style="width: 370px; margin-top: 15px; float: left; margin-left: 22%;">
                                    <input type="text" class="form-control" placeholder="详细地址" data-ng-disabled="!isShow" data-ng-model="model.C_Address" />
                                </div>
                            </div>
                            <div class="form-group" style="margin-top: 18px; float: left;" data-ng-if="UserInfo.CompanyStatus == '1' || UserInfo.CompanyStatus == '2'">
                                <label for="txtCompanyAddress" class="control-label col-md-3" style="text-align: right; margin-top: 6px; width: 20%;"><span style="color: red;">*</span>公司地址：</label>
                                <div style="float: left; margin-left: 14px; margin-top: 10px;">
                                    <label id="LabelAddress" data-ng-bind="model.FullAddress"></label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group" style="margin-top: 20px; width: 100%; height: 50px; float: left;">
                        <label for="UserType" class="control-label col-md-3" style="text-align: right; width: 16.5%; margin-top: 6px; float: left;">公司类型：</label>
                        <div class="col-md-9 inputControl" style="margin-top: 10px;">
                            <div>
                                <div class="checkbox_group">
                                    <input type="checkbox" data-ng-model="model.TypeSell" data-ng-disabled="!isShow" />
                                    <label for="TXTSeller" class="LABEL_SLCBTNUserType">天然气卖家</label>
                                </div>
                                <div class="checkbox_group">
                                    <input type="checkbox" data-ng-model="model.TypeBuy" data-ng-disabled="!isShow" />
                                    <label for="TXTBuyer" class="LABEL_SLCBTNUserType">天然气买家</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="position: absolute; right: 20px; top: 35px;">
                        <div style="width: 120px; text-align: center; float: right; margin-left: 25px">
                            <%--<label class="control-label" style="text-align: center; margin-bottom: 16px;">上传企业图片:</label>--%>
                            <div class="margintop10" data-ng-hide="false">
                                <div style="display: inline; float: left; margin-left: 10px;">
                                    <img id="ImgClient" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_imgClient}}" height="100" width="100" />
                                    <input type="text" class="txt_FileUpload" id="textUpImgClient" hidden />
                                </div>
                                <div id="filepickerCP" style="display: inline;">上传企业图片</div>
                            </div>
                        </div>
                        <div style="width: 120px; text-align: center; float: right">
                            <%--<label class="control-label" style="text-align: center; margin-bottom: 16px;">上传企业标识:</label>--%>
                            <div class="margintop10">
                                <div style="display: inline; float: left; margin-left: 10px;">
                                    <img id="ImgIcon" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_imgIcon}}" height="100" width="100" />
                                    <input id="textUpImgIcon" type="text" class="txt_FileUpload" hidden />
                                </div>
                                <div id="filepickerIcon" style="display: inline;">上传企业标识</div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="company" style="float: left; margin-top: 20px; width: 100%;">
                    <div style="margin-top: 10px;">
                        <div style="width: 100%; margin-bottom: 15px; height: 40px; border-bottom: solid thin rgba(198,196,188,1);">
                            <label style="font-size: medium; margin-left: 20px; margin-top: 8px;">企业证件</label>
                            <span style="display: inline-block; font-size: 13px; color: #817c7c; margin-left: 15px;">温馨提示：公司名称要和营业执照一致；有效期要在有效期时间内；证件要清晰。</span>
                        </div>
                        <div style="margin-top: 20px;" class="clearfix">
                            <div class="form-group">
                                <div class="col-md-3">
                                    <label class="control-label textup" style="text-align: left; padding-left: 70px;"><span class="redstar" style="left: -0.5em">*</span>营业执照:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 10px;">
                                            <img id="ImgUp1" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment1}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList1" class="uploader-list"></div>
                                        <div id="filepicker1" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 10px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label class="control-label textup" style="text-align: left; padding-left: 49px;"><span class="redstar" style="left: -0.5em">*</span>组织机构代码证:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 10px;">
                                            <img id="ImgUp2" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment2}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList2" class="uploader-list"></div>
                                        <div id="filepicker2" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 10px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label class="control-label textup" style="text-align: left; padding-left: 62px;"><span class="redstar" style="left: -0.5em">*</span>税务登记证:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 10px;">
                                            <img id="ImgUp3" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment3}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList3" class="uploader-list"></div>
                                        <div id="filepicker3" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 10px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label class="control-label textup" style="text-align: left; padding-left: 62px;"><span class="redstar" style="left: -0.5em">*</span>开户许可证:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 10px;">
                                            <img id="ImgUp4" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment4}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList4" class="uploader-list"></div>
                                        <div id="filepicker4" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 10px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="form-group">
                                <div class="col-md-3">
                                    <label class="control-label textup" style="text-align: left; padding-left: 50px;"><span class="redstar" style="left: -0.5em">*</span>法人身份证正面:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 10px;">
                                            <img id="ImgUp5" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment5}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList5" class="uploader-list"></div>
                                        <div id="filepicker5" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 10px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label class="control-label textup" style="text-align: left; padding-left: 50px;"><span class="redstar" style="left: -0.5em">*</span>法人身份证反面:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 10px;">
                                            <img id="ImgUp6" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment6}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList6" class="uploader-list"></div>
                                        <div id="filepicker6" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 10px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="margin-top: 10px;">
                        <div style="width: 100%; margin-bottom: 15px; height: 40px; border-bottom: solid thin rgba(198,196,188,1);">
                            <label style="font-size: medium; margin-left: 20px; margin-top: 8px;">行业资质</label>
                        </div>
                        <div class="clearfix" style="margin-top: 20px;">
                            <div class="form-group" style="margin-top: 10px;">
                                <div class="col-md-3">
                                    <label class="control-label textup" style="text-align: left; padding-left: 50px;"><span class="redstar" style="left: -0.5em">*</span>燃气经营许可证:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 10px;">
                                            <img id="ImgUp7" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment7}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList7" class="uploader-list"></div>
                                        <div id="filepicker7" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 10px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3" style=" padding-right: 10px;">
                                    <label class="control-label textup" style="text-align: left; padding-left: 37px;"><span class="redstar" style="left: -0.5em">*</span>危险化学品经营许可证:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 15px;">
                                            <img id="ImgUp8" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment8}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList8" class="uploader-list"></div>
                                        <div id="filepicker8" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 16px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3" style="  padding-right: 10px;">
                                    <label class="control-label textup" style="text-align: left;padding-left: 37px;"><span class="redstar" style="left: -0.5em">*</span>危险化学品运输许可证:</label>
                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 15px;">
                                            <img id="ImgUp9" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment9}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList9" class="uploader-list"></div>
                                        <div id="filepicker9" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 15px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label class="control-label textup" style="text-align: left; padding-left: 73px;"><span class="redstar" style="left: -0.5em">*</span>充装证:</label>

                                    <div class="margintop10">
                                        <div style="display: inline; float: left; margin-left: 10px;">
                                            <img id="ImgUp10" class="imgUpload" data-ng-src="/member/Data/Upload/Images/{{model.C_Attachment10}}" height="145" width="175" />
                                        </div>
                                        <div id="fileList10" class="uploader-list"></div>
                                        <div id="filepicker10" data-ng-class="{true: 'webuploader-container', false: 'webuploader-element-invisible'}[isShow]"
                                            style="display: inline; float: left; margin-left: 11px; margin-top: 8px;">
                                            选择图片
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group" style="margin-top: 20px; margin-bottom: 1px; margin-left: 10%; width: 800px; float: left;">
                    <div class="col-md-10 col-md-offset-2">
                        <div class="btn save" data-ng-show="isShow" data-ng-click="saveClient()"
                            style="background-color: #0065e6; color: white; width: 100px; height: 38px;">
                            保存
                        </div>
                        <div id="btnLogin_save" class="btn save" data-ng-show="isShow" data-ng-click="submitClient()"
                            style="background-color: #0065e6; color: white; width: 100px; height: 38px; margin-left: 150px;">
                            提交审核
                        </div>
                        <label class="error" data-ng-bind="errorMessage"></label>
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
                        <button type="button" class="btn btn-default" style="width: 80px;" data-dismiss="modal">取消</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
