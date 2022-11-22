using System.Net;

namespace OnlineShopping
{
    public interface IPayment
    {
        bool CheckIfCardIsSupported(CreditCard card);
        bool CheckIfCardIsValid(CreditCard card);
        bool CheckIfPayable(CreditCard card, decimal amount);
        bool CheckIfPinIsValid(CreditCard card, string pin);
        Response Pay(CreditCard card, decimal amount);
    }
}
