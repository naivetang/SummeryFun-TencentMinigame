﻿using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public class Init : MonoBehaviour
	{
		private void Start()
		{
			this.StartAsync().Coroutine();
			
			//Application.targetFrameRate = 60;
			
		}
		
		private async ETVoid StartAsync()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

				DontDestroyOnLoad(gameObject);
				Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

				Game.Scene.AddComponent<TimerComponent>();
				Game.Scene.AddComponent<GlobalConfigComponent>();
				//Game.Scene.AddComponent<NetOuterComponent>();
				Game.Scene.AddComponent<ResourcesComponent>();
				Game.Scene.AddComponent<PlayerComponent>();
				Game.Scene.AddComponent<UnitComponent>();

				// 下载ab包
				await BundleHelper.DownloadBundle();

                Game.Scene.AddComponent<UIComponent>();

                //Game.Hotfix.LoadHotfixAssembly();

                // 加载配置
                Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
				Game.Scene.AddComponent<ConfigComponent>();
				Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
				//Game.Scene.AddComponent<OpcodeTypeComponent>();
				//Game.Scene.AddComponent<MessageDispatcherComponent>();

				//Game.Hotfix.GotoHotfix();

				//Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
				Game.EventSystem.Run(EventIdType.EnterGame);
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

        private void FixedUpdate()
        {
            Game.EventSystem.FixedUpdate();
        }
        
        
		private void OnApplicationQuit()
		{
			Game.Hotfix.OnApplicationQuit?.Invoke();
			Game.Close();
		}
	}
}