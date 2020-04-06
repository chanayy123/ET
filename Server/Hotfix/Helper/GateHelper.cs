using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    public static class GateHelper
    {
        private static Session realSession;
        public static Session RealSession
        {
            get
            {
                if(realSession == null)
                {
                    StartConfig cfg = StartConfigComponent.Instance.RealmConfig;
                    var ip =  cfg.GetComponent<InnerConfig>().IPEndPoint;
                    realSession = Game.Scene.GetComponent<NetInnerComponent>().Get(ip);
                }
                return realSession;
            }
        }

        public static void synOnline(int userId,int gateAppId )
        {
            RealSession.Send(new GR_Online() { UserId = userId, GateAppId = gateAppId });
        }

        public static void synOffline(int userId)
        {
            RealSession.Send(new GR_Offline() { UserId = userId});
        }

    }
}
