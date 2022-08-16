using System;
using System.Collections.Generic;
using System.Text;

namespace LaVida.Models
{
    public class Connection
    {
        public string ChatPartner { get; set; }

        public ChatType ChatType { get; set; }

        public string ChatID { get; set; }

    }

    public enum ChatType
    {
        PRIVATECHAT,
        GROUPCHAT
    }
}
