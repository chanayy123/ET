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
            var roomList = Game.Scene.GetComponent<MatchRoomComponent>().GetAll(request.HallId);
            if(roomList == null)
            {
                response.Message = $"请求房间列表失败: 无效id {request.HallId}";
                reply();
                return;
            }
            response.List.AddRange(roomList);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
