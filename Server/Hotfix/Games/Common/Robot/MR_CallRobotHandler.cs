using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Robot)]
    class MR_CallRobotHandler : AMRpcHandler<MR_CallRobot, RM_CallRobot>
    {
        protected override async ETTask Run(Session session, MR_CallRobot request, RM_CallRobot response, Action reply)
        {
            List<UserInfo> list = RobotComponent.Instance.DispatchRobot(request.RoomId,request.Count);
            response.List.Clear();
            response.List.AddRange(list);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
