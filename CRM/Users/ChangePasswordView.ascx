<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ChangePasswordView.ascx.cs" Inherits="Taoqi.Users.ChangePasswordView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<script type="text/javascript">
window.onload = function()
{
	set_focus();
	try
	{
		// 01/08/2008   showLeftCol does not exist on the mobile master page. 
		showLeftCol(false, false);
	}
	catch(e)
	{
	}
}
function set_focus()
{
	var txtUSER_NAME = document.getElementById('<%= txtUSER_NAME.ClientID %>');
	txtUSER_NAME.focus();
}
</script>
<div id="divChangePassword">
	<div style="height: 80px;" runat="server" />
	<asp:Table HorizontalAlign="Center" CellPadding="0" CellSpacing="0" CssClass="ModuleActionsShadingTable" style="width: 450px;" runat="server">
		<asp:TableRow>
			<asp:TableCell ColumnSpan="3" CssClass="ModuleActionsShadingHorizontal" />
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="ModuleActionsShadingVertical" />
			<asp:TableCell>
				<asp:Table Width="100%" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" CssClass="ModuleActionsInnerTable" runat="server">
					<asp:TableRow>
						<asp:TableCell style="padding-top: 20px; padding-bottom: 20px; padding-left: 40px; padding-right: 40px;">
							<asp:Table Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="2" runat="server">
								<asp:TableRow runat="server">
									<asp:TableCell style="font-family: Arial; font-size: 14pt; font-weight: bold; color: #003564;">
										Taoqi <%# Application["CONFIG.service_level"] %>
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
							<asp:Table Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="2" HorizontalAlign="Center" Runat="server">
								<asp:TableRow>
									<asp:TableCell style="font-size: 13px; padding-top: 5px;"><asp:Label ID="lblInstructions" Text='<%# L10n.Term(".NTC_LOGIN_MESSAGE") %>' Runat="server" /></asp:TableCell>
									<asp:TableCell><asp:Label ID="lblPasswordHelp" CssClass="error" EnableViewState="false" runat="server" />&nbsp;</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow ID="trError" Visible="false" runat="server">
									<asp:TableCell ColumnSpan="2">
										<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell Width="40%" CssClass="dataLabel"><%# L10n.Term("Users.LBL_USER_NAME") %></asp:TableCell>
									<asp:TableCell Width="60%">
										<asp:TextBox ID="txtUSER_NAME" size="20" style="width: 140px" Runat="server" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell Width="40%" CssClass="dataLabel"><%# L10n.Term("Users.LBL_NEW_PASSWORD") %></asp:TableCell>
									<asp:TableCell Width="60%">
										<asp:TextBox ID="txtNEW_PASSWORD" size="20" style="width: 140px" TextMode="Password" Runat="server" />
										<Taoqi:SplendidPassword ID="ctlNEW_PASSWORD_STRENGTH" TargetControlID="txtNEW_PASSWORD" HelpStatusLabelID="lblPasswordHelp" 
											DisplayPosition="RightSide" StrengthIndicatorType="Text" TextCssClass="error" HelpHandleCssClass="PasswordHelp" HelpHandlePosition="LeftSide" runat="server" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell Width="40%" CssClass="dataLabel"><%# L10n.Term("Users.LBL_CONFIRM_PASSWORD") %></asp:TableCell>
									<asp:TableCell Width="60%">
										<asp:TextBox ID="txtCONFIRM_PASSWORD" size="20" style="width: 140px" TextMode="Password" Runat="server" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell Width="40%" />
									<asp:TableCell Width="60%">
										<asp:Button ID="btnLogin" CommandName="Login" OnCommand="Page_Command" CssClass="button" Text='<%# " "  + L10n.Term("Users.LBL_LOGIN_BUTTON_LABEL") + " "  %>' ToolTip='<%# L10n.Term("Users.LBL_LOGIN_BUTTON_TITLE") %>' Runat="server" />
									</asp:TableCell>
								</asp:TableRow>
							</asp:Table>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
			<asp:TableCell CssClass="ModuleActionsShadingVertical" />
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="3" CssClass="ModuleActionsShadingHorizontal" />
		</asp:TableRow>
	</asp:Table>
	<br />
	<br />
<%
Response.Write(Utils.RegisterEnterKeyPress(txtUSER_NAME.ClientID       , btnLogin.ClientID));
Response.Write(Utils.RegisterEnterKeyPress(txtNEW_PASSWORD.ClientID    , btnLogin.ClientID));
Response.Write(Utils.RegisterEnterKeyPress(txtCONFIRM_PASSWORD.ClientID, btnLogin.ClientID));
%>
</div>


