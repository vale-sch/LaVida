using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaVida.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountID { get; set; }
        public bool HasToRefreshConnections { get; set; }
        public List< Connection> Connections { get; set; }

    }
}
