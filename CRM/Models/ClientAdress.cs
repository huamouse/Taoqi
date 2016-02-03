using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    public class ClientAddress
    {
        public Guid ID { get; set; }				
        public string C_StationName { get; set; }			//场站名称
        public string C_StationShortName { get; set; }			//场站简称
        public decimal C_StationCapacity { get; set; }	//场站罐容（吨）
        public int C_CountyID { get; set; }				 //到岸城市的区ID
        public string C_Address { get; set; }				//到岸地点邮编地址					   
        public string C_ContactName { get; set; }			//到岸联系人
        public string C_Tel { get; set; }				//到岸联系电话
        public string C_BaiduPosition { get; set; }	//百度坐标
        public int C_UserType { get; set; }			//用户类型（请见UserCategory_dom）
        public decimal C_DailyConsumption1 { get; set; }		//日用气量（吨）
        public decimal C_DailyConsumption2 { get; set; }		//日用气量（立方米）

    }


}