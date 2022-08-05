using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI.Core.Entities {
    public class ProductModel {

        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public float product_price { get; set; }
    }
}
