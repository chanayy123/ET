using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class UserHelper
    {
        public static async ETTask<User> GetUserInfo(int userId)
        {
            var userS = NetInnerHelper.GetSessionByAppType(AppType.User);
            var usRes = (US_GetUserInfo)await userS.Call(new SU_GetUserInfo()
            {
                UserId = userId
            });
            return usRes.UserInfo;
        }

    }
}
