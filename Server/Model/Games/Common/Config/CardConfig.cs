using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    /// <summary>
    /// 卡牌配置类
    /// 卡牌id: 0-53
    /// 卡牌花色: 0 方块 1 梅花 2 红桃 3 黑桃 4 小王 5 大王
    /// 卡牌点数: 0-12 : A  2 3...K, 52 小王 53 大王
    /// </summary>
    public static class CardConfig
    {
        public const int COLOR_DIAMOND = 0;
        public const int COLOR_CLUB= 1;
        public const int COLOR_HEART = 2;
        public const int COLOR_SPADE = 3;
        public const int COLOR_JOKER_SMALL = 4;
        public const int COLOR_JOKER_BIG = 5;

        public const int ID_JOKER_SMALL = 52;
        public const int ID_JOKER_BIG = 53;

        /// <summary>
        /// 获取花色(从小到大):  0 方块 1 梅花 2 红桃 3 黑桃 4 小王 5 大王
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static int GetCardColor(int cardId)
        {
            if(cardId == ID_JOKER_SMALL)
            {
                return COLOR_JOKER_SMALL;
            }else if(cardId == ID_JOKER_BIG)
            {
                return COLOR_JOKER_BIG;
            }
            else
            {
                return cardId / 13;
            }
        }
        /// <summary>
        /// 获取卡牌点数: 0-12-> A 2 3 ..K,  小王 52 大王 53
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static int GetCardPoint(int cardId)
        {
            if (cardId == ID_JOKER_SMALL)
            {
                return ID_JOKER_SMALL;
            }
            else if (cardId == ID_JOKER_BIG)
            {
                return ID_JOKER_BIG;
            }
            else
            {
                return cardId %13;
            }
        }

    }

}
