using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    class GameConfigCacheComponentSystem : AwakeSystem<GameConfigCacheComponent>
    {
        public override void Awake(GameConfigCacheComponent self)
        {
            self.Awake();
        }
    }
    /// <summary>
    /// 匹配服需要用到游戏配置
    /// </summary>
    [ObjectSystem]
    public class GameConfigCacheComponentSystem2 : AwakeSystem<MatchRoomComponent>
    {
        public override void Awake(MatchRoomComponent self)
        {
            if (Game.Scene.GetComponent<GameConfigCacheComponent>() == null)
            {
                Game.Scene.AddComponent<GameConfigCacheComponent>();
            }
        }
    }
    /// <summary>
    /// 游戏服需要用到游戏配置
    /// </summary>
    [ObjectSystem]
    public class GameConfigCacheComponentSystem3 : AwakeSystem<GameRoomComponent>
    {
        public override void Awake(GameRoomComponent self)
        {
            if(Game.Scene.GetComponent<GameConfigCacheComponent>() == null)
            {
                Game.Scene.AddComponent<GameConfigCacheComponent>();
            }
        }
    }
}
