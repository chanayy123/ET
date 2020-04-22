﻿using System;
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
            var user = await UserCacheComponent.Instance.GetAsync(request.UserId);
            var flag = roomMgr.CanEnterRoom(request.RoomId, user);
            if(flag == 0)
            {
                var matchRoom = roomMgr.GetByRoomId(request.RoomId);
                var matchPlayer = MatchFactory.CreateMatchPlayer(request.UserId, request.RoomId,request.GateSessionId);
                var gamePlayer = GameFactory.CreatePlayerData(matchPlayer,user);
                var msg = MatchFactory.CreateMsgEnterRoom(gamePlayer, request.RoomId, matchRoom.Config.GameId);
                if(matchRoom.RoomActorId == 0)
                {
                    var gameSession = MatchHelper.RandomGameSession;
                    gameSession.Send(msg);
                }
                else //游服已经有此房间,通过roomActorId找到所在的游服进程
                {
                    NetInnerHelper.SendMsgByAcotrId(matchRoom.RoomActorId, msg);
                }
                roomMgr.EnterRoom(matchPlayer);
                reply();
                gamePlayer.Dispose();
                MatchFactory.RecycleMsg(msg);
            }
            else
            {
                response.Error = (int)flag;
                reply();
            }
        }
    }
}
