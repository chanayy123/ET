using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ObjectSystem]
    public class RobotComponentSystem : AwakeSystem<RobotComponent>
    {
        public override async void Awake(RobotComponent self)
        {
            try
            {
                self.Awake();
                //获取起始机器人账号,数量不够就注册
                List<ComponentWithId> list = await self.DBProxy.Query<UserInfo>((item) => item.IsRobot == true);
                if (list.Count < RobotComponent.INIT_COUNT)
                {
                    for (var i = 0; i < list.Count; ++i)
                    {
                        self.AddRobot(list[i] as UserInfo);
                    }
                    await self.InitRobots(RobotComponent.INIT_COUNT - list.Count);
                }
                else
                {
                    for (var i = 0; i < RobotComponent.INIT_COUNT; ++i)
                    {
                        self.AddRobot(list[i] as UserInfo);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Warning("RobotComponent awake exception: " + e);
            }
        }
    }

    [ObjectSystem]
    public class RobotUserSystem : AwakeSystem<RobotUser,UserInfo>
    {
        public override void Awake(RobotUser self,UserInfo userInfo)
        {
            self.IsAvailable = true;
            self.UserInfo = userInfo;
        }
    }
}
