using System;
using UnityEngine;

namespace ETModel
{
	public static class ConfigHelper
	{

        public static string GetText(string key)
        {
            try
            {
                //GameObject config = (GameObject)ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset("config.unity3d", "Config");
                var go = Singleton<AddressableResComponent>.Instance.LoadAsset("config") as GameObject;
                var config = go.GetComponent<ReferenceCollector>().Get<TextAsset>(key);
                return config.text;
            }
            catch (Exception e)
            {
                throw new Exception($"load config file fail, key: {key}", e);
            }
        }


        public static async ETTask<string> GetTextAsync(string key)
        {
            try
            {
                //GameObject config = (GameObject)ETModel.Game.Scene.GetComponent<ResourcesComponent>().GetAsset("config.unity3d", "Config");
                var config = await Singleton<AddressableResComponent>.Instance.LoadAssetAsync(key) as TextAsset;
                return config.text;
            }
            catch (Exception e)
            {
                throw new Exception($"load config file fail, key: {key}", e);
            }
        }

		public static T ToObject<T>(string str)
		{
			return JsonHelper.FromJson<T>(str);
		}
	}
}