using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.User)]
    class RU_LoginHandler : AMRpcHandler<RU_Login, UR_Login>
    {
        protected override async ETTask Run(Session session, RU_Login request, UR_Login response, Action reply)
        {
            var userId = await UserComponent.Instance.CheckLogin(request, response);
            response.UserId = userId;
            reply();
        }
    }
}
