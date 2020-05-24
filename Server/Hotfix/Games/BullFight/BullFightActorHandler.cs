using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [ActorMessageHandler(AppType.Game)]
    class BullFightActorLeaveRoomHandler : AMActorRpcHandler<BullFightPlayer, CS_LeaveRoom, SC_LeaveRoom>
    {
        protected override ETTask Run(BullFightPlayer player, CS_LeaveRoom request, SC_LeaveRoom response, Action reply)
        {
            var room = player.Room;
            var flag = room.CanLeaveRoom(player.UserId);
            if(flag == OpRetCode.Success)
            {
                room.LeaveRoom(player.UserId);
            }
            else
            {
                response.Error = (int)flag;
            }
            reply();
            return ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler(AppType.Game)]
    class BullFightActorOnlineHandler : AMActorHandler<BullFightPlayer, Actor_OnlineState>
    {
        protected override ETTask Run(BullFightPlayer player, Actor_OnlineState message)
        {
            var room = player.Parent as BullFightRoom;
            player.IsOnline = message.Flag;
            player.GateSessionId = message.GateSessionId;
            if (!player.IsOnline)//玩家离线,如果可以离开房间就离开
            {
                var flag = room.CanLeaveRoom(player.UserId);
                if (flag == OpRetCode.Success)
                {
                    room.LeaveRoom(player.UserId);
                }
            }
            return ETTask.CompletedTask;
        }
    }

    [ActorMessageHandler(AppType.Game)]
    class BullFightActorRoomInfoHandler : AMActorRpcHandler<BullFightPlayer, CS_GetRoomInfo, SC_GetBullRoomInfo>
    {
        protected override ETTask Run(BullFightPlayer player, CS_GetRoomInfo request, SC_GetBullRoomInfo response, Action reply)
        {
            var room = player.Room;
            response.Data = room.RoomData;
            reply();
            return ETTask.CompletedTask;
        }
    }

    [ActorMessageHandler(AppType.Game)]
    class BullFightActorOpHandler : AMActorRpcHandler<BullFightPlayer, CS_BullOp, SC_BullOp>
    {
        public readonly int[] BankerRate = {0,1,2,3,4};
        protected override ETTask Run(BullFightPlayer player, CS_BullOp request, SC_BullOp response, Action reply)
        {
            var room = player.Room;
            if (room.State == BullGameState.BullGsRobbank && request.OpCode == BullOpCode.BullOpBetBank)
            {
                if (request.Params.Count == 1)
                {
                    var index = request.Params[0];
                    if (index >= 0 && index < BankerRate.Length)
                    {
                        player.ChooseBankRate(index);
                    }
                    else
                    {
                        response.Message = "抢庄倍率无效";
                        response.Error = (int)OpRetCode.GameOpInvalid;
                    }
                }
                else
                {
                    response.Message = "参数个数无效";
                    response.Error = (int)OpRetCode.GameOpInvalid;
                }
            }
            else if (room.State == BullGameState.BullGsPlayerbet && request.OpCode == BullOpCode.BullOpBetPlayer)
            {
                if (request.Params.Count == 1)
                {
                    var index = request.Params[0];
                    if (index >= 0 && index < room.Cfg.IntParams.Length)
                    {
                        player.ChoosePlayerBet(index);
                    }
                    else
                    {
                        response.Message = "闲家倍率无效";
                        response.Error = (int)OpRetCode.GameOpInvalid;
                    }
                }
                else
                {
                    response.Message = "参数个数无效";
                    response.Error = (int)OpRetCode.GameOpInvalid;
                }
            }
            else if (room.State == BullGameState.BullGsShowcard && request.OpCode == BullOpCode.BullOpShowCard)
            {
                if (request.Params.Count == 5)
                {
                    //判断玩家手牌是否和服务器一致
                    if (CardHelper.EqualsCardList(request.Params, player.HandCards))
                    {
                        player.ChooseShowCard(request.Params);
                    }
                    else
                    {
                        response.Message = "卡牌数据无效";
                        response.Error = (int)OpRetCode.GameOpInvalid;
                    }
                }
                else
                {
                    response.Message = "参数个数无效";
                    response.Error = (int)OpRetCode.GameOpInvalid;
                }
            }
            else
            {
                response.Message = "操作时机无效";
                response.Error = (int)OpRetCode.GameOpInvalid;
            }
            reply();
            return ETTask.CompletedTask;
        }
    }

}
