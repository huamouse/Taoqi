<%@ Control Language="c#" AutoEventWireup="false" Codebehind="ModuleHeader.ascx.cs" Inherits="Taoqi.Themes.Sugar.ModuleHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

	<div id="divModuleHeader<%= sModule %>">
		<script type="text/javascript">
		function PopupHelp()
		{
			var url = document.getElementById('<%= lnkHelpText.ClientID %>').href;
			// 01/29/2011   Allow the popup options to be customized. 
			var sOptions = '<%= Application["CONFIG.help_popup_options"] %>';
			if ( sOptions == '' )
				sOptions = 'width=600,height=600,status=0,resizable=1,scrollbars=1,toolbar=0,location=1';
			window.open(url,'helpwin',sOptions);
		}
		</script>
		<p>
		<asp:Table SkinID="tabFrame" CssClass="moduleTitle" runat="server" Visible="false">
			<asp:TableRow>
				<asp:TableCell Width="99%">
					 <div class="nav">
                         <img src="/include/images/nav.jpg" class="navImage" />
                         <asp:HyperLink ID="lnkModule" Visible="false" runat="server" />&nbsp;<asp:Label ID="lblPointer" Text="&gt;" Visible="false" runat="server" />&nbsp;<asp:Label ID="lblTitle" Runat="server" />&nbsp;
					<asp:HyperLink onclick=<%# "return Taoqi_ChangeFavorites(this, \'" + sModule + "\', \'" + Request["ID"] + "\')" %> Visible="<%# !this.IsMobile && bEnableFavorites %>" Runat="server">
						<asp:Image ID="imgFavoritesAdd"    name='<%# "favAdd_" + Request["ID"] %>' SkinID="favorites_add"    style='<%# "display:" + ( Sql.IsEmptyGuid(gFAVORITE_RECORD_ID) ? "inline" : "none") %>' ToolTip='<%# L10n.Term(".LBL_ADD_TO_FAVORITES"     ) %>' Runat="server" />
						<asp:Image ID="imgFavoritesRemove" name='<%# "favRem_" + Request["ID"] %>' SkinID="favorites_remove" style='<%# "display:" + (!Sql.IsEmptyGuid(gFAVORITE_RECORD_ID) ? "inline" : "none") %>' ToolTip='<%# L10n.Term(".LBL_REMOVE_FROM_FAVORITES") %>' Runat="server" />
					</asp:HyperLink>
					</div>
				</asp:TableCell>
				<asp:TableCell VerticalAlign="top" HorizontalAlign="Right" style="padding-top:3px; padding-left: 5px;" Wrap="false">
					<div visible="<%# !PrintView %>" runat="server">
						<asp:ImageButton CommandName="Print" OnCommand="Page_Command" CssClass="utilsLink" AlternateText='<%# L10n.Term(".LNK_PRINT") %>' Visible="<%# bEnablePrint %>" SkinID="print" Runat="server" />
						<asp:LinkButton  CommandName="Print" OnCommand="Page_Command" CssClass="utilsLink" Text='<%# L10n.Term(".LNK_PRINT") %>' Visible="<%# bEnablePrint %>" Runat="server" />
						&nbsp;
						<asp:PlaceHolder Visible='<%# !Sql.ToBoolean(Application["CONFIG.hide_help"]) %>' runat="server">
							<asp:HyperLink ID="lnkHelpImage" onclick="PopupHelp(); return false;" CssClass="utilsLink" Target="_blank" Visible="<%# bEnableHelp %>" Runat="server">
								<asp:Image AlternateText='<%# L10n.Term(".LNK_HELP") %>' SkinID="help" Runat="server" />
							</asp:HyperLink>
							<asp:HyperLink ID="lnkHelpText" onclick="PopupHelp(); return false;" CssClass="utilsLink" Target="_blank" Visible="<%# bEnableHelp %>" Runat="server"><%# L10n.Term(".LNK_HELP") %></asp:HyperLink>
						</asp:PlaceHolder>
					</div>
					<div visible="<%# PrintView %>" runat="server">
						<asp:ImageButton CommandName="PrintOff" OnCommand="Page_Command" CssClass="utilsLink" AlternateText='<%# L10n.Term(".LBL_BACK") %>' Visible="<%# bEnablePrint %>" SkinID="print" Runat="server" />
						<asp:LinkButton  CommandName="PrintOff" OnCommand="Page_Command" CssClass="utilsLink" Text='<%# L10n.Term(".LBL_BACK") %>' Visible="<%# bEnablePrint %>" Runat="server" />
					</div>
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		</p>
	</div>


