using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web.Http;
using System.Data;
using Taoqi.Models;
using System.Collections;
using Taoqi;
using Taoqi.Common;

namespace Taoqi.Controllers
{
    public class HomeController : ApiController
    {
        /// <summary>
        /// 气源基础数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Dictionary<string, object> GetBaseData()
        {
            var GasType = CacheSericeEx.GetTerminologyList("GasType_dom");   // 气源类别
            var GasVariety = CacheSericeEx.GetTerminologyList("GasVariety_dom");    // 气源种类
            var GasificationRate = CacheSericeEx.GetTerminologyList("GasificationRate_dom");  // 气化率
            var CalorificValue = CacheSericeEx.GetTerminologyList("CalorificValue_dom");  // 热值
            var LiquidTemperature = CacheSericeEx.GetTerminologyList("LiquidTemperature_dom");  // 液温
            var UserType = CacheSericeEx.GetTerminologyList("UserType_dom");    // 用户类型
            var InvoiceRequest = CacheSericeEx.GetTerminologyList("InvoiceRequest_dom");    // 发票要求
            var InvoiceType = CacheSericeEx.GetTerminologyList("InvoiceType_dom");    // 发票类型

            var ProcessOfGasDifference = CacheSericeEx.GetTerminologyList("ProcessOfGasDifference_dom");    // 气差处理
            var StandardOfYaChe = CacheSericeEx.GetTerminologyList("StandardOfYaChe_dom");    // 押车标准
            var TypeOfCarFuel = CacheSericeEx.GetTerminologyList("TypeOfCarFuel_dom");    // 车辆燃料类型
            var TypeOfPay = CacheSericeEx.GetTerminologyList("TypeOfPay_dom");    // 付款方式

            DataSet Plant = CacheSericeEx.GetItem("Plant/" + Security.USER_ID.ToString()) as DataSet;    // 液化工厂
            DataTable Wharf = CacheSericeEx.GetItem("Wharf/" + Security.USER_ID.ToString()) as DataTable;  // 接收站码头
            DataTable Province = CacheSericeEx.GetItem("Province") as DataTable;    // 省份
            DataTable Zone = CacheSericeEx.GetItem("Zone") as DataTable;    // 区域
            
            Hashtable ht = new Hashtable();
            // 区域
            if (Zone == null)
            {
                Zone = DAL.GetTable("TQProvince", null, 0, null, "distinct C_ZoneID, C_ZoneName");
                CacheSericeEx.SetItem("Zone", Zone);
            }

            // 省份
            if (Province == null)
            {
                Province = DAL.GetTable("vwTQProvince", null, 100, "C_ProvinceID asc", "C_ProvinceID, C_ProvinceName");
                CacheSericeEx.SetItem("Province", Province);
            }

            // 液化工厂
            if (Plant == null)
            {
                Plant = new DataSet();
                DataTable dt;

                foreach (DataRow row in Zone.Rows)
                {
                    ht.Clear();
                    ht.Add("C_GasVarietyID", 1); 
                    ht.Add("C_GasZoneID", row["C_ZoneID"]);
                    dt = DAL.GetTable("fnTQProduct('" + Security.AccountID + "')", ht);
                    dt.TableName = row["C_ZoneName"].ToString();
                    Plant.Tables.Add(dt);
                }

                CacheSericeEx.SetItem("Plants/" + Security.AccountID.ToString(), Plant);
            }

            // 接收站码头
            if (Wharf == null)
            {
                ht.Clear();
                ht.Add("C_GasVarietyID", 2);
                Wharf = DAL.GetTable("fnTQProduct('" + Security.AccountID + "')", ht);
                CacheSericeEx.SetItem("Wharf/" + Security.AccountID.ToString(), Wharf);
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("GasType", GasType);
            dic.Add("GasVariety", GasVariety);
            dic.Add("GasificationRate", GasificationRate);
            dic.Add("CalorificValue", CalorificValue);
            dic.Add("LiquidTemperature", LiquidTemperature);
            dic.Add("UserType", UserType);
            dic.Add("InvoiceRequest", InvoiceRequest);
            dic.Add("InvoiceType", InvoiceType);

            dic.Add("ProcessOfGasDifference", ProcessOfGasDifference);
            dic.Add("StandardOfYaChe", StandardOfYaChe);
            dic.Add("TypeOfCarFuel", TypeOfCarFuel);
            dic.Add("TypeOfPay", TypeOfPay);

            dic.Add("Zone", Zone);
            dic.Add("Province", Province);
            dic.Add("Plant", Plant);
            dic.Add("Wharf", Wharf);
            dic.Add("Company", Company());
            if (Security.IsAuthenticated()) dic.Add("UserInfo", UserInfo(Security.AccountID));

            // 资料完整度
            ht.Clear();
            ht.Add("ID", Security.USER_ID);
            DataRow rowUsers = DAL.GetRow("users", ht);
            int percent = 2;
            if (rowUsers != null)
            {
                if (rowUsers["C_QQ"].ToString() != "") percent++;
                if (rowUsers["C_Weixin"].ToString() != "") percent++;
                if (rowUsers["EMAIL1"].ToString() != "") percent++;
            }
            dic.Add("Percent", percent);

            return dic;
        }

        [HttpGet]
        public DataTable Company()
        {
            DataTable dt = new DataTable();
            Hashtable ht = new Hashtable();
            ht.Clear();
            ht.Add("C_UserID", Security.USER_ID);
            dt = DAL.GetTable("vwTQAccount_List", ht, 0, null, "ID, C_UserID, C_ClientID, C_ClientName");
            CacheSericeEx.SetItem("Company/" + Security.USER_ID.ToString(), dt);

            return dt;
        }

        [HttpGet]
        public Dictionary<string, string> UserInfo(Guid id)
        {
            Dictionary<string, string> userDic = new Dictionary<string, string>();
            Permission.Load(id);

            userDic.Add("AccountID", Security.AccountID.ToString());
            userDic.Add("UserID", Security.USER_ID.ToString());
            userDic.Add("ClientID", Security.UserClientID);
            userDic.Add("isAdmin", Security.isAdmin.ToString().ToLower());  // 网站管理员
            userDic.Add("isCompany", Security.isCompany.ToString());  // 企业用户
            userDic.Add("isCompanyAdmin", Security.isCompanyAdmin.ToString());  // 公司管理员
            // 用户权限
            userDic.Add("isBuyer", Security.isBuyer.ToString());
            userDic.Add("isSeller", Security.isSeller.ToString());
            userDic.Add("isEmployee", Security.isEmployee.ToString());
            userDic.Add("isDriver", Security.isDriver.ToString());

            userDic.Add("RealName", Security.RealName);    // 真实姓名
            userDic.Add("CompanyName", Security.UserCompany);
            userDic.Add("CompanyStatus", Security.CompanyStatus);

            return userDic;
        }

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DataTable GetCityByProvince(int id = 0)
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_ProvinceID", id);

            var dt = DAL.GetTable("TQArea", ht, 50, "C_CityID asc", "C_CityID, C_CityName");

            return dt;
        }

