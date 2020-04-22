using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ActorMessageHandler(AppType.Game)]
    class BullClassicActorLeaveRoomHandler : AMActorRpcHandler<BullClassicPlayer, CS_LeaveRoom, SC_LeaveRoom>
    {
        protected override ETTask Run(BullClassicPlayer unit, CS_LeaveRoom request, SC_LeaveRoom response, Action reply)
        {
            var room = unit.Parent as BullClassicRoom;
            if(room.RoomData.State != (int)RoomState.GAMING)
            {
                room.LeaveRoom(unit.PlayerData.UserId);
            }
            else
            {
                response.Error = (int)OpRetCode.RoomAlreadyGaming;
            }
            reply();
            return ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler(AppType.Game)]
    class BullClassicActorOnlineHandler : AMActorHandler<BullClassicPlayer, Actor_OnlineState>
    {
        protected override ETTask Run(BullClassicPlayer player, Actor_OnlineState message)
        {
            var room = player.Parent as BullClassicRoom;
            player.PlayerData.Online = message.State;
            player.PlayerData.GateSessionId = message.GateSessionId;
            if (room.RoomData.State != (int)RoomState.GAMING)
            {
                //不在游戏中离线直接踢掉
                if(message.State == 0)
                    room.LeaveRoom(player.PlayerData.UserId);
            }
            else
            {
            }
            return ETTask.CompletedTask;
        }
    }
}
