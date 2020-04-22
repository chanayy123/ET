using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{

    public static class UserCacheComponentExtensions
    {
        public static async ETTask<User> GetAsync(this UserCacheComponent self,int userId)
        {
            if(self.userDic.TryGetValue(userId,out User user))
            {
                return user;
            }
            user = await UserHelper.GetUserInfo(userId);
            self.userDic.Add(userId, user);
            return user;
        }
        public static User Get(this UserCacheComponent self,int userId)
        {
            self.userDic.TryGetValue(userId, out User user);
            return user;
        }
        public static void Remove(this UserCacheComponent self,int userId)
        {
            self.userDic.Remove(userId,out User user);
            user?.Dispose();
        }
    }
}
