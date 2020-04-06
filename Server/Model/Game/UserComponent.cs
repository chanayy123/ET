using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ETHotfix;

namespace ETModel
{
    [ObjectSystem]
    public class UserComponentSystem : AwakeSystem<UserComponent>
    {
        public override void Awake(UserComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 网关用户管理类
    /// </summary>
   public class UserComponent : Component
    {
        private readonly Dictionary<int, User> userDic = new Dictionary<int, User>();
        public static UserComponent Instance { get; private set; }


        public void Awake()
        {
            Instance = this;
        }

        public void Add(int userId, User user)
        {
            bool flag = this.userDic.TryAdd(userId,user);
            if (!flag) Log.Warning($"{userId}已经存在,无法添加用户");
        }

        public User Get(int userId)
        {
            bool flag = userDic.TryGetValue(userId, out User user);
            if (flag) return user;
            else return null;
        }

        public void Remove(int userId)
        {
            userDic.Remove(userId);
        }

        public int Count
        {
            get {
                return userDic.Count;
            }
        }

        public User[] GetAll()
        {
            return userDic.Values.ToArray();
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            //每个user生命周期跟session走,这里不需要释放
            userDic.Clear();
        }



    }
}
