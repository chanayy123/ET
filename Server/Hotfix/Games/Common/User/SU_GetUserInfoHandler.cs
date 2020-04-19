using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.User)]
    class SU_GetUserInfoHandler : AMRpcHandler<SU_GetUserInfo, US_GetUserInfo>
    {
        protected override async ETTask Run(Session session, SU_GetUserInfo request, US_GetUserInfo response, Action reply)
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
            reply();
        }
    }
}
