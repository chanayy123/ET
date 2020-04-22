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
            var user = Game.Scene.GetComponent<GateUserComponent>().Get(request.UserId);
            if(user != null)
            {
                var ses = user.GetComponent<GateUserSessionComponent>().Session;
                SC_KickUser msg = SimplePool.Instance.Fetch<SC_KickUser>();
                msg.Error = (int)OpRetCode.KickOtherLogin;
                msg.Message = "你在别处登陆,已被强制踢出!";
                ses.Send(msg);
                SimplePool.Instance.Recycle(msg);
                ses.Dispose();
                reply();
                Log.Debug($"{user.UserId} 异地登陆强制踢出");
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
