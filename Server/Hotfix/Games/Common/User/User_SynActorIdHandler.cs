using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.User)]
    class User_SynActorIdHandler : AMHandler<GS_SynActorId>
    {
        protected override async ETTask Run(Session session, GS_SynActorId message)
        {
            var user= Game.Scene.GetComponent<UserComponent>().Get(message.UserId);
            if (user == null) return;
            //用户服存此字段为了玩家断线重连,更方便获得游戏服玩家对象
            user.ActorId = message.ActorId;
            Log.Debug($"用户服: 用户{message.UserId}更新actorId: {message.ActorId}");
            await ETTask.CompletedTask;
        }
    }
}
