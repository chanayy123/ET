using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    [BsonIgnoreExtraElements]
    public partial class MatchPlayer : Entity {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public long GateSessionId { get; set; }
    }

}
