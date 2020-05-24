﻿using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    public static class GateFactory
    {
        public static GS_Online CreateMsgGS_Online(int userId,long gateSessionId)
        {
            GS_Online msg = SimplePool.Instance.Fetch<GS_Online>();
            msg.UserId = userId;
            msg.GateSessionId = gateSessionId;
            return msg;
        }

        public static GS_Offline CreateMsgGS_Offline(int userId)
        {
            GS_Offline msg = SimplePool.Instance.Fetch<GS_Offline>();
            msg.UserId = userId;
            return msg;
        }

        public static Actor_OnlineState CreateMsgActor_OnlineState(long actorId,bool flag,long gateSessionId)
        {
            Actor_OnlineState msg = SimplePool.Instance.Fetch<Actor_OnlineState>();
            msg.ActorId = actorId;
            msg.Flag = flag;
            msg.GateSessionId = gateSessionId;
            return msg;
        }

        public static SC_KickUser CreateMsgSC_KickUser(int error=0,string errorMsg="")
        {
            SC_KickUser msg = SimplePool.Instance.Fetch<SC_KickUser>();
            msg.Error = error;
            msg.Message = errorMsg;
            return msg;
        }

        public static GateUser CreateUser(int userId,Session session)
        {
            var user = ComponentFactory.Create<GateUser>();
            user.UserId = userId;
            user.Session = session;
            user.ActorId = 0;
            return user;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }
    }
}
