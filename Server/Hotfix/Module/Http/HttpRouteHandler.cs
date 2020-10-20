using ETModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

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
        public const string UploadDir = "/upload/";
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

        [Post("upload")]
        //约定upload接口传输格式:纯二进制字符串
        public async ETTask<HttpResult> Upload(HttpServerContext context, string postBody)
        {
            var list = new byte[postBody.Length];
            //客户端传来的是二进制字符串,不能当普通字符串用Encoding.GetBytes来转换,默认UTF-8这一转长度就变了,所以要当一个个字节的转
            for (var i = 0; i < postBody.Length; ++i)
            {
                list[i] = Convert.ToByte(postBody[i]);
            }
            var fileName = context.Request.Headers.Get("File-Name");
            fileName = string.IsNullOrEmpty(fileName) ? "tmp":fileName;
            fileName = HttpUtility.UrlDecode(fileName);
            var path = $"{Directory.GetCurrentDirectory()}{UploadDir}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += fileName;
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await fs.WriteAsync(list, 0, list.Length);
            }          
            return Ok("upload success");
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
