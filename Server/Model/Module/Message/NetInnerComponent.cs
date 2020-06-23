using System.Collections.Generic;
using System.Net;

namespace ETModel
{
	public class NetInnerComponent: NetworkComponent
	{
		public readonly Dictionary<IPEndPoint, Session> adressSessions = new Dictionary<IPEndPoint, Session>();

		public override void Remove(long id)
		{
			Session session = this.Get(id);
			if (session == null)
			{
				return;
			}
			this.adressSessions.Remove(session.RemoteAddress);

			base.Remove(id);
		}

		/// <summary>
		/// 从地址缓存中取Session,如果没有则创建一个新的Session,并且保存到地址缓存中
		/// </summary>
		public Session Get(IPEndPoint ipEndPoint)
		{
			if (this.adressSessions.TryGetValue(ipEndPoint, out Session session))
			{
				return session;
			}
            var option = Game.Scene.GetComponent<OptionComponent>();
            var hostEndPoint = StartConfigComponent.Instance.GetInnerHost(ipEndPoint);
            if(option.RunInDocker && hostEndPoint != null)
            {
                try
                {
                    IPAddress[] list = Dns.GetHostAddresses(hostEndPoint.Host);
                    if(list != null && list.Length > 0)
                    {
                        var endPoint = new IPEndPoint(list[0], hostEndPoint.Port);
                        session = this.Create(endPoint);
                        this.adressSessions.Add(ipEndPoint, session);
                        Log.Debug($"{StartConfigComponent.Instance.StartConfig.AppType} 内网连接域名 {hostEndPoint.Host} 对应IP列表: { string.Join<IPAddress>(",", list)}");
                    }
                }
                catch (System.Exception e)
                {
                    Log.Warning($"{hostEndPoint.Host}域名解析失败: {e} 转为通过ip连接");
                    session = this.Create(ipEndPoint);
                    this.adressSessions.Add(ipEndPoint, session);
                }
            }
            else
            {
                session = this.Create(ipEndPoint);
                this.adressSessions.Add(ipEndPoint, session);
            }
			return session;
		}

    }
}