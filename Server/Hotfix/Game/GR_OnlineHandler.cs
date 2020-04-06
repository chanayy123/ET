using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    class GR_OnlineHandler : AMHandler<GR_Online>
    {
        protected override async ETTask Run(Session session, GR_Online message)
        {
            Game.Scene.GetComponent<OnlineUserComponent>().Add(message.UserId, message.GateAppId);
            await ETTask.CompletedTask;
        }
    }
}
