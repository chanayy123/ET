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
            try
            {
                self.Awake();
                await self.FetchGameCfgList();
            }
            catch (Exception e)
            {
                Log.Warning("GameConfigComponent awake异常 " + e);
            }
        }
    }



}
