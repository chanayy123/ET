using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class CS_EnterSceneHandler : AMRpcHandler<CS_EnterScene, SC_EnterScene>
    {
        protected override async ETTask Run(Session session, CS_EnterScene request, SC_EnterScene response, Action reply)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            var user = await UserCacheComponent.Instance.GetAsync(request.UserId);
            var flag = roomMgr.CanEnterMatchQueue(request.HallId, user);
            if (flag == OpRetCode.Success)
            {
                var room = roomMgr.GetByUserId(request.UserId);
                var matchPlayer = MatchFactory.CreateMatchPlayer(user.UserInfo.UserId, 0, request.GateSessionId,request.HallId);
                roomMgr.EnterMatchQueue(matchPlayer);
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
