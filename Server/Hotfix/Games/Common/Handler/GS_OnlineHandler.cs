using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    class GS_OnlineHandler : AMHandler<GS_Online>
    {
        protected override async ETTask Run(Session session, GS_Online message)
        {
            Game.Scene.GetComponent<RealmOnlineUserComponent>().Add(message.UserId, message.GateSessionId);
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(AppType.User)]
    class GS_OnlineUserHandler : AMHandler<GS_Online>
    {
        protected override async ETTask Run(Session session, GS_Online message)
        {
            User user = UserComponent.Instance.Get(message.UserId);
            if(user == null)
            {
                List<ComponentWithId> list = await UserComponent.Instance.DBProxy.Query<User>((u) => u.UserId == message.UserId);
                if(list.Count == 0)
                {
                    Log.Warning($"上线:用户{message.UserId}获取信息失败");
                    return;
                }
                user = list[0] as User;
                user.Online = true;
                UserComponent.Instance.Add(message.UserId,user);
            }
            else
            {
                user.Online = true;
            }
            await UserComponent.Instance.DBProxy.Save(user);
        }
    }
}
