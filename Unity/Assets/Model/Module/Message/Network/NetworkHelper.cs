using System.Net;

namespace ETModel
{
	public static class NetworkHelper
	{
		public static IPEndPoint ToIPEndPoint(string host, int port)
		{
			return new IPEndPoint(IPAddress.Parse(host), port);
		}

		public static IPEndPoint ToIPEndPoint(string address)
		{
			int index = address.LastIndexOf(':');
			string host = address.Substring(0, index);
			string p = address.Substring(index + 1);
			int port = int.Parse(p);
			return ToIPEndPoint(host, port);
		}

        /// <summary>
        /// 从配置地址字符串转成对应协议支持的格式
        ///  tcp/kcp格式: ip:port ,   websocket格式:  ip:port=> http://ip:port/ ,如果ip是0.0.0.转成 http://*:port/
        /// </summary>
        /// <param name="address">格式 ip:port</param>
        /// <returns></returns>
        public static string ToAvailAddress(string address,NetworkProtocol protocol)
        {
            if(protocol == NetworkProtocol.WebSocket)
            {
                if(address.IndexOf("0.0.0.0") != -1)
                {
                    var port = address.Split(":")[1];
                    return $"http://*:{port}/";
                }
                return $"http://{address}/";
            }
            else
            {
                return address;
            }
        }

	}
}
