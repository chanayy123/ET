using System.Net;
using ETModel;
using System.Threading.Tasks;

namespace ETHotfix
{

    public class LoginData
    {
        public string Acc;
    }

    [HttpHandler(AppType.Gate, "/")]
	public class HttpTest : AHttpHandler
	{
		[Get] // url-> /Login?name=11&age=1111
		public string Login(string name, int age, HttpListenerRequest req, HttpListenerResponse resp)
		{
			Log.Info(name);
			Log.Info($"{age}");
			return "ok";
		}

		[Get("t")] // url-> /t
		public int Test()
		{
			System.Console.WriteLine("");
			return 1;
		}

		[Post] // url-> /Test1
		public int Test1(HttpListenerRequest req)
		{
			return 1;
		}

        [Post] // url-> /Test1
        public string TestPost(LoginData body, HttpListenerRequest req)
        {
            return $"{body.Acc}---";
        }

        [Get] // url-> /Test2
		public int Test2(HttpListenerResponse resp)
		{
			return 1;
		}

		[Get] // url-> /GetRechargeRecord
		public async ETTask<HttpResult> GetRechargeRecord(long id)
		{
			// var db = Game.Scene.GetComponent<DBProxyComponent>();

			// var info = await db.Query<RechargeRecord>(id);

			await Task.Delay(1000); // 用于测试

			object info = null;
			if (info != null)
			{
				return Ok(data: info);
			}
			else
			{
				return Error("ID不存在！");
			}
		}

        [Get]
        public async ETTask<HttpResult> FetchGameConfig()
        {
            await GameConfigComponent.Instance.FetchGameCfgList();
            return Ok("从数据库更新游戏配置成功!");
        } 
	}
}