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
        public readonly Dictionary<int, BullClassicPlayer> playerDic = new Dictionary<int, BullClassicPlayer>();

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
