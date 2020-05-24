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
                response.Error = (int)OpRetCode.VerifyKeyInvalid;
                reply();
                return;
            }
            //判断是否在线,如果在线强制踢出
            var gateUser = GateUserComponent.Instance.Get(userId);
            if (gateUser != null)
            {
                gateUser.Session.Dispose();
            }
            //验证key成功删除key
            gateKeys.Remove(request.Key);
            //创建网关用户
            gateUser = GateFactory.CreateUser(userId, session);
            //session绑定用户信息
            session.AddComponent<SessionGateUserComponent>().User = gateUser;
            //添加邮箱组件,方便其他服务器通信
            session.AddComponent<MailBoxComponent, string>(MailboxType.GateSession);
            //添加用户进组方便管理
            GateUserComponent.Instance.Add(gateUser.UserId, gateUser);
            reply();
            //同步上线消息
            GateHelper.SynOnline(gateUser.UserId, session.Id);
            Log.Debug($"{gateUser.UserId} 上线");
            await ETTask.CompletedTask;
        }
    }
}
