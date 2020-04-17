using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    class GG_SynActorIdHandler : AMHandler<GG_SynActorId>
    {
        protected override async ETTask Run(Session session, GG_SynActorId message)
        {
            var user= Game.Scene.GetComponent<GateUserComponent>().Get(message.UserId);
            user.ActorId = message.ActorId;
            Log.Debug($"用户{message.UserId}更新actorId: {message.ActorId}");
            await ETTask.CompletedTask;
        }
    }
}
