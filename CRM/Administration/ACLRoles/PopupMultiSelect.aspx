<%@ Page language="c#" MasterPageFile="~/PopupView.Master" Codebehind="PopupMultiSelect.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.Administration.ACLRoles.PopupMultiSelect" %>
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
<asp:Content ID="cntBody" ContentPlaceHolderID="cntBody" runat="server">
	<%@ Register TagPrefix="Taoqi" Tagname="SearchBasic" Src="SearchBasic.ascx" %>
	<Taoqi:SearchBasic ID="ctlSearch" Runat="Server" />
	<br />

<script type="text/javascript">
function SelectRole(sPARENT_ID, sPARENT_NAME)
{
	if ( window.opener != null && window.opener.ChangeRole != null )
	{
		window.opener.ChangeRole(sPARENT_ID, sPARENT_NAME);
		window.close();
	}
	else
	{
		alert('Original window has closed.  Role cannot be assigned.');
	}
}
function SelectChecked()
{
	if ( window.opener != null && window.opener.ChangeRole != null )
	{
		var sACLRoles = '';
		for ( var i = 0 ; i < document.all.length ; i++ )
		{
			if ( document.all[i].name == 'chkMain' )
			{
				if ( document.all[i].checked )
				{
					if ( sACLRoles.length > 0 )
						sACLRoles += ',';
					sACLRoles += document.all[i].value;
				}
			}
		}
		window.opener.ChangeRole(sACLRoles, '');
		window.close();
	}
	else
	{
		alert('Original window has closed.  Role cannot be assigned.');
	}
}
function Cancel()
{
	window.close();
}
</script>
	<%@ Register TagPrefix="Taoqi" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<Taoqi:ListHeader Title="ACLRoles.LBL_ROLE" Runat="Server" />

	<%@ Register TagPrefix="Taoqi" Tagname="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
	<Taoqi:DynamicButtons ID="ctlDynamicButtons" Runat="Server" />

	<Taoqi:SplendidGrid id="grdMain" SkinID="grdPopupView" EnableViewState="true" runat="server">
		<Columns>
			<asp:TemplateColumn HeaderText="" ItemStyle-Width="2%">
				<ItemTemplate>
					<input name="chkMain" class="checkbox" type="checkbox" value="<%# DataBinder.Eval(Container.DataItem, "ID") %>" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn    HeaderText="ACLRoles.LBL_NAME"        DataField="NAME"        SortExpression="NAME"        ItemStyle-Width="49%" />
			<asp:BoundColumn    HeaderText="ACLRoles.LBL_DESCRIPTION" DataField="DESCRIPTION" SortExpression="DESCRIPTION" ItemStyle-Width="49%" />
		</Columns>
	</Taoqi:SplendidGrid>
	<%@ Register TagPrefix="Taoqi" Tagname="CheckAll" Src="~/_controls/CheckAll.ascx" %>
	<Taoqi:CheckAll Visible="<%# !PrintView %>" Runat="Server" />

<%@ Register TagPrefix="Taoqi" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
<Taoqi:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</asp:Content>


