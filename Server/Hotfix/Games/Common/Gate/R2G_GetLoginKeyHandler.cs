using System;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class R2G_GetLoginKeyHandler : AMRpcHandler<R2G_GetLoginKey, G2R_GetLoginKey>
	{
		protected override async ETTask Run(Session session, R2G_GetLoginKey request, G2R_GetLoginKey response, Action reply)
		{
			string key = RandomHelper.RandInt64().ToString();
			Game.Scene.GetComponent<GateSessionKeyComponent>().Add(key, request.UserId);
			response.Key = key;
			reply();
			await ETTask.CompletedTask;
		}
	}
}