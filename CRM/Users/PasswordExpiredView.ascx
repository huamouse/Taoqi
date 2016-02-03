<%@ Control Language="c#" AutoEventWireup="false" Codebehind="PasswordExpiredView.ascx.cs" Inherits="Taoqi.Users.PasswordExpiredView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
}
function set_focus()
{
	var txtOLD_PASSWORD = document.getElementById('<%= txtOLD_PASSWORD.ClientID %>');
	var txtNEW_PASSWORD = document.getElementById('<%= txtNEW_PASSWORD.ClientID %>');
	txtNEW_PASSWORD.focus();
}
</script>
<div id="divPasswordExpired">
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
							<asp:Table Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="2" HorizontalAlign="Center" Runat="server">
								<asp:TableRow>
									<asp:TableCell style="font-size: 13px; padding-top: 5px;" ColumnSpan="2"><asp:Label ID="lblInstructions" Text='<%# L10n.Term("Users.LBL_PASSWORD_EXPIRED") %>' Runat="server" /></asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell ColumnSpan="2">
										&nbsp;<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow ID="trOLD_PASSWORD" runat="server">
									<asp:TableCell Width="40%" CssClass="dataLabel"><%# L10n.Term("Users.LBL_OLD_PASSWORD") %></asp:TableCell>
									<asp:TableCell Width="60%">
										<asp:TextBox ID="txtOLD_PASSWORD" size="20" style="width: 140px" TextMode="Password" TabIndex="1" Runat="server" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell Width="40%" CssClass="dataLabel"><%# L10n.Term("Users.LBL_NEW_PASSWORD") %></asp:TableCell>
									<asp:TableCell Width="60%">
										<asp:TextBox ID="txtNEW_PASSWORD" size="20" style="width: 140px" TextMode="Password" TabIndex="2" Runat="server" />
										<Taoqi:SplendidPassword ID="ctlNEW_PASSWORD_STRENGTH" TargetControlID="txtNEW_PASSWORD" HelpStatusLabelID="lblError" 
											DisplayPosition="RightSide" StrengthIndicatorType="Text" TextCssClass="error" HelpHandleCssClass="PasswordHelp" HelpHandlePosition="LeftSide" runat="server" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell Width="40%" CssClass="dataLabel"><%# L10n.Term("Users.LBL_CONFIRM_PASSWORD") %></asp:TableCell>
									<asp:TableCell Width="60%">
										<asp:TextBox ID="txtCONFIRM_PASSWORD" size="20" style="width: 140px" TextMode="Password" TabIndex="3" Runat="server" />
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									<asp:TableCell Width="40%" />
									<asp:TableCell Width="60%">
										<asp:Button ID="btnChangePassword" CommandName="ChangePassword" OnCommand="Page_Command" CssClass="button" TabIndex="4" Text='<%# " "  + L10n.Term("Users.LBL_CHANGE_PASSWORD_BUTTON_LABEL") + " "  %>' ToolTip='<%# L10n.Term("Users.LBL_CHANGE_PASSWORD_BUTTON_TITLE") %>' Runat="server" />
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
Response.Write(Utils.RegisterEnterKeyPress(txtNEW_PASSWORD.ClientID    , btnChangePassword.ClientID));
Response.Write(Utils.RegisterEnterKeyPress(txtCONFIRM_PASSWORD.ClientID, btnChangePassword.ClientID));
%>
</div>


