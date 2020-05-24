using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    /// <summary>
    /// 管理所有心跳组件
    /// </summary>
    public class HeartBeatManagerComponnet : Component {
        public readonly Dictionary<long, HeartBeatComponent> dic = new Dictionary<long, HeartBeatComponent>();
        public static HeartBeatManagerComponnet Instance { get; private set; }
        public void Awake()
        {
            Instance = this;
        }
        public  void Add(HeartBeatComponent hb)
        {
            this.dic.Add(hb.InstanceId, hb);
        }
        public  void Remove( long id)
        {
            this.dic.Remove(id);
        }
    }
    /// <summary>
    /// 绑定session的心跳组件
    /// </summary>
    public class HeartBeatComponent:Component
    {
        //最大消息间隔
        public const int MAX_REV_INTEVAL = 120;
        //一秒钟最大接收消息数量
        public const int MAX_TIMES_PER_SEC = 10;
        /// <summary>
        /// 收到消息时间戳
        /// </summary>
        public int ReceiveTimeInterval { get; set; }
        /// <summary>
        /// 每秒钟累计发送消息数
        /// </summary>
        public int TotalNumPerSec { get; set; }

        public void Awake()
        {
            ReceiveTimeInterval = 0;
            TotalNumPerSec = 0;
        }

        public void DisposeSession()
        {
            Entity.Dispose();
        }
    }
}
