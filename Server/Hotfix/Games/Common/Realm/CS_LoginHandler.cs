﻿using System;
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
            var userSession = NetInnerHelper.GetSessionByAppType(AppType.User);
            RU_Login msg = RealmFactory.CreateMsgRU_Login(request.LoginType, request.DataStr);
            var urLogin = (UR_Login)await userSession.Call(msg);
            RealmFactory.RecycleMsg(msg);
            if(urLogin.UserId == 0)
            {
                response.Error = urLogin.Error;
                reply();
                return;
            }
            //已在线强制踢出
            await RealmHelper.KickUser(urLogin.UserId);
            var gateCfg = GateHelper.RandomGateCfg;
            var gateAdd = gateCfg.GetComponent<InnerConfig>().IPEndPoint;
            var gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateAdd);
            R2G_GetLoginKey msg2 = RealmFactory.CreateMsgR2G_GetLoginKey(urLogin.UserId);
            var loginKey = (G2R_GetLoginKey) await gateSession.Call(msg2);
            RealmFactory.RecycleMsg(msg2);
            var outAdd = gateCfg.GetComponent<OuterConfig>().Address2;
            response.Address = outAdd;
            response.Key = loginKey.Key;
            reply();
        }
    }
}
