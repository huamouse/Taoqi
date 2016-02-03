<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.TQLogisticsInvoice.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<div id="nav">
    <div class="nav_left">
        <span class="title">�ͻ�
        </span>
        <span class="site_Path">��ǰλ�ã��ͻ� > <span class="current1">�½�/�༭�ͻ�</span>

        </span>
    </div>
    <div class="nav_right">

      
        <a href="default.aspx">
            <img align="absmiddle" border="0" src="../include/images/back.png" />
            ����</a>
       
    </div>

</div>


<div class="tableStyle">
    <div class="chartTitle">
        <div class="txt">�ͻ���Ϣ</div>
    </div>

    <div class="container_body">
        <div id="divEditView" runat="server">
            <%@ Register TagPrefix="Taoqi" TagName="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
            <Taoqi:ModuleHeader ID="ctlModuleHeader" Module="TQLogisticsInvoice" ModuleDisplayName="�ͻ�" Title="�½�/�༭�ͻ�" EnablePrint="false" HelpName="EditView" EnableHelp="false" runat="Server" />
            <p>
                <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
               

                <asp:Table SkinID="tabForm" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <table id="tblMain" class="tabEditView" runat="server">
                                <tr>
                                    <th colspan="4">
                                        <h4>�ͻ���Ϣ
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
                        ����
                    </div>
                    <div class="title_expand">
                        <img src="/include/images/grid_collapse.png" align="absmiddle" />
                        չ��/����
                    </div>
                </div>
                <div id="panel4">
                   
                </div>

            </div>
             <Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="True" ShowRequired="true" runat="Server" />
            <Taoqi:DynamicButtons ID="ctlFooterButtons" Visible="False" ShowRequired="false" runat="Server" />
        </div>

    </div>
