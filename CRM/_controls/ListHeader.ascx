<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="ListHeader.ascx.cs" Inherits="Taoqi._controls.ListHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<asp:Table SkinID="tabFrame" CssClass="h3RowListheader" runat="server">
    <asp:TableRow>
        <asp:TableCell Wrap="false">
          <div class="listTitleLeft">
                        <span visible="<%# !Sql.IsEmptyString(SubPanel) %>" runat="server">
                            <asp:HyperLink ID="lnkShowSubPanel" NavigateUrl=<%# "javascript:ShowSubPanel(\'" + lnkShowSubPanel.ClientID + "\',\'" + lnkHideSubPanel.ClientID + "\',\'" + SubPanel + "\');" %> Style='<%# "display:" + (CookieValue(SubPanel) == "1" ? "inline": "none") %>' runat="server"><asp:Image SkinID="advanced_search" runat="server" /></asp:HyperLink>
                            <asp:HyperLink ID="lnkHideSubPanel" NavigateUrl=<%# "javascript:HideSubPanel(\'" + lnkShowSubPanel.ClientID + "\',\'" + lnkHideSubPanel.ClientID + "\',\'" + SubPanel + "\');" %> Style='<%# "display:" + (CookieValue(SubPanel) != "1" ? "inline": "none") %>' runat="server"><asp:Image SkinID="basic_search"    runat="server" /></asp:HyperLink>
                        </span>
                        <asp:Image SkinID="h3Arrow" Visible="false" runat="server" />
                        &nbsp;<asp:Label Text='<%# L10n.Term(Title) %>' runat="server" />
             </div>    
        </asp:TableCell>
        <asp:TableCell>
            &nbsp;
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>


