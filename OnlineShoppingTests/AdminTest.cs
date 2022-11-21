using OnlineShopping;

namespace OnlineShoppingTests;

public class AdminTest
{
    [Fact]
    public void ViewProducts_GettingExpectedProducts()
    {
        // Arrange
        var admin = new Admin();
        var expected = Product.Products;

        // Act
        var result = admin.ViewProducts();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Test1", "Group1", "SubGroup1", 10)]
    [InlineData("Test2", "Group2", "SubGroup2", 20)]
    [InlineData("Test3", "Group3", "SubGroup3", 30)]
    public void AddProduct_ValuesAreValid(string name, string group, string subGroup, decimal price)
    {
        // Arrange
        var admin = new Admin();

        // Act
        var result = admin.AddProduct(name, group, subGroup, price);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, result);
    }

    public void AddProduct_ValuesAreInvalid()
    {
        // Arrange
        var admin = new Admin();

        // Act
        var result = admin.AddProduct(string.Empty, string.Empty, string.Empty, 0);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, result);
    }

    [Fact]
    public void AddProduct_PriceIsInvalid()
    {
        // Arrange
        var admin = new Admin();

        // Act
        var result = admin.AddProduct("Test", "Group", "SubGroup", -1);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, result);
    }

    [Fact]
    public void AddProduct_ProductAlreadyExists()
    {
        // Arrange
        var admin = new Admin();

        admin.AddProduct("Test", "Group", "SubGroup", 10);

        // Act
        var result = admin.AddProduct("Test", "Group", "SubGroup", 10);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Conflict, result);
    }

    [Fact]
    public void ModifyProduct_Valid()
    {
        // Arrange
        var admin = new Admin();
        admin.AddProduct("Test", "Group", "SubGroup", 10);
        var product = new Product
        {
            Id = admin.ViewProducts()[0].Id,
            Name = "Product 1",
            Group = "Group 1",
            SubGroup = "SubGroup 1",
            Price = 10
        };

        // Act
        var result = admin.ModifyProduct(product);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, result);
    }

    [Fact]
    public void ModifyProduct_InvalidProductId()
    {
        // Arrange
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
        Assert.Equal(System.Net.HttpStatusCode.NotFound, result);
    }

    [Fact]
    public void ModifyProduct_InvalidProduct()
    {
        // Arrange
        var admin = new Admin();
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
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, result);
    }

    [Fact]
    public void ModifyProduct_InvalidPrice()
    {
        // Arrange
        var admin = new Admin();
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
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, result);
    }

    [Fact]
    public void ModifyProduct_ProductAlreadyExists()
    {
        // Arrange
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
        Assert.Equal(System.Net.HttpStatusCode.Conflict, result);
    }

    [Fact]
    public void DeleteProduct_InvalidProductId()
    {
        // Arrange
        var admin = new Admin();

        // Act
        var result = admin.DeleteProduct(-1);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, result);
    }

    [Fact]
    public void DeleteProduct_ValidProductId()
    {
        // Arrange
        var admin = new Admin();
        admin.AddProduct("Test", "Group", "SubGroup", 10);
        admin.AddProduct("Test0", "Group0", "SubGroup0", 10);

        // Act
        var result = admin.DeleteProduct(1);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, result);
    }

    [Fact]
    public void MakeShipmentTest()
    {
        // Arrange
        var admin = new Admin();

        // Act
        admin.MakeShipment();

        // Assert
        Assert.True(true);
    }
}
