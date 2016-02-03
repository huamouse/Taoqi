<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddEstimate.ascx.cs" Inherits="Taoqi.Order.AddEstimate" %>

<div style="float: left; width: 100%; min-height: 340px; background-color: #f5f5f5; padding: 20px; margin-top: 20px; border: solid 2px #f0eceb; border-top: #ff6400 solid;">

    <script>
        //评价页面为选择星级评价服务
        $(function () {
            $('.img_EstimateQuality').click(function () {
                var estimateStar = $(this).attr('index');
                $('#<%: TXTEstimateQuality.ClientID %>').attr("value", estimateStar);

                $('img.img_EstimateQuality').each(function () {
                    if ($(this).attr("index") <= estimateStar) {
                        $(this).attr("src", "/images/Estimate/MStar2.png");
                    }
                    else {
                        $(this).attr("src", "/images/Estimate/MStar1.png");
                    }
                });
                return false;
            });
        });
    </script>

    <div style="width: 100%; float: left;">
        <div>
            <img src="../App_Themes/Atlantic/images/xyjy.png" />
        </div>
        <div style="margin-top: 10px;">评价关键词</div>


        <div style="width: auto; margin-top: 10px;">
            <a href="#">
                <div class="select_div" style="color: #ff6400; margin-left: 0;" data-rb-index="1"><span class="title">气质很好</span></div>
            </a>
            <a href="#">
                <div class="select_div" style="color: #ff6400;" data-rb-index="2"><span class="title">便宜</span></div>
            </a>
            <a href="#">
                <div class="select_div" style="color: #ff6400;" data-rb-index="3"><span class="title">快捷</span></div>
            </a>
            <a href="#">
                <div class="select_div" style="color: #ff6400;" data-rb-index="4"><span class="title">方便</span></div>
            </a>
            <a href="#">
                <div class="select_div" style="color: #ff6400;" data-rb-index="4"><span class="title">纯度高</span></div>
            </a>
            <a href="#">
                <div class="select_div" style="color: #0065E6;" data-rb-index="4"><span class="title">气质一般</span></div>
            </a>
            <a href="#">
                <div class="select_div" style="color: #0065E6;" data-rb-index="4"><span class="title">态度很差</span></div>
            </a>
        </div>


        <asp:Repeater ID="RPMyHistoryEstimate" runat="server">
            <ItemTemplate>
                <div class="MyHistoryEstimate" style="float: left; margin-top: 10px;">
                    <div class="HistoryItem">
                        <div class="MyIcon">
                            <img src="/images/Estimate/EstimateBar1.jpg" />
                        </div>
                        <div class="item">
                            <div class="title">
                                <div>历次评价：</div>
                                <div><%#  Eval("DATE_ENTERED").ToString().Substring(5, 5) %></div>
                            </div>
                            <div class="context">
                                <%# Eval("C_EstimateContext") %>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="MyAddEstimate" style="float: left; width: 100%;">
        <div style="float: left; width: 100%;">
            <div style="width: 64%; float: left;">
                <div style="width: 20px; height: 120px; float: left;">
                    <img class="MyIcon" src="/images/Estimate/EstimateBar2.jpg" />
                </div>
                <div class="item" style="float: left; margin-left: 20px;">
                    <div class="title" style="float: left; width: 70px;">
                        使用感受：
                    </div>
                    <div class="context" style="width: 450px; float: left;">
                        <textarea style="width: 100%; min-height: 120px;" class="textarea_estimate" id="TXT_Estimate" runat="server"></textarea>
                    </div>
                </div>
            </div>

            <div style="width: 30%; float: left; margin-left: 20px;">

                <div style="height: 23px; margin-bottom: 10px;">
                    <h4><span class="redstar" style="color:red;">*</span>气质与描述相符:</h4>
                    <div style="position: relative; top: -22px; left: 120px;">
                        <a href="#">
                            <img src="/images/Estimate/MStar1.png" class="img_EstimateQuality " index="1" /></a>
                        <a href="#">
                            <img src="/images/Estimate/MStar1.png" class="img_EstimateQuality" index="2" /></a>
                        <a href="#">
                            <img src="/images/Estimate/MStar1.png" class="img_EstimateQuality" index="3" /></a>
                        <a href="#">
                            <img src="/images/Estimate/MStar1.png" class="img_EstimateQuality" index="4" /></a>
                        <a href="#">
                            <img src="/images/Estimate/MStar1.png" class="img_EstimateQuality" index="5" /></a>
                        <input type="hidden" id="TXTEstimateQuality" name="TXTEstimateQuality" runat="server" value="-1" />
                    </div>
                </div>
                
            </div>
        </div>

        <div style="margin-top: 10px; float: left; margin-left: 110px;">
            <button class="btn" style="background-color: #ff6400; color: white; width: 100px;" name="EstimateSubmit" id="EstimateSubmit" value="EstimateSubmit_Add" runat="server">提交评价</button>
            <label id="ErrorTxt" class="error" runat="server" enableviewstate="false"></label>
        </div>

    </div>
</div>
