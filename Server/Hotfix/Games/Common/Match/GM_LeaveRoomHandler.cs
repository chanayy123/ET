using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class GM_LeaveRoomHandler : AMHandler<GM_LeaveRoom>
    {
        protected override async ETTask Run(Session session, GM_LeaveRoom message)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            roomMgr.LeaveRoom(message.UserId);
            await ETTask.CompletedTask;
        }
    }
}
