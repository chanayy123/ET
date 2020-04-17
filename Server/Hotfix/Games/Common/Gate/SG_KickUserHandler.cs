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
                ses.Send(new SC_KickUser { Message="你在别处登陆,已被强制踢出!"});
                Log.Debug($"{user.UserId} 异地登陆强制踢出");
                ses.Dispose();
                reply();
                await ETTask.CompletedTask;
            }
            else
            {
                response.Message = "当前网关没有此用户!";
                reply();
            }
        }
    }
}
