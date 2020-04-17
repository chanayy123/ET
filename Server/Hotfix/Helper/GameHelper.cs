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
            session.Send(new GM_SynRoomData()
            {
                RoomId = roomId,
                State = state,
                RoomActorId = roomActorId
            });
        }
        /// <summary>
        /// 同步玩家游戏acotrid -> 网关服
        /// </summary>
        /// <param name="gateSessionId"></param>
        /// <param name="userId"></param>
        /// <param name="actorId"></param>
        public static void SynActorId(long gateSessionId, int userId, long actorId)
        {
            var session = NetInnerHelper.GetSessionByAppId(IdGenerater.GetAppId(gateSessionId));
            session.Send(new GG_SynActorId()
            {
                UserId = userId,
                ActorId = actorId
            });
        }
    }
}
