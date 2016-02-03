using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Web.Http;
using Taoqi.Common;
using Taoqi.Models;

namespace Taoqi.Controllers
{
    public class TransitController : ApiController
    {
        private int PageSize = 15;

        /// <summary>
        /// 在途气
        /// </summary>
        /// <returns></returns>
        public Hashtable GetTransitList(string id = "",
            string TargetProvinceID = "",
            string TargetCityID = "",
            string C_GasTypeID = "",
            string C_GasVarietyID = "",
            string C_ProductID = "",
            string GasificationRateRange = "",
            string CalorificValueRange = "",
            string C_LiquidTemperature = "",
            int pageIndex = 1,
            string orderBy = "",
            string orderDirection = "desc")
        {
            string cache_key = CacheSericeEx.MD5(string.Format("GetTransitList/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}",
                id,
                TargetProvinceID,
                TargetCityID,
                C_GasTypeID,
                C_GasVarietyID,
                C_ProductID,
                GasificationRateRange,
                CalorificValueRange,
                C_LiquidTemperature,
                pageIndex,
                orderBy,
                orderDirection
                ));

            Hashtable htResult = CacheSericeEx.GetItem(cache_key) as Hashtable;

            if (htResult == null)
            {
                Hashtable ht = new Hashtable();

                if (!string.IsNullOrEmpty(id)) ht.Add("ID", id);
                if (!string.IsNullOrEmpty(TargetProvinceID)) ht.Add("TargetProvinceId", TargetProvinceID);
                if (!string.IsNullOrEmpty(TargetCityID)) ht.Add("TargetCityId", TargetCityID);
                if (string.IsNullOrWhiteSpace(C_ProductID))
                {
                    if (!string.IsNullOrWhiteSpace(C_GasTypeID)) ht.Add("C_GasTypeID", C_GasTypeID);
                    if (!string.IsNullOrWhiteSpace(C_GasVarietyID)) ht.Add("C_GasVarietyID", C_GasVarietyID);
                }
                else
                    ht.Add("C_ProductID", C_ProductID);
                // 气化率区间值
                if (!string.IsNullOrWhiteSpace(GasificationRateRange)) ht.Add("GasificationRateRange", GasificationRateRange);
                if (!string.IsNullOrWhiteSpace(CalorificValueRange)) ht.Add("CalorificValueRange", CalorificValueRange);
                if (!string.IsNullOrWhiteSpace(C_LiquidTemperature)) ht.Add("C_LiquidTemperature", C_LiquidTemperature);

                var selectfields = "*";

                var startIndex = (pageIndex - 1) * PageSize + 1;
                var endIndex = pageIndex * PageSize;

                string strOrderBy = "DATE_ENTERED desc";

                //防止SQL注入攻击，不要直接使用orderBy和orderDirection拼接SQL
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy == "C_Quantity")
                    {
                        if (orderDirection == "desc")
                            strOrderBy = "C_Quantity desc";
                        else
                            strOrderBy = "C_Quantity asc";
                    }
                    else if (orderBy == "DATE_ENTERED")
                    {
                        if (orderDirection == "desc")
                            strOrderBy = "DATE_ENTERED desc";
                        else
                            strOrderBy = "DATE_ENTERED asc";
                    }
                }

                DataTable dt = DAL.GetTable("vwTransitForSale", ht, 0, strOrderBy, selectfields, startIndex, endIndex);

                //判断用户在每条在途气中的状态
                if (!dt.Columns.Contains("TransitStatus"))
                    dt.Columns.Add("TransitStatus", System.Type.GetType("System.Int32"));

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();

                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select count(*) from vwTQTransitMy"
                                             + " where C_TransitID = @C_TransitID";

                        if (!Security.isAdmin)
                        {
                            cmd.CommandText += " and CREATED_BY = @AccountID";
                            Sql.AddParameter(cmd, "@AccountID", Security.AccountID);
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            if (cmd.Parameters.Contains("@C_TransitID"))
                                Sql.SetParameter(cmd, "@C_TransitID", row["ID"].ToString());
                            else
                                Sql.AddParameter(cmd, "@C_TransitID", row["ID"].ToString());

                            int count = (int)cmd.ExecuteScalar();
                            if (count > 0)    //已抢
                                row["TransitStatus"] = 1;
                            else    //未抢
                                row["TransitStatus"] = 0;
                        }
                    }
                }

                int total = DAL.GetTotalByViewName("vwTQTransit_List", ht);

                htResult = new Hashtable();
                htResult.Add("items", dt);
                htResult.Add("total", total);

                CacheSericeEx.SetItem(cache_key, dt);
            }

            return htResult;
        }

        /// <summary>
        /// 在途气
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public DataTable GetTransitListTop()
        {
            string cache_key = CacheSericeEx.MD5("GetTransitListTop");

            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;

            if (dt == null)
            {
                var selectfields = @"*";

                dt = DAL.GetTable("vwTransitForSale", null, 5, "DATE_ENTERED desc", selectfields);

                CacheSericeEx.SetItem(cache_key, dt);
            }

            return dt;
        }

        [HttpGet]
        public DataTable Seller(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("CREATED_BY", id);

            return DAL.GetTable("vwTQTransit_List", ht, 0, "DATE_ENTERED desc");
        }

        [HttpGet]
        public DataTable SellAllBuyer(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_TransitID", id);

            return DAL.GetTable("vwTransitForSale_allBuyer", ht);
        }

        [HttpGet]
        public DataTable Buyer(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("CREATED_BY", id);

            return DAL.GetTable("vwTQTransitMy_List", ht, 0, "DATE_ENTERED desc");
        }

        [HttpPost]
        public string AddTransit([FromBody]Transit transit)
        {
            try
            {
                Guid id = Guid.Empty;
                Guid productID = Guid.Empty;
                SqlProcs.spTQProductAdd(ref productID, transit.Product.C_GasTypeID, transit.Product.C_GasVarietyID, transit.Product.C_GasSourceName,
                    transit.Product.C_GasificationRate, transit.Product.C_CalorificValue, transit.Product.C_LiquidTemperature, 0);
                SqlProcs.spTQTransit_Update(ref id, transit.C_ValidityTime, productID, transit.C_CarID, transit.C_Quantity, transit.C_FromCityID, transit.C_TargetCityID);
                Msg.TransitApply(id);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string AddTransitMy(Guid C_TransitID)
        {
            try
            {
                if(Security.isBuyer != 1)
                    return "提示：您无权限执行此操作。";

                if (C_TransitID == Guid.Empty)
                {
                    return "提示：请选择一个在途气。";
                }
                else
                {
                    Guid id = Guid.Empty;
                    SqlProcs.spTQTransitMy_Update(ref id, C_TransitID);

                    return "OK";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string DeleteTransit(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return "提示：请选择一个在途气。";
                }
                else
                {
                    SqlProcs.spTQTransitMy_Delete(id);

                    return "OK";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}