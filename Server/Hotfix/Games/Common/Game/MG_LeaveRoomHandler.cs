using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Game)]
    class MG_LeaveRoomHandler : AMHandler<MG_LeaveRoom>
    {
        protected override  ETTask Run(Session session, MG_LeaveRoom msg)
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            if(roomMgr.GetRoom(msg.RoomId) != null)
            {
                Game.EventSystem.Run(EventType.GameRoomLeave + msg.GameId, msg.RoomId,msg.UserId);
            }
            else
            {
                Log.Warning($"当前游服没有此{msg.RoomId}房间!");
            }
            return ETTask.CompletedTask;
        }
    }
}
