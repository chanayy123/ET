using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    public partial class MatchPlayer : Entity {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public int HallId { get; set; }
        public long GateSessionId { get; set; }
    }

}
