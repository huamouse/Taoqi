<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.TQRewardPoint.ListView" %>

<div class="row nav2">
    <div class="col-md-8 listTitle">我的积分</div>
    <div class="col-md-4">
        <%--
        <div class="search_bar">
            <input type="text" placeholder="来源" class="search_box" />

            <div class="btn_search">
                <img src="/member/Include/images/search3.jpg" />
            </div>
        </div>
         --%>
    </div>
</div>



<div class="tableout" style=" width:100%;
    
     padding: 15px 25px 10px 30px; border:1px solid #e5e5e5; height:111px; margin-bottom:13px;">


     <div style="float:left; border-right:1px solid #e5e5e5; width:26%;  height:86px; padding-left:5%; font-size:14px;color:#727272; line-height:36px;">  总积分：
       
         <h1 style="color:#ff6700; font-size:35px; line-height:40px;">45800</h1>

     </div>


     <div style="float:left; border-right:1px solid #e5e5e5; width:26%; height:86px;padding-left:5%;font-size:14px;color:#727272;line-height:36px;"> 可用积分：

     <h1 style="color:#ff6700; font-size:35px; line-height:40px;">45800</h1>

     </div>


     <div style="float:left;  width:26%; height:86px;padding-left:5%;font-size:14px;color:#727272;line-height:36px;">  冻结的积分：

       <h1 style="color:#ff6700; font-size:35px; line-height:40px;">45800</h1>

     </div>


      <a href="/mall.html" style=" display:block;float:right;height:86px;font-size:14px;color:#ff6700;line-height:86px; width:145px;">查看可兑换的礼品>>>

     

     </a>



</div>







<asp:Panel ID="Panel1" CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
    <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
</asp:Panel>

<asp:HiddenField ID="LAYOUT_LIST_VIEW" runat="server" />

<asp:UpdatePanel ID="updatePanel2" runat="server">

    <ContentTemplate>

        <div class="div_grid">
            <Taoqi:SplendidGrid ID="grdMain" SkinID="grdListView" PageSize="10"
                AllowPaging="<%# !PrintView %>" EnableViewState="true" runat="server">
                <Columns>
                </Columns>
            </Taoqi:SplendidGrid>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>







