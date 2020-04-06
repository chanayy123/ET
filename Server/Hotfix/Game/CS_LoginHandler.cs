using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    class CS_LoginHandler : AMRpcHandler<CS_Login, SC_Login>
    {
        protected override async ETTask Run(Session session, CS_Login request, SC_Login response, Action reply)
        {
            var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
            List<ComponentWithId> list = await dbProxy.Query<Account>(user => user.Acc.Equals(request.Account));
            if (list.Count == 0)
            {
                Log.Debug($"没有此账号 {request.Account}");
                response.Message = "没有此账号";
                reply();
                return;
            }
            var accInfo = list[0] as Account;
            //已在线强制踢出
            await RealmHelper.KickUser(accInfo.UserId);
            var gateCfg = Game.Scene.GetComponent<RealmGateAddressComponent>().GetAddress();
            var gateAdd = gateCfg.GetComponent<InnerConfig>().IPEndPoint;
            var gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateAdd);
            var loginKey = (G2R_GetLoginKey) await gateSession.Call(new R2G_GetLoginKey() { UserId =accInfo.UserId});
            var outAdd = gateCfg.GetComponent<OuterConfig>().Address2;
            response.Address = outAdd;
            response.Key = loginKey.Key;
            reply();
        }
    }
}
