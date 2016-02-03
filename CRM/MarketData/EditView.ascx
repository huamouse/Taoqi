<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.TQMarketData.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>


<div class="titleBar2">
        <div class="txt2" style="color: black;display:inline-block;width:auto;margin-right:24px;">新增行情数据</div> 
        <%--<span class="colorRed">*</span><span class="colorGray">为必填项</span>--%>
    </div>

    <div class="container_body">
        <div id="divEditView" runat="server">
            <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
            <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQMarketData" ModuleDisplayName="行情数据" Title="新建/编辑行业资讯" EnablePrint="false" HelpName="EditView" EnableHelp="false" runat="Server" />
            <p>
                <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
               

                <asp:Table SkinID="tabForm" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <table id="tblMain" class="tabEditView" runat="server">
                                <tr>
                                    
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </p>

           
             <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="True" ShowRequired="true" runat="Server" />
            <Taoqi:DynamicButtons ID="ctlFooterButtons" Visible="False" ShowRequired="false" runat="Server" />
        </div>

    </div>
