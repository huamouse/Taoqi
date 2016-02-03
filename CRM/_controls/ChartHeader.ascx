<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ChartHeader.ascx.cs" Inherits="Taoqi._controls.ChartHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
<asp:Table Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="0" CssClass="h3Row" runat="server">
	<asp:TableRow>
		<asp:TableCell Wrap="false">
			<h3><asp:Image SkinID="h3Arrow" Runat="server" />&nbsp;<asp:Label Text='<%# L10n.Term(sTitle) %>' runat="server" /></h3>
		</asp:TableCell>
		<asp:TableCell HorizontalAlign="Right" Wrap="false">
			<asp:ImageButton CommandName="Refresh" OnCommand="Page_Command" CssClass="chartToolsLink" AlternateText='<%# L10n.Term("Dashboard.LBL_REFRESH") %>' SkinID="refresh" ImageAlign="AbsMiddle" Runat="server" />&nbsp;
			<asp:LinkButton  CommandName="Refresh" OnCommand="Page_Command" CssClass="chartToolsLink"          Text='<%# L10n.Term("Dashboard.LBL_REFRESH") %>' Runat="server" />&nbsp;
			<span onclick="toggleDisplay('<%= DivEditName %>'); return false;">
				<asp:ImageButton CommandName="Edit" OnCommand="Page_Command" CssClass="chartToolsLink" AlternateText='<%# L10n.Term("Dashboard.LBL_EDIT"  ) %>' SkinID="edit"    ImageAlign="AbsMiddle" Runat="server" />&nbsp;
				<asp:LinkButton  CommandName="Edit" OnCommand="Page_Command" CssClass="chartToolsLink"          Text='<%# L10n.Term("Dashboard.LBL_EDIT"  ) %>' Runat="server" />
			</span>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>


