<%@ Control Language="c#" AutoEventWireup="false" Codebehind="EditView.ascx.cs" Inherits="Taoqi.Administration.DynamicLayout.EditViews.EditView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script runat="server">
/**********************************************************************************************************************
 * Taoqi is a Customer Relationship Management program created by Taoqi Software, Inc. 
 * Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved.
 * 
 * This program is free software: you can redistribute it and/or modify it under the terms of the 
 * GNU Affero General Public License as published by the Free Software Foundation, either version 3 
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 * See the GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>. 
 * 
 * You can contact Taoqi Software, Inc. at email address support@Taoqi.com. 
 * 
 * In accordance with Section 7(b) of the GNU Affero General Public License version 3, 
 * the Appropriate Legal Notices must display the following words on all interactive user interfaces: 
 * "Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved."
 *********************************************************************************************************************/
</script>
<Taoqi:InlineScript runat="server">
<script type="text/javascript">
var sDynamicLayoutModule = '<%= ViewState["MODULE_NAME"] %>';
function NewEventPopup()
{
	return ModulePopup('BusinessRules', '<%= new Taoqi.DynamicControl(this, "NEW_EVENT_ID").ClientID %>', '<%= new Taoqi.DynamicControl(this, "NEW_EVENT_NAME").ClientID %>', 'Module=' + sDynamicLayoutModule, false, null);
}
function PreLoadEventPopup()
{
	return ModulePopup('BusinessRules', '<%= new Taoqi.DynamicControl(this, "PRE_LOAD_EVENT_ID").ClientID %>', '<%= new Taoqi.DynamicControl(this, "PRE_LOAD_EVENT_NAME").ClientID %>', 'Module=' + sDynamicLayoutModule, false, null);
}
function PostLoadEventPopup()
{
	return ModulePopup('BusinessRules', '<%= new Taoqi.DynamicControl(this, "POST_LOAD_EVENT_ID").ClientID %>', '<%= new Taoqi.DynamicControl(this, "POST_LOAD_EVENT_NAME").ClientID %>', 'Module=' + sDynamicLayoutModule, false, null);
}
function ValidationEventPopup()
{
	return ModulePopup('BusinessRules', '<%= new Taoqi.DynamicControl(this, "VALIDATION_EVENT_ID").ClientID %>', '<%= new Taoqi.DynamicControl(this, "VALIDATION_EVENT_NAME").ClientID %>', 'Module=' + sDynamicLayoutModule, false, null);
}
function PreSaveEventPopup()
{
	return ModulePopup('BusinessRules', '<%= new Taoqi.DynamicControl(this, "PRE_SAVE_EVENT_ID").ClientID %>', '<%= new Taoqi.DynamicControl(this, "PRE_SAVE_EVENT_NAME").ClientID %>', 'Module=' + sDynamicLayoutModule, false, null);
}
function PostSaveEventPopup()
{
	return ModulePopup('BusinessRules', '<%= new Taoqi.DynamicControl(this, "POST_SAVE_EVENT_ID").ClientID %>', '<%= new Taoqi.DynamicControl(this, "POST_SAVE_EVENT_NAME").ClientID %>', 'Module=' + sDynamicLayoutModule, false, null);
}
function LayoutDragOver(event, nDropIndex)
{
	// 08/08/2013   IE does not support preventDefault. 
	// http://stackoverflow.com/questions/1000597/event-preventdefault-function-not-working-in-ie
	event.preventDefault ? event.preventDefault() : event.returnValue = false;
}
function LayoutDropIndex(event, nDropIndex)
{
	// 08/08/2013   IE does not support preventDefault. 
	event.preventDefault ? event.preventDefault() : event.returnValue = false;
	var hidDragStartIndex = document.getElementById('<%= new Taoqi.DynamicControl(this, "hidDragStartIndex").ClientID %>');
	var hidDragEndIndex   = document.getElementById('<%= new Taoqi.DynamicControl(this, "hidDragEndIndex"  ).ClientID %>');
	var btnDragComplete   = document.getElementById('<%= new Taoqi.DynamicControl(this, "btnDragComplete"   ).ClientID %>');
	hidDragStartIndex.value = event.dataTransfer.getData('Text');
	hidDragEndIndex.value   = nDropIndex;
	if ( hidDragStartIndex.value != hidDragEndIndex.value )
	{
		btnDragComplete.click();
	}
}
</script>
</Taoqi:InlineScript>
<div id="divEditView" runat="server">
	<%@ Register TagPrefix="Taoqi" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Administration" Title="DynamicLayout.LBL_EDIT_VIEW_LAYOUT" EnablePrint="false" HelpName="EditView" EnableHelp="true" Runat="Server" />

	<asp:HiddenField ID="hidDragStartIndex" runat="server" />
	<asp:HiddenField ID="hidDragEndIndex"   runat="server" />
	<asp:Button      ID="btnDragComplete" CommandName="Layout.DragIndex" OnCommand="Page_Command" style="display:none" runat="server" />

	<asp:Table Width="100%" runat="server">
		<asp:TableRow>
			<asp:TableCell Width="200px" VerticalAlign="Top">
				<%@ Register TagPrefix="Taoqi" Tagname="SearchBasic" Src="../_controls/SearchBasic.ascx" %>
				<Taoqi:SearchBasic ID="ctlSearch" ViewTableName="vwEDITVIEWS_Layout" ViewFieldName="EDIT_NAME" Runat="Server" />
			</asp:TableCell>
			<asp:TableCell VerticalAlign="Top">
				<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
				<Taoqi:ListHeader ID="ctlListHeader" Runat="Server" />
				
				<%@ Register TagPrefix="Taoqi" Tagname="LayoutButtons" Src="../_controls/LayoutButtons.ascx" %>
				<Taoqi:LayoutButtons ID="ctlLayoutButtons" Visible="<%# !PrintView %>" Runat="Server" />

				<asp:Table ID="tblViewEventsPanel" Width="100%" CellPadding="0" CellSpacing="0" CssClass="" runat="server">
					<asp:TableRow>
						<asp:TableCell>
							<table ID="tblViewEvents" class="tabEditView" runat="server">
							</table>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>

				<input type="hidden" id="txtFieldState" runat="server" />
				<asp:Panel ID="pnlDynamicMain" runat="server">
					<asp:Table ID="tblForm" Width="100%" CellPadding="0" CellSpacing="0" CssClass="" runat="server">
						<asp:TableRow>
							<asp:TableCell>
								<table ID="tblMain" class="tabEditView" runat="server">
								</table>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</asp:Panel>
				
				<br />
				<%@ Register TagPrefix="Taoqi" Tagname="NewRecord" Src="~/Administration/DynamicLayout/EditViews/NewRecord.ascx" %>
				<Taoqi:NewRecord ID="ctlNewRecord" Visible="false" Runat="Server" />
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</div>


