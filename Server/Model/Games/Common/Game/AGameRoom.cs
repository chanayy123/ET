using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    /// <summary>
    /// 游戏房间主逻辑基类
    /// </summary>
    public abstract class AGameRoom : Entity {
        public virtual GameRoomData RoomData { get; set; }
    }

}
