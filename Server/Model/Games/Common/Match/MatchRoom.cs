using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETModel
{
    public partial class MatchRoom : Entity {
        public RoomConfig Config { get; set; }
        public List<MatchPlayer> PlayerList{get;set;}
        public RoomType RoomType { get; set; }
        //对应游戏服actorid
        public long RoomActorId { get; set; }

        public override void Dispose()
        {
            if (IsDisposed) return;
            base.Dispose();
            foreach (var item in PlayerList)
            {
                item.Dispose();
            }
            PlayerList.Clear();
        }
    }

    public enum RoomType
    {
        List, //列表
        Match, //匹配
        Card //房卡
    }

    public enum RoomState
    {
        IDLE,
        WAIT,
        GAMING
    }

}
