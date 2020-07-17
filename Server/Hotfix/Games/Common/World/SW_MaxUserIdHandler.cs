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
            if (UserComponent.Instance.IsLocking)//已经锁定,再次锁定就返回-1
            {
                response.MaxUserId = -1;
                reply();
                return ETTask.CompletedTask;
            }
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
