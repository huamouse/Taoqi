using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    public class OrderDetail
    {
        public Guid ID { get; set; }
        public Guid CarID { get; set; }         // 车辆ID
        public ClientAddress ClientAddressInfo { get; set; }

        public int C_Quantity { get; set; }

        public float C_Price { get; set; }

        public DateTime C_ArriveTime { get; set; }
    }
}