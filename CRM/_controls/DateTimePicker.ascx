<%@ Control Language="c#" AutoEventWireup="false" Codebehind="DateTimePicker.ascx.cs" Inherits="Taoqi._controls.DateTimePicker" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
<asp:Table BorderWidth="0" CellPadding="0" CellSpacing="0" runat="server">
	<asp:TableRow>
		<asp:TableCell Wrap="false">
			<asp:TextBox ID="txtDATE" TabIndex="1" size="11" MaxLength="40" OnTextChanged="Date_Changed" Runat="server" />
			<ajaxToolkit:CalendarExtender TargetControlID="txtDATE" PopupButtonID="imgCalendar" Animated="false" runat="server" />
			&nbsp;<asp:Image ID="imgCalendar" AlternateText='<%# L10n.Term(".LBL_ENTER_DATE") %>' SkinID="Calendar" Runat="server" />
			&nbsp;
		</asp:TableCell>
		<asp:TableCell Wrap="false">
			<asp:DropDownList ID="lstHOUR"     TabIndex="1" OnSelectedIndexChanged="Date_Changed" Runat="server" />
			<asp:DropDownList ID="lstMINUTE"   TabIndex="1" OnSelectedIndexChanged="Date_Changed" Runat="server" />
			<asp:DropDownList ID="lstMERIDIEM" TabIndex="1" OnSelectedIndexChanged="Date_Changed" Visible="false" Runat="server" />
		</asp:TableCell>
		<asp:TableCell>
			<!-- 08/31/2006   We cannot use a regular expression validator because there are just too many date formats. -->
			<Taoqi:DateValidator ID="valDATE" ControlToValidate="txtDATE" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
			<asp:RequiredFieldValidator     ID="reqDATE" ControlToValidate="txtDATE" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Wrap="false"><asp:Label ID="lblDATEFORMAT" CssClass="dateFormat" Runat="server" /></asp:TableCell>
		<asp:TableCell Wrap="false"><asp:Label ID="lblTIMEFORMAT" CssClass="dateFormat" Runat="server" /></asp:TableCell>
		<asp:TableCell Wrap="false">&nbsp;</asp:TableCell>
	</asp:TableRow>
</asp:Table>


