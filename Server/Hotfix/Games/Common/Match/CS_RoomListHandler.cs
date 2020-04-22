using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class CS_RoomListHandler : AMRpcHandler<CS_RoomList, SC_RoomList>
    {
        protected override async ETTask Run(Session session, CS_RoomList request, SC_RoomList response, Action reply)
        {
            var matchMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            var roomList = matchMgr.GetAll(request.HallId);
            if(roomList == null)
            {
                response.Message = $"请求房间列表失败: 无效id {request.HallId}";
                reply();
                return;
            }
            response.List.Clear();//目前消息都从对象池取,每次使用前都需要重置一下
            response.List.AddRange(roomList);
            reply();
            //请求对应大厅的房间列表->玩家自动进入当前大厅
            var hallPlayer = matchMgr.GetHallPlayer(request.UserId, request.GateSessionId,true);
            matchMgr.EnterHall(request.HallId, hallPlayer);
            await ETTask.CompletedTask;
        }
    }
}
