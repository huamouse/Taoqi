using System;
using System.Data;
using System.Web.Http;
using System.Collections;
using Taoqi.Common;
using System.Web;

namespace Taoqi.Controllers
{
    public class ShopCartController : ApiController
    {
        [HttpGet]
        public DataTable Get()
        {
            Hashtable ht = new Hashtable();
            ht.Add("CREATED_BY", Security.AccountID);
            return DAL.GetTable("vwTQShopCart", ht);
        }

        [HttpGet]
        public string Add(Guid id)
        {
            string result;
            try
            {
                if(Security.isBuyer != 1) return "提示：您无权限执行此操作。";

                Guid gid = Guid.Empty;
                SqlProcs.spTQShopCart_Update(ref gid, id);
                result = "OK";
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            return result;
        }

        [HttpGet]
        public string Remove(Guid id)
        {
            string result;
            try
            {
                SqlProcs.spTQShopCart_Delete(id);
                result = "OK";
            }
            catch (Exception e)
            {
                result = e.ToString();
            }

            return result;
        }
    }
}