using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using System.Linq;
namespace ETHotfix
{

    public static class RobotComponentExtensions
    {
        public static async void InitRobots(this RobotComponent self, int count)
        {
            int maxUserId = await WorldHelper.LockMaxUserId();
            var accList = new List<ComponentWithId>();
            var userList = new List<ComponentWithId>();
            for(int i = -0; i < count; ++i)
            {
                var accInfo = ComponentFactory.Create<AccountInfo>();
                accInfo.Acc = Guid.NewGuid().ToString("N");
                accInfo.Pwd = string.Empty;
                //计算userId
                accInfo.UserId = ++maxUserId;
                accList.Add(accInfo);
                var userInfo = ComponentFactory.Create<UserInfo>();
                userInfo.Name = $"db{accInfo.UserId}";
                userInfo.UserId = accInfo.UserId;
                userInfo.Level = 1;
                userInfo.Coin = 10000;
                userInfo.IsRobot = true;
                userList.Add(userInfo);
                self.AddRobot(userInfo);
            }
            await self.DBProxy.SaveBatch(accList);
            await self.DBProxy.SaveBatch(userList);
            WorldHelper.UnlockMaxUserId(maxUserId);
            accList.Clear();
            userList.Clear();
        }
        public static void AddRobot(this RobotComponent self, UserInfo userInfo)
        {
            RobotUser robot =  ComponentFactory.Create<RobotUser, UserInfo>(userInfo);
            self.AvailableList.Add(robot);
        }

        public static RobotUser GetRobot(this RobotComponent self,int userId)
        {
            RobotUser robot = self.UnAvailableList.Find((item) => item.UserInfo.UserId == userId);
            if (robot == null)
            {
                robot = self.AvailableList.Find((item) => item.UserInfo.UserId == userId);
            }       
            return robot;
        }

        /// <summary>
        /// 派遣机器人
        /// </summary>
        /// <param name="self"></param>
        /// <param name="count"></param>
        public static List<UserInfo> DispatchRobot(this RobotComponent self,int roomId, int count)
        {
            self.DispatchList.Clear();
            if (self.AvailableList.Count >= count)
            {
                while (count-- > 0)
                {
                    RobotUser robot = self.AvailableList[0];
                    self.AvailableList.RemoveAt(0);
                    robot.IsAvailable = false;
                    robot.RoomId = roomId;
                    self.UnAvailableList.Add(robot);
                    self.DispatchList.Add(robot.UserInfo);
                }
            }
            else
            {
                Log.Warning("机器人数量不够!");
                count = self.AvailableList.Count;
                while (count-- > 0)
                {
                    RobotUser robot = self.AvailableList[0];
                    self.AvailableList.RemoveAt(0);
                    robot.IsAvailable = false;
                    robot.RoomId = roomId;
                    self.UnAvailableList.Add(robot);
                    self.DispatchList.Add(robot.UserInfo);
                }
            }
            return self.DispatchList;
        }
        /// <summary>
        /// 回收机器人
        /// </summary>
        /// <param name="self"></param>
        /// <param name="userId"></param>
        public static void ReturnRobot(this RobotComponent self,int userId)
        {
            var index = self.UnAvailableList.FindIndex((item) => item.UserInfo.UserId == userId);
            if(index != -1)
            {
                var robot = self.UnAvailableList[index];
                self.UnAvailableList.RemoveAt(index);
                robot.IsAvailable = true;
                robot.RoomId = 0;
                self.AvailableList.Add(robot);
            }
            else
            {
                Log.Warning($"回收机器人:无法找到{userId}");
            }
        }

    }

}
