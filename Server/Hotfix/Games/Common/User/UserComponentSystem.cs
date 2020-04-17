﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETModel;
namespace ETHotfix
{

    enum LoginType
    {
        Tourist, //游客登陆
        Voucher, //账号密码验证登陆
        Wechat, //微信登陆
    }

    public static class UserComponentExtensions
    {
        public static void Add(this UserComponent self, int userId, User user)
        {
            bool flag = self.userDic.TryAdd(userId, user);
            if (!flag) Log.Warning($"{userId}已经存在,无法添加用户");
        }

        public static User Get(this UserComponent self, int userId)
        {
            bool flag = self.userDic.TryGetValue(userId, out User user);
            if (flag) return user;
            else return null;
        }

        public static bool Remove(this  UserComponent self, int userId)
        {
            return self.userDic.Remove(userId);
        }

        public static int Count(this UserComponent self)
        {
                return self.userDic.Count;
        }

        public static User[] GetAll(this UserComponent self)
        {
            return self.userDic.Values.ToArray();
        }

        public static async ETTask<int> CheckLogin(this UserComponent self ,RU_Login msg, IResponse res)
        {
            var userId = 0;
            LoginType type = (LoginType)msg.LoginType;
            switch (type)
            {
                case LoginType.Tourist :
                    userId = await self.CheckTouristLogin(msg, res);
                    break;
                case LoginType.Wechat:
                    userId = await self.CheckWechatLogin(msg, res);
                    break;
                case LoginType.Voucher:
                    userId = await self.CheckVoucherLogin(msg, res);
                    break;
            }
            return userId;
        }

        public static async ETTask<int> CheckVoucherLogin(this UserComponent self,RU_Login msg, IResponse res)
        {
            var arr = msg.DataStr.Split("|");
            if (arr.Length != 2)
            {
                res.Error = (int)OpRetCode.LoginParamError;
                return 0;
            }
            List<ComponentWithId> list =  await self.DBProxy.Query<Account>((acc) => acc.Acc.Equals(arr[0]) && acc.Pwd.Equals(arr[1]));
            if(list.Count == 0)
            {
                res.Error = (int)OpRetCode.LoginAccPwdError;
                return 0;
            }
            var account = list[0] as Account;
            return account.UserId;
        }

        public static async ETTask<int> CheckWechatLogin(this UserComponent self, RU_Login msg, IResponse res)
        {
            await ETTask.CompletedTask;
             throw new NotImplementedException();
        }

        public static async ETTask<int> CheckTouristLogin(this UserComponent self, RU_Login msg, IResponse res)
        {
            List<ComponentWithId> list = await self.DBProxy.Query<Account>(user => user.Acc.Equals(msg.DataStr));
            if (list.Count == 0)
            {
                if (self.MaxUserId == 0)
                {
                    Log.Debug("初始化userId失败");
                    res.Error = (int)OpRetCode.AccountMaxUserIdError;
                    return 0;
                }
                //创建游客账号信息
                var accInfo = ComponentFactory.Create<Account>();
                accInfo.Acc = msg.DataStr;
                accInfo.Pwd = string.Empty;
                //计算userId
                accInfo.UserId = ++self.MaxUserId;
                //同步账号信息=>db
                await self.DBProxy.Save(accInfo);
                var userInfo = ComponentFactory.Create<User>();
                userInfo.Name = $"游客{RandomHelper.RandomNumber(0,1000)}";
                userInfo.UserId = accInfo.UserId;
                userInfo.Level = 1;
                userInfo.Coin = 10000;
                //同步用户信息=>db
                await self.DBProxy.Save(userInfo);
                return userInfo.UserId;
            }
            var acc = list[0] as Account;
            return acc.UserId;
        }

        public static async ETTask<int> CheckRegister(this UserComponent self ,RU_Register msg, IResponse res)
        {
            List<ComponentWithId> list = await self.DBProxy.Query<Account>(user => user.Acc.Equals(msg.Account));
            if (list.Count > 0)
            {
                Log.Debug($"此账号已注册 {msg.Account}");
                res.Error = (int)OpRetCode.AccountAlreadyExist;
                return 0;
            }
            if(self.MaxUserId == 0)
            {
                Log.Debug("初始化userId失败");
                res.Error = (int)OpRetCode.AccountMaxUserIdError;
                return 0;
            }
            //创建账号信息
            var accInfo = ComponentFactory.Create<Account>();
            accInfo.Acc = msg.Account;
            accInfo.Pwd = msg.Password;
            //计算userId
            accInfo.UserId = ++self.MaxUserId;
            //同步账号信息=>db
            await self.DBProxy.Save(accInfo);
            //创建默认用户信息
            var userInfo = ComponentFactory.Create<User>();
            userInfo.Name = msg.Name;
            userInfo.UserId = accInfo.UserId;
            userInfo.Level = 1;
            userInfo.Coin = 10000;
            //同步用户信息=>db
            await self.DBProxy.Save(userInfo);
            return userInfo.UserId;
        }
        /// <summary>
        /// 从数据库用户表里找到最大userId
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask<int> FetchMaxUserId(this UserComponent self)
        {
            List<User> userIdMax = await self.DBProxy.SortQuery<User>(user => true, user => user.UserId == -1, 1); //-1是降序,1是升序
            if (userIdMax.Count > 0)
            {
                self.MaxUserId = (userIdMax[0] as User).UserId;
            }
            else
            {
                self.MaxUserId = 1000;
            }
            return self.MaxUserId;
        }

    }

    [ObjectSystem]
    public class UserComponentSystem : AwakeSystem<UserComponent>
    {
        public override async void Awake(UserComponent self)
        {
            self.Awake();
            await self.FetchMaxUserId();
        }
    }
}
