using System;
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

        public static Actor_OnlineState CreateMsgActor_OnlineState(long actorId,int state,long gateSessionId)
        {
            Actor_OnlineState msg = SimplePool.Instance.Fetch<Actor_OnlineState>();
            msg.ActorId = actorId;
            msg.State = state;
            msg.GateSessionId = gateSessionId;
            return msg;
        }

        public static void RecycleMsg(object msg)
        {
            SimplePool.Instance.Recycle(msg);
        }
    }
}
