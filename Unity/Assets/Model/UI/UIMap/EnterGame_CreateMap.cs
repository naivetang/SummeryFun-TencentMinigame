using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.EnterGame)]
    public class EnterGame_CreateMap : AEvent
    {
        public override void Run()
        {
            //UIFactory.Create<UIMapComponent>(ViewLayer.UIMainLayer, UIType.UIHall).Coroutine();

            //UIFactory.Create<UIStartComponent>(ViewLayer.UIFullScreenLayer, UIType.UIStart).Coroutine();

            //UIFactory.Create<UIGuideSceneComponent>(ViewLayer.UIFullScreenLayer, UIType.UIGuideScene).Coroutine();
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();

            if (!Define.LoadFromRes)
                resourcesComponent.LoadBundle(UIType.UIStart.StringToAB());

            UIFactory.Create<UIStartComponent>(ViewLayer.UIPopupLayer, UIType.UIStart).Coroutine();

            // 如果从AssetBund加载，先在登录界面预加载
            if(!Define.LoadFromRes)
                this.LoadAllBundle().Coroutine();
            
            
        }

        /// <summary>
        /// 只有加载大场景，还没有卸载的逻辑，后面补
        /// </summary>
        /// <returns></returns>
        async ETVoid LoadAllBundle()
        {
            ResourcesComponent res = Game.Scene.GetComponent<ResourcesComponent>();

            var stopwatch = new System.Diagnostics.Stopwatch();

            // UICG


            stopwatch.Start();

            await res.LoadBundleAsync(UIType.UICG.StringToAB());

            stopwatch.Stop();
            

            Log.Info($"加载{UIType.UICG}资源所用时间(毫秒)：{stopwatch.ElapsedMilliseconds}");

            // UIMap
            
            stopwatch.Start();

            await res.LoadBundleAsync(UIType.UIMap.StringToAB());

            stopwatch.Stop();

            Log.Info($"加载{UIType.UIMap}资源所用时间(毫秒)：{stopwatch.ElapsedMilliseconds}");

            // UIMain

            stopwatch.Start();

            await res.LoadBundleAsync(UIType.UIMain.StringToAB());

            stopwatch.Stop();

            Log.Info($"加载{UIType.UIMain}资源所用时间(毫秒)：{stopwatch.ElapsedMilliseconds}");

            // UIGuideScene

            stopwatch.Start();

            await res.LoadBundleAsync(UIType.UIGuideScene.StringToAB());

            stopwatch.Stop();

            Log.Info($"加载{UIType.UIGuideScene}资源所用时间(毫秒)：{stopwatch.ElapsedMilliseconds}");

            // UIShaddockScene

            stopwatch.Start();

            await res.LoadBundleAsync(UIType.UIShaddockScene.StringToAB());

            stopwatch.Stop();

            Log.Info($"加载{UIType.UIShaddockScene}资源所用时间(毫秒)：{stopwatch.ElapsedMilliseconds}");
        }
    }


    [Event(EventIdType.CGFinish)]
    public class CGFinish_CreateHall : AEvent<GameObject>
    {

        public override void Run(GameObject a)
        {
             UIBase b = UIFactory.Create<UIMapComponent, GameObject>(ViewLayer.UIMainLayer, UIType.UIMap, a).Result;

             // Transform player = Game.Scene.GetComponent<UnitComponent>().MyUnit.GameObject.transform;
             //
             // Transform map = b.GameObject.transform;
             //
             //
             // b.AddComponent<MapFllowComponent, Transform, Transform>(player, map);

            UIFactory.Create<UIMainComponent>(ViewLayer.UIFixedLayer, UIType.UIMain).Coroutine();
        }
    }
}
