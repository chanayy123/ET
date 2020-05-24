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
            //同步离线消息
            GateHelper.SynOffline(self.User);
            GateUserComponent.Instance.Remove(self.User.UserId);
            Log.Debug($"{self.User.UserId}下线");
        }
    }
}
