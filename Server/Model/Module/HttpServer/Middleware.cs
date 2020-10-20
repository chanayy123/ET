using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace ETModel
{
    public enum MiddlewareResult
    {
        Processed = 1,
        Continue = 2
    }
    public interface IMiddleware
    {
        ETTask<MiddlewareResult> Execute(HttpServerContext context);
        void Load();
    }

    public interface IExceptionHandler
    {
        void HandlerException(HttpServerContext context, Exception ex);
    }

    public class MiddlewarePipeline
    {
        private List<IMiddleware> _middleWareList;
        private IExceptionHandler _exHandler;
        public MiddlewarePipeline()
        {
            this._middleWareList = new List<IMiddleware>();
        }

        public void Add(IMiddleware middleware)
        {
            this._middleWareList.Add(middleware);
        }

        public void Load()
        {
            try
            {
                for (int i = 0; i < this._middleWareList.Count; i++)
                {
                    this._middleWareList[i].Load();
                }
            }
            catch (Exception ex)
            {
                Log.Warning("MiddlewarePipeline Load exception " + ex);
            }
        }

        public void SetExceptionHandler(IExceptionHandler handler)
        {
            this._exHandler = handler;
        }

        public async void Execute(HttpServerContext context)
        {
            try
            {
                for (int i = 0; i < this._middleWareList.Count; i++)
                {
                    var result = await this._middleWareList[i].Execute(context);
                    if (result == MiddlewareResult.Processed)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                if (this._exHandler != null)
                {
                    this._exHandler.HandlerException(context, ex);
                }
                else
                {
                    throw;
                }
            }
        }

    }

    public interface IWebServiceBuilder
    {
        IWebServiceBuilder Use(IMiddleware middleware);
        IWebServiceBuilder SetExceptionHandler(IExceptionHandler handler);
    }

    #region 中间件定义
    public class HttpLog : IMiddleware
    {
        public ETTask<MiddlewareResult> Execute(HttpServerContext context)
        {
            var request = context.Request;
            var path = request.Url.LocalPath;
            var clientIp = request.RemoteEndPoint.ToString();
            var method = request.HttpMethod;
            Log.Debug($"http请求===path:{path} clientIp:{clientIp} method:{method}");
            return ETTask.FromResult(MiddlewareResult.Continue);
        }

        public void Load()
        {
        }
    }

    public class BlockIp : IMiddleware
    {
        private List<string> _forbiddens;
        public BlockIp(params string[] forbiddens)
        {
            _forbiddens = new List<string>(forbiddens);
        }
        public ETTask<MiddlewareResult> Execute(HttpServerContext context)
        {
            var clientIp = context.Request.RemoteEndPoint.Address;
            if (_forbiddens.Contains(clientIp.ToString()))
            {
                context.Response.Status((int)HttpStatusCode.Forbidden, "Ip Forbidden");
                return ETTask.FromResult(MiddlewareResult.Processed);
            }
            return ETTask.FromResult(MiddlewareResult.Continue);
        }
        public void Load()
        {
        }
    }

    public class CrossDomain : IMiddleware
    {
        private List<string> _allowOrigins;
        public CrossDomain(params string[] allows)
        {
            _allowOrigins = new List<string>(allows);
        }
        public ETTask<MiddlewareResult> Execute(HttpServerContext context)
        {
            var request = context.Request;
            switch (request.HttpMethod)
            {
                case "HEAD":
                    context.Response.Status((int)HttpStatusCode.OK);
                    return ETTask.FromResult(MiddlewareResult.Processed);
                case "OPTIONS":
                    var origin = context.Request.Headers.Get("Origin");
                    context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET");
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                    context.Response.Status((int)HttpStatusCode.OK);
                    return ETTask.FromResult(MiddlewareResult.Processed);
                case "GET":
                case "POST":
                    origin = context.Request.Headers.Get("Origin");
                    context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    break;
            }
            return ETTask.FromResult(MiddlewareResult.Continue);
        }
        public void Load()
        {
        }
    }

    public class SessionManager : IMiddleware
    {
        private const string CookieName = "__sessionid__";
        private readonly Dictionary<string, HttpSession> _dicSessions = new Dictionary<string, HttpSession>();
        public ETTask<MiddlewareResult> Execute(HttpServerContext context)
        {
            var cookie = context.Request.Cookies[CookieName];
            HttpSession session=null;
            if(cookie != null)
            {
                _dicSessions.TryGetValue(cookie.Value, out session);
            }
            
            if (session == null)
            {
                session = new HttpSession();
                var sessionId = GenerateSessionId();
                cookie = new Cookie(CookieName,sessionId );
                //cookie.Expires = DateTime.UtcNow.AddMinutes(3);
                _dicSessions.Add(sessionId, session);
                context.Response.SetCookie(cookie);
            }
            context.Session = session;
            return ETTask.FromResult(MiddlewareResult.Continue);
        }

        public string GenerateSessionId()
        {
            return Guid.NewGuid().ToString();
        }

        public void Load()
        {
        }
    }

    public class Authentication : IMiddleware
    {
        private const string _cookieName = "__userId__";
        private const string _sessionUserKey = "__user__";
        public ETTask<MiddlewareResult> Execute(HttpServerContext context)
        {
            IPrincipal user = null;
            var cookie = context.Request.Cookies[_cookieName];
            if(cookie != null)
            {
                user = context.Session[_sessionUserKey] as IPrincipal;
            }
            user = user ?? new AnonymousUser();
            context.User = user;
            return ETTask.FromResult(MiddlewareResult.Continue);
        }

        public static void Login(HttpServerContext context, string userName,int level)
        {
            var cookie = new Cookie(_cookieName, userName);
            context.Response.SetCookie(cookie);
            var user = new GenericUser(userName,level);
            context.Session[_sessionUserKey] = user;
            context.User = user;
        }

        public static void Logout(HttpServerContext context)
        {
            context.Session.Remove(_sessionUserKey);
            context.User = new AnonymousUser();
            var cookie = new Cookie(_cookieName, "");
            cookie.Expired = true;
            context.Response.SetCookie(cookie);
        }

        public void Load()
        {
        }
    }

    public class StaticFiles : IMiddleware
    {
        public ETTask<MiddlewareResult> Execute(HttpServerContext context)
        {
            var request = context.Request;
            var response = context.Response;
            var path = request.Url.LocalPath.TrimStart('/');
            path = string.IsNullOrEmpty(path) ? "index.html" : path;
            var ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext))//如果没有后缀名,就不当静态文件处理
            {
                return  ETTask.FromResult(MiddlewareResult.Continue);
            }
            Log.Debug($"url path " + path);
            var filePath = Path.Combine("static", path);
            if(path.Contains("favicon.ico") && !File.Exists(filePath))//浏览器默认请求图标不存在就忽略
            {
                return ETTask.FromResult(MiddlewareResult.Continue);
            }
            var bytes = File.ReadAllBytes(filePath);
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = bytes.Length;
            response.OutputStream.Write(bytes, 0, bytes.Length);
            response.OutputStream.Close();
            response.Close();
            return ETTask.FromResult(MiddlewareResult.Processed);
        }
        public void Load()
        {
        }
    }

    public class Http404 : IMiddleware
    {
        public ETTask<MiddlewareResult> Execute(HttpServerContext context)
        {
            context.Response.Status((int)HttpStatusCode.NotFound, "Not Found");
            return ETTask.FromResult(MiddlewareResult.Processed);
        }
        public void Load()
        {
        }
    }

    public class Http500 : IExceptionHandler
    {
        public void HandlerException(HttpServerContext context, Exception ex)
        {
            Log.Error(ex);
            context.Response.Status((int)HttpStatusCode.InternalServerError, "Internal Server Error: " + ex);
        }
    }

    public class Route : IMiddleware
    {
        public Dictionary<string, IHttpHandler> dispatcher;

        // 处理方法
        private Dictionary<MethodInfo, IHttpHandler> handlersMapping;

        // Get处理
        private Dictionary<string, MethodInfo> getHandlers;
        private Dictionary<string, MethodInfo> postHandlers;
        public   ETTask<MiddlewareResult> Execute(HttpServerContext context)
        {
            return InvokeHandler(context);
        }


        private async ETTask<MiddlewareResult> InvokeHandler(HttpServerContext context)
        {
            MethodInfo methodInfo = null;
            IHttpHandler httpHandler = null;
            string postbody = "";
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    this.getHandlers.TryGetValue(context.Request.Url.AbsolutePath, out methodInfo);
                    if (methodInfo != null)
                    {
                        this.handlersMapping.TryGetValue(methodInfo, out httpHandler);
                    }
                    break;
                case "POST":
                    this.postHandlers.TryGetValue(context.Request.Url.AbsolutePath, out methodInfo);
                    if (methodInfo != null)
                    {
                        this.handlersMapping.TryGetValue(methodInfo, out httpHandler);
                        using (StreamReader sr = new StreamReader(context.Request.InputStream))
                        {
                            postbody = sr.ReadToEnd();
                            Log.Debug("postbody len " + postbody.Length);
                        }
                    }
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    break;
            }

            if (httpHandler != null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                object[] args = InjectParameters(context, methodInfo, postbody);
                // 自动把返回值，以json方式响应。
                object resp = methodInfo.Invoke(httpHandler, args);
                object result = resp;
                if (resp is ETTask<HttpResult> t)
                {
                    await t;
                    result = t.Result;
                }
                if (result != null)
                {
                    using (StreamWriter sw = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
                    {
                        if (result.GetType() == typeof(string))
                        {
                            sw.Write(result.ToString());
                        }
                        else
                        {
                            //输出标准json格式
                            var data = JsonHelper.ToJson(result, new MongoDB.Bson.IO.JsonWriterSettings() { OutputMode = MongoDB.Bson.IO.JsonOutputMode.Strict });
                            sw.Write(data);
                        }
                    }
                    context.Response.Close();
                    return MiddlewareResult.Processed;
                }
                else
                {
                    return MiddlewareResult.Continue;
                }
            }
            else
            {
                return MiddlewareResult.Continue;
            }
        }

        /// <summary>
        /// 注入参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="methodInfo"></param>
        /// <param name="postbody"></param>
        /// <returns></returns>
        private static object[] InjectParameters(HttpServerContext context, MethodInfo methodInfo, string postbody)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            object[] args = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo item = parameterInfos[i];

                if (item.ParameterType == typeof(HttpServerContext))
                {
                    args[i] = context;
                    continue;
                }
                try
                {
                    switch (context.Request.HttpMethod)
                    {
                        case "POST":
                            if (item.Name == "postBody") // 约定参数名称为postBody,只传string类型。本来是byte[]，有需求可以改。
                            {
                                args[i] = postbody;
                            }
                            else if (item.ParameterType.IsClass && item.ParameterType != typeof(string) && !string.IsNullOrEmpty(postbody))
                            {
                                object entity = JsonHelper.FromJson(item.ParameterType, postbody);
                                args[i] = entity;
                            }

                            break;
                        case "GET":
                            string query = context.Request.QueryString[item.Name];
                            if (query != null)
                            {
                                object value = Convert.ChangeType(query, item.ParameterType);
                                args[i] = value;
                            }

                            break;
                        default:
                            args[i] = null;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    args[i] = null;
                }
            }

            return args;
        }

        public void Load()
        {
            this.dispatcher = new Dictionary<string, IHttpHandler>();
            this.handlersMapping = new Dictionary<MethodInfo, IHttpHandler>();
            this.getHandlers = new Dictionary<string, MethodInfo>();
            this.postHandlers = new Dictionary<string, MethodInfo>();

            List<Type> types = Game.EventSystem.GetTypes(typeof(HttpHandlerAttribute));

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(HttpHandlerAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                HttpHandlerAttribute httpHandlerAttribute = (HttpHandlerAttribute)attrs[0];
                object obj = Activator.CreateInstance(type);

                IHttpHandler ihttpHandler = obj as IHttpHandler;
                if (ihttpHandler == null)
                {
                    throw new Exception($"HttpHandler handler not inherit IHttpHandler class: {obj.GetType().FullName}");
                }

                this.dispatcher.Add(httpHandlerAttribute.Path, ihttpHandler);

                LoadMethod(type, httpHandlerAttribute, ihttpHandler);
            }
        }

        public void LoadMethod(Type type, HttpHandlerAttribute httpHandlerAttribute, IHttpHandler httpHandler)
        {
            // 扫描这个类里面的方法
            MethodInfo[] methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance);
            foreach (MethodInfo method in methodInfos)
            {
                object[] getAttrs = method.GetCustomAttributes(typeof(GetAttribute), false);
                if (getAttrs.Length != 0)
                {
                    GetAttribute get = (GetAttribute)getAttrs[0];
                    string path = method.Name;
                    if (!string.IsNullOrEmpty(get.Path))
                    {
                        path = get.Path;
                    }
                    getHandlers.Add(httpHandlerAttribute.Path + path, method);
                }


                object[] postAttrs = method.GetCustomAttributes(typeof(PostAttribute), false);
                if (postAttrs.Length != 0)
                {
                    // Post处理方法
                    PostAttribute post = (PostAttribute)postAttrs[0];

                    string path = method.Name;
                    if (!string.IsNullOrEmpty(post.Path))
                    {
                        path = post.Path;
                    }

                    postHandlers.Add(httpHandlerAttribute.Path + path, method);
                    //Log.Debug($"add handler[{httpHandler}.{method.Name}] path {httpHandlerAttribute.Path + path}");
                }

                if (getAttrs.Length == 0 && postAttrs.Length == 0)
                {
                    continue;
                }

                handlersMapping.Add(method, httpHandler);
            }
        }

    }

    #endregion

}
