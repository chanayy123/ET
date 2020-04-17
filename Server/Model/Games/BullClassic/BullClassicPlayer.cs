using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    /// <summary>
    /// 经典牛牛游戏玩家类
    /// </summary>
    public  class BullClassicPlayer : AGamePlayer {
        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            PlayerData.Dispose();
            PlayerData = null;
        }
    }
}
