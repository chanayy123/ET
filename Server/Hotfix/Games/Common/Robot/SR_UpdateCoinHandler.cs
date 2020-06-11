using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Robot)]
    class SR_UpdateCoinHandler : AMHandler<SR_UpdateCoin>
    {
        protected override async ETTask Run(Session session, SR_UpdateCoin message)
        {
            RobotUser robot = RobotComponent.Instance.GetRobot(message.UserId);
            if(robot == null)
            {
                Log.Warning($"机器人金币变更失败: 找不到机器人{message.UserId}");
                return;
            }
            robot.UserInfo.Coin = message.Coin;
            //更新数据库策略待定
            await ETTask.CompletedTask;
        }
    }

}
