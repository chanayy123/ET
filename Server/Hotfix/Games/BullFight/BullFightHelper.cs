using System;
using System.Collections.Generic;
using ETModel;
using System.Linq;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class BullFightHelper
    {
        public static readonly RepeatedField<int> sTmpList = new RepeatedField<int>();
        public static readonly int[] BankerRate = { 0, 1, 2, 3, 4 };
        /// <summary>
        /// 更新获得最佳牛型组合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static BullCardType UpdateBestSort(RepeatedField<int> list)
        {
            //5张牌:3张组合
            BullCardType type = GetBestSort(list, out RepeatedField<int> sortList);
            list.Clear();
            list.AddRange(sortList);
            return type;
        }

        /// <summary>
        /// 获取最佳牌型组合
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sortList"></param>
        /// <returns></returns>
        public static BullCardType GetBestSort(RepeatedField<int> list,out RepeatedField<int> sortList,bool isSort=true)
        {
            if (isSort)
            {
                var l2 = list.OrderByDescending((item) => item).ToList();
                list.Clear();
                list.AddRange(l2);
            }
            sortList = new RepeatedField<int>();
            if (list.Count != 5) return BullCardType.BullCtInvalid;
            var maxType = BullCardType.BullCtInvalid;
            //三层遍历所有3张牌组合
            for(var i = 0; i < 3; ++i)
            {
                for(var j = i + 1; j < 4; ++j)
                {
                    for(var k = j + 1; k < 5; ++k)
                    {
                        sTmpList.Clear();
                        sTmpList.Add(list[i]);
                        sTmpList.Add(list[j]);
                        sTmpList.Add(list[k]);
                        sTmpList.AddRange(list.Where((item) => !sTmpList.Contains(item)));
                        var type = GetBullType(sTmpList);
                        if(maxType < type)
                        {
                            maxType = type;
                            sortList.Clear();
                            sortList.AddRange(sTmpList);
                        }
                    }
                }
            }
            return maxType;
        }
        /// <summary>
        /// 已经组好牛型的从大到小排序过的牌:先比牛型,牛型一样就比最大的单牌(包括花色)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(BullFightPlayer left, BullFightPlayer right)
        {
            if(left.CardType == right.CardType)
            {
                return left.HandCards[0] - right.HandCards[0];
            }
            return left.CardType - right.CardType;
        }

        /// <summary>
        /// 根据牌组里前3个计算牛型
        /// </summary>
        /// <param name="list">已排序牌组</param>
        /// <returns></returns>
        public static BullCardType GetBullType(RepeatedField<int> list)
        {
            if(list.Count != 5)
            {
                return BullCardType.BullCtInvalid;
            }
            var bullPoint = 0;
            for(var i = 0; i < 3; ++i)
            {
                bullPoint += GetBullPoint(list[i]);
            }
            var isBull = bullPoint % 10 == 0;
            var lastTwoPoint = (GetBullPoint(list[3])+ GetBullPoint(list[4]))%10;
            if (isBull)
            {
                if(lastTwoPoint == 0)
                {
                    return BullCardType.BullCtBull10;
                }
                else
                {
                    return (BullCardType)(lastTwoPoint + 1);
                }
            }
            else
            {
                return BullCardType.BullCtBull0;
            }
        }

        public static int GetBullPoint(int cardId)
        {
            var point = CardConfig.GetCardPoint(cardId);
            return point > 9 ? 10 : point + 1;
        }
        /// <summary>
        /// 获取单张牌牌力
        /// 点数: K>Q>....A
        /// 花色: 黑>红>梅>方
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static int GetCardPower(int cardId)
        {
            var point = CardConfig.GetCardPoint(cardId);
            var color = CardConfig.GetCardColor(cardId);
            return point * 100 + color;
        }
        /// <summary>
        /// 小于牛7:倍率1
        /// 牛7-牛牛:倍率 2,3,4,5
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetTypeRate(BullCardType type)
        {
            var rate = 1;
            if(type < BullCardType.BullCtBull7)
            {
                return rate;
            }
            else
            {
                return rate + (int)(type - BullCardType.BullCtBull6);
            }
        }

        public static int GetBankerRate(int rateIdx)
        {
            return BankerRate[rateIdx];
        }

        public static int GetPlayerRate(int rateIdx,RoomConfig cfg)
        {
            return cfg.IntParams[rateIdx];
        }


    }
}
