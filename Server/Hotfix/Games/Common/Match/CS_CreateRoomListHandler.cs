using System;
using System.Collections.Generic;
using System.Text;
using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 获取玩家创建房间列表
    /// </summary>
    [MessageHandler(AppType.Match)]
    class CS_CreateRoomListHandler : AMRpcHandler<CS_CreateRoomList, SC_CreateRoomList>
    {
        protected override async ETTask Run(Session session, CS_CreateRoomList request, SC_CreateRoomList response, Action reply)
        {
            var matchMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            response.List.Clear();//目前消息都从对象池取,每次使用前都需要重置一下
            foreach (var item in matchMgr.userRoomCfgList)
            {
                if (item.CreateUserId == request.UserId)
                {
                    var matchRoom= matchMgr.GetMatchRoom(item.RoomId);
                    response.List.Add(matchRoom);
                }
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}
