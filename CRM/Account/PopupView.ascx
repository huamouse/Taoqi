<%@ Control CodeBehind="PopupView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Account.PopupView" %>
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
<div id="divPopupView">
    <%@ Register TagPrefix="Taoqi" TagName="SearchView" Src="~/_controls/SearchView.ascx" %>

    <div class="container_body">
        <Taoqi:SearchView ID="ctlSearchView" Module="Users" IsPopupSearch="true" ShowSearchTabs="false" Visible="<%# !PrintView %>" runat="Server" />

        <div style="color:silver;">说明：除了系统管理员，用户只能看见自己公司的用户。</div>

        <script type="text/javascript">
            function SelectUser(sPARENT_ID, sPARENT_NAME) {
                if (window.opener != null && window.opener.ChangeUser != null) {
                    window.opener.ChangeUser(sPARENT_ID, sPARENT_NAME);
                    window.close();
                }
                else {
                    layer.alert('Original window has closed.  User cannot be assigned.' + '\n' + sPARENT_ID + '\n' + sPARENT_NAME, {
                        area: ['385px', '178px'],
                        offset: ['195px', '500px'],
                    });
                }
            }

            function Clear() {
                if (window.opener != null && window.opener.ChangeUser != null) {
                    window.opener.ChangeUser('', '');
                    window.close();
                }
                else {
                    alert('Original window has closed.  User cannot be assigned.', {
                        area: ['385px', '178px'],
                        offset: ['195px', '500px'],
                    });
                }
            }

            function Cancel() {
                window.close();
            }
        </script>
        <%@ Register TagPrefix="Taoqi" TagName="ListHeader" Src="~/_controls/ListHeader.ascx" %>
        <Taoqi:ListHeader Title="Users.LBL_LIST_FORM_TITLE" runat="Server" Visible="false" />

        <%@ Register TagPrefix="Taoqi" TagName="DynamicButtons" Src="~/_controls/DynamicButtons.ascx" %>
        <Taoqi:DynamicButtons ID="ctlDynamicButtons" runat="Server" Visible="false" />

        <Taoqi:SplendidGrid ID="grdMain" SkinID="grdPopupView" EnableViewState="true" runat="server">
            <Columns>
            </Columns>
        </Taoqi:SplendidGrid>
    </div>
</div>


