using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public abstract class Payment
    {
        public int CustomerId { get; set; }
        public string CardHolderName { get; set; }
        public decimal Amount { get; set; }

        private string _cardNo;
        private string _cardType;

        public HttpStatusCode SetCardType(string type)
        {
            if (type == "Visa" || type == "MasterCard" || type == "AmericanExpress")
            {
                _cardType = type;
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }

        public HttpStatusCode SetCardNo(string cardNo)
        {
            if (cardNo.Length == 16)
            {
                _cardNo = cardNo;
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }

        public abstract decimal CheckBalance();

        public abstract HttpStatusCode Pay(decimal amount);
    }
}
