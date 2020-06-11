using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.World)]
    class SW_LockMaxUserIdHandler : AMRpcHandler<SW_LockMaxUserId, WS_LockMaxUserId>
    {
        protected override  ETTask Run(Session session, SW_LockMaxUserId request, WS_LockMaxUserId response, Action reply)
        {
            UserComponent.Instance.IsLocking = true;
            response.MaxUserId = UserComponent.Instance.MaxUserId;
            reply();
            return ETTask.CompletedTask;
        }
    }

    [MessageHandler(AppType.World)]
    class SW_UnlockMaxUserIdHandler : AMHandler<SW_UnlockMaxUserId>
    {
        protected override  ETTask Run(Session session, SW_UnlockMaxUserId message)
        {
            UserComponent.Instance.IsLocking = false;
            UserComponent.Instance.MaxUserId = message.MaxUserId;
            return ETTask.CompletedTask; 
        }
    }
}
