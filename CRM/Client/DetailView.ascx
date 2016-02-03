<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="EditView.ascx.cs" Inherits="Taoqi.TQClient.DetailView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<style>
    .layui-layer-setwin {
        display:block !important;
        height:25px;
        width:25px;        
    }
    img {
        max-width:900px;
    }
    .layui-layer-content {
        height:auto !important;
    }
</style>
<div id="nav">
    <div class="nav_left">
        <div class="txt2" style="color: black;font-weight:bold;font-size:18px;">企业审核</div>
    </div>
    <div class="nav_right">
    </div>
</div>

<div id="ClientEditView">
    <div class="form-group" style="margin-top: 20px;">
        <label for="txtClientName" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">企业全称：</label>
        <div class="col-md-9 inputControl" style="width: 360px;">
            <input type="text" id="txtClientName" class="form-control" runat="server" placeholder="企业全称" disabled />
        </div>
    </div>
    <div class="form-group" style="margin-top: 10px;">
        <label for="txtCompanyAbbreviation" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">企业简称：</label>
        <div class="col-md-9 inputControl" style="width: 360px;">
            <input type="text" id="txtClientShortName" class="form-control" runat="server" placeholder="企业简称" disabled />
        </div>
    </div>
    <div class="form-group" style="margin-top: 10px;">
        <label for="txtCompanyAbbreviation" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">公司管理员：</label>
        <div class="col-md-9 inputControl" style="width: 360px;">
            <asp:TextBox ID="txtUserName" ReadOnly="True" runat="server" class="form-control" Enabled="false" />
        </div>
    </div>

    <div class="form-group" style="margin-top: 10px;">
        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>用户类型：</label>
        <div class="col-md-9 inputControl ClientEditAction">
            <div class="checkbox_group">
                <input type="checkbox" id="cbxSeller" name="SLCBTNUserType" value="1" runat="server" disabled />
                <label class="LABEL_SLCBTNUserType">卖家</label>
            </div>

            <div class="checkbox_group">
                <input type="checkbox" id="cbxBuyer" name="SLCBTNUserType" value="2" runat="server" disabled />
                <label class="LABEL_SLCBTNUserType">买家</label>
            </div>
        </div>
    </div>

    <div class="form-group" style="margin-top: 10px;">
        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>审核意见：</label>

        <div class="col-md-9 inputControl ClientEditAction">
            <%--            <div>
                <input type="radio" id="C_Status_WSH" name="RDC_Status" value="1" checked runat="server" />
                <label for="C_Status_WSH">未审核</label>
            </div>--%>

            <div>
                <input type="radio" id="C_Status_TG" name="RDC_Status" value="2" runat="server" disabled />
                <label for="C_Status_TG">审核通过</label>
            </div>

            <div>
                <input type="radio" id="C_Status_WTG" name="RDC_Status" value="3" runat="server" disabled />
                <label for="C_Status_WTG">审核未通过</label>
            </div>

            <%--<div>
                <input type="radio" id="C_Status_TGBQJRRM" name="RDC_Status" value="4" runat="server" />
                <label for="C_Status_TGBQJRRM">通过并且加入热门</label>
            </div>--%>
        </div>
    </div>
    <div class="form-group" style="margin-top: 10px;">
        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">证件图片：</label>
        <div id="layer-photos" class="col-md-9">
            <asp:Image ID="Image1" runat="server" alt="营业执照" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image2" runat="server" alt="组织机构代码证" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image3" runat="server" alt="税务登记证" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image4" runat="server" alt="开户许可证" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image5" runat="server" alt="法人身份证正面" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image6" runat="server" alt="法人身份证反面" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image7" runat="server" alt="燃气经营许可证" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image8" runat="server" alt="危险化学品经营许可证" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image9" runat="server" alt="危险化学品运输许可证" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image10" runat="server" alt="充装证" Style="width: 100px; height: 100px" />
        </div>
    </div>
</div>
<div style="width:945px;height:524px;border:1px solid #ddd;position:absolute;top:33px;left:15px;">
    
</div>
<script type="text/javascript">
    layer.config({
        extend: 'extend/layer.ext.js'
    });

    layer.ready(function () { //为了layer.ext.js加载完毕再执行
        layer.photos({
            shift: 5,
            photos: '#layer-photos',
            closeBtn: [0, true],
            area: ['auto', 'auto'],
            offset:['3%','20%'],
        });
    });
</script>
