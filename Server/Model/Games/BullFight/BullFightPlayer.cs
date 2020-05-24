using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETModel
{
    /// <summary>
    /// 经典牛牛游戏玩家类
    /// </summary>
    public  class BullFightPlayer : AGamePlayer {
        public BullFightPlayerData PlayerData { get; set; }

        public PlayerState State
        {
            get
            {
                return PlayerData.Data.State;
            }
            set
            {
                PlayerData.Data.State = value;
            }
        }
        public bool IsOnline
        {
            get
            {
                return (PlayerData.Data.State & PlayerState.Online) !=PlayerState.None;
            }
            set
            {
                if (value)
                {
                    PlayerData.Data.State |= PlayerState.Online;
                }
                else
                {
                    PlayerData.Data.State &= ~PlayerState.Online;
                }
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

        public int Pos
        {
            get
            {
                return PlayerData.Data.Pos;
            }
            set
            {
                PlayerData.Data.Pos = value;
            }
        }

        public int Rate
        {
            get
            {
                return PlayerData.Rate;
            }
            set
            {
                PlayerData.Rate = value;
            }
        }

        public RepeatedField<int> HandCards
        {
            get
            {
                return PlayerData.HandCards;
            }
        }

        public BullCardType CardType
        {
            get
            {
                return PlayerData.CardType;
            }
            set
            {
                PlayerData.CardType = value;
            }
        }

        public int Coin
        {
            get
            {
                return PlayerData.Data.Coin;
            }
            set
            {
                PlayerData.Data.Coin = value;
            }
        }

        public bool IsShowCard
        {
            get
            {
                return CardType != BullCardType.BullCtInvalid;
            }
        }

        public BullFightRoom Room
        {
            get
            {
                return this.GetParent<BullFightRoom>();
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
