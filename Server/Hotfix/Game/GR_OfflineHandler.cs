using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    class GR_OfflineHandler : AMHandler<GR_Offline>
    {
        protected override async ETTask Run(Session session, GR_Offline message)
        {
            Game.Scene.GetComponent<OnlineUserComponent>().Remove(message.UserId);
            await ETTask.CompletedTask;
        }
    }
}
