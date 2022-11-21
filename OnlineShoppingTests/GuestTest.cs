using OnlineShopping;
using Xunit;

namespace OnlineShoppingTests
{
    public class GuestTest
    {
        [Fact]
        public void ViewProducts_GettingExpectedProducts()
        {
            // Arrange
            var guest = new Guest();
            var expected = Product.Products;

            // Act
            var actual = guest.ViewProducts();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("John", "John123!", "20 Main Street", "1234567890")]
        [InlineData("Mary", "Mary123!", "21 Main Street", "1234567891")]
        [InlineData("Peter", "Peter123!", "22 Main Street", "1234567892")]
        public void Register_ValuesAreValid(
            string name,
            string password,
            string address,
            string phoneNo
        )
        {
            // Arrange
            var guest = new Guest();

            // Act
            var result = guest.Register(name, password, address, phoneNo);
            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, result);
        }

        [Fact]
        public void Register_ValuesAreInvalid()
        {
            // Arrange
            var guest = new Guest();

            // Act
            var result = guest.Register(string.Empty, string.Empty, string.Empty, string.Empty);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result);
        }

        [Fact]
        public void Register_NameIsTaken()
        {
            // Arrange
            var guest = new Guest();
            guest.Register("John Doe", "John123!", "20 Main Street", "1234567890");

            // Act
            var result = guest.Register("John Doe", "John0123!", "21 Main Street", "1234567890");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Conflict, result);
        }

        [Fact]
        public void Register_PasswordIsWeak()
        {
            // Arrange
            var guest = new Guest();

            // Act
            var result = guest.Register("Doe", "John", "21 Main Street", "1234567890");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result);
        }
    }
}
