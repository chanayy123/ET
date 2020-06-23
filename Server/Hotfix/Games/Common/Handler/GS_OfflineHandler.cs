﻿using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    class GS_OfflineHandler : AMHandler<GS_Offline>
    {
        protected override async ETTask Run(Session session, GS_Offline message)
        {
            Game.Scene.GetComponent<RealmOnlineUserComponent>().Remove(message.UserId);
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(AppType.World)]
    class GS_OfflineUserHandler : AMHandler<GS_Offline>
    {
        protected override async ETTask Run(Session session, GS_Offline message)
        {
            var user = UserComponent.Instance.Get(message.UserId);
            if(user == null)
            {
                Log.Warning($"下线:用户{message.UserId}不存在");
            }
            else
            {
                user.Online = false;
                user.GateSessionId = 0;
            }
            if(user.ActorId != 0)
            {
                Actor_OnlineState msg2 = GateFactory.CreateMsgActor_OnlineState(user.ActorId, false, 0);
                NetInnerHelper.SendActorMsg(msg2);
                GateFactory.RecycleMsg(msg2);
            }
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(AppType.Match)]
    class GS_OfflineMatchHandler : AMHandler<GS_Offline>
    {
        protected override async ETTask Run(Session session, GS_Offline message)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            //只要离线,不管有没有进入游戏都退出大厅,只有客户端主动请求房间列表才会进入大厅
            var player= roomMgr.GetHallPlayer(message.UserId);
            if(player != null)
            {
                roomMgr.LeaveHall(player);
            }
            //如果在匹配队列里,离线自动退出匹配
            roomMgr.LeaveMatchQueue(message.UserId);
            await ETTask.CompletedTask;
        }
    }
}
