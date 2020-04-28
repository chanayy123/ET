using System;
using System.Threading;
using ETModel;
using NLog;

namespace App
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			// 异步方法全部会回掉到主线程
			SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
			
			try
			{			
				Game.EventSystem.Add(DLLType.Model, typeof(Game).Assembly);
				Game.EventSystem.Add(DLLType.Hotfix, DllHelper.GetHotfixAssembly());

				Options options = Game.Scene.AddComponent<OptionComponent, string[]>(args).Options;
				StartConfig startConfig = Game.Scene.AddComponent<StartConfigComponent, string, int>(options.Config, options.AppId).StartConfig;

				if (!options.AppType.Is(startConfig.AppType))
				{
					Log.Error("命令行参数apptype与配置不一致");
					return;
				}

                IdGenerater.AppId = options.AppId;

				LogManager.Configuration.Variables["appType"] = $"{startConfig.AppType}";
				LogManager.Configuration.Variables["appId"] = $"{startConfig.AppId}";
				LogManager.Configuration.Variables["appTypeFormat"] = $"{startConfig.AppType, -8}";
				LogManager.Configuration.Variables["appIdFormat"] = $"{startConfig.AppId:0000}";

				Log.Info($"server start........................ {startConfig.AppId} {startConfig.AppType}");
                // 根据不同的AppType添加不同的组件
                OuterConfig outerConfig = startConfig.GetComponent<OuterConfig>();
				InnerConfig innerConfig = startConfig.GetComponent<InnerConfig>();
				ClientConfig clientConfig = startConfig.GetComponent<ClientConfig>();
                ///////////////////////添加通用组件///////////////////////////////////////
                Game.Scene.AddComponent<TimerComponent>();
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                //内网网络组件
                Game.Scene.AddComponent<NetInnerComponent, string>(innerConfig.Address);
                //非actor消息转发组件
                //Game.Scene.AddComponent<MessageDispatcherComponent>();
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
                ///////////////////////根据不同服务器类型添加相应组件//////////////////////
                switch (startConfig.AppType)
				{
					case AppType.Manager:
						Game.Scene.AddComponent<AppManagerComponent>();
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						break;
					case AppType.Realm:
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
                        Game.Scene.AddComponent<RealmOnlineUserComponent>();
                        break;
                    case AppType.DB:
                        Game.Scene.AddComponent<DBComponent>();
                        break;
					case AppType.Gate:
						Game.Scene.AddComponent<GateUserComponent>();
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						Game.Scene.AddComponent<GateSessionKeyComponent>();
						break;
                    case AppType.Http:
                        Game.Scene.AddComponent<HttpComponent>();
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
                    case AppType.AllServer:
                        // location server
                        Game.Scene.AddComponent<LocationComponent>();
                        //db  server
                        Game.Scene.AddComponent<DBComponent>();
                        //http server
                        Game.Scene.AddComponent<HttpComponent>();
                        // 外网消息组件
                        Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
                        //realm验证服
                        Game.Scene.AddComponent<RealmOnlineUserComponent>();
                        //gate server
                        Game.Scene.AddComponent<GateSessionKeyComponent>();
                        Game.Scene.AddComponent<GateUserComponent>();
                        //user server
                        Game.Scene.AddComponent<UserComponent>();
                        Game.Scene.AddComponent<GameConfigComponent>();
                        //房间匹配服
                        Game.Scene.AddComponent<MatchRoomComponent>();
                        //游戏逻辑服
                        Game.Scene.AddComponent<GameRoomComponent>();
                        break;
					case AppType.Benchmark:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<BenchmarkComponent, string>(clientConfig.Address);
						break;
					case AppType.BenchmarkWebsocketServer:
						Game.Scene.AddComponent<NetOuterComponent, string>(outerConfig.Address);
						break;
					case AppType.BenchmarkWebsocketClient:
						Game.Scene.AddComponent<NetOuterComponent>();
						Game.Scene.AddComponent<WebSocketBenchmarkComponent, string>(clientConfig.Address);
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
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

    }
}
