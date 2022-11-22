using System.Net;
using OnlineShopping;
using Xunit;

namespace OnlineShoppingTests
{
    [Collection("Sequential")]
    public class CustomerTest
    {
        [Fact]
        public void Login_ValuesAreValid()
        {
            // Arrange
            Database.ClearDatabase();
            new Guest().Register("John", "john@ex.com", "John123!", "20 Main Street", "1234567890");
            var customer = new Customer();

            // Act
            var result = customer.Login("john@ex.com", "John123!");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Login successful", result.Message);
        }

        [Fact]
        public void Login_InvalidCredentials()
        {
            // Arrange
            Database.ClearDatabase();
            new Guest().Register("John", "john@ex.com", "John123!", "20 Main Street", "1234567890");
            var customer = new Customer();

            // Act
            var result = customer.Login("john@ex.co", "John123");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Invalid email or password", result.Message);
        }

        [Fact]
        public void ChangePassword_CustomerNotFound()
        {
            // Arrange
            Database.ClearDatabase();
            DataGenerator.GenerateCustomers(10);
            var customer = new Customer();

            // Act
            var result = customer.ChangePassword(0, "John123!", "John1234!");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Customer not found", result.Message);
        }

        [Fact]
        public void ChangePassword_InvalidOldPassword()
        {
            // Arrange
            Database.ClearDatabase();
            new Guest().Register("John", "jon@ex.com", "John123!", "20 Main Street", "1234567890");
            var customer = new Customer();

            // Act
            var result = customer.ChangePassword(1, "John123", "John1234");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Old password is incorrect", result.Message);
        }

        [Fact]
        public void ChangePassword_NewPasswordIsWeak()
        {
            // Arrange
            Database.ClearDatabase();
            new Guest().Register("John", "jon@ex.com", "John123!", "20 Main Street", "1234567890");
            var customer = new Customer();

            // Act
            var result = customer.ChangePassword(1, "John123!", "John");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("New password is weak", result.Message);
        }

        [Fact]
        public void ChangePassword_Valid()
        {
            // Arrange
            Database.ClearDatabase();
            new Guest().Register("John", "jon@ex.com", "John123!", "20 Main Street", "1234567890");
            var customer = new Customer();

            // Act
            var result = customer.ChangePassword(1, "John123!", "John1234!");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Password changed successfully", result.Message);
        }

        [Fact]
        public void UpdateProfile_ValuesAreValid()
        {
            // Arrange
            Database.ClearDatabase();
            DataGenerator.GenerateCustomers(10);
            var customer = new Customer();

            // Act
            var result = customer.UpdateProfile(
                new Customer
                {
                    Id = 1,
                    Name = "Doe",
                    Email = "deo@ex.com",
                    Address = "22 Main Street",
                    PhoneNo = "1234567890"
                }
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Profile updated successfully", result.Message);
        }

        [Fact]
        public void UpdateProfile_ValuesAreInvalid()
        {
            // Arrange
            Database.ClearDatabase();
            DataGenerator.GenerateCustomers(10);
            var customer = new Customer();

            // Act
            var result = customer.UpdateProfile(
                new Customer
                {
                    Id = 1,
                    Name = string.Empty,
                    Email = string.Empty,
                    Address = string.Empty,
                    PhoneNo = string.Empty
                }
            );

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Invalid customer details", result.Message);
        }

        [Fact]
        public void UpdateProfile_CustomerNotFound()
        {
            // Arrange
            Database.ClearDatabase();
            DataGenerator.GenerateCustomers(10);
            var customer = new Customer();

            // Act
            var result = customer.UpdateProfile(
                new Customer
                {
                    Id = 0,
                    Name = "Doe",
                    Email = "deo@ex.com",
                    Address = "22 Main Street",
                    PhoneNo = "1234567890"
                }
            );

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Customer not found", result.Message);
        }

        [Fact]
        public void UpdateProfile_InvalidEmail()
        {
            // Arrange
            Database.ClearDatabase();
            DataGenerator.GenerateCustomers(10);
            var customer = new Customer();

            // Act
            var result = customer.UpdateProfile(
                new Customer
                {
                    Id = 1,
                    Name = "Doe",
                    Email = "deoex.com",
                    Address = "22 Main Street",
                    PhoneNo = "1234567890"
                }
            );

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Invalid email address", result.Message);
        }

        [Fact]
        public void UpdateProfile_EmailAlreadyExists()
        {
            // Arrange
            Database.ClearDatabase();
            new Guest().Register("John", "john@ex.com", "John123!", "20 Main Street", "1234567890");
            new Guest().Register(
                "John2",
                "john2@ex.com",
                "John123!",
                "22 Main Street",
                "1234567890"
            );
            var customer = new Customer();

            // Act
            var result = customer.UpdateProfile(
                new Customer
                {
                    Id = 1,
                    Name = "Doe",
                    Email = "john2@ex.com",
                    Address = "22 Main Street",
                    PhoneNo = "1234567890"
                }
            );

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("Email already exists", result.Message);
        }

        [Fact]
        public void AddToCart_ProductNotFound()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();

            // Act
            var result = customer.AddToCart(0, 10);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product not found", result.Message);
        }

        [Fact]
        public void AddToCart_QuantityIsInvalid()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();

            // Act
            var result = customer.AddToCart(1, 0);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Invalid quantity", result.Message);
        }

        [Fact]
        public void RemoveFromCart_ProductNotFound()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();
            customer.AddToCart(1, 10);

            // Act
            var result = customer.RemoveFromCart(0, 1);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product not found in cart", result.Message);
        }

        [Fact]
        public void RemoveFromCart_QuantityIsInvalid()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();
            customer.AddToCart(1, 10);

            // Act
            var result = customer.RemoveFromCart(1, 0);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Invalid quantity", result.Message);
        }

        [Fact]
        public void RemoveFromCart_QuantityIsGreaterThanCartQuantity()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();
            customer.AddToCart(1, 10);

            // Act
            var result = customer.RemoveFromCart(1, 11);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Quantity is greater than the number of products in cart", result.Message);
        }

        [Fact]
        public void RemoveFromCart_Valid()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();
            customer.AddToCart(1, 10);

            // Act
            var result = customer.RemoveFromCart(1, 5);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Product removed from cart successfully", result.Message);
        }

        [Fact]
        public void ClearCart_Valid()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();

            // Act
            var result = customer.ClearCart();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Cart cleared successfully", result.Message);
        }

        [Fact]
        public void ViewCart_Valid()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();
            customer.AddToCart(1, 10);
            customer.AddToCart(2, 5);

            // Act
            var result = customer.ViewCart();

            // Assert
            Assert.Equal(15, result.Count);
        }

        [Fact]
        public void Checkout_CartIsEmpty()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();

            // Act
            var result = customer.Checkout();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Cart is empty", result.Message);
        }

        [Fact]
        public void Checkout_TotalPriceOverLimit()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();
            customer.AddToCart(1, 1000);

            // Act
            var result = customer.Checkout();

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
            Assert.Equal("Total price is over the checkout limit", result.Message);
        }

        [Fact]
        public void Checkout_CustomerNotLoggedIn()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();
            customer.AddToCart(1, 10);

            // Act
            var result = customer.Checkout();

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("Customer not logged in", result.Message);
        }

        [Fact]
        public void Checkout_Valid()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var customer = new Customer();
            customer.Id = 1;
            customer.AddToCart(1, 10);

            // Act
            var result = customer.Checkout();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Checkout successful", result.Message);
        }
    }
}
