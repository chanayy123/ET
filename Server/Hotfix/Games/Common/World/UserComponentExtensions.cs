using System;
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
            if (!flag)
            {
                Log.Warning($"{userId}已经存在,无法添加用户");
            }
        }

        public static User Get(this UserComponent self, int userId)
        {
            self.userDic.TryGetValue(userId, out User user);
            return user;
        }

        public static void AddCacheSession(this UserComponent self, Session session)
        {
            self.cacheSessionDic.TryAdd(session.Id, session);
        }

        public static void Remove(this  UserComponent self, int userId)
        {
            self.userDic.Remove(userId,out User user);
            user?.Dispose();
        }


        public static int Count(this UserComponent self)
        {
                return self.userDic.Count;
        }

        public static User[] GetAll(this UserComponent self)
        {
            return self.userDic.Values.ToArray();
        }

        public static async ETTask<int> CheckLogin(this UserComponent self ,RW_Login msg, IResponse res)
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

        public static async ETTask<int> CheckVoucherLogin(this UserComponent self,RW_Login msg, IResponse res)
        {
            var arr = msg.DataStr.Split("|");
            if (arr.Length != 2)
            {
                res.Error = (int)OpRetCode.LoginParamError;
                return 0;
            }
            List<ComponentWithId> list =  await self.DBProxy.Query<AccountInfo>((acc) => acc.Acc.Equals(arr[0]) && acc.Pwd.Equals(arr[1]));
            if(list.Count == 0)
            {
                res.Error = (int)OpRetCode.LoginAccPwdError;
                return 0;
            }
            //更新登陆时间
            var account = list[0] as AccountInfo;
            account.LastLoginTIme = DateTime.UtcNow;
            await self.DBProxy.Save(account);
            return account.UserId;
        }

        public static async ETTask<int> CheckWechatLogin(this UserComponent self, RW_Login msg, IResponse res)
        {
            await ETTask.CompletedTask;
            throw new NotImplementedException();
        }

        public static async ETTask<int> CheckTouristLogin(this UserComponent self, RW_Login msg, IResponse res)
        {
            List<ComponentWithId> list = await self.DBProxy.Query<AccountInfo>(user => user.Acc.Equals(msg.DataStr));
            if (list.Count == 0)
            {
                if (self.MaxUserId == 0)
                {
                    Log.Debug("初始化userId失败");
                    res.Error = (int)OpRetCode.AccountMaxUserIdError;
                    return 0;
                }
                if (self.IsLocking)
                {
                    Log.Debug("锁定中,不能注册");
                    res.Error = (int)OpRetCode.AccountMaxUserIdError;
                    return 0;
                }
                //创建游客账号信息
                var accInfo = ComponentFactory.Create<AccountInfo>();
                accInfo.Acc = msg.DataStr;
                accInfo.Pwd = string.Empty;
                //计算userId
                accInfo.UserId = ++self.MaxUserId;
                //同步账号信息=>db
                await self.DBProxy.Save(accInfo);
                var userInfo = ComponentFactory.Create<UserInfo>();
                userInfo.Name = $"游客{RandomHelper.RandomNumber(0,1000)}";
                userInfo.UserId = accInfo.UserId;
                userInfo.Level = 1;
                userInfo.Coin = 10000;
                //同步用户信息=>db
                await self.DBProxy.Save(userInfo);
                var userId = userInfo.UserId;
                accInfo.Dispose();
                userInfo.Dispose();
                return userId;
            }
            var account = list[0] as AccountInfo;
            //更新登陆时间
            account.LastLoginTIme = DateTime.UtcNow;
            await self.DBProxy.Save(account);
            return account.UserId;
        }

        public static async ETTask<int> CheckRegister(this UserComponent self ,RW_Register msg, IResponse res)
        {
            List<ComponentWithId> list = await self.DBProxy.Query<AccountInfo>(user => user.Acc.Equals(msg.Account));
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
            if (self.IsLocking)
            {
                Log.Debug("锁定中,不能注册");
                res.Error = (int)OpRetCode.AccountMaxUserIdError;
                return 0;
            }
            //创建账号信息
            var accInfo = ComponentFactory.Create<AccountInfo>();
            accInfo.Acc = msg.Account;
            accInfo.Pwd = msg.Password;
            //计算userId
            accInfo.UserId = ++self.MaxUserId;
            //同步账号信息=>db
            await self.DBProxy.Save(accInfo);
            //创建默认用户信息
            var userInfo = ComponentFactory.Create<UserInfo>();
            userInfo.Name = msg.Name;
            userInfo.UserId = accInfo.UserId;
            userInfo.Level = 1;
            userInfo.Coin = 10000;
            //同步用户信息=>db
            await self.DBProxy.Save(userInfo);
            var userId = userInfo.UserId;
            accInfo.Dispose();
            userInfo.Dispose();
            return userId;
        }
        /// <summary>
        /// 从数据库用户表里找到最大userId
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask<int> FetchMaxUserId(this UserComponent self)
        {
            List<UserInfo> userIdMax = await self.DBProxy.SortQuery<UserInfo>(user => true, user => user.UserId == -1, 1); //-1是降序,1是升序
            if (userIdMax.Count > 0)
            {
                self.MaxUserId = (userIdMax[0] as UserInfo).UserId;
            }
            else
            {
                self.MaxUserId = 1000;
            }
            return self.MaxUserId;
        }
    }

}
