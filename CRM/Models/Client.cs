using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    public class Client
    {
        public Guid ID { get; set; }	 //客户ID
        public string C_ClientName { get; set; }
        public int C_CityID { get; set; }
        public int C_CountyID { get; set; }
        public string C_Address { get; set; }
        public bool TypeSell { get; set; }
        public bool TypeBuy { get; set; }
        public string C_BaiduPosition { get; set; }			//百度坐标
    }

    public class Client_Information
    {
        public string ID { get; set; }	 //客户ID					
        public string C_GasificationRate { get; set; }			//气化率（准确值）
    }
}