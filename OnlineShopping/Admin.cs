using System.Net;

namespace OnlineShopping
{
    public class Admin : UserBase
    {
        public List<Product> ViewProducts()
        {
            // Code to view product
            return Product.Products;
        }

        public HttpStatusCode AddProduct(string name, string group, string subGroup, decimal price)
        {
            if (
                string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(group)
                || string.IsNullOrEmpty(subGroup)
            )
                return HttpStatusCode.BadRequest;

            if (price <= 0)
                return HttpStatusCode.Forbidden;

            if (
                Product.Products.Any(
                    p => p.Name == name && p.Group == group && p.SubGroup == subGroup
                )
            )
                return HttpStatusCode.Conflict;

            // Code to add product

            Product.Products.Add(
                new Product
                {
                    Id = Product.Products.Count + 1,
                    Name = name,
                    Group = group,
                    SubGroup = subGroup,
                    Price = price
                }
            );
            return HttpStatusCode.OK;
        }

        public HttpStatusCode DeleteProduct(int productId)
        {
            // Code to delete product
            var product = Product.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return HttpStatusCode.NotFound;

            Product.Products.Remove(product);
            return HttpStatusCode.OK;
        }

        public HttpStatusCode ModifyProduct(Product product)
        {
            // Code to update product
            var index = Product.Products.FindIndex(p => p.Id == product.Id);
            if (index == -1)
                return HttpStatusCode.NotFound;

            if (
                string.IsNullOrEmpty(product.Name)
                || string.IsNullOrEmpty(product.Group)
                || string.IsNullOrEmpty(product.SubGroup)
            )
                return HttpStatusCode.BadRequest;

            if (product.Price <= 0)
                return HttpStatusCode.Forbidden;

            if (
                Product.Products.Any(
                    p =>
                        p.Name == product.Name
                        && p.Group == product.Group
                        && p.SubGroup == product.SubGroup
                        && p.Id != product.Id
                )
            )
                return HttpStatusCode.Conflict;

            Product.Products[index] = product;
            return HttpStatusCode.OK;
        }

        public void MakeShipment()
        {
            // Code to make shipment
        }

        public void ConfirmDelivery()
        {
            // Code to confirm delivery
        }
    }
}
