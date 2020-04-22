using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    class UserCacheComponentSystem : AwakeSystem<UserCacheComponent>
    {
        public override void Awake(UserCacheComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class MatchRoomComponentAwakeSystem2 : AwakeSystem<MatchRoomComponent>
    {
        public override void Awake(MatchRoomComponent self)
        {
            Game.Scene.AddComponent<UserCacheComponent>();
        }
    }
}
