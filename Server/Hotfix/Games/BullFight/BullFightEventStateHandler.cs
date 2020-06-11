using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    /// <summary>
    /// 牛牛切换状态处理
    /// </summary>
    [Event(EventType.GameRoomStateChange+BullEventType.Bull_GS_WAIT_PLAYER)]
    public class BullFightWaitPlayerStateHandler : AEvent<BullFightRoom>
    {
        public override void Run(BullFightRoom room)
        {
            room.Reset();
            room.State = BullGameState.BullGsWaitPlayer;
            room.BroadcastGameState();
            GameHelper.SynRoomData(room.RoomId, (int)RoomState.IDLE, room.InstanceId);
        }
    }

    /// <summary>
    /// 牛牛切换状态处理
    /// </summary>
    [Event(EventType.GameRoomStateChange + BullEventType.Bull_GS_WAIT_START)]
    public class BullFightWaitStartStateHandler : AEvent<BullFightRoom>
    {
        public override void Run(BullFightRoom room)
        {
            room.Reset();
            room.State = BullGameState.BullGsWaitStart;
            room.BroadcastGameState();
            GameHelper.SynRoomData(room.RoomId, (int)RoomState.WAIT, room.InstanceId);
            room.DelayCheckSwitchState((int)BullDefines.WaitStartTime, room.CheckStart, true);
        }
    }

    /// <summary>
    /// 牛牛切换状态处理
    /// </summary>
    [Event(EventType.GameRoomStateChange + BullEventType.Bull_GS_ROBBANK)]
    public class BullFightRobBankStateHandler : AEvent<BullFightRoom>
    {
        public override void Run(BullFightRoom room)
        {
            room.State = BullGameState.BullGsRobbank;
            room.BroadcastGameState();
            GameHelper.SynRoomData(room.RoomId,(int)RoomState.GAMING, room.InstanceId);
            room.DelayCheckSwitchState((int)BullDefines.RobBankTime, room.CheckRate, true);
        }
    }

    /// <summary>
    /// 牛牛切换状态处理
    /// </summary>
    [Event(EventType.GameRoomStateChange + BullEventType.Bull_GS_SELBANK)]
    public class BullFightSelBankStateHandler : AEvent<BullFightRoom>
    {
        public override void Run(BullFightRoom room)
        {
            room.State = BullGameState.BullGsSelbank;
            room.BroadcastGameState();
            var max = room.seatPlayerDIc.Max((pair) => pair.Value.Rate);
            //在多个玩家随机选择一个庄家
            var list= room.seatPlayerDIc.Where((pair) => pair.Value.Rate == max);
            Log.Debug($"当前有{list.Count()}个玩家同时选中最高倍率:{max}");
            var randIdx = RandomHelper.RandomNumber(0, list.Count());
            var banker = list.ElementAt(randIdx).Value;
            //定完庄,切换闲家选倍率
            room.MarkBanker(banker.Pos);
            room.DelaySwitchState((int)BullDefines.SelBankTime, BullGameState.BullGsPlayerbet);
        }
    }

    /// <summary>
    /// 牛牛切换状态处理
    /// </summary>
    [Event(EventType.GameRoomStateChange + BullEventType.Bull_GS_PLAYERBET)]
    public class BullFightPlayerBetStateHandler : AEvent<BullFightRoom>
    {
        public override void Run(BullFightRoom room)
        {
            room.State = BullGameState.BullGsPlayerbet;
            room.BroadcastGameState();
            room.DelayCheckSwitchState((int)BullDefines.PlayerBetTime, room.CheckRate, true);
        }
    }

    /// <summary>
    /// 牛牛切换状态处理
    /// </summary>
    [Event(EventType.GameRoomStateChange + BullEventType.Bull_GS_DISPATCH)]
    public class BullFightDispatchStateHandler : AEvent<BullFightRoom>
    {
        public override void Run(BullFightRoom room)
        {
            room.State = BullGameState.BullGsDispatch;
            room.BroadcastGameState();
            room.DispatchCards();
            room.DelaySwitchState((int)BullDefines.DispatchTime, BullGameState.BullGsShowcard);
        }
    }

    /// <summary>
    /// 牛牛切换状态处理
    /// </summary>
    [Event(EventType.GameRoomStateChange + BullEventType.Bull_GS_SHOWCARD)]
    public class BullFightShowCardStateHandler : AEvent<BullFightRoom>
    {
        public override void Run(BullFightRoom room)
        {
            room.State = BullGameState.BullGsShowcard;
            room.BroadcastGameState();
            room.DelayCheckSwitchState((int)BullDefines.ShowCardTime, room.CheckShowCard, true);
        }
    }

    /// <summary>
    /// 牛牛切换状态处理
    /// </summary>
    [Event(EventType.GameRoomStateChange + BullEventType.Bull_GS_BILL)]
    public class BullFightBillStateHandler : AEvent<BullFightRoom>
    {
        public override void Run(BullFightRoom room)
        {
            room.State = BullGameState.BullGsBill;
            room.BroadcastGameState();
            //金币结算:暂时只考虑够赔的情况
            var banker = room.seatPlayerDIc[room.BankerPos];
            RepeatedField<BullBillInfo> billList = BullFightFactory.CreateBullBillInfoList();
            BullBillInfo bankBillInfo = BullFightFactory.CreateBullBillInfo(room.BankerPos);
            billList.Add(bankBillInfo);
            foreach (var item in room.playerDic)
            {
                var player = item.Value;
                if(player.Pos != room.BankerPos)//闲家和庄家比大小
                {
                    BullBillInfo billInfo = BullFightFactory.CreateBullBillInfo(player.Pos);
                    billList.Add(billInfo);
                    var result = BullFightHelper.Compare(banker, player);
                    if(result > 0) //庄家赢
                    {
                        var changeCoin = room.Cfg.BaseScore * BullFightHelper.GetBankerRate(banker.Rate) * BullFightHelper.GetPlayerRate(player.Rate, room.Cfg) * BullFightHelper.GetTypeRate(banker.CardType);
                        bankBillInfo.ChangeCoin += changeCoin;
                        billInfo.ChangeCoin = -changeCoin;
                        billInfo.TotalCoin = player.Coin + billInfo.ChangeCoin;
                        //同步世界服金币变更
                        player.ChangeCoin((int)billInfo.TotalCoin);
                    }
                    else
                    {
                        var changeCoin = room.Cfg.BaseScore * BullFightHelper.GetBankerRate(banker.Rate) * BullFightHelper.GetPlayerRate(player.Rate, room.Cfg) * BullFightHelper.GetTypeRate(player.CardType);
                        bankBillInfo.ChangeCoin -= changeCoin;
                        billInfo.ChangeCoin = changeCoin;
                        billInfo.TotalCoin = player.Coin + billInfo.ChangeCoin;
                        //同步世界服金币变更
                        player.ChangeCoin((int)billInfo.TotalCoin);
                    }
                }
            }
            //庄家金币结算完成同步世界服金币变更
            bankBillInfo.TotalCoin = banker.Coin + bankBillInfo.ChangeCoin;
            banker.ChangeCoin((int)bankBillInfo.TotalCoin);
            //广播结算消息
            room.BroadcastBill(billList);
            BullFightFactory.RecycleBillInfoList(billList);
            room.DelayCheckSwitchState((int)BullDefines.BillTime, room.CheckNewRound, true);
        }

    }


}
