using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class BullFightRoomExtensions
    {
        public static void Awake(this BullFightRoom self, int roomId, RoomConfig cfg)
        {
            self.RoomData = BullFightFactory.CreateRoomData(roomId, cfg);
            self.Cfg = cfg;
            self.RoomType = (RoomType)GameConfigCacheComponent.Instance.Get((int)self.Cfg.Id).RoomType;
        }
        /// <summary>
        /// 玩家匹配成功进入房间
        /// </summary>
        /// <param name="self"></param>
        /// <param name="playerList"></param>
        public static void EnterRoom(this BullFightRoom self, List<BullFightPlayer> playerList)
        {
            foreach (var item in playerList)
            {
                self.AddPlayer(item);
            }
            //广播进房间消息
            self.BroadcastRoomInfo();
            self.CheckMatchPlayerCount();
        }
        /// <summary>
        /// 玩家单个进入房间
        /// </summary>
        /// <param name="self"></param>
        /// <param name="player"></param>
        public static void EnterRoom(this BullFightRoom self, BullFightPlayer player,bool check=true)
        {
            self.AddPlayer(player);
            //广播进房间消息
            self.BroadcastPlayerEnter(player);
            if (check)
            {
                self.CheckPlayerCount();
            }
        }

        public static void LeaveRoom(this BullFightRoom self, int userId,bool check=true)
        {
            self.RemovePlayer(userId);
            self.BroadcastPlayerLeave(userId);
            GameHelper.SynLeaveRoom(self.RoomId, userId);
            if (check)
            {
                self.CheckPlayerCount();
            }
        }
        /// <summary>
        /// 房间所有玩家离开
        /// </summary>
        /// <param name="self"></param>
        /// <param name="reason">0 主动离开 1 游戏结束自动离开 2 游戏中强制离开</param>
        public static void AllLeaveRoom(this BullFightRoom self, int reason)
        {
            var userIdList = self.playerDic.Keys.ToArray();
            self.BroadcastKickPlayers(reason);
            foreach (var item in userIdList)
            {
                self.RemovePlayer(item);
            }
            GameHelper.SynLeaveRoom(self.RoomId, userIdList);
            self.CheckPlayerCount();
        }

        public static OpRetCode CanLeaveRoom(this BullFightRoom self, int userId)
        {
            if (!self.IsGaming())
            {
                return OpRetCode.Success;
            }
            else
            {
                return OpRetCode.RoomAlreadyGaming;
            }

        }
        /// <summary>
        /// 获得可用座位号: 递增顺序来查找
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int GetAvailablePos(this BullFightRoom self)
        {
            for (var i = 0; i < self.Cfg.MaxPlayers; ++i)
            {
                if (!self.seatPlayerDIc.ContainsKey(i)) return i;
            }
            return -1;
        }

        public static void AddPlayer(this BullFightRoom self, BullFightPlayer player)
        {
            player.Parent = self; //方便通过玩家对象获取房间对象
            self.RoomData.PlayerList.Add(player.PlayerData);
            self.playerDic.Add(player.UserId, player);
            //安排可用座位
            player.Pos = self.GetAvailablePos();
            self.seatPlayerDIc.Add(player.Pos, player);
            //同步玩家actorId方便以后通信
            player.AddComponent<MailBoxComponent>();
            player.IsOnline = true;
            GameHelper.SynActorId(player.GateSessionId, player.UserId, player.InstanceId, (int)GameId.BullFight, self.RoomId);
        }

        public static void RemovePlayer(this BullFightRoom self, int userId)
        {
            self.playerDic.Remove(userId, out BullFightPlayer player);
            if (player == null)
            {
                Log.Warning($"删除用户失败: 用户{userId}不存在");
                return;
            }
            self.seatPlayerDIc.Remove(player.Pos);
            self.RoomData.PlayerList.Remove(player.PlayerData);
            //离开游戏房间重置玩家actorid
            GameHelper.SynActorId(player.GateSessionId, player.UserId, 0);
            player.Dispose();
        }

        public static void SwitchState(this BullFightRoom self, BullGameState state)
        {
            if (self.State == state) return;
            //每次切换状态,取消之前可能存在的延迟切换
            self.StateCancelTS?.Cancel();
            var stateEvent = $"{EventType.GameRoomStateChange}{self.RoomData.Data.GameId}_{(int)state}";
            Game.EventSystem.Run(stateEvent, self);
        }

        public static async void DelaySwitchState(this BullFightRoom self, int delay, BullGameState state)
        {
            self.StateCancelTS?.Dispose();
            self.StateCancelTS = new CancellationTokenSource();
            await TimerComponent.Instance.WaitAsync(delay, self.StateCancelTS.Token);
            self.SwitchState(state);
        }

        public static async void DelayCheckSwitchState(this BullFightRoom self, int delay, Action<bool> action, bool flag)
        {
            self.StateCancelTS?.Dispose();
            self.StateCancelTS = new CancellationTokenSource();
            await TimerComponent.Instance.WaitAsync(delay, self.StateCancelTS.Token);
            action?.Invoke(flag);
        }

        /// <summary>
        /// 根据当前房间人数来切换状态
        /// </summary>
        /// <param name="self"></param>
        public static void CheckPlayerCount(this BullFightRoom self, bool timeout = false)
        {
            if (self.RoomData.PlayerList.Count >= self.Cfg.MinPlayers)
            {
                if (self.State == BullGameState.BullGsWaitPlayer)
                {
                    self.SwitchState(BullGameState.BullGsWaitStart);
                }else if (self.State == BullGameState.BullGsWaitStart && timeout)
                {
                    self.SwitchState(BullGameState.BullGsRobbank);
                }
                else if (self.State == BullGameState.BullGsBill && timeout)
                {
                    self.SwitchState(BullGameState.BullGsWaitStart);
                }
            }
            else
            {
                self.SwitchState(BullGameState.BullGsWaitPlayer);
                self.CheckPlayerExist();
            }
        }
        /// <summary>
        /// 玩家匹配成功,批量进入房间检测,如果全是机器人不开始游戏全部退出，否则直接开始游戏
        /// </summary>
        /// <param name="self"></param>
        public static void CheckMatchPlayerCount(this BullFightRoom self)
        {
            self.CheckPlayerExist();
            if (self.RoomData.PlayerList.Count >= self.Cfg.MinPlayers)
            {
                self.SwitchState(BullGameState.BullGsRobbank);
            }
        }

        /// <summary>
        /// 根据当前选中倍率情况来切换状态
        /// 抢庄阶段: rate -1表示没操作 0 不抢 1 2 ...倍率索引
        /// 闲家倍率阶段: rate  0 1 2 ...倍率索引
        /// </summary>
        /// <param name="self"></param>
        public static void CheckRate(this BullFightRoom self, bool timeout = false)
        {
            if (self.State == BullGameState.BullGsRobbank)
            {
                if (timeout)
                {
                    //超时默认不抢
                    foreach (var item in self.seatPlayerDIc)
                    {
                        if (item.Value.Rate == -1)
                        {
                            item.Value.ChooseBankRate(0, true);
                        }
                    }
                }
                var count = self.seatPlayerDIc.Count((pair) => pair.Value.Rate > -1);
                if (count == self.seatPlayerDIc.Count)//全部抢庄完毕或者超时
                {
                    var maxPair = self.seatPlayerDIc.Aggregate((l, r) => l.Value.Rate > r.Value.Rate ? l : r);
                    var maxCount = self.seatPlayerDIc.Count((pair) => pair.Value.Rate == maxPair.Value.Rate);
                    if (maxCount > 1)//有多个玩家最高倍率,进入随机选庄状态
                    {
                        self.SwitchState(BullGameState.BullGsSelbank);
                    }
                    else
                    {
                        //定完庄,切换闲家选倍率
                        self.MarkBanker(maxPair.Key);
                        self.SwitchState(BullGameState.BullGsPlayerbet);
                    }
                }
            }
            else if (self.State == BullGameState.BullGsPlayerbet)
            {
                if (timeout)
                {
                    //超时默认选中第一档倍率
                    foreach (var item in self.seatPlayerDIc)
                    {
                        if (item.Value.Rate == -1)
                        {
                            item.Value.ChoosePlayerBet(0, true);
                        }
                    }
                }
                var count = self.seatPlayerDIc.Count((pair) => pair.Value.Rate > -1 && pair.Value.Pos != self.BankerPos);
                if (count == self.seatPlayerDIc.Count - 1)//全部闲家选倍率完毕或者超时
                {
                    self.SwitchState(BullGameState.BullGsDispatch);
                }
            }
        }
        /// <summary>
        /// 根据亮牌情况切换状态
        /// </summary>
        /// <param name="self"></param>
        public static void CheckShowCard(this BullFightRoom self, bool timeout = false)
        {
            if (timeout)//超时默认亮牌,暂时不进行最优排序
            {
                foreach (var item in self.seatPlayerDIc)
                {
                    var player = item.Value;
                    if (!player.IsShowCard)
                    {
                        player.ChooseShowCard(player.HandCards, true);
                    }
                }
            }
            var showCount = self.playerDic.Count((pair) => pair.Value.IsShowCard);
            if (showCount == self.playerDic.Count)
            {
                self.SwitchState(BullGameState.BullGsBill);
            }
        }
        /// <summary>
        /// 结算完毕,检测是否开启新一局
        /// </summary>
        public static void CheckNewRound(this BullFightRoom self, bool timeout = false)
        {
            if (self.RoomType == RoomType.Match) //匹配模式房间:每局结束玩家自动离开
            {
                self.AllLeaveRoom(1);
            }
            else //其他模式房间:每局结束玩家离线自动离开房间
            {
                self.CheckPlayerExist();
                self.CheckPlayerCount(true);
            }
        }
        /// <summary>
        /// 延迟开始检测:离线玩家自动离开,房间只剩机器人,就全部离开
        /// </summary>
        /// <param name="self"></param>
        /// <param name="timeout"></param>
        public static void CheckStart(this BullFightRoom self, bool timeout = false)
        {
            self.CheckPlayerExist();
            self.CheckPlayerCount(true);
        }
        /// <summary>
        /// 检测真人玩家数量: 如果全是机器人就退出
        /// </summary>
        /// <param name="self"></param>
        public static void CheckPlayerExist(this BullFightRoom self)
        {
            var userIdList = self.playerDic.Keys.ToList();
            var flag = false;
            //玩家离线自动退出房间
            foreach (var item in userIdList)
            {
                if (!self.playerDic[item].IsOnline)
                {
                    self.LeaveRoom(item,false);
                    flag = true;
                }
            }
            userIdList = flag?self.playerDic.Keys.ToList():userIdList;
            var count = userIdList.Count((userId) => !self.playerDic[userId].IsRobot);
            if (count == 0)//房间只剩机器人,就全部离开
            {
                foreach (var item in userIdList)
                {
                    self.LeaveRoom(item,false);
                }
            }
        }

        /// <summary>
        /// 确定庄家
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void MarkBanker(this BullFightRoom self, int pos)
        {
            self.BankerPos = pos;
            self.BroadcastBankerPos(pos);
            var banker = self.seatPlayerDIc[pos];
            if (banker.Rate < 1) //玩家都没有抢庄,修正庄家倍率默认索引1
            {
                banker.ChooseBankRate(1, true);
            }
            //重置其他玩家倍率为-1
            foreach (var item in self.seatPlayerDIc)
            {
                if (item.Key != pos)
                {
                    item.Value.Rate = -1;
                }
            }
        }
        /// <summary>
        /// 随机打乱卡牌,从头按顺序给每个玩家分配手牌
        /// </summary>
        /// <param name="self"></param>
        public static void DispatchCards(this BullFightRoom self)
        {
            if (self.CardList.Count == 0)
            {
                for (var i = 0; i < BullFightRoom.MAX_CARDS; ++i)
                {
                    self.CardList.Add(i);
                }
            }
            CardHelper.Shuffle(self.CardList);
            var index = 0;
            var handCardNum = 5;
            //给每个玩家分配手牌
            foreach (var item in self.seatPlayerDIc)
            {
                for (var i = 0; i < handCardNum; ++i)
                {
                    item.Value.AddHandCard(self.CardList[index * handCardNum + i]);
                }
                ++index;
            }
            //发送每个玩家手牌,手牌类型为invalid表明没有亮牌
            foreach (var item in self.playerDic)
            {
                var player = item.Value;
                SC_BullCardsInfo msg = BullFightFactory.CreateMsgSC_BullCardsInfo(player.GateSessionId, player.Pos, player.HandCards, BullCardType.BullCtInvalid);
                NetInnerHelper.SendActorMsg(msg);
                BullFightFactory.RecycleMsg(msg);
            }

        }

        #region Broadcast
        public static void BroadcastRoomInfo(this BullFightRoom self)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                SC_BullRoomInfo msg = BullFightFactory.CreateMsgSC_BullRoomInfo(item.Value.GateSessionId, self.RoomData);
                NetInnerHelper.SendActorMsg(msg);
                BullFightFactory.RecycleMsg(msg);
            }
        }

        public static void BroadcastPlayerEnter(this BullFightRoom self, BullFightPlayer player)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                if (item.Key != player.UserId)
                {
                    SC_BullPlayerEnter msg = BullFightFactory.CreateMsgSC_BullPlayerEnter(item.Value.GateSessionId, player.PlayerData);
                    NetInnerHelper.SendActorMsg(msg);
                    BullFightFactory.RecycleMsg(msg);
                }
                else
                {
                    SC_BullRoomInfo msg = BullFightFactory.CreateMsgSC_BullRoomInfo(player.GateSessionId, self.RoomData);
                    NetInnerHelper.SendActorMsg(msg);
                    BullFightFactory.RecycleMsg(msg);
                }
            }
        }

        public static void BroadcastPlayerLeave(this BullFightRoom self, int userId)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                if (item.Key != userId)
                {
                    SC_PlayerLeave msg = GameFactory.CreateMsgSC_PlayerLeave(item.Value.GateSessionId, userId);
                    NetInnerHelper.SendActorMsg(msg);
                    GameFactory.RecycleMsg(msg);
                }
            }
        }

        public static void BroadcastGameState(this BullFightRoom self, List<int> param = null)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                SC_BullState msg = BullFightFactory.CreateMsgSC_BullState(item.Value.GateSessionId, self.State, param);
                NetInnerHelper.SendActorMsg(msg);
                BullFightFactory.RecycleMsg(msg);
            }
        }

        public static void BroadcastBankerRate(this BullFightRoom self, BullFightPlayer player)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                SC_BullBankerRate msg = BullFightFactory.CreateMsgSC_BullBankerRate(item.Value.GateSessionId, player.Pos, player.Rate);
                NetInnerHelper.SendActorMsg(msg);
                BullFightFactory.RecycleMsg(msg);
            }
        }

        public static void BroadcastPlayerRate(this BullFightRoom self, BullFightPlayer player)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                SC_BullPlayerRate msg = BullFightFactory.CreateMsgSC_BullPlayerRate(item.Value.GateSessionId, player.Pos, player.Rate);
                NetInnerHelper.SendActorMsg(msg);
                BullFightFactory.RecycleMsg(msg);
            }
        }

        public static void BroadcastBankerPos(this BullFightRoom self, int pos)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                SC_BullBankerPos msg = BullFightFactory.CreateMsgSC_BullBankerPos(item.Value.GateSessionId, pos);
                NetInnerHelper.SendActorMsg(msg);
                BullFightFactory.RecycleMsg(msg);
            }
        }

        public static void BroadcastShowCard(this BullFightRoom self, BullFightPlayer player)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                SC_BullCardsInfo msg = BullFightFactory.CreateMsgSC_BullCardsInfo(item.Value.GateSessionId, player.Pos, player.HandCards, player.CardType);
                NetInnerHelper.SendActorMsg(msg);
                BullFightFactory.RecycleMsg(msg);
            }
        }

        public static void BroadcastBill(this BullFightRoom self, RepeatedField<BullBillInfo> billList)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                SC_BullBillInfo msg = BullFightFactory.CreateMsgSC_BullBillInfo(item.Value.GateSessionId, billList);
                NetInnerHelper.SendActorMsg(msg);
                BullFightFactory.RecycleMsg(msg);
            }
        }

        public static void BroadcastKickPlayers(this BullFightRoom self, int reason)
        {
            foreach (var item in self.playerDic)
            {
                if (!item.Value.IsOnline || item.Value.IsRobot) continue;
                var msg = GameFactory.CreateMsgSC_KickPlayer(item.Value.GateSessionId, reason);
                NetInnerHelper.SendActorMsg(msg);
                GameFactory.RecycleMsg(msg);
            }
        }

        #endregion

        public static void Reset(this BullFightRoom self)
        {
            self.RoomData.Reset();
        }
        /// <summary>
        /// 游戏结算表明游戏已经结束
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsGaming(this BullFightRoom self)
        {
            return self.State > BullGameState.BullGsWaitStart && self.State < BullGameState.BullGsBill;
        }

        public static void Reset(this BullFightRoomData self)
        {
            self.BankerPos = -1;
            self.Data.State = -1;
            foreach (var item in self.PlayerList)
            {
                item.Reset();
            }
        }

    }
}
