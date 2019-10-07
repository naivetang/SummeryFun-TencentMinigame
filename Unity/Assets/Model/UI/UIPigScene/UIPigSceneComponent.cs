using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using ILRuntime.Runtime;
using Spine;
using Spine.Unity;
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


    [ObjectSystem]
    public class UIPigUpdateSystem : UpdateSystem<UIPigSceneComponent>
    {


        public override void Update(UIPigSceneComponent self)
        {
            self.Update();
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
        
        private GameObject pigAnimator;

        private AudioSource audioSuccess;

        private AudioSource audioWalk;

        private GameObject tishiDialog;

        private bool hadShow;

        private CancellationTokenSource dialogCancelSource;


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

            this.pigAnimator = rc.Get<GameObject>("pigAnimator");

            this.audioSuccess = rc.Get<GameObject>("hine").GetComponent<AudioSource>();

            this.audioWalk = rc.Get<GameObject>("Audiowalk").GetComponent<AudioSource>();

            this.tishiDialog = rc.Get<GameObject>("tishiDialog");

            

            this.tishiDialog.SetActive(false);

            this.dialogCancelSource = new CancellationTokenSource();

            Wait5sToShowPromptDialog(this.dialogCancelSource.Token).Coroutine();

            this.init();

            UIPointHandler pointHandler = this.pigAnimator.GetComponent<UIPointHandler>();


            pointHandler.RegisterPointDown(this.PointDown);
            pointHandler.RegisterPointUp(this.PointUp);


            //this.CollectAndShow();

        }

        

        void PointDown(PointerEventData data)
        {

            if (!this.tishiDialog.activeSelf)
            {
                this.ShowTishi();
            }
        }

        void PointUp(PointerEventData data)
        {

        }

        /// <summary>dia
        /// 等5s后自动弹提示
        /// </summary>
        /// <returns></returns>
        async ETVoid Wait5sToShowPromptDialog(CancellationToken token)
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            await timer.WaitAsync(3 * 1000, token);

            if (this.IsDisposed || this.tishiDialog.activeSelf || this.hadShow)
                return;


            this.ShowTishi();

            //this.Wait3sToHide().Coroutine();
        }

        void ShowTishi()
        {
            this.hadShow = true;

            this.tishiDialog.SetActive(true);
        }

        async ETVoid Wait3sToHide()
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            await timer.WaitAsync(3 * 1000);

            if (this.IsDisposed)
                return;

            if (this.tishiDialog != null)
            {
                this.tishiDialog.SetActive(false);
            }
        }

        public void Update()
        {
            if (!this.hadShow)
            {
                CheckClick();
            }
        }


        public void CheckClick()
        {
            if (Input.GetMouseButtonDown(0) && this.dialogCancelSource != null)
            {
                this.dialogCancelSource.Cancel();

                this.dialogCancelSource.Dispose();

                this.dialogCancelSource = null;
            }
        }


         void init()
        {
            this.child2.SetActive(false);

            this.cancel.GetComponent<Button>().onClick.AddListener(this.Close);

            RegistColliderTrriger();

            RegistDrageEvent();
            
            this.AddListener();
            
        }


        private EventProxy pigIntoProxy;
        
        void AddListener()
        {
            
            this.pigIntoProxy = new EventProxy(this.PigIntoStyFinish_Close);
            
            Game.EventSystem.RegisterEvent(EventIdType.PigIntoStyFinish, this.pigIntoProxy);
        }

        void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.PigIntoStyFinish, this.pigIntoProxy);
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
                this.audioSuccess.Play();

                this.success();

                this.audioWalk.Play();
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

            this.child2.GetComponent<CanvasGroup>().DOFade(1f, 0);

            this.hine.GetComponent<CanvasGroup>().DOFade(1f, 0);

            this.youzi.SetActive(false);

            this.cancel.SetActive(false);

            Game.EventSystem.Run<int>(EventIdType.CompleteTask, this.triggerId);

            pigAnimator.GetComponent<PigIntoSty>().enabled = true;
        }


        void PigIntoStyFinish_Close(List<object> list)
        {
            PigIntoStyFinish_Close().Coroutine();
        }

        async ETVoid PigIntoStyFinish_Close()
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            this.audioWalk.Stop();

            await timer.WaitAsync((long)(2 * 1000));

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
            
            this.RemoveListener();

            Unit player = Game.Scene.GetComponent<UnitComponent>().MyUnit;


            if (player != null && player.GetComponent<UnitCameraFollowComponent>() == null)
            {
                player.AddComponent<UnitCameraFollowComponent>();
            }
        }
    }

}