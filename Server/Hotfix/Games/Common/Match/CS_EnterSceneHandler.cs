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
            var user = await UserCacheComponent.Instance.GetAsync(request.UserId);
            var flag = TryEnterScene(request.HallId, request.GateSessionId, user.UserInfo);
            response.Error = flag;
            reply();
            //延迟邀请机器人
            DelayCallRobot(request.HallId, RandomHelper.RandomNumber(1, 4));
            await ETTask.CompletedTask;
        }

        private int TryEnterScene(int hallId, long gateSessionId,UserInfo userInfo)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            var flag = roomMgr.CanEnterMatchQueue(hallId, userInfo);
            if (flag == OpRetCode.Success)
            {
                var matchPlayer = MatchFactory.CreateMatchPlayer(userInfo, 0, gateSessionId, hallId);
                roomMgr.EnterMatchQueue(matchPlayer);
            }
            return (int)flag;
        }

        private async void DelayCallRobot(int hallId, int count, int delay = 1000)
        {
            await TimerComponent.Instance.WaitAsync(delay);
            List<UserInfo> list = await MatchHelper.CallRobot(hallId, count);
            foreach (var item in list)
            {
                var flag = TryEnterScene(hallId, 0, item);
                if (flag != 0)
                {
                    Log.Warning($"机器人{item.UserId}进入场景失败: {flag}");
                    MatchHelper.ReturnRobot(item.UserId);
                }
            }
        }
    }
}
