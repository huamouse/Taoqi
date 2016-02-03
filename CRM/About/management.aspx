<%@ Page Language="C#" MasterPageFile="~/DefaultView.master" AutoEventWireup="true" CodeBehind="management.aspx.cs" Inherits="Taoqi.About.management" ValidateRequest="false" %>

<asp:Content ID="Content4" ContentPlaceHolderID="cntBody" runat="server">
    <style>
        #about_management .title_BG{
            background-color: #f4f4f4;
            margin-bottom: 20px;
        }

        #about_management .title_font{
            color: #ff6400;
            font-size: large;
            margin-left: 20px;
            margin-top: 5px;
        }

        #about_management .title_button{
            float: right;
            margin-top: 3px;
            width: 100px;
            height: 30px;
            background-color: #ff6400;
            color: white;
        }

        #about_management .laterDiv{
            margin-top: 50px;
        }
    </style>
    <div id="about_management">
        <label style="font-size: large; font-weight: bold; margin-top: 4px;">广告位设置</label>

        <div style="border: 1px solid #ddd; padding: 40px 20px; font-size: small;">
            <div>
                <div class="title_BG">
                    <label class="title_font">企业简介</label>
                    <button type="submit" name="submit" value="qyjj" class="title_button">提交</button>
                </div>
                <!-- 加载编辑器的容器 -->
                <script id="UE_qyjj" name="UE_qyjj" type="text/plain">
                    <%= C_qyjj %>
                </script>
            </div>

            <div class="laterDiv">
                <div class="title_BG">
                    <label class="title_font">企业文化</label>
                    <button type="submit" name="submit" value="qywh" class="title_button">提交</button>
                </div>
                <!-- 加载编辑器的容器 -->
                <script id="UE_qywh" name="UE_qywh" type="text/plain">
                    <%= C_qywh %>
                </script>
            </div>

            <div class="laterDiv">
                <div class="title_BG">
                    <label class="title_font">联系我们</label>
                    <button type="submit" name="submit" value="lxwm" class="title_button">提交</button>
                </div>
                <!-- 加载编辑器的容器 -->
                <script id="UE_lxwm" name="UE_lxwm" type="text/plain">
                    <%= C_lxwm %>
                </script>
            </div>

            <div class="laterDiv">
                <div class="title_BG">
                    <label class="title_font">友情链接</label>
                    <button type="submit" name="submit" value="yqlj" class="title_button">提交</button>
                </div>
                <!-- 加载编辑器的容器 -->
                <script id="UE_yqlj" name="UE_yqlj" type="text/plain">
                    <%= C_yqlj %>
                </script>
            </div>

            <div class="laterDiv">
                <div class="title_BG">
                    <label class="title_font">法律声明</label>
                    <button type="submit" name="submit" value="flsm" class="title_button">提交</button>
                </div>
                <!-- 加载编辑器的容器 -->
                <script id="UE_flsm" name="UE_flsm" type="text/plain">
                    <%= C_flsm %>
                </script>
            </div>
            


            <!-- 配置文件 -->
            <script type="text/javascript" src="../include/UEditor/ueditor.config.js"></script>
            <!-- 编辑器源码文件 -->
            <script type="text/javascript" src="../include/UEditor/ueditor.all.js"></script>
            <!-- 实例化编辑器 -->
            <script type="text/javascript">
                var config_tools = {
                    toolbars: [
                        [
                                'undo', //撤销
                                'redo', //重做
                                'formatmatch', //格式刷
                                'removeformat', //清除格式
                                'simpleupload' //单图上传
                        ],
                        [
                                'underline', //下划线
                                'strikethrough', //删除线
                                'horizontal', //分隔线
                                '|',
                                'forecolor', //字体颜色
                                'backcolor', //背景色
                                'bold', //加粗
                                'italic', //斜体
                                'fontfamily', //字体
                                'fontsize', //字号
                                '|',
                                'paragraph', //段落格式
                                'justifyleft', //居左对齐
                                'justifyright', //居右对齐
                                'justifycenter', //居中对齐
                                'justifyjustify' //两端对齐
                        ]
                    ],
                    autoHeightEnabled: true,
                    autoFloatEnabled: true
                };

                var UE_qyjj = UE.getEditor('UE_qyjj', config_tools);

                var UE_qywh = UE.getEditor('UE_qywh', config_tools);

                var UE_lxwm = UE.getEditor('UE_lxwm', config_tools);

                var UE_yqlj = UE.getEditor('UE_yqlj', config_tools);

                var UE_flsm = UE.getEditor('UE_flsm', config_tools);
            </script>
        </div>
    </div>
        
</asp:Content>
