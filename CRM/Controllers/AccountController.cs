using System;
using System.Data;
using System.Web.Http;
using System.Collections;
using System.Web;
using Taoqi.Models;
using System.Web.Configuration;
using System.Data.Common;

namespace Taoqi.Controllers
{
    public class AccountController : ApiController
    {
        /// <summary>
        /// 短信验证码
        /// </summary>
        /// <param name="tel">手机号</param>
        [HttpPost]
        public string SMS(string tel)
        {
            SMS sms = new SMS();
            sms.SendCode(tel);

            return sms.ErrorMessage;
        }

        [HttpPost]
        public int CheckSMS(string code)
        {
            try
            {
                if (code == HttpContext.Current.Session["smsCode"].ToString()) return 0;
            }
            catch (Exception)
            {
            }

            return -1;
        }

        [HttpPost]
        public string GetSmsCode(string mobile, string code)
        {
            try
            {
                if (code.Trim().ToUpper() != HttpContext.Current.Session["verificationCode"].ToString()) return "0";

                SMS sms = new SMS();
                sms.SendCode(mobile);

                return sms.ErrorMessage;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //修改密码，手机验证
        [HttpGet]
        public string Get(string id)
        {
            try
            {
                if (id.Trim().ToUpper() != HttpContext.Current.Session["verificationCode"].ToString())
                {
                    return "0";
                }
                SMS sms = new SMS();
                sms.SendCode(Security.UserMobile);
                return sms.ErrorMessage;
            }
            catch (Exception)
            {
            }

            return "发送失败";
        }

        [HttpGet]
        public int CheckUser(string username)
        {
            Hashtable ht = new Hashtable();
            ht.Add("USER_NAME", username);
            int total = DAL.GetTotalByViewName("USERS", ht);
            if (total > 0) return 1;

            return 0;
        }

        // 注册
        [HttpPost]
        public string Register([FromBody]Register reg)
        {
            try
            {
                // 短信验证码错误
                if (HttpContext.Current.Session["smsCode"] == null || reg.SmsCode != HttpContext.Current.Session["smsCode"].ToString())
                    return "1";

                // 个人用户注册
                Guid gUserID = Guid.Empty;
                string hashPassword = Security.HashPassword(reg.Password);
                SqlProcs.spUserAdd(ref gUserID, reg.Mobile, hashPassword, reg.RealName);

                if (reg.CompanyName != null)
                {
                    Guid gClientID = Guid.Empty;
                    int result = 0;
                    SqlProcs.spTQClientAdd(ref gClientID, reg.CompanyName, ref result);
                    Guid gID = Guid.Empty;
                    SqlProcs.spTQAccountAdd(ref gID, gUserID, gClientID);
                }

                // 登录成功自动登陆
                SplendidInit.LoginUser(reg.Mobile, hashPassword, String.Empty, String.Empty, String.Empty, false, true);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost]
        public string RegisterMobile([FromBody]Register reg)
        {
            try
            {
                // 个人用户注册
                Guid gUserID = Guid.Empty;
                string hashPassword = Security.HashPassword(reg.Password);
                SqlProcs.spUserAdd(ref gUserID, reg.Mobile, hashPassword, reg.RealName);

                if (reg.CompanyName != null && reg.CompanyName.Length < 5)
                {
                    Guid gClientID = Guid.Empty;
                    int result = 0;
                    SqlProcs.spTQClientAdd(ref gClientID, reg.CompanyName, ref result);
                    Guid gID = Guid.Empty;
                    SqlProcs.spTQAccountAdd(ref gID, gUserID, gClientID);
                }

                // 登录成功自动登陆
                SplendidInit.LoginUser(reg.Mobile, hashPassword, String.Empty, String.Empty, String.Empty, false, true);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost]
        public int ResetPassword(string mobile, string password)
        {
            int result = -1;
            Guid gUserID = Security.USER_ID;
            SqlProcs.spTQUsers_resetPassword(ref gUserID, mobile, password, ref result);

            return result;
        }

        [HttpPost]
        public Guid Login(string mobile, string password)
        {
            // 登录成功自动登陆
            SplendidInit.LoginUser(mobile, password, String.Empty, String.Empty, String.Empty, false, true);

            return Security.USER_ID;
        }

        [HttpGet]
        public DataTable UserInfo(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", id);
            return DAL.GetTable("vwUSERS", ht);
        }

        [HttpGet]
        public DataTable GetClient(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", id);
            string fields = "ID, C_CategoryID, C_ClientName, C_ProvinceID, C_ProvinceName, C_CityID, C_CityName, C_CountyID, C_CountyName, C_Address, FullAddress, "
                + "C_imgIcon, C_imgClient, C_Ranking, "
                + "C_Attachment1, C_Attachment2, C_Attachment3, C_Attachment4, C_Attachment5, C_Attachment6, C_Attachment7, C_Attachment8, C_Attachment9, C_Attachment10";
            DataTable dt = DAL.GetTable("vwTQClient_List", ht, 0, null, fields);
            if (!dt.Columns.Contains("TypeSell")) dt.Columns.Add("TypeSell", System.Type.GetType("System.Boolean"));
            if (!dt.Columns.Contains("TypeBuy")) dt.Columns.Add("TypeBuy", System.Type.GetType("System.Boolean"));

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string category = row["C_CategoryID"].ToString();
                row["TypeSell"] = false;
                row["TypeBuy"] = false;
                if (category.Length > 0 && category[0] == '1') row["TypeSell"] = true;
                if (category.Length > 1 && category[1] == '1') row["TypeBuy"] = true;

                //string uploadURL = WebConfigurationManager.AppSettings["UploadPath_image"].Replace("~", HttpContext.Current.Request.ApplicationPath);
                //string[] columns = new string[] { "C_Attachment1", "C_Attachment2", "C_Attachment3", "C_Attachment4", "C_Attachment5", "C_Attachment6", 
                //                                  "C_Attachment7", "C_Attachment8", "C_Attachment9", "C_Attachment10", "C_imgIcon", "C_imgClient" };
                //foreach (string imgColumn in columns)
                //{
                //    if (!string.IsNullOrEmpty(row[imgColumn].ToString()))
                //        row[imgColumn] = uploadURL + row[imgColumn].ToString();
                //    else
                //        row[imgColumn] = "NoImage.png";
                //}
            }

            return dt;
        }

        [HttpPost]
        public string SaveClient([FromBody] Client client, int status = 0)
        {
            // 设置企业类别
            string category = "";

            if (client.TypeSell)
                category += "1";
            else
                category += "0";

            if (client.TypeBuy)
                category += "1";
            else
                category += "0";

            try
            {
                if (status == 1)
                    Msg.ClientApply(client.ID);
                else if (status == 4)
                {
                    Msg.ClientApply(client.ID, true);
                    status = 1;
                }
                SqlProcs.spTQClientSave(client.ID, category, client.C_ClientName, client.C_CountyID, client.C_Address, status);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpGet]
        public string AddClient(string companyName)
        {
            string AddClientName = companyName;

            if (string.IsNullOrEmpty(AddClientName))
                return "公司名不能为空，请重试";

            Guid AddClientID = Guid.Empty;
            Guid gID = Guid.Empty;
            Guid gUserID = Security.USER_ID;
            if (gUserID == Guid.Empty)
            {
                return "请先登录";
            }

            int result = 0;
            SqlProcs.spTQClientAdd(ref AddClientID, AddClientName, ref result);
            SqlProcs.spTQAccountAdd(ref gID, gUserID, AddClientID);
            if (result == 2) Msg.UserApply(gID);

            return "OK:" + result.ToString() + ":" + gID.ToString();
        }
    }
}