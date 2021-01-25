using System;
using ETModel;

namespace ETHotfix
{
    public static class LoginHelper
    {
        public static async void LoginAsync(string account, string pwd)
        {
            try
            {
                var realmAddress = GlobalConfigComponent.Instance.GlobalConfig.Address;
                var realmSession = SessionHelper.Create(realmAddress);
                var res = (SC_Login)await realmSession.Call(new CS_Login() { LoginType = 1, DataStr = $"{account}|{pwd}" });
                realmSession.Dispose();
                if (res.Error != 0)
                {
                    Log.Warning($"登陆错误: ${res.Error} ${res.Message}");
                    return;
                }
                var gateSession = SessionHelper.Create(res.Address);
                var res2 = (SC_VerifyKey)await gateSession.Call(new CS_VerifyKey() { Key = res.Key });
                if (res2.Error == 0)
                {
                    ETModel.Game.Scene.AddComponent<ETModel.SessionComponent>().Session = gateSession.session;
                    Game.Scene.AddComponent<SessionComponent>().Session = gateSession;
                    Log.Debug("登陆gate成功");
                }
                else
                {
                    gateSession.Dispose();
                    Log.Warning($"验证错误: ${res2.Error} ${res2.Message}");
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToStr());
            }
        }
    }
}