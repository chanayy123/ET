using System;
using System.Collections.Generic;
using System.Reflection;
using ETModel;

namespace ETHotfix
{
	public static class RealmHelper
    {
		public static  async ETTask KickUser(int userId)
		{
            var online = Game.Scene.GetComponent<RealmOnlineUserComponent>();
            var gateSessionId = online.Get(userId);
            if (gateSessionId == 0) return;
            var session = NetInnerHelper.GetSessionByAppId(IdGenerater.GetAppId(gateSessionId));
            SG_KickUser msg = RealmFactory.CreateMsgSG_KickUser(userId);
            await session.Call(msg);
            RealmFactory.RecycleMsg(msg);
        }
    }
}
