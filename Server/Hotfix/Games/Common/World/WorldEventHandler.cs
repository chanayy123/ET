using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    [Event(EventType.PropertyChange + UserInfo.Property_Coin)]
    public class WorldEventHandler : AEvent<int, object,IResponse,Action>
    {
        public override async void Run(int userId, object newValue,IResponse response,Action reply)
        {
            var user = UserComponent.Instance.Get(userId);
            if(user == null)
            {
                response.Message = $"{userId}玩家不存在";
                Log.Warning(response.Message);
                reply?.Invoke();
                return;
            }
            var beforeCoin = user.UserInfo.Coin;
            if(!int.TryParse(newValue.ToString(), out int totalCoin))
            {
                response.Message = $"玩家{userId} 更新金币值{newValue}无效";
                Log.Warning(response.Message);
                reply?.Invoke();
                return;
            }
            var changeCoin = totalCoin-beforeCoin;
            user.UserInfo.Coin = totalCoin;
            //金币变更,暂时直接更新数据库,后期可优化定时更新
            await UserComponent.Instance.DBProxy.Save(user.UserInfo);
            Log.Debug($"世界服: {userId} 玩家金币变更{changeCoin} 剩余金币{totalCoin}");
            //通知客戶端金币变更
            WorldHelper.NotifyCoinChange(user.GateSessionId, changeCoin, totalCoin);
            //纪录金币变更
            var log = ComponentFactory.Create<LogCoinChange>();
            log.UserId = userId;
            log.ChangeCoin = changeCoin;
            log.BeforeCoin = beforeCoin;
            log.NowCoin = totalCoin;
            log.ChangeTime = DateTime.UtcNow;
            await UserComponent.Instance.DBProxy.Save(log);
            log.Dispose();
            reply?.Invoke();
        }
    }
}
