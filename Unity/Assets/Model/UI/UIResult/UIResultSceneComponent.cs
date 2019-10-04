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
        private GameObject drawscene2;

        private int triggerId = 3005;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.drawscene2 = rc.Get<GameObject>("drawscene2");

            Game.EventSystem.Run<int>(EventIdType.CompleteTask, this.triggerId);

            this.CollectAndShow();
           
        }

        async ETVoid CollectAndShow()
        {
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

            Game.EventSystem.Run(EventIdType.ShowJoystic);

            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIResultScene);
        }

        public override void Dispose()
        {
            base.Dispose();

            Unit player = Game.Scene.GetComponent<UnitComponent>().MyUnit;


            if (player != null && player.GetComponent<UnitCameraFollowComponent>() == null)
            {
                player.AddComponent<UnitCameraFollowComponent>();
            }
        }
    }

}