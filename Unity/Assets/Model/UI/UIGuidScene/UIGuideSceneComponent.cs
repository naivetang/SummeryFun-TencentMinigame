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
    public class UIGuideAwakeSystem: AwakeSystem<UIGuideSceneComponent>
    {
        public override void Awake(UIGuideSceneComponent self)
        {
            self.Awake();
        }
    }

    public class UIGuideSceneComponent : Component
    {
        private GameObject hine1;
        private GameObject hine2;
        
        private GameObject zhuzi;

        private GameObject ZhuziAnimation;

        private GameObject stayHine;


        private Vector3 zhuziInitPos;

        private GameObject successShuiDiAnimation;

        private GameObject failShuiDiAnimation;

        private GameObject rightDropWater;

        private GameObject leftDropWater;

        private GameObject guideEndAnimation;

        private GameObject xiaomiaoAniamtion;

        private GameObject gamesence3;
        
        
        private GameObject drawsence2;

        private GameObject context;

        /// <summary>
        /// 小苗被点击后显示这张图
        /// </summary>
        private GameObject xiaomiaoClick;

        private GameObject xiaomiao;

        private GameObject cancel;

        private GameObject uidialog2;

        private UIGuideXiaomiaoComponent xiaomiaoComponent;

        // 进入的对应的区域事件
        private int triggerId = 3001;
        
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.hine1 = rc.Get<GameObject>("hine1");
            
            this.hine1.GetComponent<UIMultImage>().enabled = false;
            
            this.hine2 = rc.Get<GameObject>("hine2");

            this.hine2.GetComponent<UIMultImage>().enabled = false;
            
            this.zhuzi = rc.Get<GameObject>("DragZhuzi");

            this.ZhuziAnimation = rc.Get<GameObject>("ZhuziAnimation");

            this.uidialog2 = rc.Get<GameObject>("UIDialog2");
            
            this.uidialog2.SetActive(false);

            this.successShuiDiAnimation = rc.Get<GameObject>("SuccessShuidiAnimation");

            this.failShuiDiAnimation = rc.Get<GameObject>("FailShuidiAnimation");

            this.successShuiDiAnimation.SetActive(false);

            this.failShuiDiAnimation.SetActive(false);

            this.ZhuziAnimation.SetActive(false);

            this.zhuziInitPos = this.zhuzi.transform.position;

            this.rightDropWater = rc.Get<GameObject>("WaterDropRight");

            this.leftDropWater = rc.Get<GameObject>("WaterDropLeft");

            this.guideEndAnimation = rc.Get<GameObject>("GuideEndAnimation");
            
            this.guideEndAnimation.SetActive(false);

            this.xiaomiaoAniamtion = rc.Get<GameObject>("xiaomiaoAniamtion");

            this.gamesence3 = rc.Get<GameObject>("gamesence3");
            
            this.gamesence3.SetActive(false);
            
            this.drawsence2 = rc.Get<GameObject>("drawsence2");
            
            this.drawsence2.SetActive(false);

            this.context = rc.Get<GameObject>("Context");
            
            this.xiaomiao = rc.Get<GameObject>("xiaomiao");
            
            this.cancel = rc.Get<GameObject>("Cancel");

            this.xiaomiaoComponent = ComponentFactory.Create<UIGuideXiaomiaoComponent,GameObject>(this.xiaomiao);
            
            this.Init();
        }

        void Init()
        {
            
            this.cancel.GetComponent<Button>().onClick.AddListener(this.Close);
            
            this.RegistColliderTrriger();
            
            this.RegistDrageEvent();
        }

        /// <summary>
        /// 注册竹子进入退出事件
        /// </summary>
        void RegistColliderTrriger()
        {
            {
                this.hine1.GetComponent<UIColliderTrigger>().RegistOnTriggerEnter2D((p) =>
                {
                    HineTriggerEnter2D(this.hine1, p);

                    Log.Info("手机震动");

                    Handheld.Vibrate();
                });

                this.hine1.GetComponent<UIColliderTrigger>().RegistOnTriggerExit2D((p) =>
                {
                    HineTriggerExit2D(this.hine1, p);
                });

                this.hine1.GetComponent<UIColliderTrigger>().RegistOnTriggerStay2D((p) =>
                {
                    HineTriggerStaty2D(this.hine1, p);
                });

            }

            {

                this.hine2.GetComponent<UIColliderTrigger>().RegistOnTriggerEnter2D((p) =>
                {
                    HineTriggerEnter2D(this.hine2, p);

                    Log.Info("手机震动");

                    Handheld.Vibrate();
                });

                this.hine2.GetComponent<UIColliderTrigger>().RegistOnTriggerExit2D((p) =>
                {
                    HineTriggerExit2D(this.hine2, p);
                });

                this.hine2.GetComponent<UIColliderTrigger>().RegistOnTriggerStay2D((p) =>
                {
                    HineTriggerStaty2D(this.hine2, p);
                    
                });
            }
        }

        void HineTriggerEnter2D(GameObject hine, Collider2D collider2D)
        {
            Log.Info( "进入竹子区域：" + hine.name);

            UIMultImage multImage = hine.GetComponent<UIMultImage>();

            multImage.enabled = true;
            
            multImage.SetSprite(0);
            
            multImage.CrossFadeAlpha(0.5f, 0f, true);

            this.stayHine = hine;

            //this.zhuzi.transform.DOScale(Vector3.one * 1.2f, 0.15f);
        }

        void HineTriggerExit2D(GameObject hine, Collider2D collider2D)
        {
            Log.Info("离开竹子区域：" + hine.name);

            UIMultImage multImage = hine.GetComponent<UIMultImage>();

            multImage.enabled = false;

            this.stayHine = null;

            //this.zhuzi.transform.DOScale(Vector3.one, 0.15f);
        }


        void HineTriggerStaty2D(GameObject hine, Collider2D collider2D)
        {
            //Log.Info("staty");
        }

        /// <summary>
        /// 注册拖动竹子事件
        /// </summary>
        void RegistDrageEvent()
        {
            var darageable = this.zhuzi.GetComponent<UIDragable>(); 
            
            
            darageable.RegistOnBeginDrag(this.BeginDrageZhuzi);
            darageable.RegistOnDrag(this.DrageingZhuzi);
            darageable.RegistOnEndDrag(this.EndDrageZhuzi);
            
            darageable.RegisterPointDown(this.DragPointDown);
            darageable.RegisterPointUp(this.DragPointUp);
        }

        void DragPointDown(PointerEventData p)
        {
            //Log.Info("down");

            this.zhuzi.transform.DOScale(Vector3.one * 1.1f, 0.15f);

            Log.Info("手机震动0.5秒");

            Handheld.Vibrate();
            
        }
        void reset()
        {
            this.leftDropWater.SetActive(true);

            this.rightDropWater.SetActive(true);

            this.failShuiDiAnimation.SetActive(false);

            this.successShuiDiAnimation.SetActive(false);
        }




        void DragPointUp(PointerEventData p)
        {
            //Log.Info("up");

            this.zhuzi.transform.DOScale(Vector3.one, 0.15f);

            this.reset();
        }

        void BeginDrageZhuzi(PointerEventData p)
        {
            //Log.Info("begin drage");

            this.zhuzi.GetComponent<Image>().CrossFadeAlpha(1f, 0.1f, true);
            
            this.uidialog2.SetActive(true);
            
            //this.uidialog2.GetComponent<DialogTextCtl>().CloseDialog(1);
        }

        void DrageingZhuzi(PointerEventData p)
        {
            //Log.Info("draging");
        }

        void EndDrageZhuzi(PointerEventData p)
        {
            this.uidialog2.SetActive(false);
            
            //Log.Info("end draging");

            // 解题成功
            if (this.stayHine != null && this.stayHine == this.hine2)
            {
                this.SolveSucces();
            }
            else if (this.stayHine != null && this.stayHine == this.hine1)
            {
                this.LeftFailed();
            }// 解题失败，竹子掉下来
            else{
                this.SolveFaild();
            }
        }

        void SolveSucces()
        {
            Log.Info("解题成功");

            this.cancel.SetActive(false);

            Game.EventSystem.Run(EventIdType.ShowJoystic);
            
            // 发送事件，完成任务
            Game.EventSystem.Run<int>(EventIdType.CompleteTask,this.triggerId);
            
            {
                var multImage = this.hine2.GetComponent<UIMultImage>();

                multImage.SetSprite(1);

                multImage.CrossFadeAlpha(1, 0.1f, true);
            }

            {
                this.zhuzi.GetComponent<Image>().CrossFadeAlpha(0f, 0.1f, true);

                this.zhuzi.GetComponent<Image>().raycastTarget = false;
            }

            this.AddListener();
            
            
            //this.successShuiDiAnimation.SetActive(true);
            
            
        }

        void LeftFailed()
        {
            {
                var multImage = this.hine1.GetComponent<UIMultImage>();

                multImage.SetSprite(1);

                multImage.CrossFadeAlpha(1, 0.1f, true);
            }

            {
                this.zhuzi.GetComponent<Image>().CrossFadeAlpha(0f, 0.1f, true);

                this.zhuzi.transform.position = this.hine1.transform.position;

                //this.zhuzi.GetComponent<Image>().raycastTarget = false;
            }

            this.WaterDropLeftFinish();                                  
        }


        private EventProxy eventProxy;
        
        void AddListener()
        {
            this.WaterDropRightFinish();
            
            Game.EventSystem.RegisterEvent(EventIdType.WaterDropRightFinish, this.eventProxy);
        }

        void RemoveListener()
        {
            if(this.eventProxy != null)
                Game.EventSystem.UnRegisterEvent(EventIdType.WaterDropRightFinish, this.eventProxy);
        }

        void WaterDropLeftFinish()
        {
            Log.Info("左侧水滴停止播放");

            this.leftDropWater.SetActive(false);

            this.failShuiDiAnimation.SetActive(true);
        }
        
        // 右侧水滴停止播放，开始播放浇水动画
        void WaterDropRightFinish()
        {
            Log.Info("开始滴水到小苗上 1 次");

            this.successShuiDiAnimation.SetActive(true);

            this.rightDropWater.SetActive(false);


                                    
            this.PlayXiaoMiaoGrowUp().Coroutine();
        }

        
        /// <summary>
        /// 小苗开始成长
        /// </summary>
        /// <returns></returns>
        async ETVoid PlayXiaoMiaoGrowUp()
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
            
            float leng = this.successShuiDiAnimation.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
            
            // 等水滴浇灌两次
            await timerComponent.WaitAsync((long)(leng * 1 * 1000));
            
            Log.Info(" 开始小苗成长动画 ");

            if (this.IsDisposed)
                return;
            
            // 小苗隐藏
            this.xiaomiaoAniamtion.SetActive(false);
            
            // 小苗成长动画
            this.guideEndAnimation.SetActive(true);
            
            float endAnimaLeng =  this.guideEndAnimation.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
            
            //await timerComponent.WaitAsync((long)(endAnimaLeng * 1000));
            
            
            // 
            gamesence3.SetActive(true);
            
            this.gamesence3.GetComponent<CanvasGroup>().alpha = 0f;

            // 第18帧渐变到40%
            
            float jianbianTime = 18f / 104 * endAnimaLeng;
            
            Log.Info(" 墙角开始复现到40% ");
            
            this.gamesence3.GetComponent<CanvasGroup>().DOFade(0.4f, jianbianTime);

            await timerComponent.WaitAsync((long) (jianbianTime * 1000));
            
            // 第19帧到96帧不变
            
            Log.Info(" 墙角保持在40% ");

            jianbianTime = (96f - 19f) / 104 * endAnimaLeng;
            
            // 等待结束动画到96帧
            await timerComponent.WaitAsync((long) (jianbianTime * 1000));

            
            Log.Info(" 墙角开始复现到100% ");
            
            jianbianTime = (115f - 96f) / 104 * endAnimaLeng;
            
            this.gamesence3.GetComponent<CanvasGroup>().DOFade(1f, jianbianTime);
            
            // 等待动画结束
            await timerComponent.WaitAsync((long) (jianbianTime * 1000));
            
            // EndAnimation 结束
            
            Log.Info(" 出现结束图画 ");
            
            // 出画
            this.drawsence2.SetActive(true);

            this.drawsence2.GetComponent<CanvasGroup>().alpha = 0;

            this.drawsence2.GetComponent<CanvasGroup>().DOFade(1, 1);

            await timerComponent.WaitAsync(1 * 1000);
            
            // 图画完全显示出来
            
            this.CloseOtherDrawScene();
            
            await timerComponent.WaitAsync(1 * 1000);
            
            // 装进书里面
            
            this.CollectToBook().Coroutine();
        }

        void CloseOtherDrawScene()
        {
            this.context.SetActive(false);
            
            this.guideEndAnimation.SetActive(false);
            
            this.gamesence3.SetActive(false);
            
            this.successShuiDiAnimation.SetActive(false);

            this.failShuiDiAnimation.SetActive(false);

            this.zhuzi.SetActive(false);

            //this.
        }

        async ETVoid CollectToBook()
        {
            UIBase com = UIFactory.Create<UIBookComponent>(ViewLayer.UIPopupLayer, UIType.UIBook).Result;
            
            com.GetComponent<UIBookComponent>().AddImageGo(this.drawsence2);

            
            this.Close();
            
        }

        void Close()
        {
            this.drawsence2 = null;
            
            Game.EventSystem.Run(EventIdType.ShowJoystic);
            
            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIGuideScene);
        }
        

        void SolveFaild()
        {
            Log.Info("解题失败");

            // 设置掉落竹子的位置
            
            Vector3 vector3 = this.ZhuziAnimation.transform.position ;
            
            vector3.x = this.zhuzi.transform.position.x;

            this.ZhuziAnimation.transform.position = vector3;

            //this.zhuzi.GetComponent<Rigidbody2D>().gravityScale
            
            //return;

            this.ZhuziAnimation.SetActive(true);
            
            // 隐藏可拖拽的竹子
            this.zhuzi.SetActive(false);

            // 隐藏两个hine竹子

            this.hine1.GetComponent<UIMultImage>().enabled = false;

            this.hine2.GetComponent<UIMultImage>().enabled = false;

            this.stayHine = null;

            // 设置动画从第几帧开始键播

            Animator animator = this.ZhuziAnimation.GetComponent<Animator>();

            
            animator.Play("ZhuziAnimation",0,this.GetNormalizedTime());

            
            
            this.Restart().Coroutine();
            
            //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //sta

            //stateInfo.normalizedTime = 1;

            return;
            
            AnimatorClipInfo stateinfo = animator.GetCurrentAnimatorClipInfo(0)[0];

            stateinfo.clip.frameRate = 10;
        }

        async ETVoid Restart()
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            await timerComponent.WaitAsync((long) (1 * 1000));

            if (ZhuziAnimation != null)
                this.ZhuziAnimation.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
                {
                    if (this.ZhuziAnimation != null)
                    {
                        this.ZhuziAnimation.SetActive(false);
                        
                        this.ZhuziAnimation.GetComponent<CanvasGroup>().alpha = 1;

                        this.zhuzi.transform.localPosition = this.zhuziInitPos;

                        this.zhuzi.SetActive(true);
                    }
                    
                });
        }

        /// <summary>
        ///  从第几帧开始播
        /// </summary>
        /// <returns></returns>
        float GetNormalizedTime()
        {

            float y = this.zhuzi.transform.localPosition.y;
            
            Log.Info("当前竹子的高度y : " + y);
            
            if (y > 600)
                return 0f / 17;
            else if (y > 377)
                return 1f / 17;
            else if (y > 76)
                return 2f / 17;
            else if (y > -284)
                return 3f / 17;
            else
                return 4f / 17;
            
            return 0;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            this.xiaomiaoComponent.Dispose();

            Game.Scene.GetComponent<UnitComponent>().MyUnit.AddComponent<UnitCameraFollowComponent>();

            this.RemoveListener();
        }
    }
}
