using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class AccountInfo : Entity
    {
        public string Acc { get; set; }
        public string Pwd { get; set; }
        public int UserId { get; set; }
        public DateTime LastLoginTIme { get; set; }

    }
}
