using System;
using System.IO;
using System.Text;
using System.Threading;
using ETModel;
using NLog;

namespace App
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            // 在主线程调用异步方法全部会回调到主线程
            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
            Game.EventSystem.Add(DLLType.Model, typeof(Game).Assembly);
            Game.EventSystem.Add(DLLType.Hotfix, DllHelper.GetHotfixAssembly());

            Options options = Game.Scene.AddComponent<OptionComponent, string[]>(args).Options;
            StartConfig startConfig = Game.Scene.AddComponent<StartConfigComponent, string, int>(options.Config, options.AppId).StartConfig;
            if (!options.AppType.Is(startConfig.AppType))
            {
                Log.Error("命令行参数apptype与配置不一致");
                return;
            }
            Console.Title = $"{startConfig.AppId}--{startConfig.AppType}";
            IdGenerater.AppId = options.AppId;
            LogManager.Configuration.Variables["appType"] = $"{startConfig.AppType}";
            LogManager.Configuration.Variables["appId"] = $"{startConfig.AppId}";
            LogManager.Configuration.Variables["appTypeFormat"] = $"{startConfig.AppType,-8}";
            LogManager.Configuration.Variables["appIdFormat"] = $"{startConfig.AppId:0000}";
            Log.Info("进程启动参数: " + options.AppId + " " + options.AppType + " " + options.Config + " " + options.RuntimeMode);
            OuterConfig outerConfig = startConfig.GetComponent<OuterConfig>();
            ClientConfig clientConfig = startConfig.GetComponent<ClientConfig>();
            //添加通用组件
            AddCommonComponents();
            ///////////////////////根据不同服务器类型添加相应组件//////////////////////
            switch (startConfig.AppType)
            {
                case AppType.Manager:
                    Game.Scene.AddComponent<AppManagerComponent>();
                    Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol, string>(outerConfig.Protocol, outerConfig.Address);
                    break;
                case AppType.Realm:
                    Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol, string>(outerConfig.Protocol, outerConfig.Address);
                    Game.Scene.AddComponent<RealmOnlineUserComponent>();
                    break;
                case AppType.DB:
                    Game.Scene.AddComponent<DBComponent>();
                    break;
                case AppType.Gate:
                    Game.Scene.AddComponent<GateUserComponent>();
                    Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol, string>(outerConfig.Protocol, outerConfig.Address);
                    Game.Scene.AddComponent<GateSessionKeyComponent>();
                    break;
                case AppType.Http:
                    //Game.Scene.AddComponent<HttpComponent>();
                    Game.Scene.AddComponent<HttpServerComponent>();
                    break;
                case AppType.Location:
                    Game.Scene.AddComponent<LocationComponent>();
                    break;
                case AppType.World:
                    Game.Scene.AddComponent<UserComponent>();
                    Game.Scene.AddComponent<GameConfigComponent>();
                    break;
                case AppType.Match:
                    Game.Scene.AddComponent<MatchRoomComponent>();
                    break;
                case AppType.Game:
                    Game.Scene.AddComponent<GameRoomComponent>();
                    break;
                case AppType.Robot:
                    Game.Scene.AddComponent<RobotComponent>();
                    break;
                case AppType.AllServer:
                    Game.Scene.AddComponent<AppManagerComponent>();
                    // location server
                    Game.Scene.AddComponent<LocationComponent>();
                    //db  server
                    Game.Scene.AddComponent<DBComponent>();
                    //http server
                    //Game.Scene.AddComponent<HttpComponent>();
                    Game.Scene.AddComponent<HttpServerComponent>();
                    // 外网消息组件
                    Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol, string>(outerConfig.Protocol, outerConfig.Address);
                    //realm验证服
                    Game.Scene.AddComponent<RealmOnlineUserComponent>();
                    //gate server
                    Game.Scene.AddComponent<GateSessionKeyComponent>();
                    Game.Scene.AddComponent<GateUserComponent>();
                    //world server
                    Game.Scene.AddComponent<UserComponent>();
                    Game.Scene.AddComponent<GameConfigComponent>();
                    //房间匹配服
                    Game.Scene.AddComponent<MatchRoomComponent>();
                    //游戏逻辑服
                    Game.Scene.AddComponent<GameRoomComponent>();
                    //机器人服
                    Game.Scene.AddComponent<RobotComponent>();
                    break;
                case AppType.BenchmarkTCPClient:
                    Game.Scene.AddComponent<BenchmarkComponent, NetworkProtocol, string>(NetworkProtocol.TCP, clientConfig.Address);
                    break;
                case AppType.BenchmarkTCPServer:
                    Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol, string>(NetworkProtocol.TCP, outerConfig.Address);
                    break;
                case AppType.BenchmarkWebsocketClient:
                    Game.Scene.AddComponent<BenchmarkComponent, NetworkProtocol, string>(NetworkProtocol.WebSocket, clientConfig.Address);
                    break;
                case AppType.BenchmarkWebsocketServer:
                    Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol, string>(NetworkProtocol.WebSocket, outerConfig.Address);
                    break;
                case AppType.BenchmarkKCPClient:
                    Game.Scene.AddComponent<BenchmarkComponent, NetworkProtocol, string>(NetworkProtocol.KCP, clientConfig.Address);
                    break;
                case AppType.BenchmarkKCPServer:
                    Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol, string>(NetworkProtocol.KCP, outerConfig.Address);
                    break;
                case AppType.ClientH:
                    Game.Scene.AddComponent<ClientComponent, NetworkProtocol>(clientConfig.Protocol);
                    break;
                default:
                    throw new Exception($"命令行参数没有设置正确的AppType: {startConfig.AppType}");
            }

            while (true)
            {
                try
                {
                    Thread.Sleep(1);
                    OneThreadSynchronizationContext.Instance.Update();
                    Game.EventSystem.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error("捕获未处理异常: " + e.ExceptionObject);
        }

        private static void AddCommonComponents()
        {
            StartConfig startConfig = StartConfigComponent.Instance.StartConfig;
            InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            //内网网络组件
            if (innerConfig != null)
            {
                Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
            }
            //Actor消息相关
            Game.Scene.AddComponent<MailboxDispatcherComponent>();
            Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
            Game.Scene.AddComponent<ActorMessageSenderComponent>();
            Game.Scene.AddComponent<ActorLocationSenderComponent>();
            //代理相关
            Game.Scene.AddComponent<LocationProxyComponent>();
            Game.Scene.AddComponent<DBProxyComponent>();
            // 配置管理
            Game.Scene.AddComponent<ConfigComponent>();
            //控制台相关
            Game.Scene.AddComponent<ConsoleComponent>();
        }
    }
}
