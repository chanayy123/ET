using System;
using System.Net;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class BenchmarkComponentSystem : AwakeSystem<BenchmarkComponent, NetworkProtocol,  string>
	{
		public override void Awake(BenchmarkComponent self, NetworkProtocol protocol,string address)
		{
			self.Awake(protocol,address);
		}
	}

	public static class BenchmarkComponentHelper
	{
		public static void Awake(this BenchmarkComponent self, NetworkProtocol protocol,string address)
		{
			try
			{
                NetOuterComponent networkComponent = Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol>(protocol);
				for (int i = 0; i < 2000; i++) //2000
				{
					self.TestAsync(networkComponent, address, i);
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static async void TestAsync(this BenchmarkComponent self, NetOuterComponent networkComponent, string address, int j)
		{
			try
			{
                using (Session session = networkComponent.Create(address))
                {
                    int i = 0;
					while (i < 100000000)//100000000
                    {
						++i;
						await self.Send(session, j);
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static async Task Send(this BenchmarkComponent self, Session session, int j)
		{
			try
			{
                //var msg = SimplePool.Instance.Fetch<CS_Ping>();
				await session.Call(new CS_Ping());
                //SimplePool.Instance.Recycle(msg)
				++self.k;

				if (self.k % 100000 != 0)
				{
					return;
				}

				long time2 = TimeHelper.ClientNow();
				long time = time2 - self.time1;
				self.time1 = time2;
				Log.Info($"Benchmark k: {self.k} 每10万次耗时: {time} ms {session.Network.Count}");
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}