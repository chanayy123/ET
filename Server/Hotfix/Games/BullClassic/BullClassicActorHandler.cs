using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ActorMessageHandler(AppType.Game)]
    class BullClassicActorLeaveRoomHandler : AMActorRpcHandler<BullClassicPlayer, CS_LeaveRoom, SC_LeaveRoom>
    {
        protected override ETTask Run(BullClassicPlayer player, CS_LeaveRoom request, SC_LeaveRoom response, Action reply)
        {
            var room = player.Parent as BullClassicRoom;
            var flag = room.CanLeaveRoom(player.UserId);
            if(flag == OpRetCode.Success)
            {
                room.LeaveRoom(player.UserId);
            }
            else
            {
                response.Error = (int)flag;
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
            player.Online = message.State;
            player.GateSessionId = message.GateSessionId;
            if (room.State != (int)RoomState.GAMING)
            {
                //游戏没开始,离线直接踢掉
                if (!player.IsOnline)
                {
                    room.LeaveRoom(player.UserId);
                }
            }
            else
            {
            }
            return ETTask.CompletedTask;
        }
    }
}
