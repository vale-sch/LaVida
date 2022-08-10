using System;
using System.Collections.Generic;
using System.Text;

namespace LaVida.Models
{
    public class Connection
    {
        public string chatPartner;
        public ChatType chatType;
        public string chatId;
    }
    public enum ChatType
    {
        PRIVATECHAT,
        GROUPCHAT
    }
}
