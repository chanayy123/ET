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
            var user = await UserCacheComponent.Instance.GetAsync(request.UserId);
            var flag = TryEnterRoom(request.RoomId, request.GateSessionId, user.UserInfo);
            response.Error = flag;
            reply();
            //延迟邀请机器人
            DelayCallRobot(request.RoomId, RandomHelper.RandomNumber(1, 4));
        }

        private async void DelayCallRobot(int roomId,int count, int delay=1000)
        {
            await TimerComponent.Instance.WaitAsync(delay);
            List<UserInfo> list = await MatchHelper.CallRobot(roomId, count);
            foreach (var item in list)
            {
                var flag = TryEnterRoom(roomId, 0, item);
                if(flag != 0)
                {
                    Log.Warning($"机器人{item.UserId}进入房间失败: {flag}");
                    MatchHelper.ReturnRobot(item.UserId);
                }
            }
        }

        private int TryEnterRoom(int roomId, long gateSessionId, UserInfo userInfo)
        {
            var roomMgr = Game.Scene.GetComponent<MatchRoomComponent>();
            var flag = roomMgr.CanEnterRoom(roomId, userInfo);
            if (flag == OpRetCode.Success)
            {
                var matchRoom = roomMgr.GetByRoomId(roomId);
                var matchPlayer = MatchFactory.CreateMatchPlayer(userInfo, roomId, gateSessionId,0);
                var gamePlayer = GameFactory.CreatePlayerData(gateSessionId, userInfo);
                var roomCfg = matchRoom.Config;
                var msg = MatchFactory.CreateMsgMG_EnterRoom(gamePlayer, roomId, roomCfg.GameId, roomCfg.GameMode, roomCfg.HallType);
                if (matchRoom.RoomActorId == 0)//游服没有此房间就随机选个游服
                {
                    var gameSession = MatchHelper.RandomGameSession;
                    gameSession.Send(msg);
                }
                else //游服已经有此房间,通过roomActorId找到所在的游服进程
                {
                    NetInnerHelper.SendMsgByAcotrId(matchRoom.RoomActorId, msg);
                }
                MatchFactory.RecycleMsg(msg);
                gamePlayer.Dispose();
                //进入游戏房间之后,同步添加匹配玩家进入匹配房间
                roomMgr.EnterRoom(matchPlayer);
            }
            return (int)flag;
        }

    }
}
