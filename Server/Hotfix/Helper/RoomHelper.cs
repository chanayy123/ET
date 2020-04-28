using System;
using System.Reflection;
using ETModel;
namespace ETHotfix
{
	public static class RoomHelper
    {
        public const int BaseId = 100000;
        public const int SubId = 1000;
        /// <summary>
        /// 房间ID生成规则: GameId*100000 +  模式*1000 + 房间索引
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
		public static RoomConfig  GetRoomCfg(int roomId)
		{
            var gameId = roomId / BaseId;
            var mode = (roomId - gameId * BaseId) / SubId;
            var temId = gameId * BaseId+mode*SubId;
            Game.Scene.GetComponent<MatchRoomComponent>().GetRoomCfg(temId, out RoomConfig cfg);
            return cfg;
		}

        public static RoomConfig GetRoomCfg(int gameId,int mode)
        {
            var temId = gameId * BaseId + mode * SubId;
            Game.Scene.GetComponent<MatchRoomComponent>().GetRoomCfg(temId, out RoomConfig cfg);
            return cfg;
        }

        public static int GetGameId(int roomId)
        {
            return roomId / BaseId;
        }

        public static int GetMode(int roomId)
        {
            var gameId = roomId / BaseId;
            var mode = (roomId - gameId * BaseId) / SubId;
            return mode;
        }

	}
}
