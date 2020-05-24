using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class WorldFactory
    {
        public static SW_GetUserInfo CreateMsgRU_Login(int userId)
        {
            SW_GetUserInfo msg = SimplePool.Instance.Fetch<SW_GetUserInfo>();
            msg.UserId = userId;
            return msg;
        }

        public static SW_GetGameCfgList CreateMsgSW_GetGameCfgList()
        {
            SW_GetGameCfgList msg = SimplePool.Instance.Fetch<SW_GetGameCfgList>();
            return msg;
        }

        public static SC_PlayerData CreateMsgSC_PlayerData(User user,List<GameConfig> list)
        {
            SC_PlayerData msg = SimplePool.Instance.Fetch<SC_PlayerData>();
            msg.ActorId = user.GateSessionId;
            msg.GameId = user.GameId;
            msg.RoomId = user.RoomId;
            msg.UserInfo = user.UserInfo;
            msg.GameCfgList.Clear();
            msg.GameCfgList.AddRange(list);
            return msg;
        }

        public static GameConfig CreateGameCfg(RoomConfig cfg)
        {
            var gameCfg = ComponentFactory.Create<GameConfig>();
            gameCfg.GameId = cfg.GameId;
            gameCfg.GameMode = cfg.GameMode;
            gameCfg.MinLimitCoin = cfg.MinLimitCoin;
            gameCfg.MaxLimitCoin = cfg.MaxLimitCoin;
            gameCfg.HallId = cfg.Id;
            gameCfg.State = 1; //默认开启
            gameCfg.RoomType = (int)RoomType.Match; //默认匹配场
            return gameCfg;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }
    }
}
