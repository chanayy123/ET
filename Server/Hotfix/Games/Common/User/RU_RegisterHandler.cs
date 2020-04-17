using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.User)]
    class RU_RegisterHandler : AMRpcHandler<RU_Register, UR_Register>
    {
        protected override async ETTask Run(Session session, RU_Register request, UR_Register response, Action reply)
        {
            var userId = await UserComponent.Instance.CheckRegister(request, response);
            response.UserId = userId;
            reply();
        }
    }
}
