﻿using System;
using ETModel;

namespace ETHotfix
{
	public static class Init
	{
		public static void Start()
		{
#if ILRuntime
			if (!Define.IsILRuntime)
			{
				Log.Error("mono层是mono模式, 但是Hotfix层是ILRuntime模式");
			}
#else
			if (Define.IsILRuntime)
			{
				Log.Error("mono层是ILRuntime模式, Hotfix层是mono模式");
			}
#endif
			
			try
			{
				// 注册热更层回调
				ETModel.Game.Hotfix.Update = () => { Update(); };
				ETModel.Game.Hotfix.LateUpdate = () => { LateUpdate(); };
				ETModel.Game.Hotfix.OnApplicationQuit = () => { OnApplicationQuit(); };
				
				Game.Scene.AddSingletonComponent<UIManagerComponent>();
                Game.Scene.AddSingletonComponent<UIFactoryComponent>();
				Game.Scene.AddSingletonComponent<OpcodeTypeComponent>();
				Game.Scene.AddSingletonComponent<MessageDispatcherComponent>();
				Game.Scene.AddSingletonComponent<ConfigComponent>();
                UnitConfig unitConfig = Singleton<ConfigComponent>.Instance.Get(typeof(UnitConfig), 1001) as UnitConfig;
                Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");

                Game.EventSystem.Run(EventIdType.InitSceneStart);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void Update()
		{
			try
			{
				Game.EventSystem.Update();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void LateUpdate()
		{
			try
			{
				Game.EventSystem.LateUpdate();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public static void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}