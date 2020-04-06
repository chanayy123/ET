using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    public class SessionUserDestroySystem : DestroySystem<SessionUserComponent>
    {
        public override void Destroy(SessionUserComponent self)
        {
            Game.Scene.GetComponent<UserComponent>().Remove(self.User.UserId);
            //同步离线消息=>realm
            GateHelper.synOffline(self.User.UserId);
            self.User.Dispose();
            self.User = null;
        }
    }
}
