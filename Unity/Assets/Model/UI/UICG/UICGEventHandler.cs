using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.CGToHallFinish)]
    class CGToHallFinish : AEvent
    {
        public override void Run()
        {
            // 移出CG
            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UICG);
            
            this.ForceGuide().Coroutine();
        }

        private EventProxy waitCompleteDialog;
        
        /// <summary>
        /// 强制引导
        /// </summary>
        /// <returns></returns>
        async ETVoid ForceGuide()
        {
            
            
            // 隐藏TriggerArea
            TriggerAreaBtn btn = TriggerAreaBtnComponent.Instance.Get(3001);

            btn.TriggerArea.GetComponent<BoxCollider2D>().enabled = false;
            
            // 注册对话结束就显示riggerArea
            waitCompleteDialog = new EventProxy(WaitCompleteDialog);
            
            Game.EventSystem.RegisterEvent(EventIdType.CompleteDialog, waitCompleteDialog);
            
            // 大叔
            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(1001);

            // 大叔叫喊
            GameObject uiDialog = unit.GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("AudioDialog"); 
            
            uiDialog.GetComponent<DialogTextCtl>().SetText("  哎~~~~~~！  ", 2f);

            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            // 等待1s,主角寻路过去
            await timer.WaitAsync(1 * 1000);

            Unit player = Game.Scene.GetComponent<UnitComponent>().Get(-1);

            Transform playerTransform = player.GameObject.transform;
            
            // 玩家向左走
            Game.EventSystem.Run(EventIdType.MoveDirChange,MoveDir.Left);

            Vector2 endPos = Game.Scene.GetComponent<UIComponent>().GetUI(UIType.UIMap).GetComponent<UIMapComponent>().GetForceGuidePos();
            
            while (true)
            {
                if (playerTransform.position.x < endPos.x)  //-300f
                    break;

                await timer.WaitAsync((long) (0.2f * 1000));
            }
            
            Game.EventSystem.Run(EventIdType.MoveDirChange,MoveDir.Up);
            
            while (true)
            {
                if (playerTransform.position.y > endPos.y) //-662f
                    break;

                await timer.WaitAsync((long) (0.2f * 1000));
            }
            
            Game.EventSystem.Run(EventIdType.MoveDirChange,MoveDir.Stop);

            
            
            
            
        }

        void WaitCompleteDialog(List<object> list)
        {
            
            
            //list[]
            int dialog =(int) list[0];

            Log.Info(dialog + "   ");
            
            if (dialog != 2001)
            {
                return;
            }
                
            Log.Info("打开引导关卡入口");
            
            // 打开TriggerArea
            
            TriggerAreaBtn btn = TriggerAreaBtnComponent.Instance.Get(3001);
            
            btn.TriggerArea.GetComponent<BoxCollider2D>().enabled = true;   
                
            //Game.EventSystem.UnRegisterEvent(EventIdType.CompleteDialog,waitCompleteDialog);

            //this.waitCompleteDialog = null;
        }
    }

    [Event(EventIdType.EnterCG)]
    class EnterCG : AEvent
    {
        public override void Run()
        {
            UIFactory.Create<UICGComponent>(ViewLayer.UIBgLayer, UIType.UICG).Coroutine();
        }
    }

}
