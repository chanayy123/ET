using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.World)]
    class SW_UpdateUserInfoHandler : AMRpcHandler<SW_UpdateUserInfo, WS_UpdateUserInfo>
    {
        protected override async ETTask Run(Session session, SW_UpdateUserInfo request, WS_UpdateUserInfo response, Action reply)
        {
            Game.EventSystem.Run(EventType.PropertyChange + request.Key, request.UserId, request.Value,response,reply);
            //用户信息变更,同步更新其他服务器缓存
            WorldHelper.UpdateUserInoCache(request.UserId, request.Key, request.Value);
            await ETTask.CompletedTask;
        }
    }

    [MessageHandler(AppType.World)]
    class SW_GetUserInfoHandler : AMRpcHandler<SW_GetUserInfo, WS_GetUserInfo>
    {
        protected override async ETTask Run(Session session, SW_GetUserInfo request, WS_GetUserInfo response, Action reply)
        {
            User user = UserComponent.Instance.Get(request.UserId);
            if (user == null)
            {
                List<ComponentWithId> list = await UserComponent.Instance.DBProxy.Query<UserInfo>((u) => u.UserId == request.UserId);
                if (list.Count == 0)
                {
                    Log.Warning($"用户{request.UserId}获取信息失败");
                    response.Error = (int)OpRetCode.UserGetInfoError;
                    reply();
                    return;
                }
                user = ComponentFactory.Create<User, UserInfo>(list[0] as UserInfo);
                UserComponent.Instance.Add(request.UserId, user);
                response.UserInfo = user;
            }
            else
            {
                response.UserInfo = user;
            }
            UserComponent.Instance.AddCacheSession(session);
            reply();
        }
    }
}
