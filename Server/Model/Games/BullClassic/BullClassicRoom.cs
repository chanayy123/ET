using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    /// <summary>
    /// 经典牛牛游戏主逻辑类
    /// </summary>
    public  class BullClassicRoom : AGameRoom {
        public BullRoomData RoomData { get; set; }
        public RoomConfig Cfg { get; set; }
        public readonly Dictionary<int, BullClassicPlayer> playerDic = new Dictionary<int, BullClassicPlayer>();


        public int RoomId
        {
            get
            {
                return RoomData.Data.RoomId;
            }
            set
            {
                RoomData.Data.RoomId = value;
            }
        }

        public int State
        {
            get
            {
                return RoomData.Data.State;
            }
            set
            {
                RoomData.Data.State = value;
            }
        }


        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            RoomData.Dispose();
            RoomData = null;
            foreach (var item in playerDic)
            {
                item.Value.Dispose();
            }
            playerDic.Clear();
        }
    }
}
