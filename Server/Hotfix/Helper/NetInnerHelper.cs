using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    /// <summary>
    /// 获取内网各服务器连接
    /// </summary>
    public static class NetInnerHelper
    {
        public static Session GetSessionByAppId(int gateAppId)
        {
            Session session=null;
            if (gateAppId != 0)
            {
                var cfg = Game.Scene.GetComponent<StartConfigComponent>().Get(gateAppId);
                var ip = cfg.GetComponent<InnerConfig>().IPEndPoint;
                session = Game.Scene.GetComponent<NetInnerComponent>().Get(ip);
            }
            return session;
        }

        public static Session GetSessionByAppType(AppType type,int index=0)
        {
            Session session = null;
            StartConfig cfg = StartConfigComponent.Instance.Get(type,index);
            if (cfg == null) return null;
            var ip = cfg.GetComponent<InnerConfig>().IPEndPoint;
            session = Game.Scene.GetComponent<NetInnerComponent>().Get(ip);
            return session;
        }

        public static Session GetSessionByCfg(StartConfig cfg)
        {
            var ip = cfg.GetComponent<InnerConfig>().IPEndPoint;
            var session = Game.Scene.GetComponent<NetInnerComponent>().Get(ip);
            return session;
        }

        /// <summary>
        /// 根据消息字段actorid获取所在进程ip,发送actor消息
        /// </summary>
        /// <param name="msg"></param>
        public static void SendActorMsg( IActorMessage msg)
        {
            var session = GetSessionByAppId(IdGenerater.GetAppId(msg.ActorId));
            session.Send(msg);
        }
        /// <summary>
        /// 根据消息actor字段发送actor请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async ETTask<IResponse> CallActorMsg(IActorRequest request)
        {
            var session = GetSessionByAppId(IdGenerater.GetAppId(request.ActorId));
            IResponse res = await session.Call(request);
            return res;
        }

        /// <summary>
        /// 根据actorid获取所在进程ip,发送普通消息
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="msg"></param>
        public static void SendMsgByAcotrId(long actorId, IMessage msg)
        {
            var session = GetSessionByAppId(IdGenerater.GetAppId(actorId));
            session.Send(msg);
        }



    }
}
