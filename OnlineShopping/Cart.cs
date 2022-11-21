using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Cart
    {
        public int Id { get; set; }

        public List<Product> Products = new List<Product>();

        public int NumberOfProducts
        {
            get { return Products.Count; }
        }

        public decimal TotalPrice
        {
            get { return Products.Sum(p => p.Price); }
        }
    }
}
