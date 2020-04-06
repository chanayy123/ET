using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    class RG_KickUserHandler : AMRpcHandler<RG_KickUser, GR_KickUser_ACK>
    {
        protected override async ETTask Run(Session session, RG_KickUser request, GR_KickUser_ACK response, Action reply)
        {
            var user = Game.Scene.GetComponent<UserComponent>().Get(request.UserId);
            if(user != null)
            {
                var ses = user.GetComponent<UserSessionComponent>().Session;
                ses.Send(new SC_KickUser { Message="你在别处登陆,已被强制踢出!"});
                ses.Dispose();
                reply();
                await ETTask.CompletedTask;
            }
        }
    }
}
