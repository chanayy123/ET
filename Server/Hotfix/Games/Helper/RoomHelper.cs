using System;
using System.Reflection;
using ETModel;
namespace ETHotfix
{
	public static class RoomHelper
    {
        public const int BaseId = 10000000;
        public const int SubId = 100000;
        public const int Sub2Id = 1000;
        /// <summary>
        /// 房间ID生成规则: GameId*10000000 +  模式*100000 + 大厅类型*1000 + 房间索引
        /// 每种具体类型的房间最多1000个,后面不够用了再扩展
        /// </summary>
        /// <param name="roomId"></param>不包括玩家自建房间id
        /// <returns></returns>
		public static RoomConfig GetRoomCfg(int roomId)
        {
            var gameId = roomId / BaseId;
            var mode = (roomId - gameId * BaseId) / SubId;
            var hallType = (roomId - gameId * BaseId - mode * SubId) / Sub2Id;
            var hallId = gameId * BaseId + mode * SubId+hallType*Sub2Id;
            var cfg = RoomConfigComponent.Instance.Get(hallId);
            return cfg;
        }

        public static RoomConfig GetRoomCfg(int gameId,int mode,int hallType)
        {
            var hallId = gameId * BaseId + mode * SubId+hallType*Sub2Id;
            var cfg = RoomConfigComponent.Instance.Get(hallId);
            return cfg;
        }

        public static int GetGameId(int hallId)
        {
            return hallId / BaseId;
        }

        public static int GetGameMode(int hallId)
        {
            var gameId = hallId / BaseId;
            var mode = (hallId - gameId * BaseId) / SubId;
            return mode;
        }

        public static int GetHallType(int hallId)
        {
            var gameId = hallId / BaseId;
            var mode = (hallId - gameId * BaseId) / SubId;
            var hallType = (hallId - gameId * BaseId - mode * SubId) / Sub2Id;
            return hallType;
        }

	}
}
