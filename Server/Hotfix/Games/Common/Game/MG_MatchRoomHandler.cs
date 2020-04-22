using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Game)]
    class MG_MatchRoomHandler : AMHandler<MG_MatchRoom>
    {
        protected override ETTask Run(Session session, MG_MatchRoom message)
        {
            var roomMgr = Game.Scene.GetComponent<GameRoomComponent>();
            if (roomMgr.GetRoom(message.RoomId) != null)
            {
                Game.EventSystem.Run(EventType.GameRoomMatch+ message.GameId, message.RoomId, message.PlayerList);
            }
            else
            {
                Game.EventSystem.Run(EventType.GameRoomCreate + message.GameId, message.RoomId);
                Game.EventSystem.Run(EventType.GameRoomMatch + message.GameId, message.RoomId, message.PlayerList);
            }
            return ETTask.CompletedTask;
        }
    }
}
