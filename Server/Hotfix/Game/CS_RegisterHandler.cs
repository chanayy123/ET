using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    class CS_RegisterHandler : AMRpcHandler<CS_Register, SC_Register>
    {
        protected override async ETTask Run(Session session, CS_Register request, SC_Register response, Action reply)
        {
            var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
            List<ComponentWithId> list = await dbProxy.Query<Account>(user => user.Acc.Equals(request.Account));
            if (list.Count > 0)
            {
                Log.Debug($"此账号已注册 {request.Account}");
                response.Message = "此账号已注册";
                reply();
                return;
            }
            var accInfo = ComponentFactory.Create<Account>();
            accInfo.Acc = request.Account;
            accInfo.Pwd = request.Password;
            //计算userId
            accInfo.UserId = await RealmHelper.AvailableUserId();
            //同步账号信息=>db
            await dbProxy.Save(accInfo);
            //创建默认用户信息
            var userInfo = ComponentFactory.Create<User>();
            userInfo.Name = request.Name;
            userInfo.UserId = accInfo.UserId;
            userInfo.Level = 1;
            userInfo.Coin = 10000;
            //同步用户信息=>db
            await dbProxy.Save(userInfo);
            reply();
        }
    }
}
