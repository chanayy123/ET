using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETModel;
namespace ETHotfix
{

    [ObjectSystem]
    public class GameConfigComponentSystem : AwakeSystem<GameConfigComponent>
    {
        public override async void Awake(GameConfigComponent self)
        {
            self.Awake();
            await self.FetchGameCfgList();
        }
    }



}
