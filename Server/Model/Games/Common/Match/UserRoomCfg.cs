using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    /// <summary>
    /// 玩家创建的房间配置
    /// </summary>
    public  class UserRoomCfg : Entity {
        public int RoomId { get; set; }
        public int CreateUserId { get; set; }
        public int GameId { get; set; }
        public int GameMode { get; set; }
        public int HallType { get; set; }
        public string Params { get; set; }
    }

}
