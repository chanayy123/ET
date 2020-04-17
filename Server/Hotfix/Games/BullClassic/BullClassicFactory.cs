using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class BullClassicFactory
    {
        public static GameRoomData CreateRoomData(int roomId, int state)
        {
            return GameFactory.CreateRoomData(roomId, state);
        }

        public static GamePlayerData CreatePlayerData(MatchPlayer player, User userInfo)
        {
            return GameFactory.CreatePlayerData(player,userInfo);
        }

        public static BullClassicPlayer CreatePlayer(GamePlayerData player)
        {
            return ComponentFactory.Create<BullClassicPlayer, GamePlayerData>(player);
        }

    }
}
