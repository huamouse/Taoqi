<%@ Page Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="SetAdvertisement.aspx.cs" Inherits="Taoqi.Advertisement.SetAdvertisement" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cntBody" runat="server">

    <style>
        #SetAdvertisement_view .form-group{
            margin-top: 20px;
        }

        #SetAdvertisement_view .uploadDiv{
            margin-left:30%;
        }

        #SetAdvertisement_view .uploadDiv > input[type=file]{
            width:10px;
            visibility: hidden; 
            display:inline-block;
        }

        #SetAdvertisement_view .uploadDiv > .error{
            margin-left: 20px;
        }

        #SetAdvertisement_view .hint{
            margin-left:40%;
            color: rgba(198,196,188,1); 
            margin-top:10px;
        }

        #SetAdvertisement_view .uploadDiv_img{
            margin-top: 15px; 
            width:920px; 
            margin-left:auto; 
            margin-right:auto;
            height:306px;
            border: solid thin rgba(198,196,188,1);
        }

        #SetAdvertisement_view .img_showUpload{
            width:920px; 
            
        }
    </style>

    <script>
        $(function () {
            $(".hidden_FileUpload").change(function () {
                $("form").trigger("submit");
            });
        })
    </script>

    <div id="SetAdvertisement_view">

        <label style="font-size: large; font-weight: bold; margin-top: 4px;">广告位设置</label>

        <div style="border: 1px solid #ddd; padding-top: 40px; min-height: 750px; font-size: small;">

            <div class="form-group"  style="margin-top: 10px;">
                <div class="uploadDiv">
                    <label>广告位1：</label>

                    <input type="text" class="txt_FileUpload" id="txtFoo1" readonly="readonly" />

                    <input type="button" class="btn_SelectFile" onclick="btnFile1.click()" value="选择文件" />

                    <span id="error1" class="error" runat="server"></span>

                    <input type="file" class="hidden_FileUpload" name="btnFile1" onchange="txtFoo1.value=this.value;" />
                </div>
                    
                <div class="hint">
                    <p>支持JPG、JPEG、PNG格式。图片要求宽880px，高309px。</p>
                </div>

                <div class="uploadDiv_img banner1"></div>
            </div>

            <div class="form-group"  style="margin-top: 30px;">
                <div class="uploadDiv">
                    <label>广告位2：</label>

                    <input type="text" class="txt_FileUpload" id="txtFoo2" readonly="readonly" />

                    <input type="button" class="btn_SelectFile" onclick="btnFile2.click()" value="选择文件" />

                    <span id="error2" class="error" runat="server"></span>

                    <input type="file" class="hidden_FileUpload" name="btnFile2" style="visibility: hidden; display:inline-block;" onchange="txtFoo2.value=this.value;" />
                </div>
                    
                <div class="hint">
                    <p>支持JPG、JPEG、PNG格式。图片要求宽880px，高309px。</p>
                </div>

                <div class="uploadDiv_img banner2"></div>
            </div>


            <div class="form-group"  style="margin-top: 30px;">
                <div class="uploadDiv">
                    <label>广告位3：</label>

                    <input type="text" class="txt_FileUpload" id="txtFoo3" readonly="readonly" />

                    <input type="button" class="btn_SelectFile" onclick="btnFile3.click()" value="选择文件" />

                    <span id="error3" class="error" runat="server"></span>

                    <input type="file" class="hidden_FileUpload" name="btnFile3" style="visibility: hidden; display:inline-block;" onchange="txtFoo3.value=this.value;" />
                </div>
                    
                <div class="hint">
                    <p>支持JPG、JPEG、PNG格式。图片要求宽880px，高309px。</p>
                </div>

                <div class="uploadDiv_img banner3"></div>
            </div>

            <div class="form-group"  style="margin-top: 30px;">
                <div class="uploadDiv">
                    <label>广告位4：</label>

                    <input type="text" class="txt_FileUpload" id="txtFoo4" readonly="readonly" />

                    <input type="button" class="btn_SelectFile" onclick="btnFile4.click()" value="选择文件" />

                    <span id="error4" class="error" runat="server"></span>

                    <input type="file" class="hidden_FileUpload" name="btnFile4" style="visibility: hidden; display:inline-block;" onchange="txtFoo4.value=this.value;" />
                </div>
                    
                <div class="hint">
                    <p>支持JPG、JPEG、PNG格式。图片要求宽880px，高309px。</p>
                </div>

                <div class="uploadDiv_img banner4"></div>
            </div>

            <div class="form-group"  style="margin-top: 30px;">
                <div class="uploadDiv">
                    <label>广告位5：</label>

                    <input type="text" class="txt_FileUpload" id="txtFoo5" readonly="readonly" />

                    <input type="button" class="btn_SelectFile" onclick="btnFile5.click()" value="选择文件" />

                    <span id="error5" class="error" runat="server"></span>

                    <input type="file" class="hidden_FileUpload" name="btnFile5" style="visibility: hidden; display:inline-block;" onchange="txtFoo5.value=this.value;" />
                </div>
                    
                <div class="hint">
                    <p>支持JPG、JPEG、PNG格式。图片要求宽880px，高309px。</p>
                </div>

                <div class="uploadDiv_img banner5"></div>
            </div>

            <div class="form-group"  style="margin-top: 30px;">
                <div class="uploadDiv">
                    <label>广告位6：</label>

                    <input type="text" class="txt_FileUpload" id="txtFoo6" readonly="readonly" />

                    <input type="button" class="btn_SelectFile" onclick="btnFile6.click()" value="选择文件" />

                    <span id="error6" class="error" runat="server"></span>

                    <input type="file" class="hidden_FileUpload" name="btnFile6" style="visibility: hidden; display:inline-block;" onchange="txtFoo6.value=this.value;" />
                </div>
                    
                <div class="hint">
                    <p>支持JPG、JPEG、PNG格式。图片要求宽880px，高309px。</p>
                </div>

                <div class="uploadDiv_img banner6"></div>
            </div>

        </div>

    </div>

</asp:Content>