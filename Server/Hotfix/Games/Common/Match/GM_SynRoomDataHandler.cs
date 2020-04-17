using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class GM_SynRoomDataHandler : AMHandler<GM_SynRoomData>
    {
        protected override async ETTask Run(Session session, GM_SynRoomData message)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            var room = roomMgr.GetByRoomId(message.RoomId);
            room.State = message.State;
            room.RoomActorId = message.RoomActorId;
            await ETTask.CompletedTask;
        }
    }
}
