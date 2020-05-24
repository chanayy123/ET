using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class WorldHelper
    {
        public static async ETTask<User> GetUserInfo(int userId)
        {
            var userS = NetInnerHelper.GetSessionByAppType(AppType.World);
            SW_GetUserInfo msg = WorldFactory.CreateMsgRU_Login(userId);
            var usRes = (WS_GetUserInfo)await userS.Call(msg);
            WorldFactory.RecycleMsg(msg);
            return usRes.UserInfo;
        }
        /// <summary>
        /// 尝试trycount次数获取游戏配置,如果还是为空就返回null
        /// </summary>
        /// <param name="tryCount"></param>
        /// <returns></returns>
        public static async ETTask<List<GameConfig>> GetGameCfgList(int tryCount=3)
        {
            while(tryCount-- > 0)
            {
                var wSession = NetInnerHelper.GetSessionByAppType(AppType.World);
                SW_GetGameCfgList msg = WorldFactory.CreateMsgSW_GetGameCfgList();
                var wRes = (WS_GetGameCfgList)await wSession.Call(msg);
                WorldFactory.RecycleMsg(msg);
                if (wRes.GameCfgList.Count > 0)
                {
                    return wRes.GameCfgList;
                }
                else
                {
                    await TimerComponent.Instance.WaitAsync(1000);
                }
            }
            return null;
        }

        /// <summary>
        /// 同步玩家游戏acotrid ->gate server
        /// </summary>
        /// <param name="gateSessionId"></param>
        /// <param name="userId"></param>
        /// <param name="actorId"></param>
        public static void SynActorId(long gateSessionId, int userId, long actorId, int gameId = 0, int roomId = 0)
        {
            GS_SynActorId msg = GameFactory.CreateMsgGS_SynActorId(userId, actorId, gameId, roomId);
            var session = NetInnerHelper.GetSessionByAppId(IdGenerater.GetAppId(gateSessionId));
            session?.Send(msg); 
            GameFactory.RecycleMsg(msg);
        }


    }
}
