using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    /// <summary>
    /// 玩家创建的房间
    /// </summary>
    public  class UserRoom : Entity {
        public int RoomId { get; set; }
        public int CreateUserId { get; set; }
        public int GameId { get; set; }
        public int GameMode { get; set; }
        public string Params { get; set; }
    }

}
