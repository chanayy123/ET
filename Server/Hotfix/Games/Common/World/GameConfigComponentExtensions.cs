using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class GameConfigComponentExtensions
    {
        public static async ETTask FetchGameCfgList(this GameConfigComponent self)
        {
            List<ComponentWithId> list = await self.DBProxy.Query<GameConfig>((cfg) => true);
            if (list.Count == 0)//数据库没有就把本地配置文件写入
            {
                List<ComponentWithId> tmpList = new List<ComponentWithId>();
                IConfig[] _list = Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(RoomConfig));
                Array.ForEach(_list, (cfg) =>
                {
                    var gameCfg = WorldFactory.CreateGameCfg(cfg as RoomConfig);
                    self.cfgList.Add(gameCfg);
                    tmpList.Add(gameCfg);
                });
                await self.DBProxy.SaveBatch(tmpList);
            }
            else
            {
                //列表里的每一项都是收到其他进程消息 直接构造出来的,没有从组件工厂创建,dispose也没有效果,可以不管生命周期,交给垃圾回收
                self.cfgList.Clear();
                list.ForEach((cfg) =>
                {
                    self.cfgList.Add(cfg as GameConfig);
                });
            }
        }
    }
}
