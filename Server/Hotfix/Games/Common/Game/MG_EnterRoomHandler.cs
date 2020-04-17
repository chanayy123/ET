using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Game)]
    class MG_EnterRoomHandler : AMHandler<MG_EnterRoom>
    {
        protected override  ETTask Run(Session session, MG_EnterRoom request)
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            if(roomMgr.GetRoom(request.RoomId) != null)
            {
                Game.EventSystem.Run(EventType.GameRoomEnter+request.GameId, request.RoomId, request.player);
            }
            else
            {
                Game.EventSystem.Run(EventType.GameRoomCreate+request.GameId, request.RoomId);
                Game.EventSystem.Run(EventType.GameRoomEnter + request.GameId, request.RoomId, request.player);
            }
            return ETTask.CompletedTask;
        }
    }
}
