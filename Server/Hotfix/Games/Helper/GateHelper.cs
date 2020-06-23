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
                var userS = NetInnerHelper.GetSessionByAppType(AppType.World);
                userS.Send(msg);
            }
            GateFactory.RecycleMsg(msg);
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
                var userS = NetInnerHelper.GetSessionByAppType(AppType.World);
                userS.Send(msg);
                var matchS = NetInnerHelper.GetSessionByAppType(AppType.Match);
                matchS.Send(msg);
            }
            GateFactory.RecycleMsg(msg);
        }

        /// <summary>
        /// 随机获取一个网关服务器配置
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
