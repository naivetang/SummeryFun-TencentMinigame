using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UICGAwakeSystem: AwakeSystem<UICGComponent>
    {
        public override void Awake(UICGComponent self)
        {
            self.Awake();
        }
    }
    
    public class UICGComponent : Component
    {
        private GameObject one;
        private GameObject two;
        private GameObject three;

        private GameObject BG;
        private GameObject MainBG;
        
        private Text oneText;

        private UICGBind bind;

        private GameObject player;
        
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();
            
            this.bind = this.GetParent<UIBase>().GameObject.GetComponent<UICGBind>();

            this.one = rc.Get<GameObject>("one");

            this.two = rc.Get<GameObject>("two");

            this.three = rc.Get<GameObject>("three");

            this.BG = rc.Get<GameObject>("BG");
            
            this.MainBG = rc.Get<GameObject>("mainbg");

            this.player = rc.Get<GameObject>("Player");
            //
            // if (this.player != null)
            // {
            //     Log.Debug("player");
            // }

            //this.oneText = rc.Get<GameObject>("OneText").GetComponent<Text>();

             //string endText = this.oneText.text;

             //this.oneText.text = "";

             //this.oneText.DOText(endText, 4f, true, ScrambleMode.None, null).SetEase(Ease.Linear);

             Init();
        }

        void Init()
        {
            this.PlayAnimation();
        }

        void PlayAnimation()
        {
            
            this.PlayOneAnimation().Coroutine();
        }

        async ETVoid PlayOneAnimation()
        {
            this.BG.SetActive(true);

            this.BG.GetComponent<CanvasGroup>().DOFade(1f, 1f);
            
            this.one.SetActive(true);

            this.one.GetComponent<CanvasGroup>().DOFade(1f, 1f);

            var timerComponent = Game.Scene.GetComponent<TimerComponent>();

            await timerComponent.WaitAsync((long)(this.bind.FirstPaintStaryTime * 1000));

            // 隐藏
            this.one.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(() =>
            {
                // 隐藏完之后播第二个
                this.PlayTwoAnimation().Coroutine();
            });
            
        }

        async ETVoid PlayTwoAnimation()
        {
            
            this.two.SetActive(true);

            this.two.GetComponent<CanvasGroup>().DOFade(1f, 1f);

            var timerComponent = Game.Scene.GetComponent<TimerComponent>();

            await timerComponent.WaitAsync((long)(this.bind.SecondPaintStaryTime * 1000));

            // 隐藏
            this.two.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(() =>
            {
                // 隐藏完之后播第二个
                this.PlayThreeAnimation().Coroutine();
            });
        }


        async ETVoid PlayThreeAnimation()
        {
            this.three.SetActive(true);
            
            var timerComponent = Game.Scene.GetComponent<TimerComponent>();

            await timerComponent.WaitAsync((long) (0.1 * 1000));
            
            // 先向下走
            Game.EventSystem.Run(EventIdType.MoveDirChange, MoveDir.Down);
            

            this.three.GetComponent<CanvasGroup>().DOFade(1f, 1f);

            await timerComponent.WaitAsync((long)(this.bind.ThreePaintStaryTime * 1000));

            //Game.EventSystem.Run(EventIdType.MoveDirChange, MoveDir.Up);

            // // 隐藏
            // this.three.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(() =>
            // {
            //     // 隐藏完之后播第二个
            //     this.PlayTwoAnimation().Coroutine();
            // });
            
            this.ThreeAnimationComplte();
        }


        void ThreeAnimationComplte()
        {
            // CG完成，main场景从上往下出场，4s
            Game.EventSystem.Run(EventIdType.CGFinish, player);
            
            // 让主角有向上走的动画
            Game.EventSystem.Run(EventIdType.MoveDirChange, MoveDir.Up);
            
            this.MainBG.GetComponent<CanvasGroup>().DOFade(1,4);

            this.player.transform.DOScale(Vector3.one, 4).OnComplete(this.Close);
        }

        void Close()
        {
            //Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UICG);
        }
    }
}
