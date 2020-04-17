using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    public partial class GamePlayerData : Component
    {
        public long GateSessionId { get; set; }
    }
}
