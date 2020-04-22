using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class GameHelper
    {
        /// <summary>
        /// 同步游服房间状态和actorid -> 匹配服
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="state"></param>
        /// <param name="roomActorId"></param>
        public static void SynRoomData(int roomId, int state, long roomActorId)
        {
            var session = NetInnerHelper.GetSessionByAppType(AppType.Match);
            GM_SynRoomData msg = GameFactory.CreateMsgGM_SynRoomData(roomId, state, roomActorId);
            session.Send(msg);
            GameFactory.RecycleMsg(msg);
        }

        /// <summary>
        /// 游戏房间玩家离开 同步->匹配服
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="userId"></param>
        public static void SynLeaveRoom(int roomId,int userId)
        {
            GM_LeaveRoom msg= GameFactory.CreateMsgGM_LeaveRoom(roomId, userId);
            var session = NetInnerHelper.GetSessionByAppType(AppType.Match);
            session.Send(msg);
            GameFactory.RecycleMsg(msg);
        }


        /// <summary>
        /// 同步玩家游戏acotrid ->gate server/user server
        /// </summary>
        /// <param name="gateSessionId"></param>
        /// <param name="userId"></param>
        /// <param name="actorId"></param>
        public static void SynActorId(long gateSessionId, int userId, long actorId,int gameId=0,int roomId=0)
        {
            GS_SynActorId msg = GameFactory.CreateMsgGS_SynActorId(userId, actorId, gameId, roomId);
            var session = NetInnerHelper.GetSessionByAppId(IdGenerater.GetAppId(gateSessionId));
            session?.Send(msg);
            session = NetInnerHelper.GetSessionByAppType(AppType.User);
            session?.Send(msg);
            GameFactory.RecycleMsg(msg);
        }
    }
}
