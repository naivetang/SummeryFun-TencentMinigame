using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UIResultAwakeSystem : AwakeSystem<UIResultSceneComponent>
    {
        public override void Awake(UIResultSceneComponent self)
        {
            self.Awake();
        }
    }

    public class UIResultSceneComponent : Component
    {
        private GameObject scene1;

        private GameObject scene2;

        private GameObject scene3;

        private int triggerId = 3100;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.scene1 = rc.Get<GameObject>("Scene1");

            this.scene2 = rc.Get<GameObject>("Scene2");

            this.scene3 = rc.Get<GameObject>("Scene3");

            Game.EventSystem.Run<int>(EventIdType.CompleteTask, this.triggerId);

            this.EndtheGame().Coroutine();
           
        }

        async ETVoid EndtheGame()
        {

            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();


            await timerComponent.WaitAsync((long)(1 * 1000));


            //场景1
            this.scene1.GetComponent<CanvasGroup>().DOFade(1f, 1f);
                       

            await timerComponent.WaitAsync((long)(3 * 1000));

            this.scene1.GetComponent<CanvasGroup>().DOFade(0.2f, 1f);

            await timerComponent.WaitAsync((long)(1 * 1000));

            //this.scene1.SetActive(false);


            //场景2


            this.scene2.GetComponent<CanvasGroup>().DOFade(1f, 1f);

            await timerComponent.WaitAsync((long)(3 * 1000));

            this.scene2.GetComponent<CanvasGroup>().DOFade(0f, 1f);

            await timerComponent.WaitAsync((long)(1.5 * 1000));

            //this.scene2.SetActive(false);


            //场景3


            this.scene3.GetComponent<CanvasGroup>().DOFade(1f, 1f);

            await timerComponent.WaitAsync((long)(2 * 1000));

            this.scene3.GetComponent<CanvasGroup>().DOFade(0f, 2f);

            this.scene1.GetComponent<CanvasGroup>().DOFade(0f, 2f);

            



            await timerComponent.WaitAsync((long)(2 * 1000));

            

            // 回到主界面
            Game.EventSystem.Run(EventIdType.FinalResult);
            
            this.CloseAll().Coroutine();

            //this.scene3.SetActive(false);
        }
        

        async ETVoid CloseAll()
        {
            //UIFactory.Create<UIStartComponent>(ViewLayer.UIPopupLayer, UIType.UIStart).Coroutine();

            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIResultScene);

            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIMap);
           

            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIMain);

            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIResultScene);

            await ETTask.CompletedTask;

            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
            
            UIStartComponent.UpdateTask(null,0f,0f);


            await timerComponent.WaitAsync((long)(1 * 1000));

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE_WIN
            Application.Quit();
#endif

        }
        //public override void Dispose()
        //{
        //    base.Dispose();

        //    Unit player = Game.Scene.GetComponent<UnitComponent>().MyUnit;


        //    if (player != null && player.GetComponent<UnitCameraFollowComponent>() == null)
        //    {
        //        player.AddComponent<UnitCameraFollowComponent>();
        //    }
        //}
    }

}