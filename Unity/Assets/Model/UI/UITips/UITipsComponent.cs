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
    public class UITipsComponentAwakeSystem: AwakeSystem<UITipsComponent>
    {
        public override void Awake(UITipsComponent self)
        {
            self.Awake();
        }
    }


    public class UITipsComponent : Component
    {
        private Button cancel;
        private Button enter;

        public void Awake()
        {
            this.cancel = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("Cancel").GetComponent<UINextButton>();
            
            this.enter = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("Enter").GetComponent<UINextButton>();
            
            this.Addlistener();
        }


        void Addlistener()
        {
            this.cancel.onClick.AddListener(this.CancelBtnClick);
            this.enter.onClick.AddListener(this.EnterBtnClick);
        }

        void EnterBtnClick()
        {
            UIFactory.Create<UIResultSceneComponent>(ViewLayer.UIFullScreenLayer, UIType.UIResultScene).Coroutine();
            
            this.Close();
        }

        void CancelBtnClick()
        {
            this.Close();
        }

        void Close()
        {
            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UITips);
        }

        void RemoveListener()
        {
            this.cancel.onClick.RemoveAllListeners();
            this.enter.onClick.RemoveAllListeners();
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
