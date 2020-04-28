using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class RealmFactory
    {
        public static RW_Login CreateMsgRW_Login(int LoginType, string dataStr)
        {
            RW_Login msg = SimplePool.Instance.Fetch<RW_Login>();
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
        public static RW_Register CreateMsgRW_Register(string acc,string name,string pwd)
        {
            RW_Register msg = SimplePool.Instance.Fetch<RW_Register>();
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
