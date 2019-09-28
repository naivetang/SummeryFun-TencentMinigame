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
    public class UIShaddockAwakeSystem : AwakeSystem<UIShaddockSceneComponent>
    {
        public override void Awake(UIShaddockSceneComponent self)
        {
            self.Awake();
        }
    }

    public class UIShaddockSceneComponent : Component
    {
        private GameObject wall;

        private GameObject tree;

        private GameObject stick;

        private GameObject bevy;

        private GameObject whiteBG;

        private GameObject drawscene2;

        private GameObject context;

        /// <summary>
        /// 小苗被点击后显示这张图
        /// </summary>


        // 进入的对应的区域事件
        private int triggerId = 3002;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.drawscene2 = rc.Get<GameObject>("drawscene2");

            this.wall = rc.Get<GameObject>("Wall");

            this.tree = rc.Get<GameObject>("Tree");

            this.stick = rc.Get<GameObject>("Stick");

            this.bevy = rc.Get<GameObject>("Bevy");

            this.whiteBG = rc.Get<GameObject>("WhiteBG");

            this.drawscene2.SetActive(false);

            this.Init();
        }

        async ETVoid Init()
        {
            Game.EventSystem.Run(EventIdType.ShowJoystic);

            Game.EventSystem.Run<int>(EventIdType.CompleteTask, this.triggerId);

            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            Log.Info(" 出现结束图画 ");

            // 出画
            this.drawscene2.SetActive(true);

            this.drawscene2.GetComponent<CanvasGroup>().alpha = 0;

            this.drawscene2.GetComponent<CanvasGroup>().DOFade(1, 1);

            await timerComponent.WaitAsync(1 * 1000);

            // 图画完全显示出来

            this.CloseOtherDrawScene();

            await timerComponent.WaitAsync(1 * 1000);

            // 装进书里面

            this.CollectToBook().Coroutine();
        }

        void CloseOtherDrawScene()
        {
            this.wall.SetActive(false);

            this.tree.SetActive(false);

            this.stick.SetActive(false);

            this.bevy.SetActive(false);

            this.whiteBG.SetActive(false);

            //this.
        }

        async ETVoid CollectToBook()
        {
            UIBase com = UIFactory.Create<UIBookComponent>(ViewLayer.UIPopupLayer, UIType.UIBook).Result;

            com.GetComponent<UIBookComponent>().AddImageGo(this.drawscene2);


            this.Close();

        }

        void Close()
        {
            this.drawscene2 = null;

            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIGuideScene);
        }
    }
}

        


