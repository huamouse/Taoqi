<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Calendar.ListView" %>
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
<div id="divListView">
	<%@ Register TagPrefix="Taoqi" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Calendar" Title="Calendar.LBL_MODULE_TITLE" EnableModuleLabel="false" EnablePrint="true" HelpName="index" EnableHelp="true" Runat="Server" />
	
	<asp:Table SkinID="tabFrame" runat="server">
		<asp:TableRow>
			<asp:TableCell Width="65%" VerticalAlign="top">
<%--				<%@ Register TagPrefix="Taoqi" Tagname="DayGrid" Src="~/Calendar/DayGrid.ascx" %>
				<Taoqi:DayGrid ID="ctlDayGrid" Runat="Server" />--%>
			</asp:TableCell>
			<asp:TableCell style="padding-left: 10px; vertical-align: top;">
				<%--<%@ Register TagPrefix="Taoqi" Tagname="Tasks" Src="~/Calendar/Tasks.ascx" %>--%>
				<%--<Taoqi:Tasks ID="ctlTasks" Visible='<%# Taoqi.Security.GetUserAccess("Tasks", "list") >= 0 %>' Runat="Server" />--%>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<%@ Register TagPrefix="Taoqi" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
	<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</div>


