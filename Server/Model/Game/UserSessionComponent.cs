using System;
using System.Collections.Generic;
using System.Text;
using ETHotfix;
namespace ETModel
{
    [ObjectSystem]
    public class UserSessionDestroyStem : DestroySystem<UserSessionComponent>
    {
        public override void Destroy(UserSessionComponent self)
        {
            self.Session = null;
        }
    }

    public class UserSessionComponent : Component
    {
        public Session Session { get; set; }
    }
}
