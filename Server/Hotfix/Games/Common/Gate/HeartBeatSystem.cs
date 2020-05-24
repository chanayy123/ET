using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{

    /// <summary>
    /// 注册session创建事件: 只有外网session创建才绑定心跳组件
    /// </summary>
    [ObjectSystem]
   public class SessionHeartBeatAwakeSystem : AwakeSystem<Session,AChannel>
    {
        public override void Awake(Session self,AChannel ch)
        {
            if(self.Network.AppType == AppType.None) //表明是外网组件，内网组件都会配置类型
            {
                var appType = StartConfigComponent.Instance.StartConfig.AppType;
                //基准测试连接不需要检测心跳
                if (!StartConfigComponent.Instance.IsBenchmark(appType))
                {
                    self.AddComponent<HeartBeatComponent>();
                }
            }
        }
    }

    /// <summary>
    /// 注册外网组件创建事件: 只有外网组件才需要心跳管理组件
    /// </summary>
    [ObjectSystem]
    public class NetOuterHeartBeatAwakeSystem : AwakeSystem<NetOuterComponent, string>
    {
        public override void Awake(NetOuterComponent self, string address)
        {
            Game.Scene.AddComponent<HeartBeatManagerComponnet>();
        }
    }

    /// <summary>
    /// 注册外网组件创建事件: 只有外网组件才需要心跳管理组件
    /// </summary>
    [ObjectSystem]
    public class NetOuterHeartBeatAwake2System : AwakeSystem<NetOuterComponent, NetworkProtocol,string>
    {
        public override void Awake(NetOuterComponent self, NetworkProtocol protocol, string address)
        {
            Game.Scene.AddComponent<HeartBeatManagerComponnet>();
        }
    }


    [ObjectSystem]
    class HeartBeatManagerAwakeSystem : AwakeSystem<HeartBeatManagerComponnet>
    {
        public override void Awake(HeartBeatManagerComponnet self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    class HeartBeatManagerStartSystem : StartSystem<HeartBeatManagerComponnet>
    {
        public override async void Start(HeartBeatManagerComponnet self)
        {
            var removeList = new List<long>();
            var timer = TimerComponent.Instance;
            while (true)
            {
                await timer.WaitAsync(1000);
                removeList.Clear();
                foreach (var item in self.dic)
                {
                    item.Value.TotalNumPerSec = 0;
                    ++item.Value.ReceiveTimeInterval;
                    if(item.Value.ReceiveTimeInterval > HeartBeatComponent.MAX_REV_INTEVAL)
                    {
                        Log.Warning("当前session发送消息超过最大时间间隔!");
                        removeList.Add(item.Key);
                    }
                }
                //移除所有非法session
                foreach (var item in removeList)
                {
                    var hb = self.dic[item];
                    hb.DisposeSession();
                    self.dic.Remove(item);
                }

            }
        }
    }


    [ObjectSystem]
    class HeartBeatAwakeSystem : AwakeSystem<HeartBeatComponent>
    {
        public override void Awake(HeartBeatComponent self)
        {
            self.Awake();
            HeartBeatManagerComponnet.Instance.Add(self);
        }
    }

    [ObjectSystem]
    class HeartBeatDestroySystem : DestroySystem<HeartBeatComponent>
    {
        public override void Destroy(HeartBeatComponent self)
        {
            HeartBeatManagerComponnet.Instance.Remove(self.InstanceId);
        }
    }


}
