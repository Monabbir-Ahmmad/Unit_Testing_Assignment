using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Product
    {
        public static List<Product> Products = new List<Product>();

        public int Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public decimal Price { get; set; }
    }
}
