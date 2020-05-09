using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class BullClassicFactory
    {

        public static BullPlayerData CreatePlayerData(GamePlayerData data)
        {
            var playerData = ComponentFactory.Create<BullPlayerData>();
            playerData.Data = data;
            playerData.HandCards.Clear();
            playerData.Rate = 0;
            playerData.CardType = BullCardType.BctInvalid;
            return playerData;
        }

        public static BullRoomData CreateRoomData(GameRoomData data)
        {
            var roomData = ComponentFactory.Create<BullRoomData>();
            roomData.Data = data;
            roomData.BankerPos = -1;
            roomData.PlayerList.Clear();
            return roomData;
        }

        public static BullRoomData CreateRoomData(int roomId, RoomConfig cfg)
        {
            var roomData = GameFactory.CreateRoomData(roomId, cfg);
            return CreateRoomData(roomData);
        }

        public static BullClassicPlayer CreatePlayer(GamePlayerData player)
        {
            var data = CreatePlayerData(player);
            return ComponentFactory.Create<BullClassicPlayer, BullPlayerData>(data);
        }

        public static SC_BullPlayerEnter CreateMsgSC_BullPlayerEnter(long gateSessionId, BullPlayerData data)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullPlayerEnter>();
            msg.ActorId = gateSessionId;
            msg.Player = data;
            return msg;
        }

        public static SC_BullRoomInfo CreateMsgSC_BullRoomInfo(long gateSessionId, BullRoomData data)
        {
            var msg = SimplePool.Instance.Fetch<SC_BullRoomInfo>();
            msg.ActorId = gateSessionId;
            msg.Data = data;
            return msg;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }

    }
}
