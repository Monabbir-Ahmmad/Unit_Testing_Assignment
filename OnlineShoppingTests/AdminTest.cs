using System.Net;
using OnlineShopping;
using Xunit;

namespace OnlineShoppingTests
{
    [Collection("Sequential")]
    public class AdminTest
    {
        [Fact]
        public void ViewProducts_GettingExpectedProducts()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            admin.AddProduct("Product 1", "Group 1", "SubGroup 1", 100);
            admin.AddProduct("Product 2", "Group 2", "SubGroup 2", 200);
            var expected = Database.Products;

            // Act
            var result = admin.ViewProducts();

            // Assert
            Assert.Equal(expected.Count, result.Count);
        }

        [Fact]
        public void AddProduct_ValuesAreValid()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            // Act
            var result = admin.AddProduct("Test", "Group", "SubGroup", 10);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Product added successfully", result.Message);
        }

        [Fact]
        public void AddProduct_ValuesAreInvalid()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            admin.AddProduct("Test", "Group", "SubGroup", 10);

            // Act
            var result = admin.AddProduct(string.Empty, string.Empty, string.Empty, 0);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Invalid product details", result.Message);
        }

        [Fact]
        public void AddProduct_PriceIsInvalid()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            admin.AddProduct("Test", "Group", "SubGroup", 10);

            // Act
            var result = admin.AddProduct("Test", "Group", "SubGroup", -1);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
            Assert.Equal("Invalid product price", result.Message);
        }

        [Fact]
        public void AddProduct_ProductAlreadyExists()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            admin.AddProduct("Test", "Group", "SubGroup", 10);

            // Act
            var result = admin.AddProduct("Test", "Group", "SubGroup", 10);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("Product already exists", result.Message);
        }

        [Fact]
        public void ModifyProduct_Valid()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            admin.AddProduct("Test", "Group", "SubGroup", 10);
            var product = new Product
            {
                Id = 1,
                Name = "Product 1",
                Group = "Group 1",
                SubGroup = "SubGroup 1",
                Price = 10
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Product modified successfully", result.Message);
        }

        [Fact]
        public void ModifyProduct_InvalidProductId()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            var product = new Product
            {
                Id = -1,
                Name = "Product 1",
                Group = "Group 1",
                SubGroup = "SubGroup 1"
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product not found", result.Message);
        }

        [Fact]
        public void ModifyProduct_InvalidProduct()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            admin.AddProduct("Test", "Group", "SubGroup", 10);
            var product = new Product
            {
                Id = 1,
                Name = string.Empty,
                Group = string.Empty,
                SubGroup = string.Empty,
                Price = 10
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Invalid product details", result.Message);
        }

        [Fact]
        public void ModifyProduct_InvalidPrice()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            admin.AddProduct("Test", "Group", "SubGroup", 10);
            var product = new Product
            {
                Id = 1,
                Name = "Product 1",
                Group = "Group 1",
                SubGroup = "SubGroup 1",
                Price = 0
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
            Assert.Equal("Invalid product price", result.Message);
        }

        [Fact]
        public void ModifyProduct_ProductAlreadyExists()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            admin.AddProduct("Test", "Group", "SubGroup", 10);
            admin.AddProduct("Test1", "Group1", "SubGroup1", 10);
            var product = new Product
            {
                Id = 1,
                Name = "Test1",
                Group = "Group1",
                SubGroup = "SubGroup1",
                Price = 10
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("Product already exists", result.Message);
        }

        [Fact]
        public void DeleteProduct_InvalidProductId()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            // Act
            var result = admin.DeleteProduct(-1);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product not found", result.Message);
        }

        [Fact]
        public void DeleteProduct_ValidProductId()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            admin.AddProduct("Test", "Group", "SubGroup", 10);
            admin.AddProduct("Test0", "Group0", "SubGroup0", 10);

            // Act
            var result = admin.DeleteProduct(1);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Product deleted successfully", result.Message);
        }
    }
}
