using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ETHotfix;

namespace ETModel
{
    [ObjectSystem]
    public class GateUserComponentSystem : AwakeSystem<GateUserComponent>
    {
        public override void Awake(GateUserComponent self)
        {
            self.Awake();
        }
    }
    /// <summary>
    /// 网关用户管理类
    /// </summary>
    public class GateUserComponent : Singleton<GateUserComponent>
    {
        public readonly Dictionary<int, GateUser> userDic = new Dictionary<int, GateUser>();

        public void Awake()
        {
        }
        public  void Add(int userId, GateUser user)
        {
            bool flag = userDic.TryAdd(userId, user);
            if (!flag) Log.Warning($"{userId}已经存在,无法添加网关用户");
        }

        public  GateUser Get(int userId)
        {
            userDic.TryGetValue(userId, out GateUser user);
            return user;
        }

        public  void Remove(int userId)
        {
            userDic.Remove(userId, out GateUser user);
            user?.Dispose();
        }

        public  int Count()
        {
            return userDic.Count;
        }

        public  GateUser[] GetAll()
        {
            return userDic.Values.ToArray();
        }
    }
}
