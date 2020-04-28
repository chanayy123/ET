using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    class SG_KickUserHandler : AMRpcHandler<SG_KickUser, GS_KickUser>
    {
        protected override async ETTask Run(Session session, SG_KickUser request, GS_KickUser response, Action reply)
        {
            var gateUser = Game.Scene.GetComponent<GateUserComponent>().Get(request.UserId);
            if(gateUser != null)
            {
                SC_KickUser msg = GateFactory.CreateMsgSC_KickUser((int)OpRetCode.KickOtherLogin, "你在别处登陆,已被强制踢出!");
                gateUser.Session.Send(msg);
                GateFactory.RecycleMsg(msg);
                gateUser.Session.Dispose();
                reply();
                Log.Debug($"{gateUser.UserId} 异地登陆强制踢出");
                await ETTask.CompletedTask;
            }
            else
            {
                response.Error = (int)OpRetCode.GateUserNotExist;
                reply();
            }
        }
    }
}
