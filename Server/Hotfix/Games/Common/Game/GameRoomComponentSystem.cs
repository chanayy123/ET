using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class GameRoomComponentAwakeSystem : AwakeSystem<GameRoomComponent>
    {
        public override async void Awake(GameRoomComponent self)
        {
            //缓存游戏配置
            await GameConfigCacheComponent.Instance.GetAllAsync();
        }
    }

}
