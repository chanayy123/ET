using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.DB)]
	public class DBDeleteJsonRequestHandler : AMRpcHandler<DBDeleteJsonRequest, DBDeleteJsonResponse>
	{
		protected override async ETTask Run(Session session, DBDeleteJsonRequest request, DBDeleteJsonResponse response, Action reply)
		{
			long delCount = await Game.Scene.GetComponent<DBComponent>().GetDeleteJson(request.CollectionName, request.Json);
            response.Count = (int)delCount;
			reply();
		}
	}
}