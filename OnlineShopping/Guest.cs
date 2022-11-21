using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class Guest
    {
        public List<Product> ViewProducts()
        {
            return Product.Products;
        }

        public HttpStatusCode Register(string name, string password, string address, string phoneNo)
        {
            if (
                string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(address)
                || string.IsNullOrEmpty(phoneNo)
            )
                return HttpStatusCode.BadRequest;

            if (Customer.Customers.Any(c => c.Name == name))
                return HttpStatusCode.Conflict;

            var strongPassword = new System.Text.RegularExpressions.Regex(
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$"
            );

            if (!strongPassword.IsMatch(password))
                return HttpStatusCode.BadRequest;

            Customer.Customers.Add(
                new Customer
                {
                    Id = Customer.Customers.Count + 1,
                    Name = name,
                    Password = password,
                    Address = address,
                    PhoneNo = phoneNo
                }
            );
            return HttpStatusCode.OK;
        }
    }
}
