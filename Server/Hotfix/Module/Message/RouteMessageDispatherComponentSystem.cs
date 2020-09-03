using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class RouteMessageDispatherAwakeSystem : AwakeSystem<RouteMessageDispatcherComponent>
	{
		public override void Awake(RouteMessageDispatcherComponent self)
		{
			self.Load();
		}
	}

	[ObjectSystem]
	public class RouteMessageDispatcherLoadSystem : LoadSystem<RouteMessageDispatcherComponent>
	{
		public override void Load(RouteMessageDispatcherComponent self)
		{
			self.Load();
		}
	}

	/// <summary>
	/// 外网非actor消息分发组件扩展
	/// </summary>
	public static class RouteMessageDispatcherComponentExtensions
    {
		public static void Load(this RouteMessageDispatcherComponent self)
		{
			self.Handlers.Clear();

			List<Type> types = Game.EventSystem.GetTypes(typeof(MessageHandlerAttribute));

			foreach (Type type in types)
			{
				object[] attrs = type.GetCustomAttributes(typeof(MessageHandlerAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}

				MessageHandlerAttribute messageHandlerAttribute = attrs[0] as MessageHandlerAttribute;
                if (!(Activator.CreateInstance(type) is IMHandler iMHandler))
                {
                    Log.Error($"message handle {type.Name} 需要继承 IMHandler");
                    continue;
                }

                Type messageType = iMHandler.GetMessageType();
				ushort opcode = Game.Scene.GetComponent<OpcodeTypeComponent>().GetOpcode(messageType);
				if (opcode == 0)
				{
					Log.Error($"消息opcode为0: {messageType.Name}");
					continue;
				}
                self.RegisterAppType(opcode, messageHandlerAttribute.Type);
                self.RegisterHandler(opcode, iMHandler);
			}

        }

        public static void RegisterAppType(this RouteMessageDispatcherComponent self, ushort opcode, AppType type)
        {
            var appType = StartConfigComponent.Instance.StartConfig.AppType;
            if (!self.appDic.ContainsKey(opcode))
            {
                self.appDic.Add(opcode, type);
            }
            else if(type != self.appDic[opcode] && OpcodeHelper.IsOuterMessage(opcode))
            {
                Log.Warning($"外网消息{opcode} 不应该被多个类型进程处理!");
            }
        }

		public static void RegisterHandler(this RouteMessageDispatcherComponent self, ushort opcode, IMHandler handler)
		{
			if (!self.Handlers.ContainsKey(opcode))
			{
				self.Handlers.Add(opcode, new List<IMHandler>());
			}
			self.Handlers[opcode].Add(handler);
		}

		public static void Handle(this RouteMessageDispatcherComponent self, Session session, ushort opcode, object message)
		{
            AppType appType = StartConfigComponent.Instance.StartConfig.AppType;
            if(self.appDic.TryGetValue(opcode,out AppType type))
            {
                if (appType.Is(type))//如果当前进程可以处理,直接处理
                {
                    self.SelfHandle(session, opcode, message);
                }
                else
                {
                    self.RouteHandle(session, opcode, message);
                }
            }
            else
            {
                Log.Error($"消息没有处理: {opcode} {JsonHelper.ToJson(message)}");
                return;
            }
		}

        private static void SelfHandle(this RouteMessageDispatcherComponent self , Session session, ushort opcode, object message)
        {
            List<IMHandler> actions;
            if (!self.Handlers.TryGetValue(opcode, out actions))
            {
                Log.Error($"消息没有处理: {opcode} {JsonHelper.ToJson(message)}");
                return;
            }
            //IUserRequest消息服务器补足字段,不需要客户端填充
            if (message is IUserRequest iu)
            {
                    iu.GateSessionId = session.Id;
                    iu.UserId = session.GetComponent<SessionGateUserComponent>().User.UserId;
            }
            foreach (IMHandler ev in actions)
            {
                try
                {
                    ev.Handle(session, message);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        private static async void RouteHandle(this RouteMessageDispatcherComponent self, Session session, ushort opcode, object message)
        {
            //只能转发到其他单服,如果有多服应该发送actor消息,而不是走到这里
            var sendSession = NetInnerHelper.GetSessionByAppType(self.appDic[opcode]);
            switch (message)
            {
                case IRequest iRequest:
                    if(iRequest is IUserRequest iur)
                    {
                        iur.GateSessionId = session.Id;
                        iur.UserId = session.GetComponent<SessionGateUserComponent>().User.UserId;
                    }
                    var rpcId = iRequest.RpcId;
                    IResponse res = await sendSession.Call(iRequest);
                    res.RpcId = rpcId;
                    session.Reply(res);
                    break;
                case IMessage iMessage:
                    sendSession.Send(iMessage);
                    break;
                default:
                    break;
            }
        }

	}
}