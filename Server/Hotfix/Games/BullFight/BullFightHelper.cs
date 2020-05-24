using System;
using System.Collections.Generic;
using ETModel;
using System.Linq;
using Google.Protobuf.Collections;

namespace ETHotfix
{
    public static class BullFightHelper
    {
        /// <summary>
        /// 更新获得最佳牛型组合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static BullCardType UpdateBestGroup(RepeatedField<int> list)
        {
            //5张牌:3张组合
            BullCardType type = GetBullType(list);
            return type;
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

        public static int GetBullPoint(int card)
        {
            var point = CardConfig.GetCardPoint(card);
            return point > 9 ? 10 : point + 1;
        }

    }
}
