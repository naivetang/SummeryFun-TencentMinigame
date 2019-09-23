
using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    
    [ObjectSystem]
    public class UIDragonflySystem : AwakeSystem<UIDragonflyComponent>
    {
        public override void Awake(UIDragonflyComponent self)
        {
            self.Awake();
        }
    }

    public class UIDragonflyComponent : Component
    {
        private GameObject  _context;


        private GameObject _gameScene;

        private GameObject _drawScene;

        private GameObject _waterDropMiddle;
        
        private GameObject _waterDropMiddleTouch;
        
        // 蜻蜓
        private GameObject _dragonflyPlantBounce;
        
        private GameObject _waterDropLeft;
        
        private GameObject _waterDropRight;

        private Button _waterDropMiddleButton;


        private float _waitMinTime = 2;
        
        private float _waitMaxTime = 5;
        
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();
            
            this._context = rc.Get<GameObject>("Context");
            
            this._gameScene = rc.Get<GameObject>("GameScene");
            
            this._drawScene = rc.Get<GameObject>("DrawScene");
            
            this._waterDropMiddle = rc.Get<GameObject>("WaterDropMiddle");
            
            this._waterDropMiddleTouch = rc.Get<GameObject>("WaterDropMiddleTouch");
            
            this._waterDropMiddleTouch.SetActive(false);
            
            this._dragonflyPlantBounce = rc.Get<GameObject>("DragonflyPlantBounce");
            
            this._waterDropLeft = rc.Get<GameObject>("WaterDropLeft");
            
            this._waterDropRight = rc.Get<GameObject>("WaterDropRight");
            
            this._waterDropMiddleButton = rc.Get<GameObject>("WaterDropMiddleButton").GetComponent<Button>();
            
            this.PlayAnimation();
            
            this.AddListener();
        }

        private void AddListener()
        {
            this._waterDropMiddleButton.onClick.AddListener(this.OnWaterBtnClick);
            
            Game.EventSystem.RegisterEvent(EventIdType.WaterDropMiddleTouch, new EventProxy(WaterDropMiddleTouch));
        }

        private void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.WaterDropMiddleTouch, new EventProxy(WaterDropMiddleTouch));
        }

        private void WaterDropMiddleTouch(List<object> obj)
        {
            var animator = this._dragonflyPlantBounce.GetComponent<Animator>();

            animator.enabled = true;

            this._drawScene.GetComponent<CanvasGroup>().DOFade(1, 2).SetDelay(2f).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                this._context.GetComponent<CanvasGroup>().DOFade(0, 2).SetDelay(2).OnComplete(() =>
                {
                    Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIDragonfly);
                });
            });


        }

        private void OnWaterBtnClick()
        {
            // var canvsGroup = this._drawScene.GetComponent<CanvasGroup>();
            //
            // this._drawScene.SetActive(true);
            //
            // canvsGroup.alpha = 0;
            //
            // canvsGroup.DOFade(1, 2).SetDelay(2).OnComplete(() =>
            //     {
            //         this._context.GetComponent<CanvasGroup>().DOFade(0, 2).SetDelay(2).OnComplete(() =>
            //             {
            //                 Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIDragonfly);
            //             });
            //     });
            
            this._waterDropMiddle.SetActive(false);
            
            this._waterDropMiddleTouch.SetActive(true);
        }


        private void PlayAnimation()
        {
            var canvasGroup = this._gameScene.GetComponent<CanvasGroup>();
            
            this._gameScene.SetActive(true);

            canvasGroup.alpha = 0;

            canvasGroup.DOFade(1f, 2f);

            
            //this.LeftWaterAnimation();
            
            //this.RightWaterAnimation();

        }

        private void LeftWaterAnimation()
        {
            var animatior = this._waterDropLeft.GetComponent<Animator>();

            if (animatior == null)
                return;

            AnimationClip clip = animatior.GetCurrentAnimatorClipInfo(0)[0].clip;

            if (clip == null)
                return;
            
            float animDur = clip.averageDuration;
        }

        private async ETVoid RightWaterAnimation()
        {
            var animatior = this._waterDropRight.GetComponent<Animator>();

            if (animatior == null)
                return;
            
            AnimationClip clip = animatior.GetCurrentAnimatorClipInfo(0)[0].clip;

            if (clip == null)
                return;

            float animDur = clip.averageDuration;

            var timerComponent = Game.Scene.GetComponent<TimerComponent>();


            
            
            while (true)
            {
                await timerComponent.WaitAsync(Convert.ToInt64((animDur + 1) * 1000));

                animatior.enabled = true;
                
                
                
            }
            
            
        }

        public override void Dispose()
        {
            base.Dispose();
            
            this.RemoveListener();
        }
    }
}
