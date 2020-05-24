using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.World)]
    class SW_UpdateUserInfoHandler : AMRpcHandler<SW_UpdateUserInfo, WS_UpdateUserInfo>
    {
        protected override async ETTask Run(Session session, SW_UpdateUserInfo request, WS_UpdateUserInfo response, Action reply)
        {
            Game.EventSystem.Run(EventType.PropertyChange + request.Key, request.UserId, request.Value,response,reply);
            await ETTask.CompletedTask;
        }
    }
}
