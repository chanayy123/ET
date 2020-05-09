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

		[Option("config", Required = false, Default = "../Config/StartConfig/127.0.0.1.txt")]
		public string Config { get; set; }
        /// <summary>
        /// mongo容器别名:为了替代mongo容器ip,方便其他容器直接通过别名连接
        /// </summary>
        [Option("mongoAlias", Required = false, Default = "")]
        public string MongoAlias { get; set; }
    }
}