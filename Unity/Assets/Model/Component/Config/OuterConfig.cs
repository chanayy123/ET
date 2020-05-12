using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	[BsonIgnoreExtraElements]
	public class OuterConfig: AConfigComponent
	{
        /// <summary>
        /// 监听地址:和公网地址对应的内网地址
        /// </summary>
		public string Address { get; set; }
        /// <summary>
        /// 公网连接地址
        /// </summary>
		public string Address2 { get; set; }
        /// <summary>
        /// 对外连接协议
        /// </summary>
        public NetworkProtocol Protocol = NetworkProtocol.WebSocket;
	}
}