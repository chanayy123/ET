using ETHotfix;
using Google.Protobuf.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Game.Bull.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var type1 = BullFightHelper.GetBestSort(new RepeatedField<int>() { 1, 2,3,4,5 },out RepeatedField<int> sortlist1);
            var type2 = BullFightHelper.GetBestSort(new RepeatedField<int>() { 1, 3, 4, 5,2 }, out RepeatedField<int> sortlist2);           
            Assert.AreEqual(type1, type2);
            Assert.AreEqual(sortlist1, sortlist2);
        }
    }
}