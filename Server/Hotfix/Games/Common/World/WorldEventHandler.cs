using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix.Games.Common.World
{
    [Event(EventType.PropertyChange + UserInfo.Property_Coin)]
    public class WorldEventHandler : AEvent<int, string,IResponse,Action>
    {
        public override async void Run(int userId, string changeValue,IResponse response,Action reply)
        {
            var user = UserComponent.Instance.Get(userId);
            if(user == null)
            {
                response.Message = $"{userId}玩家不存在";
                Log.Warning(response.Message);
                return;
            }
            var coin = user.UserInfo.Coin;
            var beforeCoin = coin;
            if(!int.TryParse(changeValue, out int changeCoin))
            {
                response.Message = $"玩家{userId} 更新金币值{changeValue}无效";
                Log.Warning(response.Message);
                return;
            }
            var realChange = changeCoin;
            if(changeCoin +coin<0 )
            {
                realChange = -coin;
                coin = 0;
                Log.Warning($"当前玩家{userId}身上金币不够{changeCoin},实际扣除{coin}");
            }
            else
            {
                coin += changeCoin;
            }
            user.UserInfo.Coin = coin;
            //金币变更,暂时直接更新数据库,后期可优化定时更新
            await UserComponent.Instance.DBProxy.Save(user.UserInfo);
            Log.Debug($"世界服: {userId} 玩家金币变更{realChange} 剩余金币{coin}");
            //纪录金币变更
            var log = ComponentFactory.Create<LogCoinChange>();
            log.UserId = userId;
            log.ChangeCoin = realChange;
            log.BeforeCoin = beforeCoin;
            log.NowCoin = coin;
            log.ChangeTime = DateTime.UtcNow;
            await UserComponent.Instance.DBProxy.Save(log);
            log.Dispose();
            reply?.Invoke();
        }
    }
}
