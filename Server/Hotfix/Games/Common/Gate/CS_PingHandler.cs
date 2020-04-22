using System;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.AllServer)]
	public class CS_PingHandler : AMRpcHandler<CS_Ping, SC_Ping>
	{
		protected override async ETTask Run(Session session, CS_Ping request, SC_Ping response, Action reply)
		{
			reply();
			await ETTask.CompletedTask;
		}
	}
}