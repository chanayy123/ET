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
            var worldSession = NetInnerHelper.GetSessionByAppType(AppType.World);
            RW_Register msg = RealmFactory.CreateMsgRW_Register(request.Account, request.Name, request.Password);
            var urReg = (WR_Register)await worldSession.Call(msg);
            RealmFactory.RecycleMsg(msg);
            if (urReg.UserId == 0)
            {
                response.Error = urReg.Error;
                reply();
                return;
            }
            //注册成功,直接返回网关地址和key,方便客户端直接验证无需登陆
            var gateCfg = GateHelper.RandomGateCfg;
            var gateAdd = gateCfg.GetComponent<InnerConfig>().IPEndPoint;
            var gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateAdd);
            R2G_GetLoginKey msg2 = RealmFactory.CreateMsgR2G_GetLoginKey(urReg.UserId);
            var loginKey = (G2R_GetLoginKey)await gateSession.Call(msg2);
            RealmFactory.RecycleMsg(msg2);
            var outAdd = gateCfg.GetComponent<OuterConfig>().Address2;
            response.Address = outAdd;
            response.Key = loginKey.Key;
            reply();

        }
    }
}
