using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.World)]
    class RW_RegisterHandler : AMRpcHandler<RW_Register, WR_Register>
    {
        protected override async ETTask Run(Session session, RW_Register request, WR_Register response, Action reply)
        {
            var userId = await UserComponent.Instance.CheckRegister(request, response);
            response.UserId = userId;
            reply();
        }
    }
}
