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
            var user = GateUserComponent.Instance.Get(userId);
            if (user != null)
            {
                user.GetComponent<GateUserSessionComponent>().Session.Dispose();
            }
            //创建网关用户
            user = ComponentFactory.Create<GateUser>();
            user.UserId = userId;
            //session绑定用户信息
            session.AddComponent<SessionGateUserComponent>().User = user;
            user.AddComponent<GateUserSessionComponent>().Session = session;
            //添加邮箱组件,方便其他服务器通信
            session.AddComponent<MailBoxComponent, string>(MailboxType.GateSession);
            //添加用户进组方便管理
            GateUserComponent.Instance.Add(user.UserId, user);
            response.UserId = user.UserId;
            reply();
            //推送用户详情
            var userInfo = await UserHelper.GetUserInfo(userId);
            if (userInfo.UserInfo != null)
                session.Send(userInfo.UserInfo);
            else
                Log.Warning($"推送{userId}用户详情失败");
            //更新acotrid
            user.ActorId = userInfo.ActorId;
            //同步上线消息
            GateHelper.SynOnline(user.UserId, session.Id);
            Log.Debug($"{user.UserId} 上线");
        }
    }
}
