using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable()]
    public class ProductInfo
    {
        public int ID { get; set; }
        public int ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string GetDate { get; set; }
        public bool? Enable { get; set; }
    }
    [Serializable()]
    public class ProductData
    {
        public int total { get; set; }
        public IList<ProductInfo> rows { get; set; }
    }

}
