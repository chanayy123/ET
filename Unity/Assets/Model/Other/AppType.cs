using System;
using System.Collections.Generic;

namespace ETModel
{
	[Flags]
	public enum AppType
	{
		None = 0,
		Manager = 1,
		Realm = 1 << 1,
		Gate = 1 << 2,
		Http = 1 << 3,
		DB = 1 << 4,
		Location = 1 << 5,
		Map = 1 << 6,
        Match = 1<<7,
        Game= 1<<8,
        World = 1<<9,  //世界服:处理全局数据,比如用户数据,游戏配置数据,还有其他等等


        BenchmarkTCPClient = 1 << 23,
        BenchmarkTCPServer = 1 << 24,
        BenchmarkWebsocketServer = 1 << 25,
		BenchmarkWebsocketClient = 1 << 26,
        BenchmarkKCPServer = 1 << 27,
        BenchmarkKCPClient = 1 << 28,
        Robot = 1 << 29,
		// 客户端Hotfix层
		ClientH = 1 << 30,
		// 客户端Model层
		ClientM = 1 << 31,

		// 7
		AllServer = Manager | Realm | Gate | Http | DB | Location | Map | Match | Game | World | Robot | BenchmarkWebsocketServer  | BenchmarkTCPServer | BenchmarkKCPServer
    }

	public static class AppTypeHelper
	{
		public static List<AppType> GetServerTypes()
		{
			List<AppType> appTypes = new List<AppType> { AppType.Manager, AppType.Realm, AppType.Gate };
			return appTypes;
		}

		public static bool Is(this AppType a, AppType b)
		{
			if ((a & b) != 0)
			{
				return true;
			}
			return false;
		}

        public static bool IsBenchmark(this AppType type)
        {
            return type == AppType.BenchmarkKCPClient || type == AppType.BenchmarkKCPServer || type == AppType.BenchmarkTCPClient || type == AppType.BenchmarkTCPServer
                || type == AppType.BenchmarkWebsocketClient || type == AppType.BenchmarkWebsocketServer;
        }
    }
}