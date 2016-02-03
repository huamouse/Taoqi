<%@ Control CodeBehind="YearGrid.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Calendar.YearGrid" %>
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
<div id="divYear">
	<%@ Register TagPrefix="Taoqi" Tagname="CalendarHeader" Src="CalendarHeader.ascx" %>
	<Taoqi:CalendarHeader ID="ctlCalendarHeader" ActiveTab="Year" Runat="Server" />
	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
	</asp:Panel>

	<asp:Table SkinID="tabFrame" CssClass="monthBox" runat="server">
		<asp:TableRow>
			<asp:TableCell>
				<asp:Table SkinID="tabFrame" CssClass="monthHeader" runat="server">
					<asp:TableRow>
						<asp:TableCell Width="1%" CssClass="monthHeaderPrevTd" Wrap="false">
							<asp:ImageButton CommandName="Year.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term("Calendar.LBL_PREVIOUS_YEAR") %>' SkinID="calendar_previous" Runat="server" />&nbsp;
							<asp:LinkButton  CommandName="Year.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term("Calendar.LBL_PREVIOUS_YEAR") %>' Runat="server" />
						</asp:TableCell>
						<asp:TableCell Width="98%" HorizontalAlign="Center">
							<span class="monthHeaderH3"><%= dtCurrentDate.Year %></span>
						</asp:TableCell>
						<asp:TableCell Width="1%" HorizontalAlign="Right" CssClass="monthHeaderNextTd" Wrap="false">
							<asp:LinkButton  CommandName="Year.Next" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term("Calendar.LBL_NEXT_YEAR") %>' Runat="server" />&nbsp;
							<asp:ImageButton CommandName="Year.Next" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term("Calendar.LBL_NEXT_YEAR") %>' SkinID="calendar_next" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell CssClass="monthCalBody">
				<table id="tblDailyCalTable" width="100%" border="0" cellpadding="0" cellspacing="1" Runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Table SkinID="tabFrame" CssClass="monthFooter" runat="server">
					<asp:TableRow>
						<asp:TableCell Width="50%" CssClass="monthFooterPrev" Wrap="false">
							<asp:ImageButton CommandName="Year.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term("Calendar.LBL_PREVIOUS_YEAR") %>' SkinID="calendar_previous" Runat="server" />&nbsp;
							<asp:LinkButton  CommandName="Year.Previous" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term("Calendar.LBL_PREVIOUS_YEAR") %>' Runat="server" />
						</asp:TableCell>
						<asp:TableCell Width="50%" HorizontalAlign="Right" CssClass="monthFooterNext" Wrap="false">
							<asp:LinkButton  CommandName="Year.Next" OnCommand="Page_Command" CssClass="NextPrevLink" Text='<%# L10n.Term("Calendar.LBL_NEXT_YEAR") %>' Runat="server" />&nbsp;
							<asp:ImageButton CommandName="Year.Next" OnCommand="Page_Command" CssClass="NextPrevLink" AlternateText='<%# L10n.Term("Calendar.LBL_NEXT_YEAR") %>' SkinID="calendar_next" Runat="server" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</div>


