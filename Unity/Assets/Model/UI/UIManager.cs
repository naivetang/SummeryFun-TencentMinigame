using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class UIManagerAwakeSystem : AwakeSystem<UIManager,GameObject>
    {
        
        public override void Awake(UIManager self, GameObject a)
        {
            self.Awake(a);
        }
    }

    public enum ViewLayer : int
    {
        None = 0,
        UIBgLayer,
        UIMainLayer,
        UIFullScreenLayer,
        UIPopupLayer,
        UIFixedLayer,
        UITipsLayer,
        UIGuideLayer,
        UIAlertLayer,
        UIEffectLayer,
        UIToppestLayer,
        Max,
    }
    [HideInHierarchy]
    public  class UIManager : Component
    {
        public Camera uiCamera;


        private UIBase[] _viewLayer = new UIBase[(int)ViewLayer.Max];

        public Dictionary<string, UIBase> uis = new Dictionary<string, UIBase>();

        public void Awake(GameObject go )
        {
            this.GameObject = go;

            this.uiCamera = this.GameObject.transform.Find("UICamera").GetComponent<Camera>();
            
            this.SetCameraSize();

            this.InitViewLayer();
        }

        void SetCameraSize()
        {
            
            Log.Warning($"屏幕宽度：{Screen.width} , 高度：{Screen.height}"); 
            
            this.uiCamera.orthographicSize = 346.8f + (Screen.height * 1.0f / Screen.width - 1920f/1080f)* ((387.2f - 346.8f) /(2160f/1080f - 1920f/1080f));
        }

        private void InitViewLayer()
        {
            for (int i = 1; i < (int)ViewLayer.Max; i++)
            {
                this._viewLayer[i] = this.FindLayer(i);
            }
            
        }

        private UIBase FindLayer(int i)
        {
            ViewLayer layer = (ViewLayer)i;

            var t = this.GameObject.transform.Find(layer.ToString());

            if (t != null)
            {
                UIBase uibase = ComponentFactory.Create<UIBase, ViewLayer, string, GameObject>(layer, layer.ToString(), t.gameObject);

                uibase.Parent = this;


                return uibase;
            }
            return null;
        }

        public void Add(UIBase ui)
        {
            int layer = (int)ui.layer;
            
            // ui.GameObject.transform.SetParent(this._viewLayer[layer]);;

            ui.Parent = this._viewLayer[layer];
            
            this.uis.Add(ui.Name, ui);
        }

        public UIBase Get(string name)
        {
            UIBase ui = null;
            
            this.uis.TryGetValue(name, out ui);
            
            return ui;
        }

        public void Remove(string name)
        {
            if (!this.uis.TryGetValue(name, out UIBase ui))
            {
                return;
            }
            
            this.uis.Remove(name);
            
            ui.Dispose();
        }
    }
}
