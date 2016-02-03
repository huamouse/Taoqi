using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using Taoqi._code;
using Taoqi.Common;
using Taoqi.Models;

namespace Taoqi.Controllers
{

    public class MarketInfoController : ApiController
    {

        private int PageSize = 5;

        [HttpGet]
        public Hashtable GetMInfoList(
            string id = "",
            int pageIndex = 1)
        {

            string cache_key = CacheSericeEx.MD5(string.Format("MarketInfo/{0}/{1}",
                id,
                pageIndex
                ));

            Hashtable htResult = CacheSericeEx.GetItem(cache_key) as Hashtable;
          
            if (htResult == null)
            {
                Hashtable ht = new Hashtable();

                if (!string.IsNullOrEmpty(id))
                {
                    ht.Add("ID", id);
                }
                else
                {
                    ht.Add("C_ShowStatus", 1);
                }

                var selectfields = @"ID,C_Type,C_Title,C_Content,C_Introduction,C_Author,DATE_ENTERED,C_ShowStatus,C_Cover";

                var startIndex = (pageIndex - 1) * PageSize + 1;
                var endIndex = pageIndex * PageSize;

                DataTable dt = DAL.GetTable("vwTQMarketInformation_List", ht, 0, "DATE_ENTERED desc", selectfields, startIndex, endIndex);
                int total = DAL.GetTotalByViewName("vwTQMarketInformation_List", ht);

                htResult = new Hashtable();
                htResult.Add("items", dt);
                htResult.Add("total", total);

                CacheSericeEx.SetItem(cache_key, htResult);
            }


            return htResult;
        }

        [HttpGet]
        public DataTable GetMInfoCover(int total = 0)
        {

            string cache_key = CacheSericeEx.MD5(string.Format("MarketInfo/GetMInfoCover/" + total));

            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;

            if (dt == null)
            {
                Hashtable ht = new Hashtable();

                ht.Add("C_ShowStatus", 2);

                var selectfields = @"ID,C_Title,C_Introduction,C_Author,DATE_ENTERED,C_Cover";

                dt = DAL.GetTable("vwTQMarketInformation_List", ht, total, "DATE_ENTERED desc", selectfields);

                CacheSericeEx.SetItem(cache_key, dt);
            }


            return dt;
        }
    }
}