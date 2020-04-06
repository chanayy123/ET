using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    class CS_VerifyKeyHandler : AMRpcHandler<CS_VerifyKey, SC_VerifyKey>
    {
        protected override async ETTask Run(Session session, CS_VerifyKey request, SC_VerifyKey response, Action reply)
        {
            var gateKeys = Game.Scene.GetComponent<GateSessionKeyComponent>();
            var userId = gateKeys.Get(request.Key);
            if(userId == 0)
            {
                response.Message = "key失效";
                reply();
                return;
            }
            //判断是否在线,如果在线强制踢出
            var user = UserComponent.Instance.Get(userId);
            if (user != null)
            {
                user.GetComponent<UserSessionComponent>().Session.Dispose();
            } 

            //获取用户详情,并反馈给客户端
            var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
            List<ComponentWithId> list = await dbProxy.Query<User>(u=>u.UserId == userId);
            if(list.Count == 0)
            {
                response.Message = $"{userId} 用户不存在";
                reply();
                return;
            }
            user = list[0] as User;
            //session绑定用户信息
            session.AddComponent<SessionUserComponent>().User = user;
            user.AddComponent<UserSessionComponent>().Session = session;
            //添加邮箱组件,方便其他服务器通信
            session.AddComponent<MailBoxComponent, string>(MailboxType.GateSession);
            //添加用户进组方便管理
            UserComponent.Instance.Add(user.UserId, user);
            response.UserId = user.UserId;
            reply();
            //同时发送客户端用户信息
            session.Send(user);
            //同步上线消息=>realm
            GateHelper.synOnline(user.UserId, StartConfigComponent.Instance.StartConfig.AppId);
        }
    }
}
