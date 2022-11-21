using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Customer : UserBase
    {
        public static List<Customer> Customers = new List<Customer>();

        public string Address { get; set; }
        public string PhoneNo { get; set; }

        private Cart _cart = new Cart { Id = RandomNumberGenerator.GetInt32(1, 9999) };

        public List<Product> ViewProducts()
        {
            return Product.Products;
        }

        public HttpStatusCode AddToCart(int productId)
        {
            var product = Product.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return HttpStatusCode.NotFound;

            _cart.Products.Add(product);
            return HttpStatusCode.OK;
        }

        public HttpStatusCode RemoveFromCart(int productId)
        {
            var product = _cart.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return HttpStatusCode.NotFound;

            _cart.Products.Remove(product);
            return HttpStatusCode.OK;
        }
    }
}
