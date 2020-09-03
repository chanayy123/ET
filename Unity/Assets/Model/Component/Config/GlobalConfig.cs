using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{

    [Config((int)(AppType.ClientH | AppType.ClientM))]
    public partial class GlobalConfigCategory : ACategory<GlobalConfig>
    {
    }

    [BsonIgnoreExtraElements]
    public class GlobalConfig: IConfig
    {
        public long Id { get; set; }
        public string AssetBundleServerUrl;
		public string Address;

		public string GetUrl()
		{
			string url = this.AssetBundleServerUrl;
#if UNITY_ANDROID
			url += "Android/";
#elif UNITY_IOS
			url += "IOS/";
#elif UNITY_WEBGL
			url += "WebGL/";
#elif UNITY_STANDALONE_OSX
			url += "MacOS/";
#else
			url += "PC/";
#endif
			Log.Debug(url);
			return url;
		}
	}
}
