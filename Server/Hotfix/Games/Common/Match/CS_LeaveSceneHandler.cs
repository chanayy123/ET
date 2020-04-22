using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class CS_LeaveSceneHandler : AMRpcHandler<CS_LeaveScene, SC_LeaveScene>
    {
        protected override async ETTask Run(Session session, CS_LeaveScene request, SC_LeaveScene response, Action reply)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            var flag = roomMgr.CanLeaveMatchQueue(request.UserId);
            if (flag == OpRetCode.Success)
            {
                roomMgr.LeaveMatchQueue(request.UserId);
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
