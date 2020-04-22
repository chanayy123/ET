using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class UserFactory
    {
        public static SU_GetUserInfo CreateMsgRU_Login(int userId)
        {
            SU_GetUserInfo msg = SimplePool.Instance.Fetch<SU_GetUserInfo>();
            msg.UserId = userId;
            return msg;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }
    }
}
