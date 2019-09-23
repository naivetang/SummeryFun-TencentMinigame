using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{

    [ObjectSystem]
    public class UIHallComponentSystem : AwakeSystem<UIHallComponent>
    {
        public override void Awake(UIHallComponent self)
        {
            self.Awake();
        }
    }

    public class UIHallComponent : Component 
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

        /// <summary>
        /// 对话提示
        /// </summary>
        private GameObject _player;

        
        
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            _joyStick = rc.Get<GameObject>("JoyStick");

            _actionBtn = rc.Get<GameObject>("ActionBtn");

            _actionBtn.SetActive(false);

            _dialogBtn = rc.Get<GameObject>("DialogBtn");
            
            _dialogBtn.SetActive(false);

            _player = rc.Get<GameObject>("Player");
            
            this.AddListener();

            this.InitPlayer();
        }

        private void AddListener()
        {
            Game.EventSystem.RegisterEvent(EventIdType.TriggerAera, new EventProxy(TriggerArea));

            _actionBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                UIFactory.Create<UIDragonflyComponent>(ViewLayer.UIFullScreenLayer, UIType.UIDragonfly).Coroutine();
            });
        }

        private void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.TriggerAera, new EventProxy(TriggerArea));
        }

        private void TriggerArea(List<object> obj)
        {
            string action = obj[0] as string;
            int triggerId = (int)obj[1];
            
            if(triggerId == 1 && action.Equals("Enter"))
                this._actionBtn.SetActive(true);

            if (triggerId == 1 && action.Equals("Exit"))
                this._actionBtn.SetActive(false);
        }

        

        private void InitPlayer()
        {
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();

            Unit unit = ComponentFactory.CreateWithId<Unit, GameObject>(IdGenerater.GenerateId(), this._player);

            unitComponent.MyUnit = unit;

            unit.AddComponent<UnitMoveComponent>();
        }
       

        public override void Dispose()
        {
            base.Dispose();
            RemoveListener();
        }
    }
}
