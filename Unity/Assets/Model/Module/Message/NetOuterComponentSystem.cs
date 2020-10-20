using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class NetOuterComponentAwakeSystem : AwakeSystem<NetOuterComponent, NetworkProtocol>
	{
		public override void Awake(NetOuterComponent self,NetworkProtocol protocol)
		{
            self.Protocol = protocol;
			self.Awake(protocol);
			self.MessagePacker = new ProtobufPacker();
			self.MessageDispatcher = new OuterMessageDispatcher();
		}
	}

	[ObjectSystem]
	public class NetOuterComponentAwake1System : AwakeSystem<NetOuterComponent, string>
	{
		public override void Awake(NetOuterComponent self, string address)
		{
			self.Awake(self.Protocol, NetworkHelper.ToAvailAddress(address, self.Protocol));
			self.MessagePacker = new ProtobufPacker();
			self.MessageDispatcher = new OuterMessageDispatcher();
			Log.Debug($"当前【{self.Protocol}】服务器监听外网地址:{self.Protocol}");
		}
	}

    [ObjectSystem]
    public class NetOuterComponentAwake2System : AwakeSystem<NetOuterComponent, NetworkProtocol,string>
    {
        public override void Awake(NetOuterComponent self, NetworkProtocol protocol, string address)
        {
            self.Protocol = protocol;
            self.Awake(protocol, NetworkHelper.ToAvailAddress(address, self.Protocol));
            self.MessagePacker = new ProtobufPacker();
            self.MessageDispatcher = new OuterMessageDispatcher();
			Log.Debug($"当前【{protocol}】服务器监听外网地址:{address}");
		}
    }
#if SERVER
    [ObjectSystem]
    public class NetOuterComponentAwake3System : AwakeSystem<NetOuterComponent, string>
    {
        public override void Awake(NetOuterComponent self, string address)
        {
            Game.Scene.AddComponent<RouteMessageDispatcherComponent>();
        }
    }

    [ObjectSystem]
    public class NetOuterComponentAwake4System : AwakeSystem<NetOuterComponent, NetworkProtocol,string>
    {
        public override void Awake(NetOuterComponent self, NetworkProtocol protocol,string address)
        {
            Game.Scene.AddComponent<RouteMessageDispatcherComponent>();
        }
    }
#endif

    [ObjectSystem]
	public class NetOuterComponentLoadSystem : LoadSystem<NetOuterComponent>
	{
		public override void Load(NetOuterComponent self)
		{
			self.MessagePacker = new ProtobufPacker();
			self.MessageDispatcher = new OuterMessageDispatcher();
		}
	}
	
	[ObjectSystem]
	public class NetOuterComponentUpdateSystem : UpdateSystem<NetOuterComponent>
	{
		public override void Update(NetOuterComponent self)
		{
			self.Update();
		}
	}
}