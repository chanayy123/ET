using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETModel
{

    [ObjectSystem]
    public class HttpServerComponentAwakeSystem : AwakeSystem<HttpServerComponent>
    {
        public override void Awake(HttpServerComponent self)
        {
            self.Awake(20, 100);
        }
    }

    [ObjectSystem]
    public class HttpServerComponentStartSystem : StartSystem<HttpServerComponent>
    {
        public override void Start(HttpServerComponent self)
        {
            self.Start();
        }
    }

    [ObjectSystem]
    public class HttpServerComponentLoadSystem : LoadSystem<HttpServerComponent>
    {
        public override void Load(HttpServerComponent self)
        {
            self.Load();
        }
    }


    public class HttpServerComponent :Component,IWebServiceBuilder
    {
        private Semaphore _semaphore;
        private HttpListener _listener;
        private MiddlewarePipeline _pipeline;
        public AppType appType;
        public HttpConfig HttpConfig;
        public void Awake(int concurrentCount=20,int maxConcurrentCount=20)
        {
            _semaphore = new Semaphore(concurrentCount, maxConcurrentCount);
            _pipeline = new MiddlewarePipeline();
            StartConfig startConfig = StartConfigComponent.Instance.StartConfig;
            this.appType = startConfig.AppType;
            this.HttpConfig = startConfig.GetComponent<HttpConfig>();
            this.RegisterMiddlewares();
        }
        public void Load()
        {
            _pipeline.Load();
        }

        public void Start()
        {
            try
            {
                _listener = new HttpListener();
                if (this.HttpConfig.Url == null)
                {
                    this.HttpConfig.Url = "";
                }

                foreach (string s in this.HttpConfig.Url.Split(';'))
                {
                    if (s.Trim() == "")
                    {
                        continue;
                    }
                    this.Bind(s);
                }
                this._listener.Start();
                //this.Accept();
                this.AcceptEx();
                Log.Debug($"当前【http】服务器监听地址: {HttpConfig.Url}");
            }
            catch (Exception ex)
            {
                throw new Exception($"http server error: ", ex);
            }
        }

        public void Bind(string url)
        {
            _listener.Prefixes.Add(url);
        }

        public  async void Accept()
        {
            long instanceId = this.InstanceId;

            while (true)
            {
                if (this.InstanceId != instanceId)
                {
                    return;
                }
                HttpListenerContext context = await _listener.GetContextAsync();
                try
                {
                    _pipeline.Execute(new HttpServerContext(context));
                }
                catch (Exception e)
                {
                    Log.Warning("HttpComponent accept exception " + e);
                }
                finally
                {
                    context.Response.Close();
                }
            }
        }

        public void AcceptEx()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    _semaphore.WaitOne();
                    var context = await _listener.GetContextAsync();
                    _semaphore.Release();
                    OneThreadSynchronizationContext.Instance.Post(this.OnRecvComplete, new HttpServerContext(context));
                }
            });
        }

        private void OnRecvComplete(object o)
        {
            var context = o as HttpServerContext;
            _pipeline.Execute(context);
        }

        public IWebServiceBuilder Use(IMiddleware middleware)
        {
            middleware.Load();
            _pipeline.Add(middleware);
            return this;
        }

        public IWebServiceBuilder SetExceptionHandler(IExceptionHandler handler)
        {
            _pipeline.SetExceptionHandler(handler);
            return this;
        }

        private void RegisterMiddlewares()
        {
            this.Use(new HttpLog());
            this.Use(new BlockIp());
            this.Use(new CrossDomain());
            this.Use(new SessionManager());
            this.Use(new StaticFiles());
            this.Use(new Authentication());
            this.Use(new Route());
            this.Use(new Http404());
            this.SetExceptionHandler(new Http500());
        }

    }
}
