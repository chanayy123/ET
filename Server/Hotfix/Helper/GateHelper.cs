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
            if (startCfg.AppType == AppType.AllServer)
            {
                var innerSession = NetInnerHelper.GetSessionByCfg(startCfg);
                innerSession.Send(new GS_Online() { UserId = userId, GateSessionId = gateSessionId });
            }
            else
            {
                var realS = NetInnerHelper.GetSessionByAppType(AppType.Realm);
                realS.Send(new GS_Online() { UserId = userId, GateSessionId = gateSessionId });
                var userS = NetInnerHelper.GetSessionByAppType(AppType.User);
                userS.Send(new GS_Online() { UserId = userId, GateSessionId = gateSessionId });
            }
            var gateUser = GateUserComponent.Instance.Get(userId);
            if (gateUser.ActorId != 0)
            {
                var gateSID = gateUser.GetComponent<GateUserSessionComponent>().Session.InstanceId;
                NetInnerHelper.SendActorMsg(new Actor_OnlineState()
                {
                    ActorId = gateUser.ActorId,
                    State = 1,
                    GateSessionId = gateSID
                });
        }
    }

        public static void SynOffline(GateUser user)
        {
            var startCfg = StartConfigComponent.Instance.StartConfig;
            if (startCfg.AppType == AppType.AllServer)
            {
                var innerSession = NetInnerHelper.GetSessionByCfg(startCfg);
                innerSession.Send(new GS_Offline() { UserId = user.UserId});
            }
            else
            {
                var realS = NetInnerHelper.GetSessionByAppType(AppType.Realm);
                realS.Send(new GS_Offline() { UserId = user.UserId });
                var userS = NetInnerHelper.GetSessionByAppType(AppType.User);
                userS.Send(new GS_Offline() { UserId = user.UserId });
                var matchS = NetInnerHelper.GetSessionByAppType(AppType.Match);
                matchS.Send(new GS_Offline() { UserId = user.UserId });
            }
            if(user.ActorId != 0)
            {
                NetInnerHelper.SendActorMsg(new Actor_OnlineState()
                {
                    ActorId = user.ActorId,
                    State = 0,
                    GateSessionId = 0
                });
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
