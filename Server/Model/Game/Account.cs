using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class Account : Entity
    {
        public string Acc { get; set; }
        public string Pwd { get; set; }
        public int UserId { get; set; }
        public string LastLoginTIme { get; set; }

    }
}
