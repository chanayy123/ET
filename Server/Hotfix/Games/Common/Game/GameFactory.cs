using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class GameFactory
    {
        public static GameRoomData CreateRoomData(int roomId, RoomConfig cfg)
        {
            var data = ComponentFactory.Create<GameRoomData>();
            data.RoomId = roomId;
            data.State = 0;
            data.GameId = cfg.GameId;
            data.GameMode = cfg.GameMode;
            data.HallType = cfg.HallType;
            return data;
        }

        public static GamePlayerData CreatePlayerData(MatchPlayer player,User user)
        {
            var data = ComponentFactory.Create<GamePlayerData>();
            data.UserId = player.UserId;
            data.GateSessionId = player.GateSessionId;
            data.Name = user.UserInfo.Name;
            data.Head = user.UserInfo.Head;
            data.Coin = user.UserInfo.Coin;
            data.Pos = -1;
            data.State = PlayerState.None;
            return data;
        }

        public static GM_LeaveRoom CreateMsgGM_LeaveRoom(int roomId,params int[] userIdList)
        {
            GM_LeaveRoom msg = SimplePool.Instance.Fetch<GM_LeaveRoom>();
            msg.RoomId = roomId;
            msg.UserIdList.Clear();
            msg.UserIdList.AddRange(userIdList);
            return msg;
        }

        public static GM_SynRoomData CreateMsgGM_SynRoomData(int roomId, int state,long roomActorId)
        {
            GM_SynRoomData msg = SimplePool.Instance.Fetch<GM_SynRoomData>();
            msg.RoomId = roomId;
            msg.State = state;
            msg.RoomActorId = roomActorId;
            return msg;
        }

        public static GS_SynActorId CreateMsgGS_SynActorId(int userId,long actorId,int gameId,int roomId)
        {
            GS_SynActorId msg = SimplePool.Instance.Fetch<GS_SynActorId>();
            msg.UserId = userId;
            msg.ActorId = actorId;
            msg.GameId = gameId;
            msg.RoomId = roomId;
            return msg;
        }

        public static SC_PlayerLeave CreateMsgSC_PlayerLeave(long gateSessionId, int userId)
        {
            var msg = SimplePool.Instance.Fetch<SC_PlayerLeave>();
            msg.ActorId = gateSessionId;
            msg.UserId = userId;
            return msg;
        }

        public static SW_UpdateUserInfo CreateMsgSW_UpdateUserInfo(int userId, string key ,string value)
        {
            var msg = SimplePool.Instance.Fetch<SW_UpdateUserInfo>();
            msg.UserId = userId;
            msg.Key = key;
            msg.Value = value;
            return msg;
        }

        public static SC_KickPlayer CreateMsgSC_KickPlayer(long gateSessionId, int reason)
        {
            var msg = SimplePool.Instance.Fetch<SC_KickPlayer>();
            msg.ActorId = gateSessionId;
            msg.Reason = reason;
            return msg;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }

    }
}
