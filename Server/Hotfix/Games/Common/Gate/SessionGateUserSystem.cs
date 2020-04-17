using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    public class SessionGateUserDestroySystem : DestroySystem<SessionGateUserComponent>
    {
        public override void Destroy(SessionGateUserComponent self)
        {
            Game.Scene.GetComponent<GateUserComponent>().Remove(self.User.UserId);
            Log.Debug($"{self.User.UserId}下线");
            //同步离线消息=>realm
            GateHelper.SynOffline(self.User.UserId);
            self.User.Dispose();
            self.User = null;
        }
    }
}
