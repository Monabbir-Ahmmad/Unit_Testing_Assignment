using System.Net;

namespace OnlineShopping
{
    public class Guest
    {
        public List<Product> ViewProducts()
        {
            return Database.Products;
        }

        public Response Register(
            string name,
            string email,
            string password,
            string address,
            string phoneNo
        )
        {
            if (
                string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(address)
                || string.IsNullOrEmpty(phoneNo)
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid customer details"
                };

            var emailSyntax = new System.Text.RegularExpressions.Regex(
                @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
            );

            if (!emailSyntax.IsMatch(email))
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid email address"
                };

            if (Database.Customers.Any(c => c.Email == email))
                return new Response
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "Email already exists"
                };

            var strongPassword = new System.Text.RegularExpressions.Regex(
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$"
            );

            if (!strongPassword.IsMatch(password))
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Password is weak"
                };

            Database.Customers.Add(
                new Customer
                {
                    Id = Database.Customers.Count + 1,
                    Name = name,
                    Email = email,
                    Password = password,
                    Address = address,
                    PhoneNo = phoneNo
                }
            );
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Customer registered successfully"
            };
        }
    }
}
