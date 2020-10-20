namespace ETModel
{
	public class ClientConfig: AConfigComponent
	{
        /// <summary>
        /// 测试服务器地址
        /// </summary>
		public string Address { get; set; }

		/// <summary>
		/// 对外连接协议
		/// </summary>
		public NetworkProtocol Protocol { get; set; }
	}
}