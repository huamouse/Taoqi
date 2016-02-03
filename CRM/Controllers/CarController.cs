using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections;
using Taoqi.Models;
using Taoqi._code;

namespace Taoqi.Controllers
{
    public class CarController : ApiController
    {
        [HttpGet]
        public DataTable GetList(int total = 0)
        {
            var selectfields = "*";

            Hashtable ht = new Hashtable();
            ht.Add("CREATED_BY", Security.AccountID);

            var dt = DAL.GetTable("TQCar", ht, total, "DATE_ENTERED desc", selectfields);

            return dt;
        }

        /// <summary>
        /// 查询物流车流
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DataTable GetByID(Guid id)
        {
            Hashtable ht = new Hashtable();
            if (id != Guid.Empty) ht.Add("ID", id);

            return DAL.GetTable("TQCar", ht);
        }

        [HttpPost]
        public int Add([FromBody]Car car)
        {
            try
            {
                Guid gid = car.ID;
                SqlProcs.spTQCar_Update(ref gid, car.C_PlateNumber, car.C_Tonnage,
                        car.C_Driver, car.C_Driver2, car.C_Tel, car.C_Tel2, null);
                return 1;
            }
            catch (Exception)
            {
            }

            return -1;
        }

        [HttpGet]
        public int Remove(Guid id)
        {
            try
            {
                SqlProcs.spTQCar_Delete(id);
                return 0;
            }
            catch (Exception)
            {
            }

            return -1;
        }

        [HttpGet]
        public DataTable WorkPlan(string mobile)
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_Tel", mobile);
            ht.Add("C_Status", 3);
            var dt = DAL.GetTable("vwTQOrderDetail_List", ht);

            return dt;
        }

        [HttpGet]
        public DataTable Working(string mobile)
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_Tel", mobile);
            ht.Add("C_Status", 4);
            var dt = DAL.GetTable("vwTQOrderDetail_List", ht);

            return dt;
        }

        [HttpGet]
        public DataTable TransitPlan(string mobile)
        {
            string where = " (C_Status > 1 and C_Status < 5) ";
            where += " and C_Tel = '" + mobile + "'";
            var dt = DAL.GetTable("vwTQTransit_List", where);

            return dt;
        }

        /// <summary>
        /// 发货时添加物流车辆
        /// </summary>
        /// <param name="Product"></param>
        //[HttpPost]
        //public void AddCarToOrder([FromBody]Logistics logistics)
        //{
        //    /*
        //    Hashtable ht = new Hashtable();
        //    ht.Add("@ID", logistics.ID);
        //    ht.Add("@C_OrderDetailID", logistics.OrderDetailID);
        //    ht.Add("@C_ClientID", logistics.ClientID);
        //    ht.Add("@C_CarID", logistics.CarID);

        //    DAL.UpdateByProcedure("spTQOrderLogistics_Update", ht);
        //    */

        //    Guid id = Guid.Empty;
        //    SqlProcs.spTQOrderDetailCar(ref id, logistics.OrderDetailID, logistics.ClientID, logistics.CarID,1);
        //}


        [HttpGet]
        public DataTable Info(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", id);
            var dt = DAL.GetTable("vwTQCar_List", ht);

            return dt;
        }

        //[HttpGet]
        //public int Leave(Guid orderDetailID)
        //{
        //    // 发车
        //    try
        //    {
        //        //SqlProcs.spTQOrderDetail_ChangeStatus(orderDetailID, 4);

        //        return 0;
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return -1;
        //}

        //[HttpGet]
        //public int Arrive(Guid orderDetailID)
        //{
        //    // 到达
        //    try
        //    {
        //        //SqlProcs.spTQOrderLogisticsStatus(orderDetailID, 3);

        //        //SqlProcs.spTQOrderDetail_ChangeStatus(orderDetailID, 4);
        //        return 0;
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return -1;
        //}

        [HttpGet]
        public object GetPosition(Guid id)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", id);

            return DAL.ExecuteScalar("vwTQCar_List", ht, "C_BaiduPosition");
        }

        [HttpPost]
        public int SetPostion(Guid id, decimal lng, decimal lat)
        {
            // 上传百度坐标
            try
            {
                string C_BaiduPositon = lng.ToString() + "," + lat.ToString();
                SqlProcs.spTQCarPosition(id, C_BaiduPositon);
                return 0;
            }
            catch (Exception)
            {
            }

            return -1;
        }
    }
}