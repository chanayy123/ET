using System;
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

    [MessageHandler(AppType.User)]
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
                await UserComponent.Instance.DBProxy.Save(user);
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
            var room = roomMgr.GetByUserId(message.UserId);
            if(room != null && room.State != (int)RoomState.GAMING) //游戏没有开始离线自动退出房间
            {
                NetInnerHelper.SendMsgByAcotrId(room.RoomActorId, new MG_LeaveRoom()
                {
                    RoomId = room.RoomId,
                    UserId = message.UserId,
                    GameId = room.Config.GameId
                });
                roomMgr.LeaveRoom(message.UserId);
            }
            await ETTask.CompletedTask;
        }
    }
}
