using System.Net;
using OnlineShopping;
using Xunit;

namespace OnlineShoppingTests
{
    [Collection("Sequential")]
    public class GuestTest
    {
        [Fact]
        public void ViewProducts_GettingExpectedProducts()
        {
            // Arrange
            Database.ClearDatabase();
            var guest = new Guest();
            var expected = Database.Products;

            // Act
            var actual = guest.ViewProducts();

            // Assert
            Assert.Equal(expected.Count, actual.Count);
        }

        [Theory]
        [InlineData("John", "john@ex.com", "John123!", "20 Main Street", "1234567890")]
        [InlineData("Ohn", "ohn@ex.com", "John123!", "20 Main Street", "1234567890")]
        public void Register_ValuesAreValid(
            string name,
            string email,
            string password,
            string address,
            string phoneNo
        )
        {
            // Arrange
            Database.ClearDatabase();
            var guest = new Guest();

            // Act
            var result = guest.Register(name, email, password, address, phoneNo);
            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Customer registered successfully", result.Message);
        }

        [Fact]
        public void Register_ValuesAreInvalid()
        {
            // Arrange
            Database.ClearDatabase();
            var guest = new Guest();

            // Act
            var result = guest.Register(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            );

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Invalid customer details", result.Message);
        }

        [Fact]
        public void Register_EmailExists()
        {
            // Arrange
            Database.ClearDatabase();
            var guest = new Guest();
            guest.Register("John Doe", "john@ex.com", "John123!", "20 Main Street", "1234567890");

            // Act
            var result = guest.Register(
                "John Doe",
                "john@ex.com",
                "John123!",
                "20 Main Street",
                "1234567890"
            );

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("Email already exists", result.Message);
        }

        [Fact]
        public void Register_PasswordIsWeak()
        {
            // Arrange
            Database.ClearDatabase();
            var guest = new Guest();

            // Act
            var result = guest.Register(
                "John Doe",
                "john@ex.com",
                "John",
                "20 Main Street",
                "1234567890"
            );

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Password is weak", result.Message);
        }

        [Fact]
        public void Register_InvalidEmail()
        {
            // Arrange
            Database.ClearDatabase();
            var guest = new Guest();

            // Act
            var result = guest.Register(
                "John Doe",
                "johnex.com",
                "John123!",
                "20 Main Street",
                "1234567890"
            );

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Invalid email address", result.Message);
        }
    }
}
