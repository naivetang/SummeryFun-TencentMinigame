﻿using System;
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
    public class UIMapComponentSystem : AwakeSystem<UIMapComponent,GameObject>
    {
        public override void Awake(UIMapComponent self,GameObject go)
        {
            self.Awake(go);
        }
    }

    public class UIMapComponent : Component 
    {
        //private GameObject _joyStick;
        
        /// <summary>
        /// 触发某个行为：进场景
        /// </summary>
        //private GameObject _actionBtn;

        /// <summary>
        /// 对话提示
        /// </summary>
        //private GameObject _dialogBtn;


        private GameObject _player;

        private GameObject _bg;

        private GameObject _context;
        
        
        public void Awake(GameObject player)
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            //_joyStick = rc.Get<GameObject>("JoyStick");

            //_actionBtn = rc.Get<GameObject>("ActionBtn");

            //_actionBtn.SetActive(false);

            //_dialogBtn = rc.Get<GameObject>("DialogBtn");
            this._context = rc.Get<GameObject>("Context");
            
            //_dialogBtn.SetActive(false);

            //_player = rc.Get<GameObject>("Player");

            _player = player;


            this._bg = rc.Get<GameObject>("BG");

            
            this.PlayUpToDownAnimation();
            
            
        }

        void PlayUpToDownAnimation()
        {

            this.GetParent<UIBase>().GameObject.transform.DOLocalMoveY(0, 4f).SetEase(Ease.Linear).OnComplete(this.AnimationComplte);
            
            
        }

        void AnimationComplte()
        {
            
            this._bg.SetActive(true);
            
            //this._joyStick.gameObject.GetComponent<CanvasGroup>().DOFade(1f, 1f);
            
            this.AddListener();

            this.InitPlayer();

            Game.EventSystem.Run(EventIdType.CGToHallFinish);
        }
        

        private void AddListener()
        {
            //Game.EventSystem.RegisterEvent(EventIdType.TriggerAera, new EventProxy(TriggerArea));

            // _actionBtn.GetComponent<Button>().onClick.AddListener(() =>
            // {
            //     UIFactory.Create<UIDragonflyComponent>(ViewLayer.UIFullScreenLayer, UIType.UIDragonfly).Coroutine();
            // });
        }

        private void RemoveListener()
        {
            //Game.EventSystem.UnRegisterEvent(EventIdType.TriggerAera, new EventProxy(TriggerArea));
        }

        private void TriggerArea(List<object> obj)
        {
            // string action = obj[0] as string;
            // int triggerId = (int)obj[1];
            //
            // if(triggerId == 1 && action.Equals("Enter"))
            //     this._actionBtn.SetActive(true);
            //
            // if (triggerId == 1 && action.Equals("Exit"))
            //     this._actionBtn.SetActive(false);
        }

        

        private void InitPlayer()
        {
            
            
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();

            this._player.transform.SetParent(this._context.transform);

            unitComponent.MyUnit.AddComponent<UnitMoveComponent>();

            unitComponent.MyUnit.AddComponent<UnitCameraFollowComponent>();

            //
            // Unit unit = ComponentFactory.CreateWithId<Unit, GameObject>(IdGenerater.GenerateId(), this._player);
            //
            // unitComponent.MyUnit = unit;
            //
            // unit.AddComponent<UnitMoveComponent>();
        }
       

        public override void Dispose()
        {
            base.Dispose();
            RemoveListener();
        }
    }
}
