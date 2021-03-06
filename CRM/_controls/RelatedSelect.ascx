<%@ Control Language="c#" AutoEventWireup="false" Codebehind="RelatedSelect.ascx.cs" Inherits="Taoqi._controls.RelatedSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
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
<asp:UpdatePanel UpdateMode="Conditional" runat="server">
	<ContentTemplate>
		<asp:GridView ID="grdMain" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="false"
			AutoGenerateEditButton="false" AutoGenerateDeleteButton="false" OnRowCreated="grdMain_RowCreated" OnRowDataBound="grdMain_RowDataBound"
			OnRowEditing="grdMain_RowEditing" OnRowDeleting="grdMain_RowDeleting" OnRowUpdating="grdMain_RowUpdating" OnRowCancelingEdit="grdMain_RowCancelingEdit" 
			CssClass="listView" runat="server">
			<RowStyle            CssClass="oddListRowS1"  VerticalAlign="Top" />
			<AlternatingRowStyle CssClass="evenListRowS1" VerticalAlign="Top" />
			<HeaderStyle         CssClass="listViewThS1"  />
			<Columns>
				<asp:TemplateField>
					<ItemTemplate><%# Eval(sRELATED_NAME_FIELD) %></ItemTemplate>
					<EditItemTemplate>
						<asp:PlaceHolder ID="plcSELECT" OnInit="plcSELECT_Init" runat="server" />
					</EditItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField ItemStyle-Wrap="false">
					<ItemTemplate>
						<asp:ImageButton ID="btnEdit"   Enabled="<%# bEnabled %>" CommandName="Edit"   ImageUrl='<%# Session["themeURL"] + "images/edit_inline.gif"    %>' runat="server" />
						<asp:ImageButton ID="btnDelete" Enabled="<%# bEnabled %>" CommandName="Delete" ImageUrl='<%# Session["themeURL"] + "images/delete_inline.gif"  %>' runat="server" />
					</ItemTemplate>
					<EditItemTemplate>
						<asp:ImageButton ID="btnUpdate" Enabled="<%# bEnabled %>" CommandName="Update" ImageUrl='<%# Session["themeURL"] + "images/accept_inline.gif"  %>' runat="server" />
						<asp:ImageButton ID="btnCancel" Enabled="<%# bEnabled %>" CommandName="Cancel" ImageUrl='<%# Session["themeURL"] + "images/decline_inline.gif" %>' runat="server" />
					</EditItemTemplate>
				</asp:TemplateField>
				<asp:CommandField Visible="false" ButtonType="Image" ShowEditButton="true" ShowDeleteButton="true" ControlStyle-CssClass="button" ItemStyle-Width="10%" ItemStyle-Wrap="false" 
					EditText=".LBL_EDIT_BUTTON_LABEL"     EditImageUrl="~/App_Themes/Sugar/images/edit_inline.gif" 
					DeleteText=".LBL_REMOVE"              DeleteImageUrl="~/App_Themes/Sugar/images/delete_inline.gif" 
					UpdateText=".LBL_UPDATE_BUTTON_LABEL" UpdateImageUrl="~/App_Themes/Sugar/images/accept_inline.gif" 
					CancelText=".LBL_CANCEL_BUTTON_LABEL" CancelImageUrl="~/App_Themes/Sugar/images/decline_inline.gif" 
					/>
			</Columns>
		</asp:GridView>
		<Taoqi:RequiredFieldValidatorForRelatedSelect ID="valRelatedSelect" ControlToValidate="grdMain" CssClass="required" EnableClientScript="false" EnableViewState="false" Enabled="false" Runat="server" />
	</ContentTemplate>
</asp:UpdatePanel>


