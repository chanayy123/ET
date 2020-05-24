using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
namespace ETModel
{
    [BsonIgnoreExtraElements]
    public class LogCoinChange : Entity
    {
        public int UserId { get; set; }
        public int ChangeCoin  { get; set; }
        public int BeforeCoin { get; set; }
        public int NowCoin { get; set; }
        public string Operate { get; set; }
        public DateTime ChangeTime { get; set; }

    }
}
