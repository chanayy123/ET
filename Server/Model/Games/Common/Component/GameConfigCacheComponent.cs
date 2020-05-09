using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public class GameConfigCacheComponent : Component
    {
        public static GameConfigCacheComponent Instance { get; private set; }
        public readonly Dictionary<long, GameConfig> gameConfigDic = new Dictionary<long, GameConfig>();

        public void Awake()
        {
            Instance = this;
        }
        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            foreach (var item in gameConfigDic)
            {
                item.Value.Dispose();
            }
            gameConfigDic.Clear();
        }
    }
}
