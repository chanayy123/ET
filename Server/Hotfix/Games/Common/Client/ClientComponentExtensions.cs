using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ETHotfix
{

    public static class ClientComponentExtensions
    {

        public static async ETTask InitClients(this ClientComponent self)
        {
            try
            {
                Game.Scene.AddComponent<RoomConfigComponent>();
                //获取测试账号,数量不够就注册
                var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
                List<ComponentWithId> list = await dbProxy.Query<UserInfo>((item) => item.IsTest == true);
                if (list.Count < ClientComponent.INIT_COUNT)
                {
                    for (var i = 0; i < list.Count; ++i)
                    {
                        self.AddClient(list[i] as UserInfo);
                    }
                    await self.InitClients(ClientComponent.INIT_COUNT - list.Count);
                }
                else
                {
                    for (var i = 0; i < ClientComponent.INIT_COUNT; ++i)
                    {
                        self.AddClient(list[i] as UserInfo);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Warning("ClientComponent InitMatch exception: " + e);
            }
        }

        private static async ETTask InitClients(this ClientComponent self, int count)
        {
            int maxUserId = await WorldHelper.LockMaxUserId();
            var accList = new List<ComponentWithId>();
            var userList = new List<ComponentWithId>();
            var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
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
                userInfo.IsTest = true;
                userList.Add(userInfo);
                self.AddClient(userInfo);
            }
            await dbProxy.SaveBatch(accList);
            await dbProxy.SaveBatch(userList);
            WorldHelper.UnlockMaxUserId(maxUserId);
            accList.Clear();
            userList.Clear();
        }

        public static void AddClient(this ClientComponent self, UserInfo userInfo)
        {
            self.clientList.Add(userInfo);
        }

        public static void AddSession(this ClientComponent self, int userId, Session s)
        {
            if (!self.clientSessionDic.TryAdd(userId, s))
            {
                Log.Warning("ClientComponent: 重复添加用户");
            }
        }


        public static Session GetSession(this ClientComponent self,int userId)
        {
            self.clientSessionDic.TryGetValue(userId, out Session session);
            return session;
        }

        public static void RemoveSession(this ClientComponent self, int userId)
        {
            self.clientSessionDic.Remove(userId);
        }

        /// <summary>
        /// 模拟客户端登陆
        /// </summary>
        /// <param name="self"></param>
        /// <param name="count"></param>
        private static void StartLogin(this ClientComponent self, int count=0, bool match=true)
        {
            Log.Debug($"{count}个测试客户端准备登陆!");
            count = Math.Min(count,self.clientList.Count);
            var serverCfg = StartConfigComponent.Instance.StartConfig.GetComponent<ClientConfig>();
            for(var i = 0; i < count; ++i)
            {
                var userId = self.clientList[i].UserId;
                self.clientSessionDic.TryGetValue(userId, out Session session);
                if (session == null)
                {
                    session = self.Net.Create(serverCfg.Address);
                    self.AddSession(userId, session);
                    self.RequestLogin(userId,match);
                }
            }
        }

        private static async void RequestLogin(this ClientComponent self,int userId,bool match=true)
        {
            CS_Login msg = SimplePool.Instance.Fetch<CS_Login>();
            msg.LoginType = (int)LoginType.TestClient;
            msg.DataStr = userId.ToString();
            var session = self.GetSession(userId);
            try
            {
                var res = (SC_Login)await session.Call(msg);
                if (res.Error == 0)
                {
                    var serverCfg = StartConfigComponent.Instance.RealmConfig.GetComponent<OuterConfig>();
                    if (serverCfg.Address2.Equals(res.Address))
                    {
                        CS_VerifyKey msg2 = SimplePool.Instance.Fetch<CS_VerifyKey>();
                        msg2.Key = res.Key;
                        var res2 = (SC_VerifyKey)await session.Call(msg2);
                        if (res2.Error == 0)
                        {
                            Log.Debug($"测试客户端登陆成功: {userId}");
                            Log.Debug("随机匹配房间");
                            if(match) self.RequestEnterScene(userId);
                        }
                        SimplePool.Instance.Recycle(msg2);
                    }
                    else
                    {
                        session.Dispose();
                        self.RemoveSession(userId);
                        session = self.Net.Create(res.Address);
                        self.AddSession(userId, session);
                        CS_VerifyKey msg2 = SimplePool.Instance.Fetch<CS_VerifyKey>();
                        msg2.Key = res.Key;
                        var res2 = (SC_VerifyKey)await session.Call(msg2);
                        if (res2.Error == 0)
                        {
                            Log.Debug($"测试客户端登陆成功: {userId}");
                            Log.Debug("随机匹配房间");
                            if (match) self.RequestEnterScene(userId);
                        }
                        SimplePool.Instance.Recycle(msg2);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Warning("ClientComponent request login exception " + e);
            }
            SimplePool.Instance.Recycle(msg);
        }

        private static async void RequestEnterScene(this ClientComponent self,int userId)
        {
            CS_EnterScene msg = SimplePool.Instance.Fetch<CS_EnterScene>();
            var cfgDic = RoomConfigComponent.Instance.roomConfigDic;
            var index = RandomHelper.RandomNumber(0, cfgDic.Keys.Count());
            var hallId = cfgDic.Keys.ElementAt(index);
            var cfg = cfgDic[hallId];
            msg.HallId = (int)hallId;
            msg.UserId = userId;
            msg.GateSessionId = 0;
            var session = self.GetSession(userId);
            SC_EnterScene res =  (SC_EnterScene)await session.Call(msg);
            if(res.Error == 0)
            {
                Log.Debug($"玩家{userId} 开始匹配大厅{hallId}");
            }
        }

        public static async void TestLoginMatch(this ClientComponent self,int count)
        {
            await self.InitClients();
            self.StartLogin(count,false);
        }

        public static async void TestHttpRequest(this ClientComponent self,int count)
        {
            Game.Scene.AddComponent<HttpClientComponent>();
            await TimerComponent.Instance.WaitAsync(2000);
            //watchThreads();
            Log.Debug("开始测试时间: " + DateTime.Now);
            for(var i = 0; i < count; ++i)
            {
                self.TestOneClientHttpRequest();
            }
            Log.Debug("结束测试时间: " + DateTime.Now);
        }

        public static async void watchThreads()
        {
            Log.Debug("开始监控线程数量");
            Process p = Process.GetCurrentProcess();
            while (true)
            {
                await TimerComponent.Instance.WaitAsync(500);
                ProcessThreadCollection ptc = p.Threads;
                Log.Debug("当前进程线程数量: " + ptc.Count);
            }
        }

        public static async void TestOneClientHttpRequest(this ClientComponent self)
        {
            //using (var httpClient = ComponentFactory.Create<HttpClientComponent>(false))
            //{
            var httpClient = Game.Scene.GetComponent<HttpClientComponent>();
                try
                {
                    httpClient.BaseAddress = new Uri("http://192.168.2.128:8080");
                }
                catch (Exception e)
                {
                    Log.Warning("httpClient.BaseAddress 异常: " + e);
                }
                var url = "testLogin?name=abc&age=123";
                Log.Debug("请求get " + url);
                var res = await httpClient.GetAsync(url);
                Log.Debug("回复get " + res);
                url = "http://192.168.2.128:8080/testGetRechargeRecord?id=123";
                Log.Debug("请求get " + url);
                res = await httpClient.GetAsync(url);
                Log.Debug("回复get " + res);
                //url = "http://192.168.2.128:8080/FetchGameConfig";
                //Log.Debug("请求get " + url);
                //res = await httpClient.GetAsync(url);
                //Log.Debug("回复get " + res);
                url = "http://192.168.2.128:8080/testTest1";
                Log.Debug("请求post " + url);
                res = await httpClient.PostAsync(url, "");
                Log.Debug("回复post " + res);
                //url = "http://192.168.2.128:8080/TestPost";
                //Log.Debug("请求post " + url);
                //var body = MongoHelper.ToJson(new LoginData { Acc = "yy" });
                //res = await httpClient.PostAsync(url, body);
                //Log.Debug("回复post " + res);
            //}
        }

    }

}
