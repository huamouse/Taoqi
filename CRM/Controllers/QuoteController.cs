using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Taoqi._code;
using Taoqi.Common;
using Taoqi.Models;

namespace Taoqi.Controllers
{
    public class QuoteController : ApiController
    {
        private int PageSize = 15;

        [HttpGet]
        public Hashtable GetQuoteList(string id = "",
            string C_ProvinceID = "",
            string C_CityID = "",
            string C_CountyID = "",
            string C_GasTypeID = "",
            string C_GasVarietyID = "",
            string GasificationRateRange = "",
            string C_CalorificValue = "",
            string C_ProductID = "",
            string C_UserType = "",
            int pageIndex = 1,
            string orderBy = "",
            string orderDirection = "desc")
        {
            string cache_key = CacheSericeEx.MD5(string.Format("GetQuoteList/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}",
              id,
              C_ProvinceID,
              C_CityID,
              C_CountyID,
              C_GasTypeID,
              C_GasVarietyID,
              GasificationRateRange,
              C_CalorificValue,
              C_ProductID,
              C_UserType,
              pageIndex,
              orderBy,
              orderDirection,
              Security.USER_ID
              ));

            //Hashtable htResult = CacheSericeEx.GetItem(cache_key) as Hashtable;
            Hashtable htResult = null;

            if (htResult == null)
            {
                Hashtable ht = new Hashtable();
                if (!string.IsNullOrEmpty(id)) ht.Add("ID", id);
                if (!string.IsNullOrEmpty(C_ProvinceID)) ht.Add("C_ProvinceID", C_ProvinceID);
                if (!string.IsNullOrEmpty(C_CityID)) ht.Add("C_CityID", C_CityID);
                if (!string.IsNullOrEmpty(C_CountyID)) ht.Add("C_CountyID", C_CountyID);
                if (!string.IsNullOrEmpty(C_GasTypeID)) ht.Add("C_GasTypeID", C_GasTypeID);
                if (!string.IsNullOrEmpty(C_GasVarietyID)) ht.Add("C_GasVarietyID", C_GasVarietyID); //这是气化率的区间值
                if (!string.IsNullOrEmpty(GasificationRateRange))  ht.Add("GasificationRateRange", GasificationRateRange);
                if (!string.IsNullOrEmpty(C_CalorificValue)) ht.Add("C_CalorificValue", C_CalorificValue);
                if (!string.IsNullOrEmpty(C_ProductID)) ht.Add("C_ProductID", C_ProductID);
                if (!string.IsNullOrEmpty(C_UserType)) ht.Add("C_UserType", C_UserType);

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
                    else if (orderBy == "C_Status")
                    {
                        if (orderDirection == "desc")
                            strOrderBy = "C_Status desc";
                        else
                            strOrderBy = "C_Status asc";
                    }
                }

                DataTable dt = DAL.GetTable("vwQuoteDetailForPrice", ht, 0, strOrderBy, selectfields, startIndex, endIndex);
                int total = DAL.GetTotalByViewName("vwTQQuoteDetail_List", ht);   // 总记录数，供分页使用

                // 判断用户每条求购的状态
                if (!dt.Columns.Contains("QuoteStatus"))
                    dt.Columns.Add("QuoteStatus", System.Type.GetType("System.Int32"));

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();

                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select count(*) from vwTQQuotePrice"
                                             + " where C_QuoteDetailID = @C_QuoteDetailID";

                        cmd.CommandText += " and CREATED_BY = @Account";
                        Sql.AddParameter(cmd, "@Account", Security.AccountID);

                        foreach (DataRow row in dt.Rows)
                        {
                            if (cmd.Parameters.Contains("@C_QuoteDetailID"))
                                Sql.SetParameter(cmd, "@C_QuoteDetailID", row["ID"].ToString());
                            else
                                Sql.AddParameter(cmd, "@C_QuoteDetailID", row["ID"].ToString());

                            int count = (int)cmd.ExecuteScalar();
                            if (count > 0)    //已报
                                row["QuoteStatus"] = 1;
                            else    //未报
                                row["QuoteStatus"] = 0;
                        }
                    }
                }
                htResult = new Hashtable();
                htResult.Add("items", dt);
                htResult.Add("total", total);
                CacheSericeEx.SetItem(cache_key, htResult, 3);
            }

            return htResult;
        }

        [HttpGet]
        public DataTable GetQuoteListTop()
        {
            string cache_key = CacheSericeEx.MD5(string.Format("GetQuoteListTop/{0}", Security.USER_ID));

            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;

            if (dt == null)
            {
                var selectfields = "*";

                dt = DAL.GetTable("vwQuoteDetailForPrice", null, 5, "DATE_ENTERED desc", selectfields);
                // 判断用户每条求购的状态
                if (!dt.Columns.Contains("QuoteStatus"))
                    dt.Columns.Add("QuoteStatus", System.Type.GetType("System.Int32"));

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();

                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "select count(*) from vwTQQuotePrice"
                                             + " where C_QuoteDetailID = @C_QuoteDetailID";

                        cmd.CommandText += " and CREATED_BY = @AccountID";
                        Sql.AddParameter(cmd, "AccountID", Security.AccountID);

                        foreach (DataRow row in dt.Rows)
                        {
                            if (cmd.Parameters.Contains("@C_QuoteDetailID"))
                                Sql.SetParameter(cmd, "@C_QuoteDetailID", row["ID"].ToString());
                            else
                                Sql.AddParameter(cmd, "@C_QuoteDetailID", row["ID"].ToString());

                            int count = (int)cmd.ExecuteScalar();
                            if (count > 0)    //已报
                                row["QuoteStatus"] = 1;
                            else    //未报
                                row["QuoteStatus"] = 0;
                        }
                    }
                }

                CacheSericeEx.SetItem(cache_key, dt, 3);
            }

            return dt;
        }

        [HttpGet]
        public DataTable MyQuoteList()
        {
            string cache_key = CacheSericeEx.MD5(string.Format("MyQuoteList/{0}", Security.AccountID));

            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;
            if (dt == null)
            {
                string where = " C_Status > 1 ";
                where += " and CREATED_BY = '" + Security.AccountID + "' ";
                dt = DAL.GetTable("vwTQQuoteDetail_List", where, "DATE_ENTERED desc");

                CacheSericeEx.SetItem(cache_key, dt);
            }

            return dt;
        }

        [HttpGet]
        public Hashtable GetQuotePriceByID(Guid id)
        {
            string cache_key = CacheSericeEx.MD5(string.Format("GetQuotePriceByID/{0}/{1}", id.ToString(), Security.USER_ID));

            //Hashtable htResult = CacheSericeEx.GetItem(cache_key) as Hashtable;
            Hashtable htResult = null;
            if (htResult == null)
            {
                Hashtable ht = new Hashtable();
                ht.Add("CREATED_BY", Security.AccountID);
                if (!string.IsNullOrEmpty(id.ToString())) ht.Add("C_QuoteDetailID", id);

                var selectfields = "*";
                DataTable dt = DAL.GetTable("vwTQQuotePrice", ht, 0, null, selectfields);

                htResult = new Hashtable();
                htResult.Add("quotePrice", dt);

                CacheSericeEx.SetItem(cache_key, htResult, 3);
            }

            return htResult;
        }

        [HttpGet]
        public DataTable GetQuoteViewByID(Guid id)
        {
            Hashtable ht = new Hashtable();
            if (!string.IsNullOrEmpty(id.ToString())) ht.Add("C_QuoteDetailID", id);

            var selectfields = "*";
            DataTable dt = DAL.GetTable("vwTQQuotePrice", ht, 0, null, selectfields);

            return dt;
        }

        [HttpPost]
        public string AddQuote([FromBody]Quote[] quotes)
        {
            try
            {
                Guid quoteID = Guid.Empty;
                Guid addressID = Guid.Empty;
                foreach (Quote quote in quotes)
                {
                    Guid quoteDetailID = Guid.Empty;
                    if (addressID != quote.C_ClientAddressID)
                    {
                        if (quoteID != Guid.Empty)  // 更新上次求购数量
                        {
                            SqlProcs.spTQQuoteDetailQuantity(quoteID);
                            quoteID = Guid.Empty;
                        }

                        SqlProcs.spTQQuote_Update(ref quoteID, quote.C_GasTypeID, quote.C_GasVarietyID, quote.C_GasificationRateRange,
                            quote.C_CalorificValueRange, quote.C_LiquidTemperature, quote.C_ClientAddressID,
                            quote.C_ValidityTime, quote.C_InvoiceRequestID, quote.C_InvoiceRequestID, quote.C_Remark);
                        SqlProcs.spTQQuoteDetail_Update(ref quoteDetailID, quoteID, quote.C_ArriveTime);
                        Msg.QuoteApply(quoteID);
                        addressID = quote.C_ClientAddressID;
                    }
                    else
                    {
                        SqlProcs.spTQQuoteDetail_Update(ref quoteDetailID, quoteID, quote.C_ArriveTime);
                    }
                }
                if (quoteID != Guid.Empty) SqlProcs.spTQQuoteDetailQuantity(quoteID);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string AddQuotePrice([FromBody] QuotePrice[] quoteprices, Guid quoteID)
        {
            try
            {
                if(quoteID == Guid.Empty){
                    return "提示：请选择一个求购再报价。";
                }
                else
                {
                    Guid id = Guid.Empty;
                    bool isEdit = false;
                    foreach (QuotePrice quotePrice in quoteprices)
                    {
                        id = quotePrice.ID;
                        if (quotePrice.ID != Guid.Empty) isEdit = true;
                        Guid C_ProductID = quotePrice.C_ProductID;
                        if (quotePrice.Deleted)
                        {
                            SqlProcs.spTQQuotePrice_Delete(id);
                            isEdit = true;
                        }
                        else
                        {
                            SqlProcs.spTQProductAdd(ref C_ProductID, quotePrice.C_GasTypeID, quotePrice.C_GasVarietyID, quotePrice.C_GasSourceName,
                                quotePrice.C_GasificationRate, quotePrice.C_CalorificValue, quotePrice.C_LiquidTemperature, quotePrice.C_GasZoneID);
                            SqlProcs.spTQQuotePrice_Update(ref id, quoteID, quotePrice.C_Price, false, C_ProductID,
                                quotePrice.C_TypeOfPay, quotePrice.C_TypeOfCarFuel, quotePrice.C_Tonnage, quotePrice.C_StandardOfYaChe, quotePrice.C_ProcessOfGasDifference);
                        }
                        
                    }

                    SqlProcs.spUpdateTQQuotePriceTotal(quoteID);
                    // 更新求购状态
                    if (isEdit) // 编辑求购
                    {
                        Msg.QuoteDetailPrice(quoteID, true);
                    }
                    else // 首次报价
                    {
                        SqlProcs.spTQQuoteDetailChange(quoteID, 3);
                        Msg.QuoteDetailPrice(quoteID);
                    }

                    return "OK";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //[HttpPost]
        //public string RemoveQuotePrice([FromBody] Guid[] idList, Guid quoteId)
        //{
        //    try
        //    {
        //        foreach (Guid id in idList)
        //        {
        //            SqlProcs.spTQQuotePrice_Delete(id);
        //        }

        //        SqlProcs.spUpdateTQQuotePriceTotal(quoteId);

        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        [HttpGet]
        public string RemovePrice(Guid id)
        {
            try
            {
                SqlProcs.spTQQuotePrice_Delete(id);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}