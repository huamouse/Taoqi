using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    public class Product
    {
        public Guid ID { get; set; }                // 气源ID
        public int C_GasTypeID { get; set; }        // 气源类型（LNG、CNG、LPG）
        public int C_GasVarietyID { get; set; }     // 品种	
        public string C_GasSourceName { get; set; } // 气源地	   				 
        public int C_GasificationRate { get; set; } // 气化率（准确值）
        public int C_CalorificValue { get; set; }   // 热值（准确值）	   
        public int C_LiquidTemperature { get; set; }    //液温(低温液、高温液)
        public int C_GasZoneID { get; set; }
    }
}