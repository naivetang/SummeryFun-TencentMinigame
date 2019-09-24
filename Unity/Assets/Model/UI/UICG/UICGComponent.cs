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

            this.player = rc.Get<GameObject>("Player");

            if (this.player != null)
            {
                Log.Debug("player");
            }

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

            await timerComponent.WaitAsync((long)(this.bind.FirstPaintStaryTime * 1000));

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

            this.three.GetComponent<CanvasGroup>().DOFade(1f, 1f);

            var timerComponent = Game.Scene.GetComponent<TimerComponent>();

            await timerComponent.WaitAsync((long)(this.bind.FirstPaintStaryTime * 1000));
            
            

            // // 隐藏
            // this.three.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(() =>
            // {
            //     // 隐藏完之后播第二个
            //     this.PlayTwoAnimation().Coroutine();
            // });
        }
    }
}
