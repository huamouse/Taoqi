using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    // 货车
    public class Car
    {
        public Guid ID { get; set; }
        public string C_PlateNumber { get; set; }
        public float C_Tonnage { get; set; }
        public string C_Driver { get; set; }
        public string C_Driver2 { get; set; }
        public string C_Tel { get; set; }
        public string C_Tel2 { get; set; }
    }
}