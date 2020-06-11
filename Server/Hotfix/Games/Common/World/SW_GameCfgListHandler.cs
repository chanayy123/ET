using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.World)]
    class SW_GetGameCfgListHandler : AMRpcHandler<SW_GetGameCfgList, WS_GetGameCfgList>
    {
        protected override async ETTask Run(Session session, SW_GetGameCfgList request, WS_GetGameCfgList response, Action reply)
        {
            response.GameCfgList.Clear();
            response.GameCfgList.AddRange(GameConfigComponent.Instance.cfgList);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
