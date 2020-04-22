using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class RealmFactory
    {
        public static RU_Login CreateMsgRU_Login(int LoginType, string dataStr)
        {
            RU_Login msg = SimplePool.Instance.Fetch<RU_Login>();
            msg.LoginType = LoginType;
            msg.DataStr = dataStr;
            return msg;
        }
        public static R2G_GetLoginKey CreateMsgR2G_GetLoginKey(int userId)
        {
            R2G_GetLoginKey msg = SimplePool.Instance.Fetch<R2G_GetLoginKey>();
            msg.UserId = userId;
            return msg;
        }
        public static RU_Register CreateMsgRU_Register(string acc,string name,string pwd)
        {
            RU_Register msg = SimplePool.Instance.Fetch<RU_Register>();
            msg.Account = acc;
            msg.Name = name;
            msg.Password = pwd;
            return msg;
        }

        public static SG_KickUser CreateMsgSG_KickUser(int userId)
        {
            SG_KickUser msg = SimplePool.Instance.Fetch<SG_KickUser>();
            msg.UserId = userId;
            return msg;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }
    }
}