        /// <summary>
        /// 根据省份获取气源地
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DataTable GetParentClientByProvince(int id = 0)
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_ProvinceID",id);

            var dt = DAL.GetTable("vwTQClient_List", ht, 50, "C_ClientShortName asc", "ID,C_ClientShortName,C_BaiduPosition");

            return dt;
        }

        /// <summary>
        /// 根据城市获取区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DataTable GetCountyByCity(int id=0)
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_CityID", id);

            var dt = DAL.GetTable("TQCounty", ht, 50, "C_CountyID asc", "C_CountyID, C_CountyName");

            return dt;
        }

        [HttpPost]
        public string AddAssistantInfo([FromBody]Assistant assistant)
        {
            string result = "OK";
            return result;
        }

        [HttpGet]
        public DataTable GetFavorite()
        {
            Hashtable ht = new Hashtable();
            ht.Add("CREATED_BY", Security.AccountID);
            return DAL.GetTable("vwTQMyFavorite_List", ht);
        }

        [HttpGet]
        public int AddFavorite(Guid id)
        {
            int result = -1;
            try
            {
                Guid gid = Guid.Empty;
                SqlProcs.spTQMyFavorite_Update(ref gid, id);
                result = 0;
            }
            catch (Exception)
            {
            }

            return result;
        }

        [HttpGet]
        public int DeleteFavorite(Guid id)
        {
            int result = -1;
            try
            {
                SqlProcs.spTQMyFavorite_Delete(id);
                result = 0;
            }
            catch (Exception)
            {
            }

            return result;
        }

        private DateTime NetTime()
        {
            DateTime netTime;
            DateTime.TryParse((CacheSericeEx.GetItem("NetTime") ?? "").ToString(), out netTime);
            if (netTime == DateTime.MinValue)
            {
                WebClient wc = new WebClient();
                string lret = wc.DownloadString("http://open.baidu.com/special/time/");
                Match mc = Regex.Match(lret, @"window.baidu_time\(\d{13,}");
                string sdt = mc.Value.Substring(18);
                long ldt = Convert.ToInt64(sdt) / 1000;
                DateTime oritime = Convert.ToDateTime("1970-1-1 0:0:0");
                netTime = oritime.AddSeconds(ldt).AddHours(8);
                CacheSericeEx.SetItem("NetTime", netTime, 600);
            }

            return netTime; 
        }
    }
}