using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.World)]
    class RW_LoginHandler : AMRpcHandler<RW_Login, WR_Login>
    {
        protected override async ETTask Run(Session session, RW_Login request, WR_Login response, Action reply)
        {
            var userId = await UserComponent.Instance.CheckLogin(request, response);
            response.UserId = userId;
            reply();
        }
    }
}
