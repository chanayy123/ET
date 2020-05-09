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
        public BullPlayerData PlayerData { get; set; }

        public int Online
        {
            get
            {
                return PlayerData.Data.Online;
            }
            set
            {
                PlayerData.Data.Online = value;
            }
        }
        public bool IsOnline
        {
            get
            {
                return Online == 1;
            }
        }
        public long GateSessionId
        {
            get
            {
                return PlayerData.Data.GateSessionId;
            }
            set
            {
                PlayerData.Data.GateSessionId = value;
            }
        }

        public int UserId
        {
            get
            {
                return PlayerData.Data.UserId;
            }
            set
            {
                PlayerData.Data.UserId = value;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            PlayerData.Dispose();
            PlayerData = null;
        }
    }
}
