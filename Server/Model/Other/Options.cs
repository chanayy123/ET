#if SERVER
using CommandLine;
#endif

namespace ETModel
{
	public class Options
	{
		[Option("appId", Required = false, Default = 1)]
		public int AppId { get; set; }
		
		// 没啥用，主要是在查看进程信息能区分每个app.exe的类型
		[Option("appType", Required = false, Default = AppType.Manager)]
		public AppType AppType { get; set; }

		[Option("config", Required = false, Default = "../Config/StartConfig/MultiServer.txt")]
		public string Config { get; set; }
        /// <summary>
        /// 运行时模式: 0表示非docker环境,内网进程之间通过ip通信, 1表示docker环境,内网进程之间是通过域名(即容器名)而不是ip来通信
        /// </summary>
        [Option("runtimeMode", Required = false, Default = 0)]
        public int RuntimeMode { get; set; }

    }
}