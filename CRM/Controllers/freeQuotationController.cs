using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Web.Http;
using Taoqi.Common;
using Taoqi.Models;
using System.Linq;

namespace Taoqi.Controllers
{
    public class freeQuotationController : ApiController
    {
        private int PageSize = 15;


        [HttpGet]
        public Hashtable GetFreeQuotationList(
            string C_GasTypeID = "",
            string C_GasVarietyID = "",
            string GasificationRateRange = "",
            string C_ProductID = "",
            int pageIndex = 1,
            string orderBy = "",
            string orderDirection = "desc")
        {
            string cache_key = CacheSericeEx.MD5(string.Format("GetFreeQuotationList/{0}/{1}/{2}/{3}/{4}/{5}/{6}",
                C_GasTypeID,
                C_GasVarietyID,
                GasificationRateRange,
                C_ProductID,
                pageIndex,
                orderBy,
                orderDirection
                ));

            Hashtable htResult = CacheSericeEx.GetItem(cache_key) as Hashtable;

            if (htResult == null)
            {
                Hashtable ht = new Hashtable();


                if (!string.IsNullOrEmpty(C_GasTypeID))
                {
                    ht.Add("C_GasTypeID", C_GasTypeID);
                }

                if (!string.IsNullOrEmpty(C_GasVarietyID))
                {
                    ht.Add("C_GasVarietyID", C_GasVarietyID);
                }

                //这是气化率的区间值
                if (!string.IsNullOrEmpty(GasificationRateRange))
                {
                    ht.Add("GasificationRateRange", GasificationRateRange);
                }

                if (!string.IsNullOrEmpty(C_ProductID))
                {
                    ht.Add("C_ProductID", C_ProductID);
                }


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

                DataTable dt = DAL.GetTable("vwTQOrder_List_freeQuotation", ht, 0, strOrderBy, "*", startIndex, endIndex);
                int total = DAL.GetTotalByViewName("vwTQOrder_List_freeQuotation", ht);

                htResult = new Hashtable();
                htResult.Add("items", dt);
                htResult.Add("total", total);

                CacheSericeEx.SetItem(cache_key, htResult);
            }

            return htResult;
        }

        //[HttpGet]
        //public DataTable GetProductListTop10()
        //{
        //    string cache_key = CacheSericeEx.MD5("GetProductListTop10");

        //    DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;
        //    if (dt == null)
        //    {
        //        dt = DAL.GetDatatableByProcedure("spTQProductAreaForHomePage");
        //        CacheSericeEx.SetItem(cache_key, dt);
        //    }

        //    return dt;
        //}
    }
}