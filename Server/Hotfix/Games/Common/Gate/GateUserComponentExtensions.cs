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
            self.userDic.TryGetValue(userId, out GateUser user);
            return user;
        }

        public static void Remove(this  GateUserComponent self, int userId)
        {
             self.userDic.Remove(userId,out GateUser user);
            user?.Dispose();
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
}
