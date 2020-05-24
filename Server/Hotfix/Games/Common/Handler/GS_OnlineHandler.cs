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

    [MessageHandler(AppType.World)]
    class GS_OnlineUserHandler : AMHandler<GS_Online>
    {
        protected override async ETTask Run(Session session, GS_Online message)
        {
            User user = UserComponent.Instance.Get(message.UserId);
            if(user == null)
            {
                List<ComponentWithId> list = await UserComponent.Instance.DBProxy.Query<UserInfo>((u)=>u.UserId == message.UserId);
                if(list.Count == 0)
                {
                    Log.Warning($"上线:用户{message.UserId}获取信息失败");
                    return;
                }
                user = ComponentFactory.Create<User, UserInfo>(list[0] as UserInfo);
                user.Online = true;
                user.GateSessionId = message.GateSessionId;
                UserComponent.Instance.Add(message.UserId,user);
            }
            else
            {
                user.Online = true;
                user.GateSessionId = message.GateSessionId;
            }
            //如果玩家离线时在游戏中,上线后同步更新网关用户actorid
            if(user.ActorId != 0)
            {
                WorldHelper.SynActorId(user.GateSessionId, message.UserId, user.ActorId, user.GameId, user.RoomId);
            }
            //玩家上线,主动推送玩家数据
            var msg = WorldFactory.CreateMsgSC_PlayerData(user, GameConfigComponent.Instance.cfgList);
            NetInnerHelper.SendActorMsg(msg);
            WorldFactory.RecycleMsg(msg);
        }
    }
}
