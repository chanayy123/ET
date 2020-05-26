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

        public static async void ChangeCoin(this BullFightPlayer self, int changeCoin)
        {
            //同步世界服最新金币
            SW_UpdateUserInfo msg = GameFactory.CreateMsgSW_UpdateUserInfo(self.UserId, UserInfo.Property_Coin, changeCoin.ToString());
            var session = NetInnerHelper.GetSessionByAppType(AppType.World);
            var response = await session.Call(msg);
            if(response.Error != 0)
            {
                Log.Warning($"{self.UserId}玩家 同步世界服扣除金币失败");
            }
            else
            {
                self.Coin += changeCoin;
                if (self.Coin < 0)
                {
                    self.Coin = 0;
                    Log.Warning($"{self.UserId}身上金币不够扣除: {changeCoin}");
                }
            }
        }

    }






}
