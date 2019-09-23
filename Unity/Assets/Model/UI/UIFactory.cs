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
            
            resourcesComponent.LoadBundle(prefabName.StringToAB());
            
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(prefabName.StringToAB(), prefabName);
            
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

            UIBase ui = ComponentFactory.Create<UIBase, ViewLayer, string, GameObject>(layer, prefabName, gameObject);

            ui.AddComponent(typeof (T));

            Game.Scene.GetComponent<UIComponent>().AddUI(ui);

            return ui;
        }
    }
}
