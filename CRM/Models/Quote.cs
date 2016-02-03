using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    public class Quote
    {
        public int C_GasTypeID { get; set; }        //气源类型（LNG、CNG、LPG）
        public int C_GasVarietyID { get; set; }     //品种（国产气、进口气、煤层气、焦炉煤气、煤制气、页岩气）	   				 
        public int C_GasificationRateRange { get; set; } //气化率（范围值）
        public int C_CalorificValueRange { get; set; }   //热值（准确值）	   
        public int C_LiquidTemperature { get; set; }    //液温(低温液、高温液)
        public Guid C_ClientAddressID { get; set; }  //收货地址
        public int C_Quantity { get; set; }	//总量
        public DateTime C_ValidityTime { get; set; }	//报价有效期
        public int C_InvoiceRequestID { get; set; } // 发票要求
        public int C_InvoiceTypeID { get; set; }    // 发票类型
        public string C_Remark { get; set; }        // 备注
        public DateTime C_ArriveTime { get; set; }	//期待到岸时间
    }
}