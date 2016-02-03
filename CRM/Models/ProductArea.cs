using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taoqi.Models
{
    public class ProductArea
    {
        public Guid ID { get; set; }
        public bool Deleted { get; set; }
        public Guid C_ProductID { get; set; }
        public Product Product { get; set; }
        public int C_CityID { get; set; }
        public decimal C_Price_Min { get; set; }
        public int C_CarQuantity { get; set; }
    }
}