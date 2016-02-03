<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewUsers.ascx.cs" Inherits="Taoqi.Users.NewUsers" %>

<style>
    #ctl00_cntBody_ctlNewUsers_lblError {
        display:inherit;
    }
</style>
<div id="nav">
    <div class="nav_left">
        <span class="title">用户信息
        </span>
        <span class="site_Path">当前位置：用户信息 > <span class="current1">新建/编辑用户</span>

        </span>
    </div>
</div>

<div id="ClientEditView">
    <div class="form-group" style="margin-top: 10px;">
        <label for="textrealname" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>真实姓名：</label>

        <div class="col-md-9 inputControl" style="width: 360px;">
            <input type="text" id="textrealname" class="form-control" runat="server" placeholder="真实姓名" />
        </div>
    </div>

    <div class="form-group" style="margin-top: 10px;margin-bottom:2px;">
        <label for="textphone" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>绑定手机号：</label>

        <div class="col-md-9 inputControl" style="width: 360px;">
            <input type="text" id="textphone" class="form-control" runat="server" placeholder="绑定手机号" />
        </div>
    </div>
    <div>
            <span style="display:inline-block;margin-left:259px;margin-top:0px;color:#828181;">修改手机号，对应的用户名也会改变。</span>
    </div>


    <div class="form-group" style="margin-top: 10px;">
        <label for="textemail" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">E-mail：</label>

        <div class="col-md-9 inputControl" style="width: 360px;">
            <input type="text" class="form-control" id="textemail" runat="server" placeholder="E-mail" />
        </div>
    </div>


    <div class="form-group" style="margin-top: 10px;">
        <label for="textqq" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>绑定QQ号码：</label>

        <div class="col-md-9 inputControl" style="width: 360px;">
            <input type="text" class="form-control" id="textqq" runat="server" placeholder="E-mail" />
        </div>
    </div>

    <div class="form-group" style="margin-top: 10px;">
        <label for="textwechat" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">微信号：</label>

        <div class="col-md-9 inputControl" style="width: 360px;">
            <input id="textwechat" type="text" class="form-control" runat="server" placeholder="微信号" />
        </div>
    </div>
<%--
 <div class="form-group" style="margin-top: 20px;">
        <label for="UserType" class="control-label col-md-3" style="text-align: right; margin-top: 6px;"><span style="color: red;">*</span>职位：</label>
        <div class="col-md-9 inputControl ClientEditAction">
            <div class="checkbox_group">
                <input type="checkbox" id="saler" name="SLCBTNUserType" value="1" runat="server" />
                <label for="YHGC" class="LABEL_SLCBTNUserType">销售员</label>
            </div>

            <div class="checkbox_group">
                <input type="checkbox" id="employee" name="SLCBTNUserType" value="2" runat="server" />
                <label for="JSZ" class="LABEL_SLCBTNUserType">场站员工</label>
            </div>

            <div class="checkbox_group">
                <input type="checkbox" id="driver" name="SLCBTNUserType" value="3" runat="server" />
                <label for="CYJQZ" class="LABEL_SLCBTNUserType">驾驶员</label>
            </div>
        </div>
    </div>--%>


    <div class="form-group" style="margin-top: 10px;margin-bottom:2px;">
        <label for="textpassword" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">新密码：</label>

        <div class="col-md-9 inputControl" style="width: 360px;">
            <input id="textpassword" type="text" class="form-control" runat="server" placeholder="新密码"/>
        </div>
    </div>
    <div>
            <span style="display:inline-block;margin-left:259px;margin-top:0px;color:#828181;">密码要求是6-16个字符。（你可以选择不修改密码）</span>
            
    </div>

    <div class="form-group" style="margin-top: 10px;margin-bottom:2px;">
        <label for="textpassword2" class="control-label col-md-3" style="text-align: right; margin-top: 6px;">重复新密码：</label>

        <div class="col-md-9 inputControl" style="width: 360px;">
            <input id="textpassword2" type="text" class="form-control" onkeyup="PWdetect()" runat="server" placeholder="重复新密码"/>
        </div>
    </div>
    <div>
            <span style="display:inline-block;margin-left:259px;margin-top:0px;color:#828181;">请再次输入密码。（你可以选择不修改密码）</span>
            
    </div>

<div style="margin-left: 250px; width: 260px;margin-top:15px;">
        <button id="Button1" type="submit" class="btnBlue_large" name="Save" value="Save" runat="server">保存</button>
        <%--<button id="Button2" type="submit" class="btnBlue_large" name="Cancel" value="Cancel" runat="server">取消</button>--%>
        <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />
    </div>
</div>

<script src="/js/jquery.min.js""></script>
<script src="/js/layer/layer.js"></script>
<%--<script>

    function PWdetect() {
        var $pw1 = $('#textpassword').val();
        var $pw2 = $('#textpassword2').val();
        if ($pw2 != $pw1) {
            layer.alert("两次密码输入请一致！", {
                area: ['385px', '178px'],
                offset: ['195px', '500px'],
            });
            return false;
        }
    }
</script>--%>

