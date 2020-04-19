using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class MatchFactory
    {
        public static MatchPlayer CreateMatchPlayer(int userId, int roomId, long sessionId)
        {
            var player = ComponentFactory.Create<MatchPlayer, int,int, long>(userId,roomId, sessionId);
            return player;
        }

        public static HallPlayer CreateHallPlayer(int userId, long sessionId)
        {
            var player = ComponentFactory.Create<HallPlayer>();
            player.UserId = userId;
            player.GateSessionId = sessionId;
            player.HallId = 0;
            return player;
        }


    }
}
