using System.Net;
using ETModel;
using System.Threading.Tasks;
using System;

namespace ETHotfix
{

    public class LoginData
    {
        public string Acc;
        public string Pwd;
    }

    public class LoginResponse
    {
        public string Acc;
        public long SessionId;
        public int Level;
    }

    [HttpHandler(AppType.Http, "/test")]
	public class HttpTest : AHttpHandler
	{
		[Post] // url-> /Login
		public async ETTask<HttpResult> Login(LoginData login, HttpListenerRequest req, HttpListenerResponse resp)
		{
			Log.Info($"login {login.Acc}  {login.Pwd}");
            if(login.Acc == "yy" && login.Pwd == "123")
            {
                var res = new LoginResponse()
                {
                    Acc=login.Acc,
                    SessionId = IdGenerater.GenerateId(),
                    Level = 999
                };
                var cookie = new Cookie("SessionId", res.SessionId.ToString());
                //cookie.Expires = DateTime.UtcNow.AddMinutes(3);
                resp.SetCookie(cookie);
                //resp.AppendCookie(new Cookie("Domain", "http://192.168.2.128:8002"));
                return await ETTask.FromResult(Ok("登陆成功",res));
            }else if(login.Acc == "abc" && login.Pwd == "123")
            {
                var res = new LoginResponse()
                {
                    Acc = login.Acc,
                    SessionId = IdGenerater.GenerateId(),
                    Level = 1
                };
                return await ETTask.FromResult(Ok("登陆成功", res));
            }
			return Error("用户名密码错误!");
		}

		[Get("t")] // url-> /t
		public int Test(HttpListenerResponse resp)
		{
            return 1;
		}
        [Get]
        public string StaticHtml(HttpListenerRequest req,HttpListenerResponse resp)
        {
            return "静态网页";
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
				return Error("ID不存在！"+id);
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