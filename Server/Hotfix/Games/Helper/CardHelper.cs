using System;
using System.Collections.Generic;
using ETModel;
using Google.Protobuf.Collections;
using System.Linq;

namespace ETHotfix
{
	public static class CardHelper
    {
        public static void Shuffle(List<int> list)
        {
            list.Sort((l, r) => RandomHelper.RandomNumber(-1, 2));
        }

        /// <summary>
        /// 判断俩列表内容是否相等: 不考虑顺序只考虑内容
        /// 要求:每个列表内容不能重复
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static bool EqualsCardList(IEnumerable<int> l1, IEnumerable<int> l2)
        {
            return l1.Count() == l2.Count() && l1.All(l2.Contains);
        }
	}
}
