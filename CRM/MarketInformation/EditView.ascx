<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.TQMarketInformation.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>


<link href="../Include/javascript/Upload/webuploader.css" rel="stylesheet" />
<script src="../Include/javascript/Upload/webuploader.js"></script>
<script src="../Include/javascript/Upload/UploadJS.js"></script>
<style>
    #ctl00_cntBody_ctlEditView_ctlDynamicButtons_pnlDynamicButtons {
        width:9px;
    }
</style>
<style>
    .C_MarketInformationContext{
       height: 356px;
    }
</style>

<script>
    jQuery(function () {
        //第一个参数是  “按钮触发弹框上传文件”，第二个是存保存的图片地址,第四个是存储的表明 第五个是字段
        CreateUploader('#filepickerIcon', '#textUpImgIcon', 'TQMarketInformation', 'C_Cover', '#ImgIcon');
    });

    $(function () {
        $('#<%: FindControl("tblMain").ClientID %> > tbody > tr:eq(5)').addClass("C_MarketInformationContext");
    })
</script>

<div class="tableStyle">

    <div class="container_body">
        <div id="divEditView" runat="server">
            <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
            <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQMarketInformation" ModuleDisplayName="行业资讯" Title="新建/编辑行业资讯" EnablePrint="false" HelpName="EditView" EnableHelp="false" runat="Server" />
            <p>
                <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
               

                <asp:Table SkinID="tabForm" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <table id="tblMain" class="tabEditView" runat="server">
                                <tr>
                                    <th colspan="4">
                                        <h4 style="font-size:18px;font-weight:bold;margin-bottom:17px;padding-left:0;">新建/编辑行业资讯
                                        </h4>
                                    </th>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </p>
            
            <asp:PlaceHolder ID="UploadImg" runat="server">
                <div>
                    <label class="control-label">封面图片:</label>
                    <div class="margintop10">
                        <div style="display: inline; float: left; margin-left: 10px;">
                            <img id="ImgIcon" src="<%: C_CoverURL %>" height="100" width="100" />
                            <input id="textUpImgIcon" type="text" class="txt_FileUpload" hidden />
                        </div>
                        <div id="filepickerIcon" style="display: inline; float: left; margin-left: 19px; margin-top: 8px;">选择图片</div>
                    </div>
                </div>
            </asp:PlaceHolder>
            

           
             <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="True" ShowRequired="true" runat="Server" />
            <Taoqi:DynamicButtons ID="ctlFooterButtons" Visible="False" ShowRequired="false" runat="Server" />
        </div>

    </div>
