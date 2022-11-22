using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopping
{
    public class UserBase
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
