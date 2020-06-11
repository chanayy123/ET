using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Robot)]
    class MR_ReturnRobotHandler : AMHandler<MR_ReturnRobot>
    {
        protected override async ETTask Run(Session session, MR_ReturnRobot message)
        {
            foreach (var item in message.List)
            {
                RobotComponent.Instance.ReturnRobot(item);
            }
            await ETTask.CompletedTask;
        }
    }
}
