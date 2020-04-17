using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class CS_EnterRoomHandler : AMRpcHandler<CS_EnterRoom, SC_EnterRoom>
    {
        protected override async ETTask Run(Session session, CS_EnterRoom request, SC_EnterRoom response, Action reply)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            //从用户服取最新数据判断是否能进房间
            var userInfo = await UserHelper.GetUserInfo(request.UserId);
            var flag = roomMgr.CanEnterRoom(request.RoomId, userInfo);
            if(flag == 0)
            {
                var matchPlayer = MatchFactory.CreateMatchPlayer(request.UserId, request.RoomId,request.GateSessionId);
                var gamePlayer = GameFactory.CreatePlayerData(matchPlayer, userInfo);
                var gameSession = MatchHelper.RandomGameSession;
                gameSession.Send(new MG_EnterRoom() { player = gamePlayer,RoomId =request.RoomId,GameId=request.GameId});
                roomMgr.EnterRoom(matchPlayer);
                gamePlayer.Dispose();
                reply();
            }
            else
            {
                response.Error = (int)flag;
                reply();
            }
        }
    }
}
