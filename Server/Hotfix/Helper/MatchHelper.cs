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
                    if (!matchMgr.IsRoomExist(roomId) || matchMgr.IsCreateRoomIdValid(roomId)) return roomId;
                }
            }
        }

    }
}
