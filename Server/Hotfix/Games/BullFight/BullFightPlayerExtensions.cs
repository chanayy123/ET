using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class BullFightPlayerExtensions
    {
        public static void Awake(this BullFightPlayer self, BullFightPlayerData data)
        {
            self.PlayerData = data;
            self.PlayerData.Parent = self;
        }

        public static void Reset(this BullFightPlayerData self)
        {
            self.HandCards.Clear();
            self.Rate = -1;
            self.CardType = BullCardType.BullCtInvalid;
        }

        public static void ChooseBankRate(this BullFightPlayer self ,int index, bool timeout=false)
        {
            self.Rate = index;
            self.Room.BroadcastBankerRate(self);
            if(!timeout) self.Room.CheckRate();
        }

        public static void ChoosePlayerBet(this BullFightPlayer self,int index, bool timeout = false)
        {
            self.Rate = index;
            self.Room.BroadcastPlayerRate(self);
            if (!timeout) self.Room.CheckRate();
        }
        /// <summary>
        /// 不管客户端怎么亮牌,都以服务器卡牌来计算最优组合
        /// </summary>
        /// <param name="self"></param>
        /// <param name="list"></param>
        /// <param name="timeout"></param>
        public static void ChooseShowCard(this BullFightPlayer self, RepeatedField<int> list,bool timeout =false)
        {
            BullCardType type = BullFightHelper.UpdateBestSort(self.HandCards);
            self.CardType = type;
            self.Room.BroadcastShowCard(self);
            if(!timeout) self.Room.CheckShowCard();
        }

        public static void ClearHandCards(this BullFightPlayer self)
        {
            self.HandCards.Clear();
        }

        public static void AddHandCard(this BullFightPlayer self,int card)
        {
            self.HandCards.Add(card);
        }

        public static void AddHandCards(this BullFightPlayer self, IEnumerable<int> cards)
        {
            self.HandCards.AddRange(cards);
        }

        public static async void ChangeCoin(this BullFightPlayer self, int totalCoin)
        {
            //玩家同步world服金币,不包括机器人
            if (!self.IsRobot)
            {
                self.Coin = totalCoin; //为了防止结算阶段玩家离开导致player被回收,这里直接修改coin不必等到SW_UpdateUserInfo异步回调再赋值
                SW_UpdateUserInfo msg = GameFactory.CreateMsgSW_UpdateUserInfo(self.UserId, UserInfo.Property_Coin, totalCoin);
                var session = NetInnerHelper.GetSessionByAppType(AppType.World);
                var response = await session.Call(msg);
                GameFactory.RecycleMsg(msg);
                if (response.Error != 0)
                {
                    Log.Warning($"{self.UserId}玩家 同步世界服更新金币失败");
                }
            }
            else //机器人同步更新robot服金币
            {
                SR_UpdateCoin msg = GameFactory.CreateMsgSR_UpdateCoin(self.UserId, totalCoin);
                var session = NetInnerHelper.GetSessionByAppType(AppType.Robot);
                session.Send(msg);
                GameFactory.RecycleMsg(msg);
            }
        }

    }






}
