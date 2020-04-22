using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class GateHelper
    {

        public static void SynOnline(int userId,long gateSessionId )
        {
            var startCfg = StartConfigComponent.Instance.StartConfig;
            GS_Online msg = GateFactory.CreateMsgGS_Online(userId, gateSessionId);
            if (startCfg.AppType == AppType.AllServer)
            {
                var innerSession = NetInnerHelper.GetSessionByCfg(startCfg);
                innerSession.Send(msg);
            }
            else
            {
                var realS = NetInnerHelper.GetSessionByAppType(AppType.Realm);
                realS.Send(msg);
                var userS = NetInnerHelper.GetSessionByAppType(AppType.User);
                userS.Send(msg);
            }
            GateFactory.RecycleMsg(msg);
            var gateUser = GateUserComponent.Instance.Get(userId);
            if (gateUser.ActorId != 0)
            {
                var gateSID = gateUser.GetComponent<GateUserSessionComponent>().Session.InstanceId;
                Actor_OnlineState msg2 = GateFactory.CreateMsgActor_OnlineState(gateUser.ActorId, 1, gateSID);
                NetInnerHelper.SendActorMsg(msg2);
                GateFactory.RecycleMsg(msg2);
        }
    }

        public static void SynOffline(GateUser user)
        {
            var startCfg = StartConfigComponent.Instance.StartConfig;
            GS_Offline msg = GateFactory.CreateMsgGS_Offline(user.UserId);
            if (startCfg.AppType == AppType.AllServer)
            {
                var innerSession = NetInnerHelper.GetSessionByCfg(startCfg);
                innerSession.Send(msg);
            }
            else
            {
                var realS = NetInnerHelper.GetSessionByAppType(AppType.Realm);
                realS.Send(msg);
                var userS = NetInnerHelper.GetSessionByAppType(AppType.User);
                userS.Send(msg);
                var matchS = NetInnerHelper.GetSessionByAppType(AppType.Match);
                matchS.Send(msg);
            }
            GateFactory.RecycleMsg(msg);
            if(user.ActorId != 0)
            {
                Actor_OnlineState msg2 = GateFactory.CreateMsgActor_OnlineState(user.ActorId, 0, 0);
                NetInnerHelper.SendActorMsg(msg2);
                GateFactory.RecycleMsg(msg2);
            }
        }

        /// <summary>
        /// 随机获取一个网关服务器地址
        /// </summary>
        /// <returns></returns>
        public static StartConfig RandomGateCfg
        {
            get{
                var cfgs = StartConfigComponent.Instance.GateConfigs;
                var index = RandomHelper.RandomNumber(0, cfgs.Count);
                return cfgs[index];
            }
        }

    }
}
