<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.TQLogisticsInvoice.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<div id="nav">
    <div class="nav_left">
        <span class="title">客户
        </span>
        <span class="site_Path">当前位置：客户 > <span class="current1">新建/编辑客户</span>

        </span>
    </div>
    <div class="nav_right">

      
        <a href="default.aspx">
            <img align="absmiddle" border="0" src="../include/images/back.png" />
            返回</a>
       
    </div>

</div>


<div class="tableStyle">
    <div class="chartTitle">
        <div class="txt">客户信息</div>
    </div>

    <div class="container_body">
        <div id="divEditView" runat="server">
            <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
            <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQLogisticsInvoice" ModuleDisplayName="客户" Title="新建/编辑客户" EnablePrint="false" HelpName="EditView" EnableHelp="false" runat="Server" />
            <p>
                <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
               

                <asp:Table SkinID="tabForm" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <table id="tblMain" class="tabEditView" runat="server">
                                <tr>
                                    <th colspan="4">
                                        <h4>客户信息
                                        </h4>
                                    </th>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </p>

             <div class="grid" style="margin-top: 15px;" runat="server" id="gridAttachment">
                <div>
                    <div class="title">
                        附件
                    </div>
                    <div class="title_expand">
                        <img src="/include/images/grid_collapse.png" align="absmiddle" />
                        展开/收起
                    </div>
                </div>
                <div id="panel4">
                   
                </div>

            </div>
             <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="True" ShowRequired="true" runat="Server" />
            <Taoqi:DynamicButtons ID="ctlFooterButtons" Visible="False" ShowRequired="false" runat="Server" />
        </div>

    </div>
