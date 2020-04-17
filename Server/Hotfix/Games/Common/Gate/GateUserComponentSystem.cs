using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class GateUserComponentExtensions
    {
        public static void Add(this GateUserComponent self, int userId, GateUser user)
        {
            bool flag = self.userDic.TryAdd(userId, user);
            if (!flag) Log.Warning($"{userId}已经存在,无法添加网关用户");
        }

        public static GateUser Get(this GateUserComponent self, int userId)
        {
            bool flag = self.userDic.TryGetValue(userId, out GateUser user);
            if (flag) return user;
            else return null;
        }

        public static bool Remove(this  GateUserComponent self, int userId)
        {
            return self.userDic.Remove(userId);
        }

        public static int Count(this GateUserComponent self)
        {
                return self.userDic.Count;
        }

        public static GateUser[] GetAll(this GateUserComponent self)
        {
            return self.userDic.Values.ToArray();
        }
    }

    [ObjectSystem]
    public class GateUserComponentSystem : AwakeSystem<GateUserComponent>
    {
        public override void Awake(GateUserComponent self)
        {
            self.Awake();
        }
    }
}
