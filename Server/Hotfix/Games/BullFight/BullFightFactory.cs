using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class BullFightFactory
    {

        public static BullFightPlayerData CreatePlayerData(GamePlayerData data)
        {
            var playerData = ComponentFactory.Create<BullFightPlayerData>();
            playerData.Data = data;
            playerData.HandCards.Clear();
            playerData.Rate = 0;
            playerData.CardType = BullCardType.BullCtInvalid;
            return playerData;
        }

        public static BullFightRoomData CreateRoomData(GameRoomData data)
        {
            var roomData = ComponentFactory.Create<BullFightRoomData>();
            roomData.Data = data;
            roomData.BankerPos = -1;
            roomData.PlayerList.Clear();
            return roomData;
        }

        public static BullFightRoomData CreateRoomData(int roomId, RoomConfig cfg)
        {
            var roomData = GameFactory.CreateRoomData(roomId, cfg);
            return CreateRoomData(roomData);
        }

        public static BullFightPlayer CreatePlayer(GamePlayerData player)
        {
            var data = CreatePlayerData(player);
            return ComponentFactory.Create<BullFightPlayer, BullFightPlayerData>(data);
        }

        public static SC_BullPlayerEnter CreateMsgSC_BullPlayerEnter(long gateSessionId, BullFightPlayerData data)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullPlayerEnter>();
            msg.ActorId = gateSessionId;
            msg.Player = data;
            return msg;
        }

        public static SC_BullRoomInfo CreateMsgSC_BullRoomInfo(long gateSessionId, BullFightRoomData data)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullRoomInfo>();
            msg.ActorId = gateSessionId;
            msg.Data = data;
            return msg;
        }

        public static SC_BullState CreateMsgSC_BullState(long gateSessionId, BullGameState state, List<int> param=null)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullState>();
            msg.ActorId = gateSessionId;
            msg.State = state;
            msg.Params.Clear();
            if(param != null)
            {
                msg.Params.AddRange(param);
            }
            return msg;
        }

        public static SC_BullBankerRate CreateMsgSC_BullBankerRate(long gateSessionId, int pos ,int rate)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullBankerRate>();
            msg.ActorId = gateSessionId;
            msg.Pos = pos;
            msg.Rate = rate;
            return msg;
        }

        public static SC_BullPlayerRate CreateMsgSC_BullPlayerRate(long gateSessionId, int pos, int rate)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullPlayerRate>();
            msg.ActorId = gateSessionId;
            msg.Pos = pos;
            msg.Rate = rate;
            return msg;
        }

        public static SC_BullBankerPos CreateMsgSC_BullBankerPos(long gateSessionId, int pos)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullBankerPos>();
            msg.ActorId = gateSessionId;
            msg.Pos = pos;
            return msg;
        }

        public static SC_BullCardsInfo CreateMsgSC_BullCardsInfo(long gateSessionId,int pos, RepeatedField<int> cards, BullCardType type)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullCardsInfo>();
            msg.ActorId = gateSessionId;
            msg.Pos = pos;
            msg.Cards.Clear();
            msg.Cards.AddRange(cards);
            msg.CardType = type;
            return msg;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }

    }
}
