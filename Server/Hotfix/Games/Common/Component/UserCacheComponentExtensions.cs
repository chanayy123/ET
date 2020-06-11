﻿using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using FastMember;
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
            user = await WorldHelper.GetUserInfo(userId);
            bool flag = self.userDic.TryAdd(userId, user);
            flag = self.userWrapperDic.TryAdd(userId, ObjectAccessor.Create(user.UserInfo));
            if (!flag)
            {
                Log.Warning($"UserCacheComponent: 重复添加{userId}");
            }
            return user;
        }
        public static User Get(this UserCacheComponent self,int userId)
        {
            self.userDic.TryGetValue(userId, out User user);
            return user;
        }

        public static ObjectAccessor GetWrapper(this UserCacheComponent self, int userId)
        {
            self.userWrapperDic.TryGetValue(userId, out ObjectAccessor user);
            return user;
        }

        public static void Remove(this UserCacheComponent self,int userId)
        {
            self.userDic.Remove(userId,out User user);
            user?.Dispose();
            self.userWrapperDic.Remove(userId);
        }
    }
}
