using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    public class GateUserComponentSystem : AwakeSystem<GateUserComponent>
    {
        public override void Awake(GateUserComponent self)
        {
            self.Awake();
        }
    }
}
