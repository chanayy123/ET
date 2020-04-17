using System.Collections.Generic;

namespace ETModel
{
	/// <summary>
	/// 网关消息分发组件:根据注册消息回调的进程id来转发消息到该进程
	/// </summary>
	public class RouteMessageDispatcherComponent : Component
	{
        public readonly Dictionary<ushort, AppType> appDic = new Dictionary<ushort, AppType>();
		public readonly Dictionary<ushort, List<IMHandler>> Handlers = new Dictionary<ushort, List<IMHandler>>();
	}
}