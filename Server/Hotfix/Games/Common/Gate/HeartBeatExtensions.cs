using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{


    /// <summary>
    /// 心跳管理组件扩展
    /// </summary>
    public static  class HeartBeatManagerComponnetExtensions
    {
        public static void Add(this HeartBeatManagerComponnet self, HeartBeatComponent hb)
        {
            self.dic.Add(hb.InstanceId, hb);
        }
        public static void Remove(this HeartBeatManagerComponnet self, long id)
        {
            self.dic.Remove(id);
        }
    }

}
