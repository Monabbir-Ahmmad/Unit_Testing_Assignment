using System.Net;

namespace OnlineShopping
{
    public abstract class Payment
    {
        public abstract HttpStatusCode Pay(CreditCard card, string pin, decimal amount);
    }
}
