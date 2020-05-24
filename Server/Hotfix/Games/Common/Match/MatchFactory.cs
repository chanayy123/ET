using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class MatchFactory
    {
        /// <summary>
        /// 默认列表模式房间列表数量
        /// </summary>
        public const int DEFAULT_LISTMODE_COUNT = 50;
        /// <summary>
        /// 大厅id:当前匹配房间索引
        /// </summary>
        public static readonly Dictionary<int,int> matchIndexDic = new Dictionary<int,int>();

        public static MatchPlayer CreateMatchPlayer(int userId, int roomId, long sessionId,int hallId=0)
        {
            var player = ComponentFactory.Create<MatchPlayer, int, int, long>(userId, roomId, sessionId);
            player.HallId = hallId;
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

        public static MatchRoom CreateListModeRoom(int roomId, RoomConfig cfg)
        {
            var room = ComponentFactory.Create<MatchRoom, int, RoomConfig>(roomId, cfg);
            room.RoomType = RoomType.List;
            return room;
        }
        /// <summary>
        /// 创建匹配房间: 索引从列表模式房间数量往下累加
        /// </summary>
        /// <param name="hallId"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public static MatchRoom CreateMatchModeRoom(int hallId, RoomConfig cfg)
        {
            if (!matchIndexDic.TryGetValue(hallId, out int index))
            {
                index = DEFAULT_LISTMODE_COUNT+1;
                matchIndexDic[hallId] = index;
            }
            else
            {
                ++matchIndexDic[hallId];
            }
            var roomId = hallId+ matchIndexDic[hallId];
            var room = ComponentFactory.Create<MatchRoom, int, RoomConfig>(roomId, cfg);
            room.RoomType = RoomType.Match;
            return room;
        }

        public static MatchRoom CreateCardModeRoom(int roomId,int gameId,int mode,int hallType,string otherParams="")
        {
            var cfg = RoomHelper.GetRoomCfg(gameId,mode,hallType);
            var room = ComponentFactory.Create<MatchRoom,int,RoomConfig>(roomId,cfg);
            room.RoomType = RoomType.Card;
            return room;
        }

        public static UserRoomCfg CreateUserRoomCfg(int roomId,int userId,int gameId, int gameMode,int hallType, string gameParams)
        {
            UserRoomCfg room = ComponentFactory.Create<UserRoomCfg>();
            room.RoomId = roomId;
            room.CreateUserId = userId;
            room.GameId = gameId;
            room.GameMode = gameMode;
            room.HallType = hallType;
            room.Params = gameParams;
            return room;
        }

        

        public static MG_EnterRoom CreateMsgMG_EnterRoom(GamePlayerData player,int roomId,int gameId,int gameMode,int hallType)
        {
            var msg = SimplePool.Instance.Fetch<MG_EnterRoom>();
            msg.Player = player;
            msg.RoomId = roomId;
            msg.GameId = gameId;
            msg.GameMode = gameMode;
            msg.HallType = hallType;
            return msg;
        }

        public static MG_MatchRoom CreateMsgMG_MatchRoom(int roomId, int gameId,int gameMode,int hallType, List<GamePlayerData> list)
        {
            MG_MatchRoom msg = SimplePool.Instance.Fetch<MG_MatchRoom>();
            msg.GameId = gameId;
            msg.RoomId = roomId;
            msg.GameMode = gameMode;
            msg.HallType = hallType;
            msg.PlayerList = list;
            return msg;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }



    }
}
