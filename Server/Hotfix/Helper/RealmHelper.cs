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
            var online = Game.Scene.GetComponent<OnlineUserComponent>();
            var gateId = online.Get(userId);
            if (gateId != 0)
            {
                var cfg = Game.Scene.GetComponent<StartConfigComponent>().Get(gateId);
                var ip = cfg.GetComponent<InnerConfig>().IPEndPoint;
                var ses = Game.Scene.GetComponent<NetInnerComponent>().Get(ip);
                await ses.Call(new RG_KickUser { UserId = userId });
            }
        }

        private static int UserId { get; set; }
        public static async ETTask<int> AvailableUserId()
        {
            ETTaskCompletionSource<int> tcs = new ETTaskCompletionSource<int>();
            if (UserId > 0)
            {
                tcs.SetResult(++UserId);
                return await tcs.Task;
            }
            var dbProxyComponent = Game.Scene.GetComponent<DBProxyComponent>();
            List<ComponentWithId> list = await dbProxyComponent.Query<Account>(acc => true);
            list.Sort((l, r) =>
            {
                var la = (Account)l;
                var ra = (Account)r;
                return la.UserId - ra.UserId;
            });
            if (list.Count > 0)
            {
                var maxAcc = (Account)list[list.Count - 1];
                UserId = maxAcc.UserId + 1;
            }
            else
            {
                UserId = 1000; //默认从1000开始
            }
            tcs.SetResult(UserId);
            return await tcs.Task;
        }
    }
}
