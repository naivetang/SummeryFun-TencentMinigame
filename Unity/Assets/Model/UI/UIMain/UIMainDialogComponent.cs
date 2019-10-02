using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UIMainDialogComponentAwakeSystem: AwakeSystem<UIMainDialogComponent, GameObject>
    {
        public override void Awake(UIMainDialogComponent self, GameObject go)
        {
            self.Awake(go);
        }
    }

    [HideInHierarchy]
    public class UIMainDialogComponent: Component
    {

        /// <summary>
        /// 对话组开始的id
        /// </summary>
        private int lastDialogId;
        /// <summary>
        /// 当前对话组id
        /// </summary>
        private int currentDialogId;

        private Button btn;


        private EventProxy nextDialog;
        
        public void Awake(GameObject go)
        {

            this.GameObject = go;

            btn = this.GameObject.GetComponent<UINextButton>();

            this.btn.gameObject.SetActive(false);

            this.AddLsitener();
        }

        void AddLsitener()
        {
            this.btn.onClick.AddListener(this.BtnOnClick);
            
            this.nextDialog = new EventProxy(NextDialogHandler);

            Game.EventSystem.RegisterEvent(EventIdType.NextDialog, this.nextDialog);

            this.dialogAeraHandler = new EventProxy(this.DialogAeraHandler);

            Game.EventSystem.RegisterEvent(EventIdType.DialogAera, this.dialogAeraHandler);
        }

        private void NextDialogHandler(List<object> obj)
        {
            lastDialogId = currentDialogId = (int)obj[0];
            
            this.GameObject.SetActive(true);
            
            this.CheckDialogBtn();
        }

        void BtnOnClick()
        {
            //this.currentDialogId;

            DialogConfig dialogConfig = Game.Scene.GetComponent<ConfigComponent>().Get(typeof (DialogConfig), this.currentDialogId) as DialogConfig;

            if (dialogConfig == null)
            {

                Log.Error("不存在dialog配置,dialog id = " + this.currentDialogId);

                return;
            }

            Unit unit = Game.Scene.GetComponent<UnitComponent>().Get(dialogConfig.RoleId);

            UnityEngine.GameObject dialogGo = unit.GameObject.GetComponent<ReferenceCollector>().Get<GameObject>("UIDialog");

            DialogShowText(dialogGo, dialogConfig.Text, dialogConfig.ShowTime, dialogConfig.NextId != -1);


            this.lastDialogId = this.currentDialogId;
            
            this.currentDialogId = dialogConfig.NextId;
            

            this.CheckDialogBtn();
        }

        void CheckDialogBtn()
        {
            if (this.currentDialogId == -1)
            {
                Log.Info("结束对话");
                
                Game.EventSystem.Run(EventIdType.CompleteDialog, this.lastDialogId);
                
                this.GameObject.SetActive(false);
            }
        }

        void DialogShowText(GameObject dialogGo, string text, float closeTime, bool hasNext)
        {
            Log.Info("对话内容：" + text);

            ///GameObject textGo = dialogGo.GetComponent<ReferenceCollector>().Get<GameObject>("TextContext");

            dialogGo.SetActive(true);

            dialogGo.GetComponent<DialogTextCtl>().SetText(text, closeTime, hasNext);
            
            //UIFactory.Create<UIGuideSceneComponent>()
        }


        private EventProxy dialogAeraHandler;

        private void DialogAeraHandler(List<object> obj)
        {
            string state = obj[0] as string;

            int dialogId = (int) obj[1];

            
            
            // 进入对话区域
            if (state.Equals("Enter"))
            {
                Log.Info("进入对话区域，id ： " + dialogId);


                return;

                this.lastDialogId = dialogId;
                
                currentDialogId = dialogId;

                this.GameObject.SetActive(true);
            }

            // 离开对话区域
            else if (state.Equals("Exit"))
            {

                this.GameObject.SetActive(false);
                
                return;
                
                this.currentDialogId = 0;

                this.GameObject.SetActive(false);
            }

        }

        void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.NextDialog, this.nextDialog);

            Game.EventSystem.UnRegisterEvent(EventIdType.DialogAera, this.dialogAeraHandler);
        }

        public override void Dispose()
        {
            base.Dispose();

            this.RemoveListener();
        }
    }
}