using System;
using System.Collections.Generic;
using System.Text;

namespace LaVida.Models
{
    public class Account
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string [] Connections { get; set; }
    }
}
