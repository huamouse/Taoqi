<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="MetaHeader.ascx.cs" 
    Inherits="Taoqi._controls.MetaHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<meta name="keywords" content="<%= Application["CONFIG.header_keywords"] %>" />
<script type="text/javascript" src="<%= Application["scriptURL"] %>cookie.js"></script>
<script type="text/javascript" src="<%= Application["scriptURL"] %>Taoqi.js"></script>
<script type="text/javascript" src="<%= Application["scriptURL"] %>KeySortDropDownList.js"></script>
<%--<script type="text/javascript">
    var sREMOTE_SERVER = '<%# Application["rootURL"] %>';
    var sUSER_EXTENSION = '<%# Sql.EscapeJavaScript(Sql.ToString(Session["EXTENSION"   ])) %>';
    var sUSER_FULL_NAME = '<%# Sql.EscapeJavaScript(Sql.ToString(Session["FULL_NAME"   ])) %>';
    var sUSER_PHONE_WORK = '<%# Sql.EscapeJavaScript(Sql.ToString(Session["PHONE_WORK"  ])) %>';
    var sUSER_SMS_OPT_IN = '<%# Sql.EscapeJavaScript(Sql.ToString(Session["SMS_OPT_IN"  ])) %>';
    var sUSER_PHONE_MOBILE = '<%# Sql.EscapeJavaScript(Sql.ToString(Session["PHONE_MOBILE"])) %>';
</script>--%>

