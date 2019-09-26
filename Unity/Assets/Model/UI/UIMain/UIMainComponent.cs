using System;
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
    public class UIMainComponentSystem : AwakeSystem<UIMainComponent>
    {
        public override void Awake(UIMainComponent self)
        {
            self.Awake();
        }
    }

    public class UIMainComponent : Component
    {
        private GameObject _joyStick;

        /// <summary>
        /// 触发某个行为：进场景
        /// </summary>
        private GameObject _actionBtn;

        /// <summary>
        /// 对话提示
        /// </summary>
        private GameObject _dialogBtn;


        //private GameObject _player;

        //private GameObject _bg;

        private GameObject _context;

        private TriggerAreaConfig _currenArea;

        /// <summary>
        /// 引导关入口
        /// </summary>
        //private Button _yinDaoBtn;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            _joyStick = rc.Get<GameObject>("JoyStick");

            _actionBtn = rc.Get<GameObject>("ActionBtn");

            //this._yinDaoBtn = rc.Get<GameObject>("YinDaoBtn").GetComponent<UINextButton>();
            
            //this._yinDaoBtn.gameObject.SetActive(false);

            _actionBtn.SetActive(false);

            _dialogBtn = rc.Get<GameObject>("DialogBtn");
            this._context = rc.Get<GameObject>("Context");

            _dialogBtn.SetActive(false);

            //_player = rc.Get<GameObject>("Player");

            //_player = player;


            //this._bg = rc.Get<GameObject>("BG");


            //this.PlayUpToDownAnimation();

            this.AddListener();
        }
        //
        // void PlayUpToDownAnimation()
        // {
        //
        //     this.GetParent<UIBase>().GameObject.transform.DOLocalMoveY(0, 4f).SetEase(Ease.Linear).OnComplete(this.AnimationComplte);
        //
        //
        // }
        //
        // void AnimationComplte()
        // {
        //
        //     //this._bg.SetActive(true);
        //
        //     this._joyStick.gameObject.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        //
        //     this.AddListener();
        //
        //     Game.EventSystem.Run(EventIdType.CGToHallFinish);
        // }


        private void AddListener()
        {
            Game.EventSystem.RegisterEvent(EventIdType.TriggerAera, new EventProxy(TriggerArea));

            _actionBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                UIFactory.Create<UIDragonflyComponent>(ViewLayer.UIFullScreenLayer, UIType.UIDragonfly).Coroutine();
            });
            
            Game.EventSystem.RegisterEvent(EventIdType.CGToHallFinish, new EventProxy(this.ShowByAnimation));
        }

        private void ShowByAnimation(List<object> obj)
        {
            this._context.GetComponent<CanvasGroup>().DOFade(1, 1);
        }
        

        private void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.TriggerAera, new EventProxy(TriggerArea));
            Game.EventSystem.UnRegisterEvent(EventIdType.CGToHallFinish, new EventProxy(ShowByAnimation));
        }

        
        // 区域触发事件
        private void TriggerArea(List<object> obj)
        {
            string action = obj[0] as string;
            int triggerId = (int)obj[1];

            // 进入
            
            if (action.Equals("Enter"))
            {
                this._actionBtn.SetActive(true);

                this._currenArea = Game.Scene.GetComponent<ConfigComponent>().Get(typeof (TriggerAreaConfig), triggerId) as TriggerAreaConfig;
                
                
                
                if(this._currenArea == null)
                    Log.Error("不存在触发区域id：" + triggerId);
                else
                {
                    Log.Info("进入事件区域,id：" + this._currenArea.Id);
                }

                if (!string.IsNullOrEmpty(this._currenArea.EventName))
                {
                    Log.Info("事件名：" + this._currenArea.EventName);
                }
                
                SolveQiPao(true);
            }

            // 退出

            if (action.Equals("Exit"))
            {
                this._actionBtn.SetActive(false);

                SolveQiPao(false);
                
                this._currenArea = null;
            }
                
        }

        private void SolveQiPao(bool isEnter)
        {
            TriggerAreaBtn btn = TriggerAreaBtnComponent.Instance.Get(this._currenArea.Id);

            if (btn == null)
            {
                Log.Error("不存在区域 id : " + this._currenArea.Id);
                return;
            }
            // 进入
            if (isEnter)
            {
                btn.GameObject.SetActive(true);
            }
            // 退出
            else
            {
                btn.GameObject.SetActive(false);
            }
        }
        


        public override void Dispose()
        {
            base.Dispose();
            RemoveListener();
        }
    }
}
