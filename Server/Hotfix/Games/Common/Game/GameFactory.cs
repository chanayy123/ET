using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class GameFactory
    {
        public static GameRoomData CreateRoomData(int roomId, int state)
        {
            var data = ComponentFactory.Create<GameRoomData>();
            data.RoomId = roomId;
            data.State = state;
            data.PlayerList.Clear();
            return data;
        }

        public static GamePlayerData CreatePlayerData(MatchPlayer player, User userInfo)
        {
            var data = ComponentFactory.Create<GamePlayerData>();
            data.UserId = player.UserId;
            data.GateSessionId = player.GateSessionId;
            data.Name = userInfo.Name;
            data.Head = userInfo.Head;
            return data;
        }

    }
}
