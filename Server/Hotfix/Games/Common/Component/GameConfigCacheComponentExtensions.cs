using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{

    public static class GameConfigCacheComponentExtensions
    {
        public static async ETTask<GameConfig> GetAsync(this GameConfigCacheComponent self,int hallId)
        {
            if(self.gameConfigDic.TryGetValue(hallId, out GameConfig cfg))
            {
                return cfg;
            }
            List<GameConfig> cfgList = await WorldHelper.GetGameCfgList();
            if(cfgList == null)
            {
                Log.Warning("GameConfigCacheComponent 获取游戏配置失败!");
                return null;
            }
            cfgList.ForEach((gameCfg) =>
            {
                self.gameConfigDic.Add(gameCfg.HallId, gameCfg);
            });
            self.gameConfigDic.TryGetValue(hallId, out cfg);
            return cfg;
        }

        public static async ETTask GetAllAsync(this GameConfigCacheComponent self)
        {
            if(self.gameConfigDic.Count == 0)
            {
                List<GameConfig> cfgList = await WorldHelper.GetGameCfgList();
                if (cfgList == null)
                {
                    Log.Warning("GameConfigCacheComponent 获取游戏配置失败!");
                    return;
                }
                cfgList.ForEach((gameCfg) =>
                {
                    self.gameConfigDic.Add(gameCfg.HallId, gameCfg);
                });
            }
        }

        public static bool IsHallOpen(this GameConfigCacheComponent self,long hallId)
        {
            self.gameConfigDic.TryGetValue(hallId, out GameConfig cfg);
            return cfg != null && cfg.State == 1;
        }

        public static GameConfig Get(this GameConfigCacheComponent self,int hallId)
        {
            self.gameConfigDic.TryGetValue(hallId, out GameConfig cfg);
            return cfg;
        }
    }
}
