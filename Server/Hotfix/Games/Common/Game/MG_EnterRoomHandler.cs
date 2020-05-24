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
                Game.EventSystem.Run($"{EventType.GameRoomEnter}{message.GameId}_{message.GameMode}", message.RoomId, message.Player);
            }
            else
            {
                var roomCfg = RoomHelper.GetRoomCfg(message.GameId, message.GameMode, message.HallType);
                Game.EventSystem.Run($"{EventType.GameRoomCreate}{message.GameId}_{message.GameMode}", message.RoomId, roomCfg);
                Game.EventSystem.Run($"{EventType.GameRoomEnter}{message.GameId}_{message.GameMode}", message.RoomId, message.Player);
            }
            return ETTask.CompletedTask;
        }
    }
}
