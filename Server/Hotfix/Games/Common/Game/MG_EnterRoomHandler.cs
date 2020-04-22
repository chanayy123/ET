using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Game)]
    class MG_EnterRoomHandler : AMHandler<MG_EnterRoom>
    {
        protected override  ETTask Run(Session session, MG_EnterRoom message)
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            if(roomMgr.GetRoom(message.RoomId) != null)
            {
                Game.EventSystem.Run(EventType.GameRoomEnter+message.GameId, message.RoomId, message.player);
            }
            else
            {
                Game.EventSystem.Run(EventType.GameRoomCreate+message.GameId, message.RoomId);
                Game.EventSystem.Run(EventType.GameRoomEnter + message.GameId, message.RoomId, message.player);
            }
            return ETTask.CompletedTask;
        }
    }
}
