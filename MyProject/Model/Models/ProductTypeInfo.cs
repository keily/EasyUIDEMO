using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    [Serializable()]
    public class ProductTypeInfo
    {
        public int ID { get; set; }
        public string ProductTypeName { get; set; }
        public string Description { get; set; }
    }

    [Serializable()]
    public class ProductTypeData
    {
        public int total { get; set; }
        public IList<ProductTypeInfo> rows { get; set; }
    }
}
