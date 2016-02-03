using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    public class Transit
    {
        public Guid ID { get; set; }
        public DateTime C_ValidityTime { get; set; }
        public Product Product { get; set; }
        public Guid C_CarID { get; set; }
        public float C_Quantity { get; set; }
        public int C_FromCityID { get; set; }
        public int C_TargetCityID { get; set; }
    }


}