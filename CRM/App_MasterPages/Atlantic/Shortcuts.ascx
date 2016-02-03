<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Shortcuts.ascx.cs" Inherits="Taoqi.Themes.Sugar.Shortcuts" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Import Namespace="System.Data" %>
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
<div id="divShortcuts">
<p>
<%@ Register TagPrefix="Taoqi" Tagname="HeaderLeft" Src="~/_controls/HeaderLeft.ascx" %>
<Taoqi:HeaderLeft ID="ctlHeaderLeft" runat="Server" Title=".LBL_SHORTCUTS" />


<div style="width: 180;">
	<ul class="subMenu">
<%
string sApplicationPath = Request.ApplicationPath;
if ( !sApplicationPath.EndsWith("/") )
	sApplicationPath += "/";
DataTable dt = SplendidCache.Shortcuts(sSubMenu);
if ( (Taoqi.Security.AdminUserAccess(sSubMenu, "access") >= 0) || !AdminShortcuts )
	{
	foreach(DataRow row in dt.Rows)
	{
		string sRELATIVE_PATH = Sql.ToString(row["RELATIVE_PATH"]);
		string sIMAGE_NAME    = Sql.ToString(row["IMAGE_NAME"   ]);
		string sID            = Sql.ToString(row["DISPLAY_NAME" ]).Replace(" ", "_");
		string sDISPLAY_NAME  = L10n.Term(Sql.ToString(row["DISPLAY_NAME"]));
		if ( sRELATIVE_PATH.StartsWith("~/") )
			sRELATIVE_PATH = sRELATIVE_PATH.Replace("~/", sApplicationPath);
		%>
		<li><a href="<%= sRELATIVE_PATH %>"><img src="<%= Sql.ToString(Session["themeURL"]) + "images/" + sIMAGE_NAME %>" alt="<%= L10n.Term(sDISPLAY_NAME) %>" border="0" width="16" height="16" align="absmiddle">&nbsp;<%= L10n.Term(sDISPLAY_NAME) %></a></li>
		<%
	}
}
%>
	</ul>
</div>
</p>
<asp:Image SkinID="spacer" Height="1" Width="180" runat="server" /><br />
</div>


