<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditView_MyAccount.ascx.cs" Inherits="Taoqi.Users.EditView_MyAccount" %>

<div id="myAccount_view">

    <script>
        var CityID = <%:CityID %>;
        var ProvinceID = <%: ProvinceID%>

        $(document).ready(function () {
            $("#SLC_ProvinceName option[value=1]").attr("selected","selected");
            //$("#SLC_CityName option[value=1]").attr("selected","selected");

            $("#<%: A_uploadClientImg.ClientID %>").click(function(){
                $("#UploadClientImg").trigger("click");
            });
            $("#UploadClientImg").change(function(){
                $("#btn_UploadClientImg").trigger("click");
            });
        })
    </script>

    <label style="font-size: large; font-weight: bold; margin-top: 4px; margin-bottom: 16px;">账户信息</label>

    <div style="border: solid #ddd; padding-top: 20px; padding-left: 15px; padding-right: 15px; min-height: 2380px; font-size: small;">

        <div id="person" style="height: 420px;">

            <div style="background-color: rgb(241,241,241); width: 100%; margin-bottom: 40px; height: 40px;">
                <label style="font-size: larger; margin-left: 15px; margin-top: 8px;">个人信息</label>
            </div>

            <div style="float: left; width: 100%;">
                <div style="float: left;">
                    <div class="form-group" style="min-height: 23px; margin-bottom: 26px;">
                        <label for="lblUSER_PHONE" class="col-md-3 control-label" style="text-align: right;">手机号码：</label>

                        <div class="col-md-9" style="text-align: left; line-height: 18px; margin-top: 1px;">
                            <label>
                                <asp:Literal ID="lblUSER_PHONE" runat="server"></asp:Literal></label>
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px; height: 50px;">
                        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>用户类型：</label>

                        <div class="col-md-9 inputControl">
                            <div class="checkbox_group">
                                <input type="checkbox" id="YHGC" name="SLCBTNUserType" value="1" runat="server" />
                                <label for="YHGC" class="LABEL_SLCBTNUserType">液化工厂</label>
                            </div>

                            <div class="checkbox_group">
                                <input type="checkbox" id="JSZ" name="SLCBTNUserType" value="2" runat="server" />
                                <label for="JSZ" class="LABEL_SLCBTNUserType">接收站</label>
                            </div>

                            <div class="checkbox_group">
                                <input type="checkbox" id="CYJQZ" name="SLCBTNUserType" value="3" runat="server" />
                                <label for="CYJQZ" class="LABEL_SLCBTNUserType">车用加气站</label>
                            </div>

                            <div class="checkbox_group">
                                <input type="checkbox" id="GYQHZ" name="SLCBTNUserType" value="4" runat="server" />
                                <label for="GYQHZ" class="LABEL_SLCBTNUserType">工业气化站</label>
                            </div>

                            <div class="checkbox_group">
                                <input type="checkbox" id="CSRQ" name="SLCBTNUserType" value="5" runat="server" />
                                <label for="CSRQ" class="LABEL_SLCBTNUserType">城市燃气</label>
                            </div>

                            <div class="checkbox_group">
                                <input type="checkbox" id="WLS" name="SLCBTNUserType" value="6" runat="server" />
                                <label for="WLS" class="LABEL_SLCBTNUserType">物流商</label>
                            </div>

                            <div class="checkbox_group">
                                <input type="checkbox" id="MYS" name="SLCBTNUserType" value="7" runat="server" />
                                <label for="MYS" class="LABEL_SLCBTNUserType">贸易商</label>
                            </div>
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtEMail" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">E-mail：</label>

                        <div class="col-md-9 inputControl" style="width: 360px;">
                            <asp:TextBox ID="txtEMail" TextMode="Email" CssClass="form-control" runat="server" placeholder="E-mail" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtRealName" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>真实姓名：</label>

                        <div class="col-md-9 inputControl" style="width: 260px;">
                            <asp:TextBox ID="txtRealName" CssClass="form-control" runat="server" placeholder="真实姓名" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtLinkman" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>公司联系人：</label>

                        <div class="col-md-9 inputControl" style="width: 260px;">
                            <asp:TextBox ID="txtLinkman" CssClass="form-control" runat="server" placeholder="联系人" />
                        </div>
                    </div>

                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtQQ" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">QQ账号：</label>

                        <div class="col-md-9 inputControl" style="width: 260px;">
                            <asp:TextBox ID="txtQQ" CssClass="form-control" runat="server" placeholder="QQ" />
                        </div>
                    </div>
                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtCompanyName" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>公司名称：</label>

                        <div class="col-md-9 inputControl" style="width: 360px;">
                            <asp:TextBox ID="txtCompanyName" CssClass="form-control" runat="server" placeholder="公司名称" />
                        </div>
                    </div>
                    <div class="form-group" style="margin-top: 20px;">
                        <label for="txtCompanyAbbreviation" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>公司简称：</label>

                        <div class="col-md-9 inputControl" style="width: 360px;">
                            <asp:TextBox ID="txtCompanyAbbreviation" CssClass="form-control" runat="server" placeholder="公司名称" />
                        </div>
                    </div>
                </div>

                
                <div style="float: right; margin-right: 20px;">
                    <div style="width: 120px; text-align: center;">
                        <img src="../Include/images/userPhoto.jpg" id="ClientImg" style="height: 120px; width: inherit;" runat="server"/>
                        <a style="color: red; margin-top: 5px;" id="A_uploadClientImg" href="#" runat="server">上传企业标识</a>
                        <input type="file" style="visibility:hidden" name="btnFile8" id="UploadClientImg" />
                        <asp:Label ID="error8" CssClass="error" EnableViewState="false" runat="server" />
                        <button type="submit" id="btn_UploadClientImg" name="btn_UploadClientImg" value="btn_UploadClientImg" style="visibility:hidden" />
                    </div>
                </div>
            </div>

            <div class="form-group" style="margin-top: 18px; width: 100%; float: left;">
                <label for="txtCompanyAddress" class="control-label col-md-3" style="text-align: right; margin-top: 6px; width: 14%;"><span style="color: red;">*</span>公司地址：</label>

              
                <div class="col-md-9 inputControl" style="margin-top: 4px; width: auto;">

                    
                    <select id="SLC_ProvinceName" name="SLC_ProvinceName" class="searchBar"
                         ng-change="ProvinceChange()" ng-model="entity.C_ProvinceID"
                         data-ng-init="entity.C_ProvinceID=<%= this.ProvinceID %>;ProvinceChange();"
                        style="height: 34px; background-color: rgb(241,241,241); padding-left: 10px; min-width: 120px;">
                        <option value="">省</option>
                        <option ng-repeat="item in baseData.Province" value="{{item.C_ProvinceID}}" 
                            ng-selected="item.C_ProvinceID == <%= this.ProvinceID %>">{{item.C_ProvinceName}}</option>
                    </select>
                 

                    <select id="SLC_CityName" name="SLC_CityName" class="searchBar" ng-model="entity.C_CityID" style="height: 34px; background-color: rgb(241,241,241); padding-left: 10px; min-width: 120px;">
                        <option value="">市</option>
                         <option ng-repeat="item in cityList" value="{{item.C_CityID}}"
                                        ng-selected="item.C_CityID == <%= this.CityID %>">{{item.C_CityName}}</option>

                
                    </select>

                    <%--
                    <select id="SLC_CountyName" name="SLC_CountyName" class="searchBar" ng-model="C_CountyID" ng-change="search()" style="height: 34px; background-color: rgb(241,241,241); padding-left: 10px; min-width: 120px;">
                        <option value="">区/县</option>
                        <option ng-repeat="item in regionList" value="{{item.C_CountyID}}">{{item.C_CountyName}}</option>
                    </select>
                    --%>

                    <div style="width: 370px; margin-left: 5px; display: inline-block;">
                        <asp:TextBox ID="detailAddress" CssClass="form-control" runat="server" placeholder="详细地址" />
                    </div>
                </div>
            </div>

            <div class="form-group" style="margin-top: 20px; margin-bottom: 1px; margin-left: 10px; float: left;">
                <div class="col-md-10 col-md-offset-2">
                    <asp:Button ID="btnLogin" OnClick="btnLogin_Click"
                        CssClass="btn" Text='保存' runat="server" Style="background-color: rgb(1,111,252); color: white; width: 100px; height: 46px;" />
                    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
                </div>
            </div>
        </div>

        <div id="company" style="float: left; margin-top: 20px;">

            <div style="background-color: rgb(241,241,241); width: 100%; margin-bottom: 40px; height: 40px;">
                <label style="font-size: larger; margin-left: 15px; margin-top: 8px;">企业信息</label>

                <div style="margin-top: 10px;">
                    <div style="width: 100%; margin-bottom: 15px; height: 40px; border-bottom: solid thin rgba(198,196,188,1);">
                        <label style="font-size: medium; margin-left: 20px; margin-top: 8px;">企业证件</label>
                    </div>

                    <div>
                        <div class="form-group" style="margin-top: 20px;">
                            <label class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>营业执照：</label>

                            <div class="col-md-9">
                                <input type="text" class="txt_FileUpload" id="txtFoo1" readonly="readonly" />

                                <input type="button" class="btn_SelectFile" onclick="btnFile1.click()" value="选择文件" />

                                <input type="file" class="hidden_FileUpload" name="btnFile1" style="visibility: hidden" onchange="txtFoo1.value=this.value;" />
                            </div>

                            <div class="col-md-9" style="margin-top: 15px;">
                                <img class="img_showUpload" id="img_Upload1" runat="server" style="float: left;" />

                                <div style="float: left; margin-top: 90px; margin-left: 10px;">
                                    <asp:Button CssClass="btn_Upload" ID="btn_Upload1" OnClick="btn_Upload1_Click" Text="上传" Style="margin-bottom: 10px;" runat="server" />
                                    <a id="error1" class="error" EnableViewState="false" href="#" target="_Blank" runat="server"></a>
                                    
                                    <p style="color: rgba(198,196,188,1);">支持JPG、JPEG、PNG格式，大小限制2M以内</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="margin-top: 10px;">

                        <div class="form-group" style="margin-top: 20px;">
                            <label class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>企业证件：</label>

                            <div class="col-md-9">
                                <input type="text" class="txt_FileUpload" id="txtFoo2" readonly="readonly" />

                                <input type="button" class="btn_SelectFile" onclick="btnFile2.click()" value="选择文件" />

                                <input type="file" class="hidden_FileUpload" name="btnFile2" style="visibility: hidden" onchange="txtFoo2.value=this.value;" />
                            </div>

                            <div class="col-md-9" style="margin-top: 15px;">
                                <img class="img_showUpload" id="img_Upload2" runat="server" style="float: left;" />

                                <div style="float: left; margin-top: 90px; margin-left: 10px;">
                                    <asp:Button CssClass="btn_Upload" ID="btn_Upload2" OnClick="btn_Upload2_Click" Text="上传" Style="margin-bottom: 10px;" runat="server" />
                                    <a id="error2" class="error" EnableViewState="false" href="#" target="_Blank" runat="server"></a>
                                    
                                    <p style="color: rgba(198,196,188,1);">支持JPG、JPEG、PNG格式，大小限制2M以内</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="margin-top: 10px;">

                        <div class="form-group" style="margin-top: 20px;">
                            <label class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>组织机构代码证：</label>

                            <div class="col-md-9">
                                <input type="text" class="txt_FileUpload" id="txtFoo3" readonly="readonly" />

                                <input type="button" class="btn_SelectFile" onclick="btnFile3.click()" value="选择文件" />

                                <input type="file" class="hidden_FileUpload" name="btnFile3" style="visibility: hidden" onchange="txtFoo3.value=this.value;" />
                            </div>

                            <div class="col-md-9" style="margin-top: 15px;">
                                <img class="img_showUpload" id="img_Upload3" runat="server" style="float: left;" />

                                <div style="float: left; margin-top: 90px; margin-left: 10px;">
                                    <asp:button CssClass="btn_Upload" ID="btn_Upload3" onclick="btn_Upload3_Click" Text="上传" style="margin-bottom: 10px;" runat="server" />
                                    <a id="error3" class="error" EnableViewState="false" href="#" target="_Blank" runat="server"></a>
                                    
                                    <p style="color: rgba(198,196,188,1);">支持JPG、JPEG、PNG格式，大小限制2M以内</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="margin-top: 10px;">

                        <div class="form-group" style="margin-top: 20px;">
                            <label class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>税务登记证：</label>

                            <div class="col-md-9">
                                <input type="text" class="txt_FileUpload" id="txtFoo4" readonly="readonly" />

                                <input type="button" class="btn_SelectFile" onclick="btnFile4.click()" value="选择文件" />

                                <input type="file" class="hidden_FileUpload" name="btnFile4" style="visibility: hidden" onchange="txtFoo4.value=this.value;" />
                            </div>

                            <div class="col-md-9" style="margin-top: 15px;">
                                <img class="img_showUpload" id="img_Upload4" runat="server" style="float: left;" />

                                <div style="float: left; margin-top: 90px; margin-left: 10px;">
                                    <asp:button CssClass="btn_Upload"  ID="btn_Upload4" onclick="btn_Upload4_Click" Text="上传" style="margin-bottom: 10px;" runat="server" />
                                    <a id="error4" class="error" EnableViewState="false" href="#" target="_Blank" runat="server"></a>
                                    
                                    <p style="color: rgba(198,196,188,1);">支持JPG、JPEG、PNG格式，大小限制2M以内</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="margin-top: 10px;">

                        <div class="form-group" style="margin-top: 20px;">
                            <label class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>开户许可证：</label>

                            <div class="col-md-9">
                                <input type="text" class="txt_FileUpload" id="txtFoo5" readonly="readonly" />

                                <input type="button" class="btn_SelectFile" onclick="btnFile5.click()" value="选择文件" />

                                <input type="file" class="hidden_FileUpload" name="btnFile5" style="visibility: hidden" onchange="txtFoo5.value=this.value;" />
                            </div>

                            <div class="col-md-9" style="margin-top: 15px;">
                                <img class="img_showUpload" id="img_Upload5" runat="server" style="float: left;" />

                                <div style="float: left; margin-top: 90px; margin-left: 10px;">
                                    <asp:button CssClass="btn_Upload" ID="btn_Upload5" onclick="btn_Upload5_Click" Text="上传" style="margin-bottom: 10px;" runat="server" />
                                    <a id="error5" class="error" EnableViewState="false" href="#" target="_Blank" runat="server"></a>
                                    
                                    <p style="color: rgba(198,196,188,1);">支持JPG、JPEG、PNG格式，大小限制2M以内</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div style="margin-top: 10px;">

                        <div class="form-group" style="margin-top: 20px;">
                            <label class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>法人身份证：</label>

                            <div class="col-md-9">
                                <input type="text" class="txt_FileUpload" id="txtFoo6" readonly="readonly" />

                                <input type="button" class="btn_SelectFile" onclick="btnFile6.click()" value="选择文件" />

                                <input type="file" class="hidden_FileUpload" name="btnFile6" style="visibility: hidden" onchange="txtFoo6.value=this.value;" />
                            </div>

                            <div class="col-md-9" style="margin-top: 15px;">
                                <img class="img_showUpload" id="img_Upload6" runat="server" style="float: left;" />

                                <div style="float: left; margin-top: 90px; margin-left: 10px;">
                                    <asp:button CssClass="btn_Upload" ID="btn_Upload6" onclick="btn_Upload6_Click" Text="上传" style="margin-bottom: 10px;" runat="server" />
                                    <a id="error6" class="error" EnableViewState="false" href="#" target="_Blank" runat="server"></a>
                                    
                                    <p style="color: rgba(198,196,188,1);">支持JPG、JPEG、PNG格式，大小限制2M以内</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div style="margin-top: 10px;">
                    <div style="width: 100%; margin-bottom: 15px; height: 40px; border-bottom: solid thin rgba(198,196,188,1);">
                        <label style="font-size: medium; margin-left: 20px; margin-top: 8px;">行业资质</label>
                    </div>

                    <div>
                        <div class="form-group" style="margin-top: 20px;">
                            <label class="control-label col-md-3" style="text-align: right; margin-top: 6px; width: 18%; margin-left: 6%;"><span style="color: red;">*</span>燃气（危险化学品）经营许可证：</label>

                            <div class="col-md-9">
                                <input type="text" class="txt_FileUpload" id="txtFoo7" readonly="readonly" />

                                <input type="button" class="btn_SelectFile" onclick="btnFile7.click()" value="选择文件" />

                                <input type="file" class="hidden_FileUpload" name="btnFile7" style="visibility: hidden" onchange="txtFoo7.value=this.value;" />
                            </div>

                            <div class="col-md-9" style="margin-top: 15px;">
                                <img class="img_showUpload" id="img_Upload7" runat="server" style="float: left;" />

                                <div style="float: left; margin-top: 90px; margin-left: 10px;">
                                    <asp:button CssClass="btn_Upload" ID="btn_Upload7" onclick="btn_Upload7_Click" Text="上传" style="margin-bottom: 10px;" runat="server" />
                                    <a id="error7" class="error" EnableViewState="false" href="#" target="_Blank" runat="server"></a>

                                    <p style="color: rgba(198,196,188,1);">支持JPG、JPEG、PNG格式，大小限制2M以内</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<%--<script type="text/javascript">
    function btnLogin_Click(){
        var tca= $('#<%= txtCompanyAbbreviation.ClientID %>').val();
        if(tca.length>7){
            alert('公司简称请勿超过七个字！');
            return false;
        }
    }
</script>--%>