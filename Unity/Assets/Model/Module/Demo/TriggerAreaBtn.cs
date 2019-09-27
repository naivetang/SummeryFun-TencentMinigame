using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{

    [ObjectSystem]
    public class TriggerAreaBtnAwakeSystem : AwakeSystem<TriggerAreaBtn, GameObject, TriggerAreaConfig>
    {
        public override void Awake(TriggerAreaBtn self, GameObject gameObject, TriggerAreaConfig conf)
        {
            self.Awake(gameObject,conf);
        }
    }

    [HideInHierarchy]
    public sealed class TriggerAreaBtn : Entity
    {
        public TriggerAreaConfig config { get; set; }

        public void Awake(GameObject gameObject, TriggerAreaConfig conf)
        {
            this.GameObject = gameObject;

            this.config = conf;
            
            this.AddListener();
        }

        void AddListener()
        {
            Button btn = this.GameObject.GetComponent<UINextButton>();

            if (btn == null)
            {
                Log.Error("btn is null");
            }
            
            if (this.config.ShowWindow != null)
            {
                btn.onClick.AddListener(() =>
                {
                    UIFactory.CreateByTrigger(ViewLayer.UIFullScreenLayer, this.config.ShowWindow).Coroutine();
                });
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}
