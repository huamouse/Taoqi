using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    public class QuotePrice
    {
        public Guid ID { get; set; }
        public bool Deleted { get; set; }
        public Guid C_ProductID { get; set; }   // 气源地ID		
        public decimal C_Price { get; set; }	// 报价
        public int C_GasTypeID { get; set; }    // 气源类型（LNG、CNG、LPG）
        public int C_GasVarietyID { get; set; } // 品种（管道气、进口气、煤层气、焦炉煤气、煤制气、页岩气）
        public string C_GasSourceName { get; set; } //气源地
        public string DeliveryName { get; set; }    //主要用来判断“其他”
        public int C_GasificationRate { get; set; } //气化率 准确值
        public int C_CalorificValue { get; set; }   //热值 准确值
        public int C_LiquidTemperature { get; set; }    //液温(低温液、高温液)
        public int C_GasZoneID { get; set; }    //气源地所在区域ID（0未定义 1华北 2东北 3华东 4华中 5华南 6西南 7西北）

        public int C_TypeOfPay { get; set; }    //付款方式
        public int C_TypeOfCarFuel { get; set; }    //车辆燃料类型
        public float C_Tonnage { get; set; }      //车辆罐容
        public int C_StandardOfYaChe { get; set; }  //押车标准
        public int C_ProcessOfGasDifference { get; set; }   //气差处理
    }
}