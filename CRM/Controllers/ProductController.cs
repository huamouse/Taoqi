using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Web.Http;
using Taoqi.Common;
using Taoqi.Models;

namespace Taoqi.Controllers
{
    public class ProductController : ApiController
    {
        private int PageSize = 15;

        /// <summary>
        /// 我要卖气
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Hashtable GetProductList(string id = "",
            string C_ProvinceID = "",
            string C_CityID = "",
            string C_ProductID = "",
            string C_GasTypeID = "",
            string C_GasVarietyID = "",
            string C_GasSourceName = "",
            string GasificationRateRange = "",
            string CalorificValueRange = "",
            string C_LiquidTemperature = "",
            string SellerClientID = "",
            int pageIndex = 1,
            string orderBy = "",
            string orderDirection = "desc")
        {
            string cache_key = CacheSericeEx.MD5(string.Format("GetProductList/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}/{13}",
                id,
                C_ProvinceID,
                C_CityID,
                C_ProductID,
                C_GasTypeID,
                C_GasVarietyID,
                C_GasSourceName,
                GasificationRateRange,
                CalorificValueRange,
                C_LiquidTemperature,
                SellerClientID,
                pageIndex,
                orderBy,
                orderDirection
                ));

            Hashtable htResult = CacheSericeEx.GetItem(cache_key) as Hashtable;

            if (htResult == null)
            {
                Hashtable ht = new Hashtable();

                if (!string.IsNullOrWhiteSpace(id)) ht.Add("ID", id);
                if (!string.IsNullOrWhiteSpace(C_ProvinceID)) ht.Add("C_ProvinceID", C_ProvinceID);
                if (!string.IsNullOrWhiteSpace(C_CityID)) ht.Add("C_CityID", C_CityID);
                if (!string.IsNullOrWhiteSpace(SellerClientID)) ht.Add("SellerClientID", SellerClientID);
                if (string.IsNullOrWhiteSpace(C_ProductID))
                {
                    if (!string.IsNullOrWhiteSpace(C_GasTypeID)) ht.Add("C_GasTypeID", C_GasTypeID);
                    if (!string.IsNullOrWhiteSpace(C_GasVarietyID)) ht.Add("C_GasVarietyID", C_GasVarietyID);
                    if (!string.IsNullOrWhiteSpace(C_GasSourceName)) ht.Add("C_GasSourceName", C_GasSourceName);
                    if (!string.IsNullOrWhiteSpace(GasificationRateRange)) ht.Add("GasificationRateRange", GasificationRateRange);
                    if (!string.IsNullOrWhiteSpace(CalorificValueRange)) ht.Add("CalorificValueRange", CalorificValueRange);
                    if (!string.IsNullOrWhiteSpace(C_LiquidTemperature)) ht.Add("C_LiquidTemperature", C_LiquidTemperature);
                }
                else
                {
                    Hashtable htProduct = new Hashtable();
                    htProduct.Add("ID", C_ProductID);
                    DataTable dtProduct = DAL.GetTable("TQProduct", htProduct);
                    foreach (DataRow row in dtProduct.Rows)
                    {
                        ht.Add("C_GasTypeID", row["C_GasTypeID"]);
                        ht.Add("C_GasVarietyID", row["C_GasVarietyID"]);
                        ht.Add("C_GasSourceName", row["C_GasSourceName"]);
                        break;
                    }
                }

                var selectfields = "*";

                var startIndex = (pageIndex - 1) * PageSize + 1;
                var endIndex = pageIndex * PageSize;

                string strOrderBy = "DATE_ENTERED desc";

                //防止SQL注入攻击，不要直接使用orderBy和orderDirection拼接SQL
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy == "C_Price_Min")
                    {
                        if (orderDirection == "desc")
                            strOrderBy = "C_Price_Min desc";
                        else
                            strOrderBy = "C_Price_Min asc";
                    }
                    else if (orderBy == "C_Price_Max")
                    {
                        if (orderDirection == "desc")
                            strOrderBy = "C_Price_Max desc";
                        else
                            strOrderBy = "C_Price_Max asc";
                    }
                    else if (orderBy == "C_CarQuantity")
                    {
                        if (orderDirection == "desc")
                            strOrderBy = "C_CarQuantity desc";
                        else
                            strOrderBy = "C_CarQuantity asc";
                    }
                    else if (orderBy == "DATE_ENTERED")
                    {
                        if (orderDirection == "desc")
                            strOrderBy = "DATE_ENTERED desc";
                        else
                            strOrderBy = "DATE_ENTERED asc";
                    }
                }

                DataTable dt = DAL.GetTable("vwTQProductAreaForSale", ht, 0, strOrderBy, selectfields, startIndex, endIndex);
                int total = DAL.GetTotalByViewName("vwTQProductAreaForSale", ht);

                htResult = new Hashtable();
                htResult.Add("items", dt);
                htResult.Add("total", total);

                CacheSericeEx.SetItem(cache_key, htResult);
            }

