using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ETHotfix;

namespace ETModel
{

    /// <summary>
    /// 游戏配置管理类
    /// </summary>
    public class GameConfigComponent : Singleton<GameConfigComponent>
    {
        public readonly List<GameConfig> cfgList = new List<GameConfig>();
        public DBProxyComponent DBProxy { get; private set; }
        public void Awake()
        {
            DBProxy = Game.Scene.GetComponent<DBProxyComponent>();

        }
    }
}
