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

    }
}
