using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Database
    {
        public static readonly List<Product> Products = new List<Product>();

        public static readonly List<Customer> Customers = new List<Customer>();

        public static readonly List<Cart> Carts = new List<Cart>();

        public static void ClearDatabase()
        {
            Products.Clear();
            Customers.Clear();
            Carts.Clear();
        }
    }
}
