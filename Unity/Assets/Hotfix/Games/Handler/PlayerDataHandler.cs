using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class PlayerDataHandler : AMHandler<SC_PlayerData>
    {
        protected override ETTask Run(ETModel.Session session, SC_PlayerData message)
        {
            Log.Debug("收到玩家数据: " + JsonHelper.ToJson(message));
            Game.EventSystem.Run(EventIdType.LoginFinish);
            return ETTask.CompletedTask;
        }
    }
}
