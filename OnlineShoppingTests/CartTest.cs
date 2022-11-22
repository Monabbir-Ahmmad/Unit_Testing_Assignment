using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using OnlineShopping;
using Xunit;

namespace OnlineShoppingTests
{
    [Collection("Sequential")]
    public class CartTest
    {
        [Fact]
        public void ViewProducts_GettingExpectedProducts()
        {
            // Arrange
            DataGenerator.GenerateProducts(10);
            var cart = new Cart();
            cart.AddToCart(Database.Products[0]);
            cart.AddToCart(Database.Products[1]);

            // Act
            var result = cart.ViewProductsInCart();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void AddToCart_Valid()
        {
            // Arrange
            var cart = new Cart();

            // Act
            var response = cart.AddToCart(
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Group = "Group 1",
                    SubGroup = "SubGroup 1",
                    Price = 100
                }
            );

            // Assert
            Assert.Equal(1, cart.NumberOfProducts);
            Assert.Equal(100, cart.TotalPrice);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Product added to cart successfully", response.Message);
        }

        [Fact]
        public void AddToCart_NullInvalid()
        {
            // Arrange
            var cart = new Cart();

            // Act
            var response = cart.AddToCart(null);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Product cannot be null", response.Message);
        }

        [Fact]
        public void RemoveFromCart_Valid()
        {
            // Arrange
            var cart = new Cart();
            cart.AddToCart(
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Group = "Group 1",
                    SubGroup = "SubGroup 1",
                    Price = 100
                }
            );

            // Act
            var response = cart.RemoveFromCart(1);

            // Assert
            Assert.Equal(0, cart.NumberOfProducts);
            Assert.Equal(0, cart.TotalPrice);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Product removed from cart successfully", response.Message);
        }

        [Fact]
        public void RemoveFromCart_NotFound()
        {
            // Arrange
            var cart = new Cart();

            // Act
            var response = cart.RemoveFromCart(1);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("Product not found", response.Message);
        }

        [Fact]
        public void ClearCart_Valid()
        {
            // Arrange
            var cart = new Cart();

            // Act
            var response = cart.ClearCart();

            // Assert
            Assert.Equal(0, cart.NumberOfProducts);
            Assert.Equal(0, cart.TotalPrice);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Cart cleared successfully", response.Message);
        }
    }
}
