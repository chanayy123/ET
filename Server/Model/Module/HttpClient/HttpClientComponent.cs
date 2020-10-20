using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{

    [ObjectSystem]
    public class HttpClientComponentAwakeSystem : AwakeSystem<HttpClientComponent>
    {
        public override void Awake(HttpClientComponent self)
        {
            self.Awake();
        }
    }

    public class HttpClientComponent:Component
    {
        private static HttpClientComponent _instance;
        private HttpClient _client;

        public static HttpClientComponent Instance { get; private set; }
        public void Awake()
        {
            Instance = this;
            this._client = new HttpClient();
        }

        public Uri BaseAddress
        {
            get
            {
                return this._client.BaseAddress;
            }
            set
            {
                if(this._client.BaseAddress != value)
                {
                    this._client.BaseAddress = value;
                    //预热httpclient:第一次请求比较慢
                    this._client.SendAsync(new HttpRequestMessage()
                    {
                        Method = new HttpMethod("HEAD"),
                        RequestUri = new Uri($"{ value.AbsoluteUri}/")
                    }).Result.EnsureSuccessStatusCode();
                }
            }
        }

        public  async ETTask<string> PostAsync(string url, string body)
        {
            try
            {
                var content = new StringContent(body, Encoding.UTF8);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var res = await this._client.PostAsync(url, content);
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    return data;
                }
                else
                {
                    return res.StatusCode.ToString();
                }
            }
            catch (Exception e)
            {
                Log.Warning($"PostAsync  {url}  {body} 异常: {e}");
                return null;
            }
        }

        public string Post(string url,string body)
        {
            try
            {
                var content = new StringContent(body, Encoding.UTF8);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                Task<HttpResponseMessage> t = this._client.PostAsync(url, content);
                if (t.Result.IsSuccessStatusCode)
                {
                    var data = t.Result.Content.ReadAsStringAsync().Result;
                    return data;
                }
                return null;
            }
            catch (Exception e)
            {
                Log.Warning($"Post {url}  {body} 异常: {e}");
                return null;
            }
        }

        public string Get(string url)
        {
            try
            {
                Task<string> t = this._client.GetStringAsync(url);
                 return t.Result;
            }
            catch (Exception e)
            {
                Log.Warning($"Get {url} 异常: {e}");
                return null;
            }
        }

        public async ETTask<string> GetAsync(string url)
        {
            try
            {
                var content = await this._client.GetStringAsync(url);
                return content;
            }
            catch (Exception e)
            {
                Log.Warning($"GetAsync {url} 异常: {e}");
                return null;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            base.Dispose();
            this._client.Dispose();
            this._client = null;
            Instance = null;
        }


    }
}
