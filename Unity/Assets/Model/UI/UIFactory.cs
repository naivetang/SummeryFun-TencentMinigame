using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public static class UIFactory
    {
        public static async ETTask<UIBase>  Create<T>(ViewLayer layer,string prefabName) where  T : Component
        {
            await ETTask.CompletedTask;

            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            
            if(!resourcesComponent.hasBeforeLoad(prefabName))
                resourcesComponent.LoadBundle(prefabName.StringToAB());
            
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(prefabName.StringToAB(), prefabName);
            
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

            UIBase ui = ComponentFactory.Create<UIBase, ViewLayer, string, GameObject>(layer, prefabName, gameObject);

            ui.AddComponent(typeof (T));

            Game.Scene.GetComponent<UIComponent>().AddUI(ui);

            return ui;
        }

        public static async ETTask<UIBase> Create<T,P1,P2>(ViewLayer layer, string prefabName, P1 p1,P2 p2) where T : Component, new()
        {
            await ETTask.CompletedTask;

            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();

            if (!resourcesComponent.hasBeforeLoad(prefabName))
                resourcesComponent.LoadBundle(prefabName.StringToAB());

            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(prefabName.StringToAB(), prefabName);

            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

            UIBase ui = ComponentFactory.Create<UIBase, ViewLayer, string, GameObject>(layer, prefabName, gameObject);

            ui.AddComponent<T, P1, P2>(p1, p2);
            
            //ui.AddComponent(typeof(T));

            Game.Scene.GetComponent<UIComponent>().AddUI(ui);

            return ui;
        }


        public static async ETTask<UIBase> CreateByTrigger(ViewLayer layer, string prefabName) 
        {
            await ETTask.CompletedTask;

            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();

            //var stopwatch = new System.Diagnostics.Stopwatch();


            //stopwatch.Start();


            if (!resourcesComponent.hasBeforeLoad(prefabName))
                resourcesComponent.LoadBundle(prefabName.StringToAB());
            
            
            //stopwatch.Stop();
            
            //Log.Info($"加载{prefabName}资源所用时间(毫秒)：{stopwatch.ElapsedMilliseconds}");
            
            //stopwatch.Restart();

            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(prefabName.StringToAB(), prefabName);

            //stopwatch.Stop();

            //Log.Info($"获取{prefabName} 的Asset所用时间(毫秒)：{stopwatch.ElapsedMilliseconds}");

            //stopwatch.Restart();

            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

            //stopwatch.Stop();

           // Log.Info($"实例化{prefabName} 的所用时间(毫秒)：{stopwatch.ElapsedMilliseconds}");

            UIBase ui = ComponentFactory.Create<UIBase, ViewLayer, string, GameObject>(layer, prefabName, gameObject);

            Type type = WindowComponent.Instance.GetWindowCom(prefabName);
            
            ui.AddComponent(type);

            Game.Scene.GetComponent<UIComponent>().AddUI(ui);

            return ui;
        }
    }
}
