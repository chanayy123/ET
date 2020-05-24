using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    class Gate_SynActorIdHandler : AMHandler<GS_SynActorId>
    {
        protected override async ETTask Run(Session session, GS_SynActorId message)
        {
            var user= Game.Scene.GetComponent<GateUserComponent>().Get(message.UserId);
            if (user == null) return;
            user.ActorId = message.ActorId;
            Log.Debug($"网关服: 用户{message.UserId}更新actorId: {message.ActorId}");
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(AppType.World)]
    class User_SynActorIdHandler : AMHandler<GS_SynActorId>
    {
        protected override async ETTask Run(Session session, GS_SynActorId message)
        {
            var user = Game.Scene.GetComponent<UserComponent>().Get(message.UserId);
            if (user == null) return;
            user.ActorId = message.ActorId;
            user.GameId = message.GameId;
            user.RoomId = message.RoomId;
            Log.Debug($"世界服: 用户{message.UserId}更新actorId: {message.ActorId} gameId: {message.GameId} roomId: {message.RoomId}");
            await ETTask.CompletedTask;
        }
    }
}
