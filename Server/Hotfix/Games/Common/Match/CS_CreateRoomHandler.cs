using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Match)]
    class CS_CreateRoomHandler : AMRpcHandler<CS_CreateRoom, SC_CreateRoom>
    {
        protected override async ETTask Run(Session session, CS_CreateRoom request, SC_CreateRoom response, Action reply)
        {
            var dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            if (roomMgr.IsInMatchOrRoom(request.UserId))
            {
                response.Error = (int)OpRetCode.CreateRoomAlreadyIn;
                reply();
                return;
            }
            var user = await UserCacheComponent.Instance.GetAsync(request.UserId);
            //随机有效的不重复的房间id
            var roomId = MatchHelper.RandomRoomId;
            var userRoom = MatchFactory.CreateUserRoom(roomId, request.UserId, request.GameId, request.GameMode, request.Params);
            //创建房间配置同步数据库并缓存
            await dbProxy.Save(userRoom);
            roomMgr.AddUserRoom(userRoom);
            var matchRoom = MatchFactory.CreateCardModeRoom(roomId, request.GameId, request.GameMode);
            roomMgr.AddMatchRoom(roomId, matchRoom);
            //创建房间成功,玩家自动进入房间
            var matchPlayer = MatchFactory.CreateMatchPlayer(request.UserId, roomId, request.GateSessionId);
            var gamePlayer = GameFactory.CreatePlayerData(matchPlayer, user);
            var msg = MatchFactory.CreateMsgEnterRoom(gamePlayer, roomId, matchRoom.Config.GameId);
            var gameSession = MatchHelper.RandomGameSession;
            gameSession.Send(msg);
            //同步游服创建房间之后,添加匹配玩家进入匹配服
            roomMgr.EnterRoom(matchPlayer);
            gamePlayer.Dispose();
            MatchFactory.RecycleMsg(msg);
            response.RoomId = roomId;
            reply();
        }
    }
}
