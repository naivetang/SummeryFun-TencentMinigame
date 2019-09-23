using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{

    [ObjectSystem]
    public class UIBaseSystem : AwakeSystem<UIBase,ViewLayer,string,GameObject>
    {
        public override void Awake(UIBase self, ViewLayer a, string b, GameObject c)
        {
            self.Awake(a, b, c);
        }
    }

    [HideInHierarchy]
    public class UIBase : Entity
    {
        public ViewLayer layer;

        public string Name { get; private set; }

        public Dictionary<string, UIBase> children = new Dictionary<string, UIBase>();


        public void Awake(ViewLayer layer,string name, GameObject gameObject)
        {
            this.children.Clear();

            gameObject.AddComponent<ComponentView>().Component = this;
            
            gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);

            this.layer = layer;
            
            this.Name = name;
            
            this.GameObject = gameObject;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (UIBase ui in this.children.Values)
            {
                ui.Dispose();
            }

            UnityEngine.Object.Destroy(GameObject);
            
            children.Clear();
        }

        public void SetAsFirstSibling()
        {
            this.GameObject.transform.SetAsFirstSibling();
        }

        public void Add(UIBase ui)
        {
            this.children.Add(ui.Name, ui);
            ui.Parent = this;
        }

        public void Remove(string name)
        {
            UIBase ui;
            if (!this.children.TryGetValue(name, out ui))
            {
                return;
            }
            this.children.Remove(name);
            ui.Dispose();
        }

        public UIBase Get(string name)
        {
            UIBase child;
            if (this.children.TryGetValue(name, out child))
            {
                return child;
            }
            GameObject childGameObject = this.GameObject.transform.Find(name)?.gameObject;
            if (childGameObject == null)
            {
                return null;
            }

            child = ComponentFactory.Create<UIBase, ViewLayer, string, GameObject>(this.layer, name, childGameObject);
            
            this.Add(child);
            
            return child;
        }
    }
}
