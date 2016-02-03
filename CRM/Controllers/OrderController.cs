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
    public class OrderController : ApiController
    {
        private int PageSize = 15;

        [HttpGet]
        public DataTable GetOrderList(int pageIndex = 1)
        {
            string cache_key = CacheSericeEx.MD5("GetOrderList" + pageIndex);

            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;
            if (dt == null)
            {
                var selectfields = "*";

                var startIndex = (pageIndex - 1) * PageSize + 1;
                var endIndex = pageIndex * PageSize;

                dt = DAL.GetTable("vwTQOrder_List", null, 0, "DATE_ENTERED desc", selectfields, startIndex, endIndex);
                
                CacheSericeEx.SetItem(cache_key, dt);
            }

            return dt;
        }

        [HttpGet]
        public DataTable GetDetailList(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_OrderID", id);

            return DAL.GetTable("vwTQOrderDetail_List", ht, 0, "DATE_ENTERED desc");;
        }

        [HttpGet]
        public DataTable GetByID(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", id);

            return DAL.GetTable("vwTQOrder_List", ht);
        }
        [HttpGet]
        public DataTable GetDetailByID(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", id);

            return DAL.GetTable("vwTQOrderDetail_List", ht);
        }

        [HttpGet]
        public string Delete(Guid id)
        {
            try
            {
                SqlProcs.spTQOrder_Delete(id);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpGet]
        public DataTable Buyer(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("BuyerID", id);

            return DAL.GetTable("vwTQOrder_List", ht, 0, "DATE_ENTERED desc");
        }

        [HttpGet]
        public DataTable Seller(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("SellerID", id);

            return DAL.GetTable("vwTQOrder_List", ht, 0, "DATE_ENTERED desc");
        }

        [HttpGet]
        public DataTable GetOrderListTop10()
        {
            string cache_key = CacheSericeEx.MD5("GetOrderListTop10");

            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;

            if (dt == null)
            {
                var selectfields = "*";

                dt = DAL.GetTable("vwTQOrderDetail_List", null, 10, "DATE_ENTERED desc", selectfields);

                CacheSericeEx.SetItem(cache_key, dt);
            }

            return dt;
        }

        [HttpGet]
        public DataTable GetOrderListTop25()
        {
            string cache_key = CacheSericeEx.MD5("GetOrderListTop25");

            DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;

            if (dt == null)
            {
                var selectfields = @"ID, GasVarietyName, StatusName,DATE_ENTERED";

                dt = DAL.GetTable("vwTQOrder_List", null, 25, "DATE_ENTERED desc", selectfields);
                
                CacheSericeEx.SetItem(cache_key, dt);
            }

            return dt;
        }

        [HttpPost]
        public string AddOrder([FromBody]OrderDetail[] orderDetailInfos, Guid id)
        {
            try
            {
                if (orderDetailInfos.Length > 0)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("ID", id);
                    var dtProductArea = DAL.GetTable("vwTQProductArea", ht, 1);

                    if (dtProductArea.Rows.Count > 0)
                    {
                        Guid orderID = Guid.Empty;
                        SqlProcs.spTQOrder_Update(ref orderID, id, Guid.Empty, 0);
                        SqlProcs.spTQShopCartRemove(id);
                        foreach (OrderDetail item in orderDetailInfos)
                        {
                            for (int i = 0; i < int.Parse(item.C_Quantity.ToString()); i++)
                            {
                                Guid orderDetailID = Guid.Empty;
                                SqlProcs.spTQOrderDetail_Update(ref orderDetailID
                                    , orderID
                                    , item.ClientAddressInfo.ID
                                    , Guid.Empty
                                    , 1
                                    , item.C_Price
                                    , 0
                                    , 0
                                    , item.C_ArriveTime);
                            }
                        }

                        string SN = "";
                        Msg.Order(orderID, ref SN);
                        return "OK:" + SN;
                    }
                    else
                    {
                        return "提示：没有该商品或该商品已下架。";
                    }
                }

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 修改订单价格
        /// </summary>
        /// <param name="id">详单ID</param>
        /// <param name="price"></param>
        /// <returns></returns>
        [HttpGet]
        public string ModifyPrice(Guid id, float price)
        {
            try
            {
                Msg.ModifyPrice(id, price);
                SqlProcs.spTQOrderDetailPriceModify(id, price);
                
                return "OK";
            }
            catch (Exception)
            {
            }

            return "ERROR";
        }

        [HttpGet]
        public string ModifyQuantity(Guid id, float quantity)
        {
            try
            {
                SqlProcs.spTQOrderDetailQuantityModify(id, quantity);
                SqlProcs.spTQOrderDetail_ChangeStatus(id, 6);
                Msg.Receipt(id);

                return "OK";
            }
            catch (Exception)
            {
            }

            return "ERROR";
        }

        [HttpGet]
        public string SendCar(Guid orderDetailID, Guid carID)
        {
            try
            {
                SqlProcs.spTQOrderDetailCar(orderDetailID, carID);
                string msgContent = "";
                Msg.DispatchCar(orderDetailID, ref msgContent);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpGet]
        public object GetPosition(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", id);

            return DAL.ExecuteScalar("vwTQOrderDetail_List", ht, "C_BaiduPosition");
        }
    }
}