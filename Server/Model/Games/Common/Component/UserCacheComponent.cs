using System;
using System.Collections.Generic;
using System.Text;
using FastMember;

namespace ETModel
{
    public class UserCacheComponent:Singleton<UserCacheComponent>
    {
        public readonly Dictionary<int, User> userDic = new Dictionary<int, User>();
        public readonly Dictionary<int, ObjectAccessor> userWrapperDic = new Dictionary<int, ObjectAccessor>();

        public void Awake()
        {
        }
        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            foreach (var item in userDic)
            {
                item.Value.Dispose();
            }
            userDic.Clear();
        }
    }
}
