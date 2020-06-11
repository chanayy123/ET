using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class MatchHelper
    {
        /// <summary>
        /// 随机获取一个游戏服务器地址
        /// </summary>
        /// <returns></returns>
        public static  Session RandomGameSession
        {
            get
            {
                var gameCfgs = StartConfigComponent.Instance.GameConfigs;
                var index = RandomHelper.RandomNumber(0, gameCfgs.Count);
                return NetInnerHelper.GetSessionByCfg(gameCfgs[index]);
            }
        }
        /// <summary>
        /// 返回玩家创建房间随机roomid(在固定范围且唯一)
        /// </summary>
        public static int RandomRoomId
        {
            get
            {
                var matchMgr = Game.Scene.GetComponent<MatchRoomComponent>();
                var roomId = 0;
                while (true)
                {
                    roomId = RandomHelper.RandomNumber(10000, 100000);
                    if (!matchMgr.IsRoomExist(roomId)) return roomId;
                }
            }
        }

        public static async ETTask<List<UserInfo>> CallRobot(int roomId, int count)
        {
            MR_CallRobot msg = MatchFactory.CreateMsgMR_CallRobot(roomId,count);
            var robotSession = NetInnerHelper.GetSessionByAppType(AppType.Robot);
            RM_CallRobot response = (RM_CallRobot)await robotSession.Call(msg);
            MatchFactory.RecycleMsg(msg);
            return response.List;
        }

        public static void ReturnRobot(params int[] userIdList)
        {
            MR_ReturnRobot msg = MatchFactory.CreateMsgMR_ReturnRobot(userIdList);
            var robotSession = NetInnerHelper.GetSessionByAppType(AppType.Robot);
            robotSession.Send(msg);
            MatchFactory.RecycleMsg(msg);
        }

    }
}
