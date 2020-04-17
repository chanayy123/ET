using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.DB)]
    public class DBSortQueryJsonRequestHandler : AMRpcHandler<DBSortQueryJsonRequest, DBQueryJsonResponse>
    {
        private string[] strs = new string[3];
        protected override async ETTask Run(Session session, DBSortQueryJsonRequest message, DBQueryJsonResponse response, Action reply)
        {
            DBComponent db = Game.Scene.GetComponent<DBComponent>();
            strs[0] = message.CollectionName;
            strs[1] = message.QueryJson;
            strs[2] = message.SortJson;
            List<ComponentWithId> components = await db.GetJson(strs, message.Count);
            response.Components = components;
            reply();
        }
    }
}