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

    [HideInHierarchy]
    public class UIGuideXiaomiaoComponent : Component
    {
        private GameObject xiaomiao;


        private GameObject xiaomiaoAimation;

        private GameObject xiaomiaoClick;
        
        private GameObject tishiDialog;
        
        public void Awake(GameObject go)
        {
            this.xiaomiao = go;


            ReferenceCollector rc = this.xiaomiao.GetComponent<ReferenceCollector>();

            this.xiaomiaoAimation = rc.Get<GameObject>("xiaomiaoAniamtion");
            this.xiaomiaoClick = rc.Get<GameObject>("xiaomiaoClick");
            this.tishiDialog = rc.Get<GameObject>("tishiDialog");
            
            

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
                this.tishiDialog.SetActive(true);
                
                this.DialogWait3SToHide().Coroutine();
            }
        }

        async ETVoid DialogWait3SToHide()
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            await timer.WaitAsync(3 * 1000);
            
            this.tishiDialog.SetActive(false);
        }
        
        void PointUp(PointerEventData data)
        {
            this.xiaomiaoClick.SetActive(false);
            
            this.xiaomiaoAimation.SetActive(true);
        }
    }
}