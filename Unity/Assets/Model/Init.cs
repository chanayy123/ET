using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public class Init : MonoBehaviour
	{
		private void Start()
		{
			this.StartAsync().Coroutine();
		}
		
		private async ETVoid StartAsync()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

				DontDestroyOnLoad(gameObject);
				Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);             

                Game.Scene.AddComponent<TimerComponent>();
				Game.Scene.AddComponent<NetOuterComponent, NetworkProtocol>(NetworkProtocol.KCP);
                Game.Scene.AddComponent<AddressableResComponent>();
				Game.Scene.AddComponent<UIManagerComponent>();
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();
                //检测是否有更新
                await AddressableResComponent.Instance.CheckAndDownloadAsync();
                //开始加载预更新资源
                await AddressableResComponent.Instance.DownloadAssetsAsync(new List<object> { "preload" });
                //开始加载更新资源,并发送进度事件
                await AddressableResComponent.Instance.DownloadAssetsAsync(new List<object> { "hotfix" }, true);
                //缓存配置资源到内存,方便以后同步加载
                await AddressableResComponent.Instance.CacheConfigAsync();
                Game.Scene.AddComponent<ConfigComponent>();
                Game.Scene.AddComponent<GlobalConfigComponent>();
                await Game.Hotfix.LoadHotfixAssembly();
                Game.Hotfix.GotoHotfix();
                //释放内存缓存配置资源
                AddressableResComponent.Instance.ReleaseConfigCache();
                Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
            }
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			OneThreadSynchronizationContext.Instance.Update();
			Game.Hotfix.Update?.Invoke();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.Hotfix.LateUpdate?.Invoke();
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Hotfix.OnApplicationQuit?.Invoke();
			Game.Close();
		}
	}
}