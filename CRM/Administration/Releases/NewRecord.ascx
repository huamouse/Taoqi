<%@ Control Language="c#" AutoEventWireup="false" Codebehind="NewRecord.ascx.cs" Inherits="Taoqi.Administration.Releases.NewRecord" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
<div id="divNewRecord">
	<%@ Register TagPrefix="Taoqi" Tagname="HeaderLeft" Src="~/_controls/HeaderLeft.ascx" %>
	<Taoqi:HeaderLeft ID="ctlHeaderLeft" Title="Releases.LBL_NEW_FORM_TITLE" Width=<%# uWidth %> Visible="<%# ShowHeader %>" Runat="Server" />

	<asp:Panel ID="pnlMain" Width="100%" CssClass="leftColumnModuleS3" runat="server">
		<%@ Register TagPrefix="Taoqi" Tagname="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
		<Taoqi:DynamicButtons ID="ctlDynamicButtons" Visible="<%# ShowTopButtons && !PrintView %>" Runat="server" />

		<asp:Panel ID="pnlEdit" CssClass="" style="margin-bottom: 4px;" Width=<%# uWidth %> runat="server">
			<asp:Literal Text='<%# "<h4>" + L10n.Term("Releases.LNK_NEW_RELEASE") + "</h4>" %>' Visible="<%# ShowInlineHeader %>" runat="server" />
			<table ID="tblMain" class="tabEditView" runat="server">
			</table>
		</asp:Panel>

		<Taoqi:DynamicButtons ID="ctlFooterButtons" Visible="<%# ShowBottomButtons && !PrintView %>" Runat="server" />
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>
</div>


