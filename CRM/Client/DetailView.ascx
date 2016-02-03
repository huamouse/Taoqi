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
        <div class="txt2" style="color: black;font-weight:bold;font-size:18px;">��ҵ���</div>
    </div>
    <div class="nav_right">
    </div>
</div>

<div id="ClientEditView">
    <div class="form-group" style="margin-top: 20px;">
        <label for="txtClientName" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">��ҵȫ�ƣ�</label>
        <div class="col-md-9 inputControl" style="width: 360px;">
            <input type="text" id="txtClientName" class="form-control" runat="server" placeholder="��ҵȫ��" disabled />
        </div>
    </div>
    <div class="form-group" style="margin-top: 10px;">
        <label for="txtCompanyAbbreviation" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">��ҵ��ƣ�</label>
        <div class="col-md-9 inputControl" style="width: 360px;">
            <input type="text" id="txtClientShortName" class="form-control" runat="server" placeholder="��ҵ���" disabled />
        </div>
    </div>
    <div class="form-group" style="margin-top: 10px;">
        <label for="txtCompanyAbbreviation" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">��˾����Ա��</label>
        <div class="col-md-9 inputControl" style="width: 360px;">
            <asp:TextBox ID="txtUserName" ReadOnly="True" runat="server" class="form-control" Enabled="false" />
        </div>
    </div>

    <div class="form-group" style="margin-top: 10px;">
        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>�û����ͣ�</label>
        <div class="col-md-9 inputControl ClientEditAction">
            <div class="checkbox_group">
                <input type="checkbox" id="cbxSeller" name="SLCBTNUserType" value="1" runat="server" disabled />
                <label class="LABEL_SLCBTNUserType">����</label>
            </div>

            <div class="checkbox_group">
                <input type="checkbox" id="cbxBuyer" name="SLCBTNUserType" value="2" runat="server" disabled />
                <label class="LABEL_SLCBTNUserType">���</label>
            </div>
        </div>
    </div>

    <div class="form-group" style="margin-top: 10px;">
        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>��������</label>

        <div class="col-md-9 inputControl ClientEditAction">
            <%--            <div>
                <input type="radio" id="C_Status_WSH" name="RDC_Status" value="1" checked runat="server" />
                <label for="C_Status_WSH">δ���</label>
            </div>--%>

            <div>
                <input type="radio" id="C_Status_TG" name="RDC_Status" value="2" runat="server" disabled />
                <label for="C_Status_TG">���ͨ��</label>
            </div>

            <div>
                <input type="radio" id="C_Status_WTG" name="RDC_Status" value="3" runat="server" disabled />
                <label for="C_Status_WTG">���δͨ��</label>
            </div>

            <%--<div>
                <input type="radio" id="C_Status_TGBQJRRM" name="RDC_Status" value="4" runat="server" />
                <label for="C_Status_TGBQJRRM">ͨ�����Ҽ�������</label>
            </div>--%>
        </div>
    </div>
    <div class="form-group" style="margin-top: 10px;">
        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">֤��ͼƬ��</label>
        <div id="layer-photos" class="col-md-9">
            <asp:Image ID="Image1" runat="server" alt="Ӫҵִ��" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image2" runat="server" alt="��֯��������֤" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image3" runat="server" alt="˰��Ǽ�֤" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image4" runat="server" alt="�������֤" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image5" runat="server" alt="�������֤����" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image6" runat="server" alt="�������֤����" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image7" runat="server" alt="ȼ����Ӫ���֤" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image8" runat="server" alt="Σ�ջ�ѧƷ��Ӫ���֤" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image9" runat="server" alt="Σ�ջ�ѧƷ�������֤" Style="width: 100px; height: 100px" />
            <asp:Image ID="Image10" runat="server" alt="��װ֤" Style="width: 100px; height: 100px" />
        </div>
    </div>
</div>
<div style="width:945px;height:524px;border:1px solid #ddd;position:absolute;top:33px;left:15px;">
    
</div>
<script type="text/javascript">
    layer.config({
        extend: 'extend/layer.ext.js'
    });

    layer.ready(function () { //Ϊ��layer.ext.js���������ִ��
        layer.photos({
            shift: 5,
            photos: '#layer-photos',
            closeBtn: [0, true],
            area: ['auto', 'auto'],
            offset:['3%','20%'],
        });
    });
</script>
