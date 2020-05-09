using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    class RoomConfigComponentSystem : AwakeSystem<RoomConfigComponent>
    {
        public override void Awake(RoomConfigComponent self)
        {
            self.Awake();
            IConfig[] list = Game.Scene.GetComponent<ConfigComponent>().GetAll(typeof(RoomConfig));
            foreach (var item in list)
            {
                self.roomConfigDic.Add(item.Id, item as RoomConfig);
            }
        }
    }

    [ObjectSystem]
    public class RoomConfigComponentSystem2 : AwakeSystem<MatchRoomComponent>
    {
        public override void Awake(MatchRoomComponent self)
        {
            if(Game.Scene.GetComponent<RoomConfigComponent>() == null)
            {
                Game.Scene.AddComponent<RoomConfigComponent>();
            }
        }
    }

    /// <summary>
    /// 游戏服需要用到游戏配置
    /// </summary>
    [ObjectSystem]
    public class RoomConfigComponentSystem3 : AwakeSystem<GameRoomComponent>
    {
        public override void Awake(GameRoomComponent self)
        {
            if (Game.Scene.GetComponent<RoomConfigComponent>() == null)
            {
                Game.Scene.AddComponent<RoomConfigComponent>();
            }
        }
    }
}
