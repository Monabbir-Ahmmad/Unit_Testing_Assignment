using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class CreditCard
    {
        public string CardNumber { get; set; }

        public string CardType { get; set; }

        public string CardHolderName { get; set; }

        public string PIN { get; set; }

        public decimal Balance { get; set; }
    }
}
