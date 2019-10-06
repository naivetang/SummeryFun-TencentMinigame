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
    public class UIPigAwakeSystem : AwakeSystem<UIPigSceneComponent>
    {
        public override void Awake(UIPigSceneComponent self)
        {
            self.Awake();
        }
    }

    public class UIPigSceneComponent : Component
    {
        private GameObject drawscene2;

        private GameObject child1;

        private GameObject child2;

        private GameObject youzi;

        private GameObject hine;

        private GameObject stayHine;

        private Vector2 inityouziPosition;

        private GameObject cancel;

        private int triggerId = 3003;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.drawscene2 = rc.Get<GameObject>("drawscene2");

            this.hine = rc.Get<GameObject>("hine");

            this.child1 = rc.Get<GameObject>("child1");

            this.child2 = rc.Get<GameObject>("child2");

            this.youzi = rc.Get<GameObject>("youzi");

            this.inityouziPosition = this.youzi.transform.localPosition;

            this.cancel = rc.Get<GameObject>("Cancel");

            this.init();
            

            

            //this.CollectAndShow();
           
        }

        void init()
        {
            this.child2.SetActive(false);

            this.cancel.GetComponent<Button>().onClick.AddListener(this.Close);

            RegistColliderTrriger();

            RegistDrageEvent();
            
        }

        void AddListener()
        {

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
        
        void RegistDrageEvent()
        {
            var darageable = this.youzi.GetComponent<UIDragable>();


            darageable.RegistOnBeginDrag(this.BeginDrageyouzi);
            darageable.RegistOnDrag(this.Drageingyouzi);
            darageable.RegistOnEndDrag(this.EndDrageyouzi);

            darageable.RegisterPointDown(this.DragPointDown);
            darageable.RegisterPointUp(this.DragPointUp);
        }

        void DragPointDown(PointerEventData p)
        {
            //Log.Info("down");

            this.youzi.transform.DOScale(Vector3.one * 1.1f, 0.15f);
        }

        void DragPointUp(PointerEventData p)
        {
            //Log.Info("up");

            this.youzi.transform.DOScale(Vector3.one, 0.15f);
           
        }

        void BeginDrageyouzi(PointerEventData p)
        {
            //Log.Info("begin drage");

            this.youzi.GetComponent<Image>().CrossFadeAlpha(1f, 0.1f, true);
                        
        }

        void Drageingyouzi(PointerEventData p)
        {
            this.child1.SetActive(false);

            this.child2.GetComponent<CanvasGroup>().DOFade(0.4f, 0);

            this.child2.SetActive(true);
                        
            //Log.Info("draging");
        }

        void EndDrageyouzi(PointerEventData p)
        {
            //Log.Info("end draging");

            // 解题成功
            if (this.stayHine != null)
            {
                this.success();
            }
            else
            {
                this.fail();
            }

        }

        //注册柚子进入Hine状态
        void RegistColliderTrriger()
        {
            this.hine.GetComponent<UIColliderTrigger>().RegistOnTriggerEnter2D((p) =>
            {
                HineTriggerEnter2D(this.hine, p);
            });

             this.hine.GetComponent<UIColliderTrigger>().RegistOnTriggerExit2D((p) =>
             {
                 HineTriggerExit2D(this.hine, p);
             });

             this.hine.GetComponent<UIColliderTrigger>().RegistOnTriggerStay2D((p) =>
             {
                 HineTriggerStay2D(this.hine, p);
             });
            
        }


        void HineTriggerEnter2D(GameObject gameobject, Collider2D collider2D)
        {
            if (!collider2D.gameObject.tag.Equals("youzi"))
            {
                return;
            }
            
            
            gameobject.GetComponent<CanvasGroup>().DOFade(0.4f, 0);
                       
            this.stayHine = gameobject;
        }

        void HineTriggerExit2D(GameObject gameobject, Collider2D collider2D)
        {
            if (!collider2D.gameObject.tag.Equals("youzi"))
            {
                return;
            }

            gameobject.GetComponent<CanvasGroup>().DOFade(0f, 0);

            this.stayHine = null;
        }


        void HineTriggerStay2D(GameObject hine, Collider2D collider2D)
        {

        }

        async ETVoid success()
        {
            this.child1.SetActive(false);

            this.child2.SetActive(true);

            Game.EventSystem.Run<int>(EventIdType.CompleteTask, this.triggerId);

            this.CollectAndShow().Coroutine();

            await ETTask.CompletedTask;
        }

        void fail()
        {
            this.child1.SetActive(true);

            this.child2.GetComponent<CanvasGroup>().DOFade(1, 0);

            this.child2.SetActive(false);

            this.youzi.transform.localPosition = this.inityouziPosition;

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

            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIPigScene);
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