<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ExportHeader.ascx.cs" Inherits="Taoqi._controls.ExportHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<asp:Table SkinID="tabFrame" CssClass="h3Row" runat="server" Style="border:none; background:none;">
		<asp:TableRow >
<%--			<asp:TableCell Wrap="false" >

                <div class="listTitleLeft">
				<asp:Image SkinID="h3Arrow" Runat="server" Visible="false" />&nbsp;<asp:Label Text='<%# L10n.Term(sTitle) %>' runat="server" />
			</div>
            </asp:TableCell>--%>
			<asp:TableCell HorizontalAlign="Right" CssClass="listTitleRight" >
				<div id="divExport" Visible="true" Runat="server" Style="border:none; background:none;" >
					<asp:DropDownList ID="lstEXPORT_RANGE"  Runat="server" Width="85" Visible="false" />
					<asp:DropDownList ID="lstEXPORT_FORMAT" Runat="server" Width="105" Visible="false" />
					<asp:Button       ID="btnExport" CommandName="Export"  OnCommand="Page_Command" CssClass="button" Text='����Excel' ToolTip='����Excel' Runat="server" Style="border:none; background:none;" />
				</div>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>


