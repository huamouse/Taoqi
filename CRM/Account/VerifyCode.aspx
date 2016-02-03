<%@ Page Language="C#" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<script runat="server">
protected void Page_Load(object sender, EventArgs e)
{
	//设置不缓存此页
    Response.AppendHeader("pragma", "no-cache");
    Response.AppendHeader("Cache-Control", "no-cache, must-revalidate");
    Response.AppendHeader("expires", "0");

    Bitmap image = VerificationCode.NextImage();
	
	Response.ContentType = "image/jpeg";
	Response.Clear();
	Response.BufferOutput = true;
	image.Save(Response.OutputStream, ImageFormat.Jpeg);
	
	Response.Flush();
}

</script>
