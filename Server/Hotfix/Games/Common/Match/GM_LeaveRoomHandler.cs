using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class GM_LeaveRoomHandler : AMHandler<GM_LeaveRoom>
    {
        protected override async ETTask Run(Session session, GM_LeaveRoom message)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            foreach (var item in message.UserIdList)
            {
                var flag = roomMgr.userRoomDic.TryGetValue(item, out MatchPlayer player);
                if (flag)
                {
                    roomMgr.LeaveRoom(item);
                    if ( player.IsRobot)
                    {
                        MatchHelper.ReturnRobot(item);
                    }
                }
                else
                {
                    Log.Warning($"匹配服离开房间 :玩家{item}不存在");
                }           
            }
            await ETTask.CompletedTask;
        }
    }
}
