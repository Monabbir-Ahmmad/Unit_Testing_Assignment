using System.Net;

namespace OnlineShopping
{
    public class Customer : UserBase
    {
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        private Cart _cart = new Cart();

        public Response Login(string email, string password)
        {
            var customer = Database.Customers.FirstOrDefault(
                c => c.Email == email && c.Password == password
            );
            if (customer == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Invalid email or password"
                };

            Id = customer.Id;
            return new Response { StatusCode = HttpStatusCode.OK, Message = "Login successful" };
        }

        public Response ChangePassword(int id, string oldPassword, string newPassword)
        {
            var customer = Database.Customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Customer not found"
                };

            if (customer.Password != oldPassword)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Old password is incorrect"
                };

            var strongPassword = new System.Text.RegularExpressions.Regex(
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$"
            );

            if (!strongPassword.IsMatch(newPassword))
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "New password is weak"
                };

            Database.Customers.Find(c => c.Id == id).Password = newPassword;

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Password changed successfully"
            };
        }

        public Response UpdateProfile(Customer customer)
        {
            if (
                customer == null
                || string.IsNullOrEmpty(customer.Name)
                || string.IsNullOrEmpty(customer.Email)
                || string.IsNullOrEmpty(customer.Address)
                || string.IsNullOrEmpty(customer.PhoneNo)
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid customer details"
                };

            var existingCustomer = Database.Customers.FirstOrDefault(c => c.Id == customer.Id);

            if (existingCustomer == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Customer not found"
                };

            var emailSyntax = new System.Text.RegularExpressions.Regex(
                @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
            );

            if (!emailSyntax.IsMatch(customer.Email))
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid email address"
                };

            if (Database.Customers.Any(c => c.Email == customer.Email && c.Id != customer.Id))
                return new Response
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "Email already exists"
                };

            var customerIndex = Database.Customers.IndexOf(existingCustomer);

            Database.Customers[customerIndex] = customer;

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Profile updated successfully"
            };
        }

        public Response AddToCart(int productId, int quantity)
        {
            var product = Database.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found"
                };

            if (quantity <= 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid quantity"
                };

            for (int i = 0; i < quantity; i++)
            {
                _cart.AddToCart(product);
            }

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product added to cart successfully"
            };
        }

        public Response RemoveFromCart(int productId, int quantity)
        {
            var products = _cart.ViewProductsInCart().FindAll(p => p.Id == productId);

            if (products.Count == 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found in cart"
                };

            if (quantity <= 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid quantity"
                };

            if (quantity > products.Count)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Quantity is greater than the number of products in cart"
                };

            for (int i = 0; i < quantity; i++)
            {
                _cart.RemoveFromCart(productId);
            }

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product removed from cart successfully"
            };
        }

        public Response ClearCart()
        {
            _cart.ClearCart();

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Cart cleared successfully"
            };
        }

        public Response Checkout()
        {
            if (_cart.NumberOfProducts == 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Cart is empty"
                };

            if (_cart.TotalPrice > 1000)
                return new Response
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Message = "Total price is over the checkout limit"
                };

            if (Id == 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Message = "Customer not logged in"
                };

            _cart.Id = Database.Carts.Count + 1;
            _cart.CustomerId = Id;

            Database.Carts.Add(_cart);

            _cart = new Cart();

            return new Response { StatusCode = HttpStatusCode.OK, Message = "Checkout successful" };
        }

        public List<Product> ViewCart()
        {
            return _cart.ViewProductsInCart();
        }

        public Response MakePurchase(int cartId, CreditCard card, string cardPin, IPayment payment)
        {
            var cart = Database.Carts.FirstOrDefault(c => c.Id == cartId && c.CustomerId == Id);

            if (cart == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Cart not found"
                };

            if (!payment.CheckIfCardIsSupported(card))
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Card not supported"
                };

            if (!payment.CheckIfCardIsValid(card))
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Card is invalid"
                };

            if (!payment.CheckIfPayable(card, cart.TotalPrice))
                return new Response
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Message = "Not enough funds in card"
                };

            if (!payment.CheckIfPinIsValid(card, cardPin))
                return new Response
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Message = "Invalid card pin"
                };

            return payment.Pay(card, cart.TotalPrice);
        }
    }
}
