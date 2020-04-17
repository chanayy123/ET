using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
namespace ETModel
{
    [ObjectSystem]
    public class GateUserSessionDestroyStem : DestroySystem<GateUserSessionComponent>
    {
        public override void Destroy(GateUserSessionComponent self)
        {
            self.Session = null;
        }
    }

    public class GateUserSessionComponent : Component
    {
        public Session Session { get; set; }
    }
}