            return htResult;
        }

        [HttpGet]
        public DataTable GetProductListTop()
        {
            string cache_key = CacheSericeEx.MD5("GetProductListTop");

            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;
            if (dt == null)
            {
                dt = DAL.GetDatatableByProcedure("spTQProductAreaForHomePage");
                CacheSericeEx.SetItem(cache_key, dt);
            }

            return dt;
        }

        /// <summary>
        /// 关键词检索产品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DataTable Search(string keyword,
            int pageIndex = 1,
            string orderBy = "",
            string orderDirection = "desc")
        {
            string cache_key = CacheSericeEx.MD5(string.Format("GetProductList/{0}/{1}/{2}/{3}", keyword,
                pageIndex,
                orderBy,
                orderDirection
                ));
            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;

            if (dt == null)
            {
                var selectfields = "*";

                var startIndex = (pageIndex - 1) * PageSize + 1;
                var endIndex = pageIndex * PageSize;

                string strOrderBy = "DATE_ENTERED desc";

                //防止SQL注入攻击，不要直接使用orderBy和orderDirection拼接SQL
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy == "C_Price_Min")
                    {
                        if (orderDirection == "desc")
                        {
                            strOrderBy = "C_Price_Min desc";
                        }
                        else
                        {
                            strOrderBy = "C_Price_Min asc";
                        }
                    }
                    else if (orderBy == "C_Price_Max")
                    {
                        if (orderDirection == "desc")
                        {
                            strOrderBy = "C_Price_Max desc";
                        }
                        else
                        {
                            strOrderBy = "C_Price_Max asc";
                        }
                    }
                    else if (orderBy == "C_CarQuantity")
                    {
                        if (orderDirection == "desc")
                        {
                            strOrderBy = "C_CarQuantity desc";
                        }
                        else
                        {
                            strOrderBy = "C_CarQuantity asc";
                        }
                    }
                    else if (orderBy == "DATE_ENTERED")
                    {
                        if (orderDirection == "desc")
                        {
                            strOrderBy = "DATE_ENTERED desc";
                        }
                        else
                        {
                            strOrderBy = "DATE_ENTERED asc";
                        }
                    }
                }

                dt = DAL.GetTable("fnTQProductArea_Search('" + keyword + "')", null, 0, strOrderBy, selectfields, startIndex, endIndex);

                CacheSericeEx.SetItem(cache_key, dt);
            }

            return dt;
        }

        [HttpPost]
        public string AddProductArea([FromBody]ProductArea[] productAreaList)
        {
            try
            {
                foreach (ProductArea item in productAreaList)
                {
                    Guid id = item.ID;
                    Guid productID = item.Product.ID;
                    SqlProcs.spTQProductAdd(ref productID, item.Product.C_GasTypeID, item.Product.C_GasVarietyID, item.Product.C_GasSourceName,
                        item.Product.C_GasificationRate, item.Product.C_CalorificValue, item.Product.C_LiquidTemperature, item.Product.C_GasZoneID);

                    if (item.Deleted || item.C_CarQuantity == 0)
                        SqlProcs.spTQProductArea_Delete(id);
                    else
                        SqlProcs.spTQProductArea_Update(ref id, productID, item.C_CityID, item.C_Price_Min, 0, item.C_CarQuantity, 0);
                }

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productAreaList"></param>
        /// <param name="mode">0：重新发布 1：改价不发布</param>
        /// <returns></returns>
        [HttpPost]
        public string ModifyPrice([FromBody]ProductArea[] productAreaList, int mode = 0)
        {
            try
            {
                Guid id = Guid.Empty;
                foreach (ProductArea item in productAreaList)
                {
                    if (item.Deleted || item.C_CarQuantity == 0)
                        SqlProcs.spTQProductArea_Delete(item.ID);
                    else
                    {
                        if (mode == 1) id = item.ID;
                        SqlProcs.spTQProductArea_Update(ref id, item.C_ProductID, item.C_CityID, item.C_Price_Min, 0, item.C_CarQuantity, 0);
                    }
                        
                }

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //[HttpGet]
        //public string ModifyPrice(Guid id, decimal price, int mode)
        //{
        //    try
        //    {
        //        SqlProcs.spTQProductPriceModify(id, price, mode);
        //        return "OK";
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return "ERROR";
        //}

        [HttpGet]
        public Client GetClientPositionById(Guid id)
        {
            Client client = new Client();

            Hashtable ht = new Hashtable();
            ht.Add("ID", id);

            var selectfields = @"ID,C_BaiduPosition";
            var dt = DAL.GetTable("vwTQClient", ht, 1, null, selectfields);

            if (dt.Rows.Count > 0)
            {
                //Guid.TryParse(dt.Rows[0]["ID"].ToString(), out client.ID);
                client.C_BaiduPosition = Convert.ToString(dt.Rows[0]["C_BaiduPosition"]);
            }

            return client;
        }
        
        [HttpGet]
        public DataTable GetCity(Guid productID, int provinceID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("SellerID", Security.AccountID);
            ht.Add("C_ProductID", productID);
            ht.Add("C_ProvinceID", provinceID);

            return DAL.GetTable("vwTQProductAreaForSale", ht);
        }

        [HttpGet]
        public Client_Information GetClientInformationById(string ID)
        {
            Client_Information client = new Client_Information();

            Hashtable ht = new Hashtable();
            ht.Add("ID", ID);

            var selectfields = @"ID,C_GasificationRate";
            var dt = DAL.GetTable("vwTQClient", ht, 1, null, selectfields);

            if (dt.Rows.Count > 0)
            {
                client.ID = Convert.ToString(dt.Rows[0]["ID"]);
                client.C_GasificationRate = dt.Rows[0]["C_GasificationRate"].ToString();
            }

            return client;
        }
    }
}