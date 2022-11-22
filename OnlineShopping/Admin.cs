using System.Net;

namespace OnlineShopping
{
    public class Admin : UserBase
    {
        public List<Product> ViewProducts()
        {
            // Code to view product
            return Database.Products;
        }

        public Response AddProduct(string name, string group, string subGroup, decimal price)
        {
            if (
                string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(group)
                || string.IsNullOrEmpty(subGroup)
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid product details"
                };

            if (price <= 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Message = "Invalid product price"
                };

            if (
                Database.Products.Any(
                    p => p.Name == name && p.Group == group && p.SubGroup == subGroup
                )
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "Product already exists"
                };

            // Code to add product
            Database.Products.Add(
                new Product
                {
                    Id = Database.Products.Count + 1,
                    Name = name,
                    Group = group,
                    SubGroup = subGroup,
                    Price = price
                }
            );
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product added successfully"
            };
        }

        public Response DeleteProduct(int productId)
        {
            // Code to delete product
            var product = Database.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found"
                };

            Database.Products.Remove(product);
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product deleted successfully"
            };
        }

        public Response ModifyProduct(Product product)
        {
            // Code to update product
            var index = Database.Products.FindIndex(p => p.Id == product.Id);
            if (index == -1)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found"
                };

            if (
                string.IsNullOrEmpty(product.Name)
                || string.IsNullOrEmpty(product.Group)
                || string.IsNullOrEmpty(product.SubGroup)
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid product details"
                };

            if (product.Price <= 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    Message = "Invalid product price"
                };

            if (
                Database.Products.Any(
                    p =>
                        p.Name == product.Name
                        && p.Group == product.Group
                        && p.SubGroup == product.SubGroup
                        && p.Id != product.Id
                )
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "Product already exists"
                };

            Database.Products[index] = product;
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product modified successfully"
            };
        }
    }
}
