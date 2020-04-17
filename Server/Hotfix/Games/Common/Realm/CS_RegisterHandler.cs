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
            var userSession = NetInnerHelper.GetSessionByAppType(AppType.User);
            var urReg =  (UR_Register)await userSession.Call(new RU_Register()
            {
                Account = request.Account,
                Name = request.Name,
                Password = request.Password
            });
            if(urReg.UserId == 0)
            {
                response.Error = urReg.Error;
                reply();
                return;
            }
            //注册成功,直接返回网关地址和key,方便客户端直接验证无需登陆
            var gateCfg = GateHelper.RandomGateCfg;
            var gateAdd = gateCfg.GetComponent<InnerConfig>().IPEndPoint;
            var gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateAdd);
            var loginKey = (G2R_GetLoginKey)await gateSession.Call(new R2G_GetLoginKey() { UserId = urReg.UserId });
            var outAdd = gateCfg.GetComponent<OuterConfig>().Address2;
            response.Address = outAdd;
            response.Key = loginKey.Key;
            reply();

        }
    }
}
