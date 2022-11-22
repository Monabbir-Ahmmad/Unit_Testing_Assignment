using System.Net;

namespace OnlineShopping
{
    public class Cart
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        private List<Product> _products = new List<Product>();

        public int NumberOfProducts
        {
            get { return _products.Count; }
        }

        public decimal TotalPrice
        {
            get { return _products.Sum(p => p.Price); }
        }

        public Response AddToCart(Product product)
        {
            if (product == null)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Product cannot be null"
                };
            }

            _products.Add(product);
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product added to cart successfully"
            };
        }

        public Response RemoveFromCart(int productId)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found"
                };

            _products.Remove(product);
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product removed from cart successfully"
            };
        }

        public List<Product> ViewProductsInCart()
        {
            return _products;
        }

        public Response ClearCart()
        {
            _products.Clear();
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Cart cleared successfully"
            };
        }
    }
}
