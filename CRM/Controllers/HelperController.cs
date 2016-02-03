using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Web.Http;
using Taoqi.Common;
using Taoqi.Models;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;

namespace Taoqi.Controllers
{
    public class HelperController : ApiController
    {
        HttpRequest request = HttpContext.Current.Request;

        [HttpPost]
        public string ClientImg(Guid clientID, string column)
        {
            if (clientID == Guid.Empty) return "error：公司ID不能为空";

            string btnFileNum = "uploadFile";
            if (!(request.Files.Count == 1 && request.Files[btnFileNum] != null && request.Files[btnFileNum].ContentLength > 0))
                return "error：上传文件为空";

            //对上传类型的判断
            string FileType = request.Files[btnFileNum].ContentType;
            string FilePostFix = ".";

            if (FileType != "image/jpeg" && FileType != "image/jpg" && FileType != "image/png")
            {
                return "error：不支持的图片类型";
            }
            else
            {
                FilePostFix += "PNG";
            }

            string virtual_UploadURL = string.Format("{0}-{1}{2}", column, clientID, FilePostFix);
            string absolute_UploadURL = request.MapPath(WebConfigurationManager.AppSettings["UploadPath_image"] + virtual_UploadURL);
            try
            {
                request.Files[btnFileNum].SaveAs(absolute_UploadURL);

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();
                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        if (column == "C_Attachment1" || column == "C_Attachment2" || column == "C_Attachment3" || column == "C_Attachment4" || column == "C_Attachment5" || column == "C_Attachment6" 
                            || column == "C_Attachment7" || column == "C_Attachment8" || column == "C_Attachment9" || column == "C_Attachment10" || column == "C_imgIcon" || column == "C_imgClient")
                        {
                            cmd.CommandText = string.Format("UPDATE TQClient SET {0} = '{1}' WHERE ID = '{2}'", column, virtual_UploadURL, clientID);
                        }
                        else
                            return "error：非法的列名";

                        using (DbDataAdapter da = dbf.CreateDataAdapter())
                        {
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            cmd.ExecuteScalar();
                        }
                    }
                }

                return "OK";
            }
            catch
            {
                return "error：图片上传失败";
            }
        }

        [HttpPost]
        public string ShippingOrder(Guid orderDetailID)
        {
            if (orderDetailID == Guid.Empty) return "error：详单ID不能为空";

            string btnFileNum = "uploadFile";
            if (!(request.Files.Count == 1 && request.Files[btnFileNum] != null && request.Files[btnFileNum].ContentLength > 0))
                return "error：上传文件为空";

            //对上传类型的判断
            string FileType = request.Files[btnFileNum].ContentType;
            if (FileType != "image/jpeg" && FileType != "image/jpg" && FileType != "image/png")
                return "error：不支持的图片类型";

            string FilePostFix = ".png";
            string virtual_UploadURL = string.Format("Shipping-{0}{1}", orderDetailID, FilePostFix);
            string absolute_UploadURL = request.MapPath(WebConfigurationManager.AppSettings["UploadPath_image"] + virtual_UploadURL);
            try
            {
                request.Files[btnFileNum].SaveAs(absolute_UploadURL);

                SqlProcs.spTQOrderDetailUpdateShipping(orderDetailID, virtual_UploadURL);
                
                SqlProcs.spTQOrderDetail_ChangeStatus(orderDetailID, 4);    // 设置订单状态为已发车
                Msg.DepartCar(orderDetailID, true); // 给买家发送消息
                Msg.DepartCar(orderDetailID, false); // 给卖家发送消息

                return "OK";
            }
            catch
            {
                return "error：图片上传失败";
            }
        }

        [HttpPost]
        public string LandingOrder(Guid orderDetailID)
        {
            if (orderDetailID == Guid.Empty) return "error：详单ID不能为空";

            string btnFileNum = "uploadFile";
            if (!(request.Files.Count == 1 && request.Files[btnFileNum] != null && request.Files[btnFileNum].ContentLength > 0))
                return "error：上传文件为空";

            //对上传类型的判断
            string FileType = request.Files[btnFileNum].ContentType;
            if (FileType != "image/jpeg" && FileType != "image/jpg" && FileType != "image/png")
                return "error：不支持的图片类型";

            string FilePostFix = ".png";
            string virtual_UploadURL = string.Format("Landing-{0}{1}", orderDetailID, FilePostFix);
            string absolute_UploadURL = request.MapPath(WebConfigurationManager.AppSettings["UploadPath_image"] + virtual_UploadURL);
            try
            {
                request.Files[btnFileNum].SaveAs(absolute_UploadURL);

                SqlProcs.spTQOrderDetailUpdateLanding(orderDetailID, virtual_UploadURL);
                SqlProcs.spTQOrderDetail_ChangeStatus(orderDetailID, 5);
                return "OK";
            }
            catch
            {
                return "error：图片上传失败";
            }
        }

        [HttpPost]
        public string LandingTransit(Guid transitMyID)
        {
            if (transitMyID == Guid.Empty) return "error：我的在途气ID不能为空";

            string btnFileNum = "uploadFile";
            if (!(request.Files.Count == 1 && request.Files[btnFileNum] != null && request.Files[btnFileNum].ContentLength > 0))
                return "error：上传文件为空";

            //对上传类型的判断
            string FileType = request.Files[btnFileNum].ContentType;
            if (FileType != "image/jpeg" && FileType != "image/jpg" && FileType != "image/png")
                return "error：不支持的图片类型";

            string FilePostFix = ".png";
            string virtual_UploadURL = string.Format("TransitLand-{0}{1}", transitMyID, FilePostFix);
            string absolute_UploadURL = request.MapPath(WebConfigurationManager.AppSettings["UploadPath_image"] + virtual_UploadURL);
            try
            {
                request.Files[btnFileNum].SaveAs(absolute_UploadURL);

                //SqlProcs.spTQTransitMyUpdateLanding(transitMyID, virtual_UploadURL);
                SqlProcs.spTQTransitMy_Change(transitMyID, 3);
                return "OK";
            }
            catch
            {
                return "error：图片上传失败";
            }
        }
    }
}