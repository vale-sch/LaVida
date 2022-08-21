using System;
using System.Collections.Generic;
using System.Text;

namespace LaVida.Models
{
    public class Connection
    {
        public string ChatPartner { get; set; }
        public string ChatPhoneNumber { get; set; }
        public ChatType ChatType { get; set; }
        public List<string> AccountIDS { get; set; }
        public string ChatID { get; set; }
        public bool IsActive { get; set; }

    }

    public enum ChatType
    {
        PRIVATECHAT,
        GROUPCHAT
    }
}
