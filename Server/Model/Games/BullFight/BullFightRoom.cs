using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using System.Threading;

namespace ETHotfix
{
    /// <summary>
    /// 经典牛牛游戏主逻辑类
    /// </summary>
    public  class BullFightRoom : AGameRoom {
        public const int MAX_CARDS = 54;
        public const int MAX_PLAYERS = 5;
        public BullFightRoomData RoomData { get; set; }
        public RoomConfig Cfg { get; set; }
        public RoomType RoomType { get; set; }
        public CancellationTokenSource StateCancelTS { get; set; }
        /// <summary>
        /// 一副卡牌数据 54张
        /// </summary>
        public List<int> CardList { get; } = new List<int>();
        /// <summary>
        /// key:玩家userid value:对应玩家对象
        /// </summary>
        public readonly Dictionary<int, BullFightPlayer> playerDic = new Dictionary<int, BullFightPlayer>();
        /// <summary>
        /// key: 玩家座位号 value:对应玩家对象
        /// </summary>
        public readonly Dictionary<int, BullFightPlayer> seatPlayerDIc = new Dictionary<int, BullFightPlayer>();

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

        public BullGameState State
        {
            get
            {
                return (BullGameState)RoomData.Data.State;
            }
            set
            {
                RoomData.Data.State = (int)value;
                Log.Debug($"游戏【{(GameId)RoomData.Data.GameId}】房间【{RoomData.Data.RoomId}】当前状态: {(BullGameState)RoomData.Data.State}");
            }
        }

        public int BankerPos
        {
            get
            {
                return RoomData.BankerPos;
            }
            set
            {
                RoomData.BankerPos= value;
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
