using ETModel;

namespace ETHotfix
{
	public class OuterMessageDispatcher: IMessageDispatcher
	{
		public void Dispatch(Session session, ushort opcode, object message)
		{
            //心跳检测
            if (!this.CheckSessionValid(session)) return;
            DispatchAsync(session, opcode, message).Coroutine();
		}
		
        /// <summary>
        /// 检测客户端连接心跳是否合法: 发送间隔和频率来判断
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool CheckSessionValid(Session session)
        {
            var hb = session.GetComponent<HeartBeatComponent>();
            if (hb == null)
            {
                //Log.Warning("当前session没有心跳组件 默认合法!");
                return true;
            }
            else
            {
                //每次收到消息重置间隔
                hb.ReceiveTimeInterval = 0;
                hb.TotalNumPerSec += 1;
                if(hb.TotalNumPerSec > HeartBeatComponent.MAX_TIMES_PER_SEC)
                {
                    Log.Warning("当前session发送消息频率太快!");
                    hb.DisposeSession();
                    return false;
                }
            }
            return true;
        }

		public async ETVoid DispatchAsync(Session session, ushort opcode, object message)
		{
			// 根据消息接口判断是不是Actor消息，不同的接口做不同的处理
			switch (message)
			{
				case IActorLocationRequest actorLocationRequest: // gate session收到actor rpc消息，先向actor 发送rpc请求，再将请求结果返回客户端
				{
					long unitId = session.GetComponent<SessionPlayerComponent>().Player.UnitId;
					ActorLocationSender actorLocationSender = Game.Scene.GetComponent<ActorLocationSenderComponent>().Get(unitId);

					int rpcId = actorLocationRequest.RpcId; // 这里要保存客户端的rpcId
					long instanceId = session.InstanceId;
					IResponse response = await actorLocationSender.Call(actorLocationRequest);
					response.RpcId = rpcId;

					// session可能已经断开了，所以这里需要判断
					if (session.InstanceId == instanceId)
					{
						session.Reply(response);
					}
					
					break;
				}
				case IActorLocationMessage actorLocationMessage:
				{
					long unitId = session.GetComponent<SessionPlayerComponent>().Player.UnitId;
					ActorLocationSender actorLocationSender = Game.Scene.GetComponent<ActorLocationSenderComponent>().Get(unitId);
					actorLocationSender.Send(actorLocationMessage);
					break;
				}
				case IActorRequest actorRequest:  // 分发IActorRequest消息
				{
                        var gateUser = session.GetComponent<SessionGateUserComponent>().User;
                        if (gateUser.ActorId == 0)
                        {
                            Log.Warning("actorRequest actorid不能为空");
                            return;
                        }
                        int rpcId = actorRequest.RpcId;
                        long instanceId = session.InstanceId;
                        actorRequest.ActorId = gateUser.ActorId;
                        var response = await NetInnerHelper.CallActorMsg(actorRequest);
                        response.RpcId = rpcId;
                        // session可能已经断开了，所以这里需要判断
                        if (session.InstanceId == instanceId)
                        {
                            session.Reply(response);
                        }
                        break;
				}
				case IActorMessage actorMessage:  // 分发IActorMessage消息
				{
                        var gateUser = session.GetComponent<SessionGateUserComponent>().User;
                        if (gateUser.ActorId == 0)
                        {
                            Log.Warning("收到actorMessage actorid不能为空");
                            return;
                        }
                        actorMessage.ActorId = gateUser.ActorId;
                        NetInnerHelper.SendActorMsg(actorMessage);
                        break;
				}
				default:
				{
					// 非Actor消息
					//Game.Scene.GetComponent<MessageDispatcherComponent>().Handle(session, new MessageInfo(opcode, message));
                    Game.Scene.GetComponent<RouteMessageDispatcherComponent>().Handle(session, opcode, message);
					break;
				}
			}
		}
	}
}
