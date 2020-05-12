namespace ETModel
{
    /// <summary>
    /// 基准测试客户端
    /// </summary>
	public class BenchmarkComponent: Component
	{
		public int k;

		public long time1 = TimeHelper.ClientNow();
	}
}