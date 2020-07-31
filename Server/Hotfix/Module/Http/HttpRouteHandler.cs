using ETModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ETHotfix
{
    public class LoginRequest
    {
        public string Acc;
        public string Pwd;
    }

    public class ManagerData
    {
        public string Acc;
        public int Level;
    }

    [HttpHandler(AppType.Http, "/")]
    public class HttpRouteHandler : AHttpHandler
    {
        [Post]
        public HttpResult Login(LoginRequest data, HttpServerContext context)
        {
            if(data.Acc.Equals("yy") && data.Pwd.Equals("123"))
            {
                int level = 999;
                Authentication.Login(context,data.Acc,level);
                return Ok("",new ManagerData() { Acc = data.Acc, Level = level });
            }
            else if (data.Acc.Equals("abc") && data.Pwd.Equals("123"))
            {
                int level = 1;
                Authentication.Login(context, data.Acc, level);
                return Ok("", new ManagerData() { Acc = data.Acc, Level = level });
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Error();
            }
        }

        [Post]
        public HttpResult Logout(HttpServerContext context)
        {
            Authentication.Logout(context);
            return Ok();
        }

        [Get]
        public HttpResult  ManagerData(HttpServerContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                return Error("无效用户!");
            }
            else
            {
                var user = context.User as GenericUser;
                return Ok("",new ManagerData() { Acc=user.Name,Level=user.Level});
            }
        }

        [Get]
        public async ETTask<HttpResult> GameConfig()
        {
            await GameConfigComponent.Instance.FetchGameCfgList();
            return Ok("从数据库更新游戏配置成功!");
        }


    }
}
