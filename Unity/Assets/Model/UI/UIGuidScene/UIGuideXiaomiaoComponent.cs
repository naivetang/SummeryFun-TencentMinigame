using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    [ObjectSystem]
    public class UIGuideXiaomiaoComponentAwakeSystem : AwakeSystem<UIGuideXiaomiaoComponent,GameObject>
    {

        public override void Awake(UIGuideXiaomiaoComponent self, GameObject a)
        {
            self.Awake(a);
        }
    }

    
    [ObjectSystem]
    public class UIGuideXiaomiaoComponentUpdateSystem : UpdateSystem<UIGuideXiaomiaoComponent>
    {
        public override void Update(UIGuideXiaomiaoComponent self)
        {
            self.Update();
        }
    }
    
    [HideInHierarchy]
    public class UIGuideXiaomiaoComponent : Component
    {
        private GameObject xiaomiao;


        private GameObject xiaomiaoAimation;

        private GameObject xiaomiaoClick;
        
        private GameObject tishiDialog;

        private UIGuideXiaomiaoBind bind;

        private CancellationTokenSource cancellationTokenSource;

        private bool hadShow;
        
        public void Awake(GameObject go)
        {
            this.xiaomiao = go;

            hadShow = false;

            bind = this.xiaomiao.GetComponent<UIGuideXiaomiaoBind>();
            
            this.cancellationTokenSource = new CancellationTokenSource();
            
            this.DialogWaitbindTimeToShow(this.cancellationTokenSource.Token).Coroutine();
            
            //this.CheckClick().Coroutine();

            ReferenceCollector rc = this.xiaomiao.GetComponent<ReferenceCollector>();

            this.xiaomiaoAimation = rc.Get<GameObject>("xiaomiaoAniamtion");
            this.xiaomiaoClick = rc.Get<GameObject>("xiaomiaoClick");
            this.tishiDialog = rc.Get<GameObject>("tishiDialog");
            
            
            this.xiaomiaoClick.SetActive(false);
            
            this.tishiDialog.SetActive(false);

            UIPointHandler pointHandler = this.xiaomiao.GetComponent<UIPointHandler>();
            
            pointHandler.RegisterPointDown(this.PointDown);
            
            pointHandler.RegisterPointUp(this.PointUp);
        }


        void PointDown(PointerEventData data)
        {
            this.xiaomiaoClick.SetActive(true);
            
            this.xiaomiaoAimation.SetActive(false);

            if (!this.tishiDialog.activeSelf)
            {
                this.ShowTishi();
                
                this.DialogWait3SToHide().Coroutine();
            }
        }
        
        

        async ETVoid DialogWait3SToHide()
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            await timer.WaitAsync(3 * 1000);

            if (this.IsDisposed)
                return;
            
            this.tishiDialog.SetActive(false);
        }

        void ShowTishi()
        {
            this.hadShow = true;
            
            this.tishiDialog.SetActive(true);
        }

        /// <summary>
        /// 进入引导关n秒之后，自动弹提示
        /// </summary>
        /// <returns></returns>
        async ETVoid DialogWaitbindTimeToShow(CancellationToken token)
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            await timer.WaitAsync((long) (this.bind.waitTimeToPrompt * 1000), token);

            if (this.IsDisposed)
                return;

            if (!this.tishiDialog.activeSelf && !this.hadShow) 
            {
                this.ShowTishi();
                
                this.DialogWait3SToHide().Coroutine();
            }
        }

        async ETVoid CheckClick()
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            await timer.WaitAsync((long) 1f * 1000);
            
            while (true)
            {
                await timer.WaitAsync((long) (0.1f * 1000));
                
                if (this.IsDisposed)
                    return;
                
                //this.Update();
                
            }

           
        }
        
        void PointUp(PointerEventData data)
        {
            this.xiaomiaoClick.SetActive(false);
            
            this.xiaomiaoAimation.SetActive(true);
        }

        public void Update()
        {
            if (this.hadShow)
                return;
            
            #if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0) && this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                
                this.cancellationTokenSource.Dispose();

                this.cancellationTokenSource = null;
            }
            
            #else
            if (Input.touchCount > 0 && this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                
                this.cancellationTokenSource.Dispose();

                this.cancellationTokenSource = null;
            }
            
            #endif
            
        }

        public override void Dispose()
        {
            base.Dispose();

            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Dispose();

                this.cancellationTokenSource = null;
            }
        }
    }
}