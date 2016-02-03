<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FirstEstimate.ascx.cs" Inherits="Taoqi.Order.FirstEstimate" %>

<style>
    .redstar {
        color:red;
    }
</style>
<div style="width: 100%; float: left; min-height: 340px; background-color: #f5f5f5; padding: 20px; margin-top: 20px; border: solid 2px #f0eceb; border-top: #ff6400 solid;">

    <script>
        //评价页面为选择星级评价服务
        $(function () {
            $('.img_EstimateLogistics').click(function () {
                var estimateStar = $(this).attr('index');
                $('#<%: TXTEstimateLogistics.ClientID %>').attr("value", estimateStar);

                $('img.img_EstimateLogistics').each(function () {
                    if ($(this).attr("index") <= estimateStar) {
                        $(this).attr("src", "/images/Estimate/MStar2.png");
                    }
                    else {
                        $(this).attr("src", "/images/Estimate/MStar1.png");
                    }
                });
                return false;
            });

            $('.img_EstimateService').click(function () {
                var estimateStar = $(this).attr('index');
                $('#<%: TXTEstimateService.ClientID %>').attr("value", estimateStar);

                $('img.img_EstimateService').each(function () {
                    if ($(this).attr("index") <= estimateStar) {
                        $(this).attr("src", "/images/Estimate/MStar2.png");
                    }
                    else {
                        $(this).attr("src", "/images/Estimate/MStar1.png");
                    }
                });
                return false;
            });

            $('.img_DriverService').click(function () {
                var estimateStar = $(this).attr('index');
                $('#<%: TXTDriverService.ClientID %>').attr("value", estimateStar);

                $('img.img_DriverService').each(function () {
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

    <div style="width: 100%;">
        <div>
            <img src="../App_Themes/Atlantic/images/xyjy.png" /></div>

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



    </div>

    <div style="width: 100%; float: left; margin-top: 30px;">


        <div style="width: 50%; float: left;">
            <div style="display: block;">
                <label for="TXT_Estimate">评价商品</label>
                <textarea style="width: 100%; min-height: 100px;" class="textarea_estimate" id="TXT_Estimate" runat="server"></textarea>
            </div>
            <div style="margin-top: 10px; float: left; margin-left: 3px;">
                <button class="btn" style="background-color: #ff6400; color: white; width: 100px;" value="EstimateSubmit_First" name="EstimateSubmit" id="EstimateSubmit">提交评价</button>
                <label id="ErrorTxt" class="error" runat="server" enableviewstate="false"></label>
            </div>
        </div>

        <div style="width: 40%; float: right;margin-top:30px;padding-left:20px;">
            <div style="height: 23px;margin-bottom:10px;">
                <h4><span class="redstar">*</span>物流到岸及时率:</h4>
                <div style="position: relative; top: -22px; left: 120px;">
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateLogistics " index="1" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateLogistics" index="2" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateLogistics" index="3" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateLogistics" index="4" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateLogistics" index="5" /></a>
                    <input type="hidden" id="TXTEstimateLogistics" name="TXTEstimateLogistics" runat="server" value="-1" />
                </div>
            </div>

            <div style="margin-bottom: 10px; height: 23px;">
                <h4><span class="redstar">*</span>买家的服务态度:</h4>
                <div style="position: relative; top: -22px; left: 120px;">
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateService" index="1" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateService" index="2" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateService" index="3" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateService" index="4" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_EstimateService" index="5" /></a>
                    <input type="hidden" id="TXTEstimateService" name="TXTEstimateService" runat="server" value="-1" />
                </div>
            </div>

            <div style="height: 23px;">
                <h4><span class="redstar">*</span>驾驶员服务态度:</h4>
                <div style="position: relative; top: -22px; left: 120px;">
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_DriverService" index="1" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_DriverService" index="2" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_DriverService" index="3" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_DriverService" index="4" /></a>
                    <a href="#">
                        <img src="/images/Estimate/MStar1.png" class="img_DriverService" index="5" /></a>
                    <input type="hidden" id="TXTDriverService" name="TXTDriverService" runat="server" value="-1" />
                </div>
            </div>
        </div>


    </div>
</div>
