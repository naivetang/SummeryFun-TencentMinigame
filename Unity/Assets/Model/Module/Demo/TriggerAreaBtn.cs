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
        public GameObject TriggerArea;
        
        public TriggerAreaConfig config { get; set; }


        /// <summary>
        /// 0是未成功，1是成功
        /// </summary>
        private UIMultImage multImage;
        
        private bool hasFinish;

        public void Awake(GameObject gameObject, TriggerAreaConfig conf)
        {
            this.GameObject = gameObject;

            this.config = conf;

            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();

            this.multImage= rc.Get<GameObject>("image").GetComponent<UIMultImage>();
            
            this.AddListener();
        }


        private EventProxy completeTask;
        
        void AddListener()
        {
            Button btn = this.GameObject.GetComponent<UINextButton>();

            if (btn == null)
            {
                Log.Error("btn is null");

                return;
            }
            
            btn.onClick.AddListener(this.OnBtnClick);
            

            this.completeTask = new EventProxy(this.CompleteTask);
            
            Game.EventSystem.RegisterEvent(EventIdType.CompleteTask, this.completeTask);
        }

        void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.CompleteTask, this.completeTask);
        }

        private void CompleteTask(List<object> obj)
        {
            int triggerId = (int)obj[0];
            if (this.config.Id.Equals(triggerId))
            {
                this.hasFinish = true;
                
                this.multImage.SetSprite(1);
                
                if(this.config.BookIndex >= 0)
                    UIBookComponent.hadOpenPage[this.config.BookIndex] = true;
                
                this.UpdateTask();
            }
            //this.Hide();
        }

        void UpdateTask()
        {
            List<int> task = new List<int>();
            for (int i = 0; i < UIBookComponent.hadOpenPage.Count; i++)
            {
                if (UIBookComponent.hadOpenPage[i])
                {
                    task.Add(i);
                }
            }

            GameObject p = Game.Scene.GetComponent<UnitComponent>().MyUnit.GameObject;
            
            UIStartComponent.UpdateTask(task,p.transform.position.x,p.transform.position.y);
        }

        void OnBtnClick()
        {

            if (this.hasFinish)
            {
                Game.EventSystem.Run(EventIdType.OpenBook, this.config.BookIndex);
            }
            
            else if (this.config.ShowWindow != null)
            {
                if (this.config.ShowWindow.Equals("UIShaddockScene"))
                {
                    //Log.Error("移出");
                    //Game.Scene.GetComponent<UnitComponent>().MyUnit.RemoveComponent<UnitCameraFollowComponent>();
                }

                if (Game.Scene.GetComponent<UnitComponent>().MyUnit != null)
                    Game.Scene.GetComponent<UnitComponent>().MyUnit.RemoveComponent<UnitCameraFollowComponent>();


                UIFactory.CreateByTrigger(ViewLayer.UIFullScreenLayer, this.config.ShowWindow).Coroutine();
            }
        }

        public void Show()
        {
            this.GameObject.SetActive(true);
            
            // 猪关
            if (this.config.Id == 3003 && !hasFinish)
            {
                
                Button btn = this.GameObject.GetComponent<UINextButton>();
                
                if (!CanFllowPigComponent.isFllowed)
                {
                    this.multImage.SetSprite(2);


                    btn.onClick.RemoveAllListeners();
                }
                else
                {
                    this.multImage.SetSprite(0);


                    btn.onClick.RemoveAllListeners();
                    
                    btn.onClick.AddListener(this.OnBtnClick);
                }

                return;
            }

            this.multImage.SetSprite(this.hasFinish ? 1 : 0);

            
        }

        public void Hide()
        {
            this.GameObject.SetActive(false);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
            
            this.RemoveListener();
        }
    }
}
