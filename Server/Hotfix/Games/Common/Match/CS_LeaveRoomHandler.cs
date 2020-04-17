using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class CS_LeaveRoomHandler : AMRpcHandler< CS_LeaveRoom, SC_LeaveRoom>
    {
        protected override async ETTask Run(Session session, CS_LeaveRoom request, SC_LeaveRoom response, Action reply)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            var flag = roomMgr.CanLeaveRoom(request.UserId);
            if(flag == OpRetCode.Success)
            {
                var room = roomMgr.GetByUserId(request.UserId);
                NetInnerHelper.SendMsgByAcotrId(room.RoomActorId, new MG_LeaveRoom()
                {
                    RoomId = room.RoomId,
                    UserId = request.UserId,
                    GameId =room.Config.GameId
                });
                roomMgr.LeaveRoom(request.UserId);
                reply();
            }
            else
            {
                response.Error = (int)flag;
                reply();
            }
            await ETTask.CompletedTask;
        }
    }
}
