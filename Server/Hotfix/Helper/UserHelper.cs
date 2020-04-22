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
            SU_GetUserInfo msg = UserFactory.CreateMsgRU_Login(userId);
            var usRes = (US_GetUserInfo)await userS.Call(msg);
            UserFactory.RecycleMsg(msg);
            return usRes.UserInfo;
        }

    }
}
