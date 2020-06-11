using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class WS_UserInfoChangedHandler : AMHandler<WS_UserInfoChanged>
    {
        protected override async ETTask Run(Session session, WS_UserInfoChanged message)
        {
            var userCache = UserCacheComponent.Instance;
            if(userCache == null)
            {
                Log.Warning("当前进程缺少UserCacheComponent组件!");
                return;
            }
            var user = userCache.Get(message.UserId);
            if(user == null)
            {
                Log.Warning($"当前进程没有用户{message.UserId}");
                return;
            }
            var wrapper = userCache.GetWrapper(message.UserId);
            wrapper[message.Key] = message.Value;
            await ETTask.CompletedTask;
        }
    }
}
