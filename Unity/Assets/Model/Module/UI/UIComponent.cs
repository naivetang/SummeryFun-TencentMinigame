using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ETModel
{
	[ObjectSystem]
	public class UIComponentAwakeSystem : AwakeSystem<UIComponent>
	{
		public override void Awake(UIComponent self)
		{
			//self.Camera = Component.Global.transform.Find("UICamera").gameObject;
            
            self.Awake();
		}
	}


    /// <summary>
    /// 管理所有UI
    /// </summary>
    public class UIComponent: Component
    {
        public Camera Camera
        {
            get
            {
                return this.uiManager.Camera;
            }
        }


        public Dictionary<string, UI> uis = new Dictionary<string, UI>();

        private readonly string UIRoot = "UIRoot";

        private UIManager uiManager;
        

        public void Awake()
        {
            this.LoadUIManager();
        }

        private void LoadUIManager()
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle(this.UIRoot.StringToAB());
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(this.UIRoot.StringToAB(), this.UIRoot);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);

            UIManager ui = ComponentFactory.Create<UIManager, GameObject>(gameObject);

            ui.Parent = this;
            
            this.uiManager = ui ;
        }

        public void AddUI(UIBase ui)
        {
            this.uiManager.Add(ui);
        }
        
        public UIBase GetUI(string name)
        {
            return this.uiManager.Get(name);
        }
        
        public void RemoveUI(string name)
        {
            this.uiManager.Remove(name);
        }




        public void Add(UI ui)
		{
			ui.GameObject.GetComponent<Canvas>().worldCamera = this.Camera.GetComponent<Camera>();
			
			this.uis.Add(ui.Name, ui);
			ui.Parent = this;
		}

		public void Remove(string name)
		{
			if (!this.uis.TryGetValue(name, out UI ui))
			{
				return;
			}
			this.uis.Remove(name);
			ui.Dispose();
		}

		public UI Get(string name)
		{
			UI ui = null;
			this.uis.TryGetValue(name, out ui);
			return ui;
		}
	}
}