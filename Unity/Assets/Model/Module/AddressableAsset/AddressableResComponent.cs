using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ETModel
{
    public class AddressableResComponent : Singleton<AddressableResComponent>
	{
		private readonly Dictionary<string, UnityEngine.Object> _resourceCache = new Dictionary<string, UnityEngine.Object>();

        /// <summary>
        /// 检测所有可用更新并下载catalogs缓存到本地
        /// </summary>
        /// <returns></returns>
        public  async ETTask CheckAndDownloadAsync()
        {
            await Addressables.InitializeAsync().Task;
            var check =  Addressables.CheckForCatalogUpdates(false);
            var list = await check.Task;
            if(list != null && list.Count> 0)
            {
                Debug.Log($"有Catalogs需要更新");
                var update = Addressables.UpdateCatalogs(list,false);
                await update.Task;
                Debug.Log("keys: " + update.Result[0].Keys.ToList().ListToString());
                Debug.Log($"更新Catalogs完成");
                Addressables.Release(update);
                Addressables.Release(check);
            }
            else
            {
                Debug.Log("没有更新下载");
            }
        }
        /// <summary>
        /// 预加载对应标签资源分组
        /// </summary>
        /// <returns></returns>
        public async ETTask DownloadAssetsAsync(List<object> keys, bool sendEvent=false)
        {
            var op = Addressables.GetDownloadSizeAsync(keys);
            long size = await op.Task;
            Log.Debug($"预加载资源【{keys.ListToString()}】大小: " + size);
            if (size > 0)
            {
                if (sendEvent)
                {
                    Game.EventSystem.Run(EventIdType.LoadingBegin,keys);
                }
                var handler = Addressables.DownloadDependenciesAsync(keys,Addressables.MergeMode.Union);
                while (!handler.IsDone)
                {
                    Log.Debug($"加载进度: {handler.PercentComplete}");
                    Game.EventSystem.Run(EventIdType.LoadingProgress, handler.PercentComplete);
                    await Game.Scene.GetComponent<TimerComponent>().WaitAsync(100);
                }
                if (sendEvent)
                {
                    Game.EventSystem.Run(EventIdType.LoadingFinish, keys);
                }
                Log.Debug($"加载【{keys.ListToString()}】完成");
                Addressables.Release(handler);
            }
            Addressables.Release(op);
        }

        /// <summary>
        /// 缓存一些资源到内存,方便后续同步加载使用,比如配置资源
        /// </summary>
        /// <returns></returns>
        public async ETTask CacheConfigAsync()
        {
            var go = await LoadAssetAsync("config") as GameObject;
            var count = go.GetComponent<ReferenceCollector>().data.Count;
            Debug.Log($"共缓存了{count}个配置文件");
        }

        public void ReleaseConfigCache()
        {
            ReleaseAsset("config");
        }

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();
            ReleaseConfigCache();
            this._resourceCache.Clear();
		}

        public bool IsValid(string key)
        {
            return _resourceCache.ContainsKey(key);
        }

        public void ReleaseAsset(string key)
        {
            if(_resourceCache.TryGetValue(key,out UnityEngine.Object res))
            {
                Addressables.Release(res);
                _resourceCache.Remove(key);
            }
        }

		public UnityEngine.Object LoadAsset(string key)
		{
            _resourceCache.TryGetValue(key, out UnityEngine.Object res);
			return res;
		}

        public async ETTask<UnityEngine.Object> LoadAssetAsync(string key)
        {
            _resourceCache.TryGetValue(key, out UnityEngine.Object res);
            if(res == null)
            {
                res = await  Addressables.LoadAssetAsync<UnityEngine.Object>(key).Task;
                _resourceCache.Add(key, res);
            }
            return res;
        }

	}
}